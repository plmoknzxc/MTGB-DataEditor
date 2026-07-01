# MTGB Data Editor

MTGB Data Editor 是用于编辑 MTGB `.mtgbdb` SQLite 卡牌数据库的 Windows 桌面工具。

它面向自制卡牌数据库维护场景，重点解决三件事：

- 编辑卡牌基础数据、规则文本和类别数值。
- 预览卡图并维护 Lua 脚本路径。
- 为 Lua 效果准备可复用提示文本和关联数据。

## 当前状态

- 目标平台：Windows
- UI 技术：WinForms / .NET 8
- 数据库：SQLite `.mtgbdb`
- 当前数据库 Schema：`2`
- 当前主要编辑视图：`BasicCardEditorView`

Schema v2 是破坏性版本要求。编辑器会拒绝打开 schema 不是 `2` 的数据库；旧数据库需要迁移或重新创建。

## 主要功能

### 数据库

- 打开 `.mtgbdb` 数据库。
- 新建 `.mtgbdb` 数据库。
- 校验当前数据库。
- 按卡名、系列、卡图编号或类型搜索卡牌。
- 新建、保存、删除卡牌。
- 保存前检查未保存的卡牌修改和 Lua 脚本修改。

### 卡牌基础信息

可编辑字段包括：

- 卡名
- 系列代号
- 卡图编号
- Oracle ID
- 关联特征
- 法术力费用
- 主类别
- 超类别
- 副类别
- 类别数值
- 规则文本
- 提示文本
- Lua 脚本路径

内部 `card_id` 仍由数据库和运行端使用，但普通界面不直接编辑它。新建卡牌时编辑器会自动生成唯一 ID。

### 卡牌类别

主类别使用可组合按钮：

- 神器
- 战役
- 生物
- 结界
- 瞬间
- 亲族
- 地
- 鹏洛客
- 法术

超类别使用可组合按钮：

- 基本
- 传奇
- 长效
- 雪境
- 普世

副类别目前是纯文本输入，例如：

```text
Human Soldier
Island
Adventure
```

当前只强制一条类别规则：`亲族/Kindred` 不能单独存在，必须同时具有另一种主类别。

### 类别数值

编辑器不会把生物、鹏洛客、战役设为互斥。复合类别会同时显示所需数值：

- 生物：力量、防御力
- 鹏洛客：初始忠诚
- 战役：布防值

保存时：

- 生物力量和防御力写入 `cards.power`、`cards.toughness`。
- 鹏洛客初始忠诚写入 `card_characteristics.loyalty`。
- 战役布防值写入 `card_characteristics.defense`。
- 只有战役、不是生物的卡牌，会把布防值镜像到旧的 `cards.toughness`，方便旧运行端兼容读取。

### 法术力费用

法术力费用仍使用文本格式，例如：

```text
{2}{U}
{G/W}
{2/W}
{W/P}
```

界面提供“插入符号”菜单，用于插入常用法术力符号类别。菜单只辅助输入，不会改变数据库存储格式。

### 卡图预览

选择卡牌后，编辑器会按系列代号和卡图编号查找卡图。

找不到卡图时，会显示默认卡背。

支持扩展名：

- `.png`
- `.jpg`
- `.jpeg`
- `.webp`

### Lua 脚本

编辑器内置 Lua 脚本页：

- 读取当前卡牌关联脚本。
- 脚本不存在时可提示创建。
- 保存脚本。
- 另存为脚本。
- 打开脚本目录。
- 行号显示。
- 基础语法高亮。
- Tab 缩进。
- `Ctrl+S` 保存。

脚本命名建议：

```text
m<卡图编号>.lua
```

例如：

```text
m65.lua
```

### 提示文本

提示文本用于为 Lua 效果准备可复用字符串。

界面中显示为表格：

- 标识
- Lua 索引
- 提示文本

`str1` 对应 Lua 索引 `0`，`str2` 对应 Lua 索引 `1`，依此类推。

### 关联特征 / multipart

Schema v2 新增 `multipart` 表：

```sql
multipart(card_key, child_key)
```

它用于记录一张卡牌和另一张卡牌之间的关联，例如：

- 冒险牌主体与冒险咒语
- 双面牌正反面
- 其它副特征

当前这是关联原型，不是完整双面牌系统：

- 当前 UI 只有“关联特征”输入框。
- 当前服务层只读取一个关联目标。
- 还没有正反面专用编辑 UI。
- 还没有关联面之间的一键跳转。
- 删除卡牌时 multipart 链清理仍待完善。
- 多关联、双向关系、one-to-many / many-to-many 仍待设计。

## 数据库结构

核心表：

```sql
metadata(key, value)
cards(...)
card_effects(...)
card_strings(card_key, string_index, text)
card_characteristics(card_key, loyalty, defense)
multipart(card_key, child_key)
```

`cards` 表在 schema v2 中包含：

```sql
supertype_flags INTEGER NOT NULL
subtypes TEXT
```

`card_effects` 是旧版效果数据表。新版界面不直接编辑它，但保存卡牌时会尽量保留已有记录，避免旧数据丢失。

## 资源目录

编辑器按 MTGB 根目录组织数据库、卡图和脚本。

推荐结构：

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

也兼容英文目录：

```text
MTGB/
  CardDatabases/
    cards.mtgbdb
  CardImages/
    SOS/
      65.png
  Scripts/
    SOS/
      m65.lua
```

MTGB 根目录判定规则：

- 如果 `.mtgbdb` 位于 `数据库`、`Databases`、`Database` 或 `CardDatabases` 中，则这些目录的上一级视为 MTGB 根目录。
- 卡图优先查找 `CardImages`，不存在时兼容 `卡图`。
- 脚本优先查找 `Scripts`，不存在时兼容 `脚本`。

卡图查找路径：

```text
卡图/<系列代号>/<卡图编号>.png
CardImages/<系列代号>/<卡图编号>.png
```

脚本默认路径：

```text
Scripts/<系列代号>/m<卡图编号>.lua
```

`script_path` 存储相对于 MTGB 根目录的路径。编辑器拒绝写入 MTGB 根目录之外的 Lua 文件。

## 从源码运行

需要：

- Windows 10 或更高版本
- .NET 8 Desktop Runtime / SDK

运行：

```powershell
dotnet run --project MTGB.CardDatabaseEditor.csproj
```

## 数据库校验

```powershell
dotnet run --project MTGB.CardDatabaseEditor.csproj -- --validate path\to\cards.mtgbdb
```

输出示例：

```text
schema=2 cards=10 characteristics=10 strings=4 legacy_effects=0
```

## 自检

```powershell
dotnet run --project MTGB.CardDatabaseEditor.csproj -- --self-test
```

成功时输出：

```text
self-test passed
```

## 重要限制

- 目前没有 v1 到 v2 的自动迁移逻辑。
- `multipart` 仍是原型实现。
- 双面牌目前只支持记录关联，不支持完整双面牌编辑流程。
- 副类别目前是纯文本，不做 i18n 或规则校验。
- 旧版 `card_effects` 不在新版 UI 中直接编辑。

## 字段说明

- `card_key`：由 `系列代号/卡图编号` 组成，例如 `SOS/65`。
- `card_id`：运行端内部唯一编号，编辑器新建卡牌时自动生成。
- `collector_number`：界面中的“卡图编号”，同时用于卡图和脚本文件名。
- `oracle_id`：同一规则卡牌不同印刷版本的共享标识；自制卡可以留空。
- `type_flags`：主类别位标记。
- `supertype_flags`：超类别位标记。
- `subtypes`：副类别文本。
- `multipart.child_key`：关联到的另一张卡牌 `card_key`。

