namespace MTGB.CardDatabaseEditor;

internal static class Program
{
    [STAThread]
    private static int Main(string[] args)
    {
        if (args.Length > 0 && args[0].Equals("--self-test", StringComparison.OrdinalIgnoreCase))
            return RunSelfTest();

        if (args.Length >= 2 && args[0].Equals("--validate", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                using var database = new CardDatabaseService(args[1]);
                DatabaseValidationResult result = database.ValidateDatabase();
                Console.WriteLine(
                    $"schema={result.SchemaVersion} cards={result.CardCount} " +
                    $"strings={result.StringCount} legacy_effects={result.EffectCount}");
                foreach (string error in result.Errors)
                    Console.Error.WriteLine(error);
                return result.IsValid ? 0 : 2;
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.Message);
                return 1;
            }
        }

        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm(ResolveInitialDatabase(args)));
        return 0;
    }

    private static int RunSelfTest()
    {
        string path = Path.Combine(Path.GetTempPath(), $"mtgb-card-editor-{Guid.NewGuid():N}.mtgbdb");
        try
        {
            CardDatabaseService.Create(path, Path.Combine(AppContext.BaseDirectory, "schema.sql"));
            using var database = new CardDatabaseService(path);
            var card = new CardRecord
            {
                CardId = 900001,
                OracleId = "self-test-oracle",
                SetCode = "TST",
                CollectorNumber = "45a",
                Name = "Editor Self Test",
                Types = CardTypeFlags.Kindred | CardTypeFlags.Sorcery,
                ManaCost = "{2}{U}",
                RulesText = "Draw a card.",
                Enabled = true
            };
            card.Effects.Add(new CardEffectRecord
            {
                Order = 0,
                Trigger = "on_play",
                EffectKey = "Draw1",
                ParametersJson = "{}"
            });
            card.Strings.Add(new CardStringRecord { Index = 0, Text = "抽一张牌" });
            card.Strings.Add(new CardStringRecord { Index = 1, Text = "选择一个目标" });
            database.Save(card);

            CardRecord loaded = database.LoadCard("TST/45a");
            if (loaded.Name != card.Name || loaded.Types != card.Types || loaded.Effects.Count != 1 || loaded.Strings.Count != 2)
                throw new InvalidOperationException("Saved card did not round-trip correctly.");

            loaded.Name = "Editor Self Test Updated";
            loaded.SetCode = "TS2";
            database.Save(loaded);
            CardRecord renamed = database.LoadCard("TS2/45a");
            if (renamed.Effects.Count != 1 || renamed.Strings.Count != 2)
                throw new InvalidOperationException("Child records were not preserved after card key update.");

            DatabaseValidationResult validation = database.ValidateDatabase();
            if (!validation.IsValid || validation.CardCount != 1 || validation.EffectCount != 1 || validation.StringCount != 2)
                throw new InvalidOperationException("Database validation failed after update.");

            database.Delete("TS2/45a");
            if (database.LoadSummaries().Count != 0)
                throw new InvalidOperationException("Card deletion failed.");

            Console.WriteLine("self-test passed");
            return 0;
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine(exception);
            return 1;
        }
        finally
        {
            try
            {
                if (File.Exists(path)) File.Delete(path);
            }
            catch
            {
                // Ignore cleanup errors in the self-test.
            }
        }
    }

    private static string? ResolveInitialDatabase(string[] args)
    {
        if (args.Length > 0 && File.Exists(args[0]))
            return Path.GetFullPath(args[0]);

        DirectoryInfo? directory = new(AppContext.BaseDirectory);
        while (directory != null)
        {
            string candidate = Path.Combine(directory.FullName, "CardDatabases", "core.mtgbdb");
            if (File.Exists(candidate)) return candidate;
            directory = directory.Parent;
        }
        return null;
    }
}
