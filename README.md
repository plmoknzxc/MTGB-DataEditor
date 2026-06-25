# MTGB Data Editor

用于编辑 MTGB `.mtgbdb` SQLite 卡牌数据库的 Windows 桌面工具。

## 当前功能

- 打开和新建 `.mtgbdb` 数据库。
- 按卡名、系列、卡图编号和类型搜索卡牌；内部 ID 仍可用于搜索，但不在普通界面中显示或编辑。
- 编辑卡名、系列代号、卡图编号、Oracle ID、法术力费用、卡牌类型和规则文本。
- 卡牌类型使用可组合的切换按钮，不再使用原先的勾选列表。
- 根据所选类别同时显示对应数值：
  - 生物：力量、防御力
  - 鹏洛客：初始忠诚
  - 战役：布防值
  - 同时具有多个类别时，同时显示全部相关数值
- 用独立的“提示文本”页面编辑 `str1`、`str2` 等文本。
- 在程序内直接打开、编辑和保存 Lua 文件。
- Lua 编辑器包含行号、基础语法高亮、Tab 缩进和 `Ctrl+S` 保存。
- 切换卡牌、打开数据库或关闭程序前检查未保存的卡牌与 Lua 修改。
- 校验数据库中的卡牌、类别数值、提示文本和旧版效果记录。

## 界面调整

- 移除了 WinForms 原生 `TabControl`，改为深色页面导航，避免标签栏右侧出现白色区域。
- 卡牌标题区域改用独立的两行布局，避免“未选择卡牌”和系列信息被裁切。
- “启用此卡牌”已从基本信息页面移除；通过编辑器保存的卡牌会固定写入 `enabled = 1`。
- 基本信息页面可滚动，高 DPI 或较小窗口下不会因为固定高度而丢失字段。

## 数据兼容性

数据库 Schema 版本仍为 `1`。新版编辑器会在已有数据库中自动补充：

```sql
card_strings(card_key, string_index, text)
card_characteristics(card_key, loyalty, defense)
```

`card_characteristics` 用于保存鹏洛客的初始忠诚和战役的独立布防值。当前 Unity 加载代码不会读取这张表，因此本次修改不会要求改动 Unity，也不会影响现有显式查询。

为兼容当前 Unity 只读取 `power` 与 `toughness` 的行为：

- 普通生物继续把力量和防御力保存到 `power`、`toughness`。
- 只有战役、不是生物的卡牌，会同时把布防值镜像到旧的 `toughness` 列。
- 同时是生物与战役的卡牌会分别保存生物防御力和战役布防值；Unity 当前只会读到生物防御力，独立布防值要等以后获得许可再接入运行端。
- 鹏洛客初始忠诚当前也只由编辑器保存，Unity 尚未读取。

旧版 `card_effects` 表不会显示在新版界面里，但编辑器会在保存卡牌时原样保留已有记录，避免旧数据丢失。

## 类型组合

编辑器不会强制把生物、鹏洛客、战役等类别设为互斥。选择多个类别时，会显示这些类别各自需要的全部数值。

当前只强制一条明确规则：`亲族/Kindred` 不能单独存在，必须同时具有另一种卡牌类型。

## 资源目录

编辑器按 MTGB 根目录组织数据库、卡图和脚本。如果 `.mtgbdb` 位于 `MTGB/数据库/`，则 MTGB 根目录是 `数据库` 的上一级目录：

```text
MTGB/
  数据库/
    cards.mtgbdb
  卡图/
    SOS/
      65.png
  脚本/
    SOS/
      m65.lua
```

- 卡图路径按 `卡图/<系列代号>/<卡图编号>.(png|jpg|jpeg|webp)` 查找。
- `script_path` 是相对于 MTGB 根目录的路径，例如 `脚本/SOS/m65.lua`。
- 新建脚本默认保存到 `脚本/<系列代号>/m<卡图编号>.lua`。
- 编辑器拒绝写入 MTGB 根目录之外的 Lua 文件。

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

## 字段说明

- `card_id` 是运行端使用的内部唯一编号。编辑器会保留旧值，并在新建卡牌时自动生成。
- “卡图编号”对应数据库的 `collector_number`，同时用于 `卡图/<系列代号>/<卡图编号>.png` 和 `脚本/<系列代号>/m<卡图编号>.lua`。
- Oracle ID 用来关联同一张规则卡牌的不同印刷版本；自制卡可以留空。
