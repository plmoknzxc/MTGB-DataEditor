# Build notes

## 当前版本重点

- 数据库 Schema 版本为 `2`。
- `cards` 表包含主类别、超类别和副类别字段：
  - `type_flags`
  - `supertype_flags`
  - `subtypes`
- `card_characteristics` 保存鹏洛客初始忠诚和战役布防值。
- `card_strings` 保存 Lua 可复用提示文本。
- `multipart` 保存卡牌之间的关联关系，目前仍是原型实现。

## 本地检查命令

```powershell
dotnet clean MTGB.CardDatabaseEditor.csproj
dotnet build MTGB.CardDatabaseEditor.csproj
dotnet run --project MTGB.CardDatabaseEditor.csproj -- --self-test
dotnet run --project MTGB.CardDatabaseEditor.csproj
```

`--self-test` 成功时应显示：

```text
self-test passed
```

## 需要重点人工验证的界面

1. 打开数据库后左侧卡牌列表能正常加载。
2. 卡图预览能加载 `CardImages/系列/卡图编号.png` 或 `卡图/系列/卡图编号.png`。
3. 找不到卡图时显示默认卡背。
4. “卡牌标识”区域显示卡名、系列代号、卡图编号、Oracle ID 和关联特征。
5. “卡牌类型”区域能选择主类别和超类别。
6. 副类别文本能保存并重新读取。
7. “数值 / 法术力”区域能保存法术力、力量、防御力、初始忠诚和布防值。
8. “插入符号”菜单能把 `{W}`、`{2/W}` 等符号插入法术力文本框。
9. “规则文本”输入框能完整显示并保存长文本。
10. “提示文本”表格能新增、删除、上移、下移并保存。
11. Lua 脚本页能创建、加载、保存脚本。

## 数据库兼容性注意

- 当前编辑器要求 `metadata.schema_version = 2`。
- 旧版 v1 数据库不会自动迁移。
- 如果要继续使用旧数据库，需要先写迁移脚本或重新创建数据库。
- `multipart` 当前只保存一个关联目标；双面牌、冒险牌和副特征的完整工作流仍需继续设计。

## 不应提交的文件

不要提交以下本地生成内容：

```text
bin/
obj/
.vs/
*.user
custom.mtgbdb
commitLog.txt
changelog.txt
```

