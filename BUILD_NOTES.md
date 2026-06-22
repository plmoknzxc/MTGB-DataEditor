# Build notes

## 本次完成的检查

- 对全部 C# 文件执行了字符串、注释、括号和大括号结构检查，未发现未闭合结构。
- 使用 SQLite 实际执行了 `schema.sql`，确认 `card_characteristics` 可以创建、写入和读取。
- 确认增加编辑器专用数据表后，当前 Unity 使用的 `cards` 显式查询仍然有效。
- 自检代码已扩展为验证力量、防御力、初始忠诚和布防值的保存、读取及卡牌键更新。
- 没有修改 MTGB Unity 项目的任何文件。

## 仍需在 Windows 上验证

当前环境没有 .NET 8 Windows Desktop SDK，也不能启动 WinForms 窗口，因此这一版尚未在此处实际编译或目视检查。

请在 Windows 项目目录运行：

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

建议先使用数据库副本检查以下内容：

1. 顶部页面导航没有白色区域。
2. “未选择卡牌”及其说明完整显示。
3. 同时选择生物、鹏洛客和战役时，力量、防御力、初始忠诚和布防值全部出现。
4. 保存并重新打开后，初始忠诚和布防值仍然存在。
