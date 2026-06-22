# MTGB Data Editor

用于编辑 MTGB `.mtgbdb` SQLite 卡牌数据库的 Windows 桌面工具。

## 当前功能

- 打开和新建 `.mtgbdb` 数据库。
- 按名称、内部 ID、系列、收藏编号和类型搜索卡牌。
- 编辑卡牌基础信息、类型、法术力费用、规则文本、力量、防御和启用状态。
- 用独立的“提示文本”页面编辑 `str1`、`str2` 等文本。
- 在程序内直接打开、编辑和保存 Lua 文件。
- Lua 编辑器包含行号、基础语法高亮、Tab 缩进和 `Ctrl+S` 保存。
- 切换卡牌、打开数据库或关闭程序前检查未保存的卡牌与 Lua 修改。
- 校验数据库中的卡牌、提示文本和旧版效果记录。

## 数据兼容性

数据库 Schema 版本仍为 `1`。新版编辑器会在已有数据库中自动补充：

```sql
card_strings(card_key, string_index, text)
```

Unity 当前可以忽略这张额外的数据表。提示文本目前只会由数据编辑器保存，Unity 运行端尚未读取它。

旧版 `card_effects` 表不会显示在新版界面里，但编辑器会在保存卡牌时原样保留已有记录，避免旧数据丢失。

## Lua 路径

这一版本没有修改 MTGB Unity 项目，因此继续遵守当前运行端规则：

- `script_path` 是相对于当前 `.mtgbdb` 所在目录的路径。
- 编辑器拒绝写入数据库目录之外的 Lua 文件。
- Lua 文件名没有固定规则，也不会自动按照卡片 ID 命名。

独立 `CardScripts` 根目录需要 Unity 端配合修改后才能启用，本仓库目前没有执行该修改。

## 环境

- Windows 10 或更高版本
- .NET 8 Desktop Runtime / SDK

## 从源码运行

```powershell
dotnet run --project MTGB.CardDatabaseEditor.csproj
```

## 数据库校验

```powershell
dotnet run --project MTGB.CardDatabaseEditor.csproj -- --validate path/to/cards.mtgbdb
```

## 自检

```powershell
dotnet run --project MTGB.CardDatabaseEditor.csproj -- --self-test
```
