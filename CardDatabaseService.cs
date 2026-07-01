using System.Text.Json;

namespace MTGB.CardDatabaseEditor;

internal sealed class CardDatabaseService : IDisposable
{
    public const int SupportedSchemaVersion = 2;

    private readonly NativeSqlite database;

    public CardDatabaseService(string path)
    {
        Path = System.IO.Path.GetFullPath(path);
        database = NativeSqlite.Open(Path, false);

        int version = ReadSchemaVersion();
        if (version != SupportedSchemaVersion)
        {
            database.Dispose();
            throw new InvalidDataException($"不支持的数据库版本 {version}，当前编辑器需要版本 {SupportedSchemaVersion}。");
        }

        EnsureEditorTables();
    }

    public string Path { get; }
    public string DirectoryPath => System.IO.Path.GetDirectoryName(Path) ?? AppContext.BaseDirectory;

    public static void Create(string path, string schemaPath)
    {
        if (File.Exists(path)) throw new IOException("目标数据库已经存在。");
        Directory.CreateDirectory(System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(path))!);

        using NativeSqlite created = NativeSqlite.Open(path, true);
        created.Execute(File.ReadAllText(schemaPath));
        created.ExecuteNonQuery(
            "INSERT INTO metadata(key, value) VALUES (?, ?);",
            "schema_version",
            SupportedSchemaVersion.ToString());
        created.ExecuteNonQuery(
            "INSERT INTO metadata(key, value) VALUES (?, ?);",
            "database_id",
            $"custom.{Guid.NewGuid():N}");
        created.ExecuteNonQuery(
            "INSERT INTO metadata(key, value) VALUES (?, ?);",
            "display_name",
            System.IO.Path.GetFileNameWithoutExtension(path));
    }

    public List<CardSummary> LoadSummaries()
    {
        var result = new List<CardSummary>();
        database.Query(@"
SELECT card_key, card_id, set_code, collector_number, name, type_flags
FROM cards
ORDER BY name COLLATE NOCASE, set_code, collector_number;", row =>
        {
            result.Add(new CardSummary
            {
                CardKey = row.String(0),
                CardId = row.Int32(1),
                SetCode = row.String(2),
                CollectorNumber = row.String(3),
                Name = row.String(4),
                Types = (CardTypeFlags)row.Int32(5)
            });
        });
        return result;
    }

    public CardRecord LoadCard(string cardKey)
    {
        CardRecord? card = null;
        database.Query(@"
SELECT card_key, card_id, oracle_id, set_code, collector_number, name,
       type_flags, mana_cost, rules_text, power, toughness, script_path, enabled, supertype_flags, subtypes 
FROM cards WHERE card_key = ? LIMIT 1;", row =>
        {
            card = new CardRecord
            {
                OriginalCardKey = row.String(0),
                CardId = row.Int32(1),
                OracleId = row.String(2),
                SetCode = row.String(3),
                CollectorNumber = row.String(4),
                Name = row.String(5),
                Types = (CardTypeFlags)row.Int32(6),
                ManaCost = row.String(7),
                RulesText = row.String(8),
                Power = row.Int32(9),
                Toughness = row.Int32(10),
                ScriptPath = row.String(11),
                Enabled = row.Int32(12) != 0,
                Supertypes = (CardSupertypeFlags)row.Int32(13),
                Subtypes = row.String(14)
            };
        }, cardKey);

        if (card == null) throw new KeyNotFoundException($"找不到卡牌 {cardKey}。");
        CardRecord loadedCard = card;

        bool hasCharacteristics = false;
        database.Query(@"
SELECT loyalty, defense
FROM card_characteristics WHERE card_key = ? LIMIT 1;", row =>
        {
            hasCharacteristics = true;
            loadedCard.Loyalty = row.Int32(0);
            loadedCard.Defense = row.Int32(1);
        }, cardKey);

        // Version 1 of the editor stored a battle-only card's defense value in toughness.
        // Preserve that value when opening a database that has not yet saved editor characteristics.
        if (!hasCharacteristics
            && loadedCard.Types.HasFlag(CardTypeFlags.Battle)
            && !loadedCard.Types.HasFlag(CardTypeFlags.Creature))
        {
            loadedCard.Defense = loadedCard.Toughness;
        }

        database.Query(@"
SELECT effect_order, trigger, effect_key, parameters_json
FROM card_effects WHERE card_key = ? ORDER BY effect_order;", row =>
        {
            loadedCard.Effects.Add(new CardEffectRecord
            {
                Order = row.Int32(0),
                Trigger = row.String(1),
                EffectKey = row.String(2),
                ParametersJson = row.String(3)
            });
        }, cardKey);

        database.Query(@"
SELECT string_index, text
FROM card_strings WHERE card_key = ? ORDER BY string_index;", row =>
        {
            loadedCard.Strings.Add(new CardStringRecord
            {
                Index = row.Int32(0),
                Text = row.String(1)
            });
        }, cardKey);

        loadedCard.MultipartID = String.Empty;
        database.Query(@"
SELECT child_key 
FROM multipart WHERE card_key = ?;", row =>
        {
            // TODO multiply multipart support
            loadedCard.MultipartID = row.String(0);
        }, cardKey);

        return loadedCard;
    }

    public void Save(CardRecord card)
    {
        ValidateCard(card);
        string newKey = card.CardKey;

        database.Execute("BEGIN IMMEDIATE;");
        try
        {
            if (card.OriginalCardKey == null)
            {
                database.ExecuteNonQuery(@"
INSERT INTO cards(card_key, card_id, oracle_id, set_code, collector_number, name,
                  type_flags, mana_cost, rules_text, power, toughness, script_path, enabled, supertype_flags, subtypes)
VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);",
                    newKey,
                    card.CardId,
                    card.OracleId.Trim(),
                    card.SetCode.Trim().ToUpperInvariant(),
                    card.CollectorNumber.Trim(),
                    card.Name.Trim(),
                    (int)card.Types,
                    card.ManaCost.Trim(),
                    card.RulesText,
                    card.Power,
                    card.Toughness,
                    card.ScriptPath.Trim(),
                    card.Enabled,
                    card.Supertypes,
                    card.Subtypes);
            }
            else
            {
                // 子表没有 ON UPDATE CASCADE。先暂存于 CardRecord，再删除并在更新后恢复。
                database.ExecuteNonQuery("DELETE FROM card_effects WHERE card_key = ?;", card.OriginalCardKey);
                database.ExecuteNonQuery("DELETE FROM card_strings WHERE card_key = ?;", card.OriginalCardKey);
                database.ExecuteNonQuery("DELETE FROM card_characteristics WHERE card_key = ?;", card.OriginalCardKey);

                int changed = database.ExecuteNonQuery(@"
UPDATE cards SET card_key = ?, card_id = ?, oracle_id = ?, set_code = ?, collector_number = ?,
                 name = ?, type_flags = ?, mana_cost = ?, rules_text = ?, power = ?, toughness = ?,
                 script_path = ?, enabled = ?, supertype_flags = ?, subtypes = ? 
WHERE card_key = ?;",
                    newKey,
                    card.CardId,
                    card.OracleId.Trim(),
                    card.SetCode.Trim().ToUpperInvariant(),
                    card.CollectorNumber.Trim(),
                    card.Name.Trim(),
                    (int)card.Types,
                    card.ManaCost.Trim(),
                    card.RulesText,
                    card.Power,
                    card.Toughness,
                    card.ScriptPath.Trim(),
                    card.Enabled,
                    card.Supertypes,
                    card.Subtypes,
                    card.OriginalCardKey);

                if (changed != 1)
                    throw new InvalidOperationException("保存失败：原卡牌记录不存在。");
            }

            database.ExecuteNonQuery(@"
INSERT INTO card_characteristics(card_key, loyalty, defense)
VALUES (?, ?, ?);",
                newKey,
                card.Loyalty,
                card.Defense);

            foreach (CardEffectRecord effect in card.Effects.OrderBy(effect => effect.Order))
            {
                database.ExecuteNonQuery(@"
INSERT INTO card_effects(card_key, effect_order, trigger, effect_key, parameters_json)
VALUES (?, ?, ?, ?, ?);",
                    newKey,
                    effect.Order,
                    effect.Trigger.Trim(),
                    effect.EffectKey.Trim(),
                    effect.ParametersJson.Trim());
            }

            foreach (CardStringRecord text in card.Strings.OrderBy(text => text.Index))
            {
                database.ExecuteNonQuery(@"
INSERT INTO card_strings(card_key, string_index, text)
VALUES (?, ?, ?);",
                    newKey,
                    text.Index,
                    text.Text);
            }

            if (!String.IsNullOrEmpty(card.MultipartID)) {
                // FIXME delete whole chain
                database.ExecuteNonQuery(@"
DELETE FROM multipart 
WHERE (card_key == ? OR card_key == ?);",
                    newKey,
                    card.OriginalCardKey ?? String.Empty);
                database.ExecuteNonQuery(@"
INSERT INTO multipart(card_key, child_key) 
VALUES (?, ?);",
                    newKey,
                    card.MultipartID);
            }

            database.Execute("COMMIT;");
            card.OriginalCardKey = newKey;
        }
        catch
        {
            database.Execute("ROLLBACK;");
            throw;
        }
    }

    public void Delete(string cardKey)
    {
        database.ExecuteNonQuery("DELETE FROM cards WHERE card_key = ?;", cardKey);
        // TODO delete multipart chain
    }

    public DatabaseValidationResult ValidateDatabase()
    {
        var result = new DatabaseValidationResult
        {
            SchemaVersion = ReadSchemaVersion(),
            CardCount = database.ScalarInt("SELECT COUNT(*) FROM cards;"),
            EffectCount = database.ScalarInt("SELECT COUNT(*) FROM card_effects;"),
            StringCount = database.ScalarInt("SELECT COUNT(*) FROM card_strings;"),
            CharacteristicCount = database.ScalarInt("SELECT COUNT(*) FROM card_characteristics;")
        };

        if (result.SchemaVersion != SupportedSchemaVersion)
            result.Errors.Add($"Schema 版本应为 {SupportedSchemaVersion}，实际为 {result.SchemaVersion}。");

        database.Query(@"
SELECT card_key FROM cards
WHERE trim(name) = '' OR trim(set_code) = '' OR trim(collector_number) = '' OR card_id <= 0;",
            row => result.Errors.Add($"卡牌 {row.String(0)} 缺少必要字段。"));

        database.Query(@"
SELECT card_key, parameters_json FROM card_effects;", row =>
        {
            try
            {
                JsonDocument.Parse(row.String(1)).Dispose();
            }
            catch
            {
                result.Errors.Add($"卡牌 {row.String(0)} 的旧版效果参数不是有效 JSON。");
            }
        });

        database.Query(@"
SELECT card_key, string_index FROM card_strings WHERE string_index < 0;", row =>
            result.Errors.Add($"卡牌 {row.String(0)} 存在无效的提示文本索引 {row.Int32(1)}。"));

        return result;
    }

    public void Dispose() => database.Dispose();

    private void EnsureEditorTables()
    {
        database.Execute(@"
CREATE TABLE IF NOT EXISTS card_strings (
    card_key TEXT NOT NULL,
    string_index INTEGER NOT NULL,
    text TEXT NOT NULL DEFAULT '',
    PRIMARY KEY (card_key, string_index),
    FOREIGN KEY (card_key) REFERENCES cards(card_key) ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS card_characteristics (
    card_key TEXT PRIMARY KEY,
    loyalty INTEGER NOT NULL DEFAULT 0,
    defense INTEGER NOT NULL DEFAULT 0,
    FOREIGN KEY (card_key) REFERENCES cards(card_key) ON DELETE CASCADE
);");
    }

    private int ReadSchemaVersion()
    {
        string? value = database.ScalarString(
            "SELECT value FROM metadata WHERE key = 'schema_version' LIMIT 1;");
        return int.TryParse(value, out int version) ? version : 0;
    }

    private static void ValidateCard(CardRecord card)
    {
        if (card.CardId <= 0)
            throw new InvalidDataException("内部卡牌 ID 必须大于 0。");
        if (string.IsNullOrWhiteSpace(card.Name))
            throw new InvalidDataException("卡名不能为空。");
        if (string.IsNullOrWhiteSpace(card.SetCode))
            throw new InvalidDataException("系列代号不能为空。");
        if (card.SetCode.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            throw new InvalidDataException("系列代号包含不能用于图片目录的字符。");
        if (string.IsNullOrWhiteSpace(card.CollectorNumber))
            throw new InvalidDataException("卡图编号不能为空。");
        if (card.CollectorNumber.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
            throw new InvalidDataException("卡图编号包含不能用于图片文件名的字符。");
        if (card.Types == CardTypeFlags.None)
            throw new InvalidDataException("至少选择一种卡牌类型。");
        if (card.Types == CardTypeFlags.Kindred)
            throw new InvalidDataException("亲族不能单独存在，必须同时选择另一种卡牌类型。");

        var effectOrders = new HashSet<int>();
        foreach (CardEffectRecord effect in card.Effects)
        {
            if (!effectOrders.Add(effect.Order))
                throw new InvalidDataException($"旧版效果顺序 {effect.Order} 重复。");
            if (string.IsNullOrWhiteSpace(effect.Trigger))
                throw new InvalidDataException("旧版效果触发时机不能为空。");
            if (string.IsNullOrWhiteSpace(effect.EffectKey))
                throw new InvalidDataException("旧版效果键不能为空。");
            try
            {
                JsonDocument.Parse(effect.ParametersJson).Dispose();
            }
            catch (JsonException exception)
            {
                throw new InvalidDataException(
                    $"旧版效果 {effect.EffectKey} 的参数 JSON 无效：{exception.Message}");
            }
        }

        var stringIndexes = new HashSet<int>();
        foreach (CardStringRecord text in card.Strings)
        {
            if (text.Index < 0)
                throw new InvalidDataException("提示文本索引不能小于 0。");
            if (!stringIndexes.Add(text.Index))
                throw new InvalidDataException($"提示文本索引 {text.Index} 重复。");
        }
    }
}
