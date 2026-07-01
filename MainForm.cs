using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace MTGB.CardDatabaseEditor;

internal sealed partial class MainForm : Form
{
    private readonly BasicCardEditorView basicEditorView = new();
    private readonly TextBox searchBox = new();
    private readonly ListView cardList = new();
    private readonly Label cardCountLabel = new();
    private readonly Label cardHeaderTitle = new();
    private readonly Label cardHeaderMeta = new();
    private readonly Panel editorPageHost = new();
    private readonly List<Control> editorPages = new();
    private readonly List<Button> editorPageButtons = new();
    private int selectedEditorPage;

    private readonly NumericUpDown cardIdInput = new();
    private readonly TextBox scriptPathInput = new();
    private readonly Label scriptStatusLabel = new();
    private readonly LuaCodeEditor luaEditor = new();

    private readonly ToolStrip toolStrip = new();
    private readonly ToolStripStatusLabel statusLabel = new();
    private readonly SplitContainer mainSplitContainer = new();
    private readonly StatusStrip statusStrip = new();

    private CardDatabaseService? database;
    private List<CardSummary> summaries = new();
    private readonly List<CardEffectRecord> preservedEffects = new();
    private string? currentCardKey;
    private string? loadedScriptFullPath;
    private string? scriptCreatePromptDismissedForCardKey;
    private Image? currentCardPreviewImage;
    private bool cardDirty;
    private bool scriptDirty;
    private bool loading;
    private bool suppressSelection;
    private PictureBox cardImagePreview =>
    basicEditorView.CardImagePreview;

    private Label cardImageStatusLabel =>
        basicEditorView.CardImageStatusLabel;

    private TextBox cardNameInput =>
        basicEditorView.CardNameInput;

    private TextBox oracleIdInput =>
        basicEditorView.OracleIdInput;

    private TextBox multipartInput =>
        basicEditorView.MultipartInput;

    private TextBox setCodeInput =>
        basicEditorView.SetCodeInput;

    private TextBox collectorInput =>
        basicEditorView.CollectorInput;

    private TextBox manaCostInput =>
        basicEditorView.ManaCostInput;

    private TextBox subtypesInput =>
        basicEditorView.SubtypesInput;

    private NumericUpDown powerInput =>
        basicEditorView.PowerInput;

    private NumericUpDown toughnessInput =>
        basicEditorView.ToughnessInput;

    private NumericUpDown loyaltyInput =>
        basicEditorView.LoyaltyInput;

    private NumericUpDown defenseInput =>
        basicEditorView.DefenseInput;

    private TextBox rulesTextInput =>
        basicEditorView.RulesTextInput;

    private DataGridView stringsGrid =>
        basicEditorView.StringsGrid;


    public MainForm() : this(null)
    {
    }

    public MainForm(string? initialDatabase)
    {
        InitializeComponent();

        EditorTheme.Apply(this);
        ApplyControlSpecificTheme();

        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
        {
            UpdateStatus("设计器预览：运行时会加载数据库、卡图和脚本。", false);
            return;
        }

        HookEvents();

        if (initialDatabase != null)
            OpenDatabase(initialDatabase);
        else
        {
            ClearForm();
            UpdateStatus("打开或新建一个 .mtgbdb 数据库。", false);
        }
    }

    private void InitializeComponent()
    {
        Text = "MTGB 卡牌数据编辑器";
        MinimumSize = new Size(1180, 760);
        Size = new Size(1480, 920);
        StartPosition = FormStartPosition.CenterScreen;
        AutoScaleMode = AutoScaleMode.Dpi;
        WindowState = FormWindowState.Normal;
        Font = new Font("Microsoft YaHei UI", 9.8f);
        BackColor = EditorTheme.Window;
        ForeColor = EditorTheme.Text;
        KeyPreview = true;

        mainSplitContainer.Dock = DockStyle.Fill;
        mainSplitContainer.FixedPanel = FixedPanel.Panel1;
        // SplitContainer validates SplitterDistance against its current Size.
        // Give it a realistic design-time size before applying the panel limits;
        // otherwise the default 150px width can make the form crash at startup.
        mainSplitContainer.Size = new Size(1200, 700);
        mainSplitContainer.SplitterDistance = 350;
        mainSplitContainer.Panel1MinSize = 270;
        mainSplitContainer.Panel2MinSize = 650;
        mainSplitContainer.SplitterWidth = 6;
        mainSplitContainer.Panel1.Padding = new Padding(12);
        mainSplitContainer.Panel2.Padding = new Padding(12);

        Controls.Add(mainSplitContainer);
        Controls.Add(statusStrip);
        Controls.Add(toolStrip);

        BuildUi();
    }
    private void BuildUi()
    {
        BuildToolbar();

        statusStrip.Items.Clear();
        statusStrip.BackColor = EditorTheme.Surface;
        statusStrip.ForeColor = EditorTheme.Muted;
        statusStrip.Font = new Font("Microsoft YaHei UI", 9.2f);
        statusStrip.SizingGrip = false;
        statusStrip.Renderer = new ToolStripProfessionalRenderer(new DarkColorTable());
        statusStrip.Items.Add(statusLabel);

        mainSplitContainer.Panel1.Controls.Clear();
        mainSplitContainer.Panel2.Controls.Clear();
        mainSplitContainer.Panel1.BackColor = EditorTheme.Window;
        mainSplitContainer.Panel2.BackColor = EditorTheme.Window;
        mainSplitContainer.BackColor = EditorTheme.Border;
        mainSplitContainer.Panel1.Controls.Add(BuildCardBrowser());
        mainSplitContainer.Panel2.Controls.Add(BuildEditor());
    }

    private void BuildToolbar()
    {
        toolStrip.Items.Clear();
        toolStrip.Dock = DockStyle.Top;
        toolStrip.GripStyle = ToolStripGripStyle.Hidden;
        toolStrip.Padding = new Padding(10, 6, 10, 6);
        toolStrip.AutoSize = false;
        toolStrip.Height = 46;
        toolStrip.BackColor = EditorTheme.Surface;
        toolStrip.ForeColor = EditorTheme.Text;
        toolStrip.Renderer = new ToolStripProfessionalRenderer(new DarkColorTable());

        toolStrip.Items.Add(CreateToolButton("打开数据库", OpenDatabaseDialog));
        toolStrip.Items.Add(CreateToolButton("新建数据库", CreateDatabaseDialog));
        toolStrip.Items.Add(new ToolStripSeparator());
        toolStrip.Items.Add(CreateToolButton("新建卡牌", NewCard));
        toolStrip.Items.Add(CreateToolButton("保存全部", () => SaveAll(), accent: true));
        toolStrip.Items.Add(CreateToolButton("删除卡牌", DeleteCurrentCard, danger: true));
        toolStrip.Items.Add(new ToolStripSeparator());
        toolStrip.Items.Add(CreateToolButton("校验数据库", ValidateCurrentDatabase));
    }

    private Control BuildCardBrowser()
    {
        var shell = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = EditorTheme.Surface,
            Padding = new Padding(12)
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            BackColor = EditorTheme.Surface
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 54));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 46));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 34));

        var header = new Panel { Dock = DockStyle.Fill, BackColor = EditorTheme.Surface };
        var title = new Label
        {
            Text = "卡牌库",
            Dock = DockStyle.Left,
            AutoSize = false,
            Width = 160,
            Font = new Font("Microsoft YaHei UI", 13f, FontStyle.Bold),
            ForeColor = EditorTheme.Text,
            TextAlign = ContentAlignment.MiddleLeft
        };
        cardCountLabel.Dock = DockStyle.Right;
        cardCountLabel.Width = 112;
        cardCountLabel.ForeColor = EditorTheme.Muted;
        cardCountLabel.TextAlign = ContentAlignment.MiddleRight;
        header.Controls.Add(cardCountLabel);
        header.Controls.Add(title);

        searchBox.Dock = DockStyle.Fill;
        searchBox.PlaceholderText = "搜索卡名、系列、卡图编号或类型";
        searchBox.Margin = new Padding(0, 5, 0, 7);

        cardList.Dock = DockStyle.Fill;
        cardList.View = View.Details;
        cardList.FullRowSelect = true;
        cardList.HideSelection = false;
        cardList.MultiSelect = false;
        cardList.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        cardList.OwnerDraw = true;
        cardList.Columns.Add("名称", 160);
        cardList.Columns.Add("系列 / 卡图编号", 110);

        var footer = new Label
        {
            Dock = DockStyle.Fill,
            Text = "Ctrl+S 保存　Ctrl+N 新建卡牌",
            ForeColor = EditorTheme.Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            Font = new Font("Microsoft YaHei UI", 8.6f)
        };

        layout.Controls.Add(header, 0, 0);
        layout.Controls.Add(searchBox, 0, 1);
        layout.Controls.Add(cardList, 0, 2);
        layout.Controls.Add(footer, 0, 3);
        shell.Controls.Add(layout);
        return shell;
    }

    private Control BuildEditor()
    {
        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = EditorTheme.Window
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var navigation = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = EditorTheme.Surface,
            Padding = new Padding(8, 6, 8, 6),
            Margin = new Padding(0)
        };
        var navigationButtons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = EditorTheme.Surface,
            Margin = new Padding(0),
            Padding = new Padding(0)
        };
        navigation.Controls.Add(navigationButtons);

        editorPageHost.Dock = DockStyle.Fill;
        editorPageHost.BackColor = EditorTheme.Window;
        editorPageHost.Padding = new Padding(0);

        editorPages.Clear();
        editorPageButtons.Clear();
        editorPages.Add(basicEditorView);
        editorPages.Add(BuildLuaTab());

        string[] pageNames = { "基本信息", "Lua 脚本" };
        for (int i = 0; i < editorPages.Count; i++)
        {
            int pageIndex = i;
            Control page = editorPages[i];
            page.Dock = DockStyle.Fill;
            page.Visible = false;
            editorPageHost.Controls.Add(page);

            Button button = CreateEditorPageButton(pageNames[i], () => ShowEditorPage(pageIndex));
            editorPageButtons.Add(button);
            navigationButtons.Controls.Add(button);
        }

        layout.Controls.Add(navigation, 0, 0);
        layout.Controls.Add(editorPageHost, 0, 1);
        ShowEditorPage(0);
        return layout;
    }

    private Control BuildLuaTab()
    {
        var tab = new Panel
        {
            BackColor = EditorTheme.Window,
            Padding = new Padding(12)
        };

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            BackColor = EditorTheme.Window
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 158));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var pathPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = EditorTheme.Surface,
            Padding = new Padding(14, 10, 12, 10)
        };
        var pathLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 4,
            BackColor = EditorTheme.Surface
        };
        pathLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
        pathLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        pathLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        pathLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        pathLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        pathLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

        var pathLabel = new Label
        {
            Text = "脚本路径",
            Dock = DockStyle.Fill,
            ForeColor = EditorTheme.Muted,
            TextAlign = ContentAlignment.MiddleLeft
        };
        scriptPathInput.Dock = DockStyle.Fill;
        scriptPathInput.Margin = new Padding(0, 5, 0, 5);
        scriptPathInput.PlaceholderText = "相对于 MTGB 根目录，例如 脚本/SOS/m65.lua";

        var buttons = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.LeftToRight,
            WrapContents = false,
            BackColor = EditorTheme.Surface,
            Padding = new Padding(0, 4, 0, 0),
            Margin = new Padding(0)
        };
        buttons.Controls.Add(CreateButton("选择", BrowseExistingScript, 72));
        buttons.Controls.Add(CreateButton("新建", CreateNewScript, 72));
        buttons.Controls.Add(CreateButton("重新加载", ReloadScript, 92));
        buttons.Controls.Add(CreateButton("保存脚本", () => SaveScriptFile(), 96, accent: true));
        buttons.Controls.Add(CreateButton("打开目录", OpenScriptFolder, 92));

        scriptStatusLabel.Dock = DockStyle.Fill;
        scriptStatusLabel.ForeColor = EditorTheme.Muted;
        scriptStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
        scriptStatusLabel.AutoEllipsis = true;

        var runtimeNote = new Label
        {
            Dock = DockStyle.Fill,
            Text = "脚本按 MTGB/脚本/<系列>/m<卡图编号>.lua 保存，script_path 记录相对 MTGB 根目录的路径。",
            ForeColor = EditorTheme.Muted,
            Font = new Font("Microsoft YaHei UI", 8.6f),
            TextAlign = ContentAlignment.MiddleLeft,
            AutoEllipsis = false
        };

        pathLayout.Controls.Add(pathLabel, 0, 0);
        pathLayout.Controls.Add(scriptPathInput, 1, 0);
        pathLayout.Controls.Add(scriptStatusLabel, 1, 1);
        pathLayout.Controls.Add(buttons, 1, 2);
        pathLayout.Controls.Add(runtimeNote, 0, 3);
        pathLayout.SetColumnSpan(runtimeNote, 2);
        pathPanel.Controls.Add(pathLayout);

        luaEditor.Dock = DockStyle.Fill;
        layout.Controls.Add(pathPanel, 0, 0);
        layout.Controls.Add(luaEditor, 0, 1);
        tab.Controls.Add(layout);
        return tab;
    }

    private void HookEvents()
    {
        basicEditorView.ViewChanged +=
        (_, _) => MarkCardDirty();

        basicEditorView.AddPromptRequested +=
            (_, _) => AddPromptString();

        basicEditorView.DeletePromptRequested +=
            (_, _) => DeleteSelectedPromptString();

        basicEditorView.MovePromptRequested +=
            direction => MovePromptString(direction);

        searchBox.TextChanged +=
            (_, _) => RefreshCardList(currentCardKey);

        cardList.SelectedIndexChanged +=
            (_, _) => CardSelectionChanged();

        cardList.DoubleClick +=
            (_, _) => ShowEditorPage(0);

        cardList.DrawColumnHeader += DrawCardListHeader;
        cardList.DrawItem += (_, e) =>
        {
            if (cardList.View != View.Details) e.DrawDefault = true;
        };
        cardList.DrawSubItem += DrawCardListSubItem;
        cardList.Resize += (_, _) => ResizeCardListColumns();

        foreach (TextBox input in new[]
                 {
                     cardNameInput,
                     oracleIdInput,
                     multipartInput,
                     setCodeInput,
                     collectorInput,
                     manaCostInput,
                     rulesTextInput,
                     subtypesInput
                 })
            input.TextChanged += (_, _) => MarkCardDirty();

        cardNameInput.TextChanged += (_, _) => UpdateCardHeader();
        setCodeInput.TextChanged += (_, _) =>
        {
            UpdateCardHeader();
            UpdateCardImagePreview();
        };
        collectorInput.TextChanged += (_, _) =>
        {
            UpdateCardHeader();
            UpdateCardImagePreview();
        };

        scriptPathInput.TextChanged += (_, _) =>
        {
            MarkCardDirty();
            UpdateScriptPathStatus();
        };

        foreach (NumericUpDown input in new[] { powerInput, toughnessInput, loyaltyInput, defenseInput })
            input.ValueChanged += (_, _) => MarkCardDirty();

        stringsGrid.CellValueChanged += (_, _) => MarkCardDirty();
        stringsGrid.KeyDown += (_, e) =>
        {
            if (e.KeyCode == Keys.Delete)
            {
                e.SuppressKeyPress = true;
                DeleteSelectedPromptString();
            }
        };

        luaEditor.CodeChanged += (_, _) => MarkScriptDirty();
        luaEditor.SaveRequested += (_, _) => SaveScriptFile();

        KeyDown += MainFormKeyDown;
        FormClosing += (_, eventArgs) =>
        {
            if (!ConfirmDiscardOrSave())
                eventArgs.Cancel = true;
            else
            {
                currentCardPreviewImage?.Dispose();
                database?.Dispose();
            }
        };
    }

    private void ApplyControlSpecificTheme()
    {
        toolStrip.BackColor = EditorTheme.Surface;
        EditorTheme.StyleGrid(stringsGrid);
        cardList.BackColor = EditorTheme.Input;
        cardList.ForeColor = EditorTheme.Text;
        scriptStatusLabel.ForeColor = EditorTheme.Muted;
        UpdateEditorPageButtons();
    }

    private void OpenDatabaseDialog()
    {
        if (!ConfirmDiscardOrSave()) return;
        using var dialog = new OpenFileDialog
        {
            Filter = "MTGB 卡牌数据库 (*.mtgbdb)|*.mtgbdb|所有文件 (*.*)|*.*",
            CheckFileExists = true
        };
        if (dialog.ShowDialog(this) == DialogResult.OK)
            OpenDatabase(dialog.FileName);
    }

    private void CreateDatabaseDialog()
    {
        if (!ConfirmDiscardOrSave()) return;
        using var dialog = new SaveFileDialog
        {
            Filter = "MTGB 卡牌数据库 (*.mtgbdb)|*.mtgbdb",
            DefaultExt = "mtgbdb",
            AddExtension = true,
            FileName = "custom.mtgbdb"
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        try
        {
            string schemaPath = Path.Combine(AppContext.BaseDirectory, "schema.sql");
            CardDatabaseService.Create(dialog.FileName, schemaPath);
            OpenDatabase(dialog.FileName);
        }
        catch (Exception exception)
        {
            ShowError("无法创建数据库", exception);
        }
    }

    private void OpenDatabase(string path)
    {
        try
        {
            var opened = new CardDatabaseService(path);
            database?.Dispose();
            database = opened;
            currentCardKey = null;
            loadedScriptFullPath = null;
            cardDirty = false;
            scriptDirty = false;
            LoadSummaries();

            if (summaries.Count > 0)
            {
                LoadCard(opened.LoadCard(summaries[0].CardKey));
                RefreshCardList(currentCardKey);
            }
            else
            {
                ClearForm();
            }

            UpdateStatus($"已打开 {path}", false);
        }
        catch (Exception exception)
        {
            ShowError("无法打开数据库", exception);
        }
    }

    private void LoadSummaries(string? selectKey = null)
    {
        summaries = database?.LoadSummaries() ?? new List<CardSummary>();
        RefreshCardList(selectKey);
    }

    private void RefreshCardList(string? selectKey = null)
    {
        string filter = searchBox.Text.Trim();
        int visibleCount = 0;
        suppressSelection = true;
        cardList.BeginUpdate();
        cardList.Items.Clear();

        foreach (CardSummary summary in summaries)
        {
            if (!Matches(summary, filter)) continue;
            visibleCount++;
            var item = new ListViewItem(summary.Name) { Tag = summary };
            item.SubItems.Add($"{summary.SetCode}/{summary.CollectorNumber}");
            cardList.Items.Add(item);
            if (selectKey != null && summary.CardKey.Equals(selectKey, StringComparison.OrdinalIgnoreCase))
            {
                item.Selected = true;
                item.Focused = true;
                item.EnsureVisible();
            }
        }

        cardList.EndUpdate();
        ResizeCardListColumns();
        suppressSelection = false;
        cardCountLabel.Text = filter.Length == 0
            ? $"{summaries.Count} 张"
            : $"{visibleCount}/{summaries.Count}";
        cardList.Invalidate();
    }

    private void CardSelectionChanged()
    {
        if (suppressSelection || cardList.SelectedItems.Count == 0 || database == null) return;
        if (cardList.SelectedItems[0].Tag is not CardSummary summary) return;
        if (summary.CardKey == currentCardKey) return;

        if (!ConfirmDiscardOrSave())
        {
            RefreshCardList(currentCardKey);
            return;
        }

        try
        {
            LoadCard(database.LoadCard(summary.CardKey));
        }
        catch (Exception exception)
        {
            ShowError("无法读取卡牌", exception);
        }
    }

    private void LoadCard(CardRecord card)
    {
        loading = true;
        currentCardKey = card.OriginalCardKey;
        cardIdInput.Value = Math.Clamp(card.CardId, 1, int.MaxValue);
        cardNameInput.Text = card.Name;
        oracleIdInput.Text = card.OracleId;
        setCodeInput.Text = card.SetCode;
        collectorInput.Text = card.CollectorNumber;
        multipartInput.Text = card.MultipartID;
        manaCostInput.Text = card.ManaCost;
        subtypesInput.Text = card.Subtypes;
        powerInput.Value = Math.Clamp(card.Power, -999, 999);
        toughnessInput.Value = Math.Clamp(card.Toughness, -999, 999);
        loyaltyInput.Value = Math.Clamp(card.Loyalty, -999, 999);
        defenseInput.Value = Math.Clamp(card.Defense, -999, 999);
        rulesTextInput.Text = card.RulesText;
        scriptPathInput.Text = card.ScriptPath;

        basicEditorView.SelectedTypes = card.Types;
        basicEditorView.SelectedSupertypes = card.Supertypes;

        preservedEffects.Clear();
        preservedEffects.AddRange(card.Effects.Select(effect => effect.Clone()));

        stringsGrid.Rows.Clear();
        foreach (CardStringRecord text in card.Strings.OrderBy(text => text.Index))
            stringsGrid.Rows.Add($"str{text.Index + 1}", text.Index, text.Text);

        loading = false;
        cardDirty = false;
        scriptCreatePromptDismissedForCardKey = null;
        UpdateCardHeader();
        UpdateCardImagePreview();
        LoadScriptFromCurrentPath(silent: true);
        UpdateStatus($"正在编辑 {card.CardKey}", false);
    }

    private void NewCard()
    {
        if (database == null)
        {
            MessageBox.Show(this, "请先打开数据库。", "没有数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ConfirmDiscardOrSave()) return;

        currentCardKey = null;
        suppressSelection = true;
        cardList.SelectedItems.Clear();
        suppressSelection = false;
        ClearForm();

        loading = true;
        cardIdInput.Value = summaries.Count == 0
            ? 1
            : Math.Min(int.MaxValue, summaries.Max(card => card.CardId) + 1L);
        setCodeInput.Text = "CUSTOM";
        collectorInput.Text = NextCollectorNumber();
        basicEditorView.SelectedTypes = CardTypeFlags.Creature;
        loading = false;

        cardDirty = true;
        cardNameInput.Focus();
        UpdateCardHeader();
        UpdateStatus("新卡牌尚未保存。", true);
    }

    private bool SaveAll()
    {
        if (database == null) return false;
        if (scriptDirty && !SaveScriptFile()) return false;
        if (cardDirty && !SaveCurrentCard()) return false;
        UpdateStatus(currentCardKey == null ? "没有需要保存的内容。" : $"已保存 {currentCardKey}", false);
        return true;
    }

    private bool SaveCurrentCard()
    {
        if (database == null) return false;

        try
        {
            CardRecord card = ReadForm();
            database.Save(card);
            currentCardKey = card.OriginalCardKey;
            cardDirty = false;
            LoadSummaries(currentCardKey);
            UpdateCardHeader();
            UpdateStatus($"已保存卡牌 {currentCardKey}", scriptDirty);
            return true;
        }
        catch (Exception exception)
        {
            ShowError("无法保存卡牌", exception);
            return false;
        }
    }

    private void DeleteCurrentCard()
    {
        if (database == null || currentCardKey == null) return;
        if (!ConfirmDiscardOrSave()) return;
        if (MessageBox.Show(
                this,
                $"确定删除 {currentCardKey}？\n此操作不会删除对应的 Lua 文件。",
                "删除卡牌",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
            return;

        try
        {
            database.Delete(currentCardKey);
            currentCardKey = null;
            cardDirty = false;
            scriptDirty = false;
            LoadSummaries();
            ClearForm();
            UpdateStatus("卡牌已删除。", false);
        }
        catch (Exception exception)
        {
            ShowError("无法删除卡牌", exception);
        }
    }

    private CardRecord ReadForm()
    {
        CardTypeFlags types = ReadTypes();
        bool isCreature = types.HasFlag(CardTypeFlags.Creature);
        bool isPlaneswalker = types.HasFlag(CardTypeFlags.Planeswalker);
        bool isBattle = types.HasFlag(CardTypeFlags.Battle);
        var card = new CardRecord
        {
            OriginalCardKey = currentCardKey,
            CardId = decimal.ToInt32(cardIdInput.Value),
            Name = cardNameInput.Text,
            OracleId = oracleIdInput.Text,
            SetCode = setCodeInput.Text,
            CollectorNumber = collectorInput.Text,
            ManaCost = manaCostInput.Text,
            Power = isCreature ? decimal.ToInt32(powerInput.Value) : 0,
            // Keep legacy battle-only databases compatible with the current Unity reader,
            // which still reads its generic defence value from the toughness column.
            Toughness = isCreature
                ? decimal.ToInt32(toughnessInput.Value)
                : isBattle ? decimal.ToInt32(defenseInput.Value) : 0,
            Loyalty = isPlaneswalker ? decimal.ToInt32(loyaltyInput.Value) : 0,
            Defense = isBattle ? decimal.ToInt32(defenseInput.Value) : 0,
            ScriptPath = scriptPathInput.Text.Replace('\\', '/'),
            RulesText = rulesTextInput.Text,
            Enabled = true,
            Types = types,
            Supertypes = ReadSupertypes(),
            Subtypes = subtypesInput.Text,
            MultipartID = multipartInput.Text
        };

        card.Effects.AddRange(preservedEffects.Select(effect => effect.Clone()));
        for (int i = 0; i < stringsGrid.Rows.Count; i++)
        {
            string text = Convert.ToString(stringsGrid.Rows[i].Cells["StringText"].Value) ?? string.Empty;
            card.Strings.Add(new CardStringRecord { Index = i, Text = text });
        }
        return card;
    }

    private void ValidateCurrentDatabase()
    {
        if (database == null) return;
        try
        {
            DatabaseValidationResult result = database.ValidateDatabase();
            string message =
                $"Schema：{result.SchemaVersion}\n" +
                $"卡牌：{result.CardCount}\n" +
                $"类别数值：{result.CharacteristicCount}\n" +
                $"提示文本：{result.StringCount}\n" +
                $"旧版效果记录：{result.EffectCount}";
            if (result.Errors.Count > 0)
                message += "\n\n" + string.Join("\n", result.Errors.Take(12));

            MessageBox.Show(
                this,
                message,
                result.IsValid ? "数据库有效" : "数据库存在问题",
                MessageBoxButtons.OK,
                result.IsValid ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
        }
        catch (Exception exception)
        {
            ShowError("无法校验数据库", exception);
        }
    }

    private bool ConfirmDiscardOrSave()
    {
        if (!cardDirty && !scriptDirty) return true;

        string content = cardDirty && scriptDirty
            ? "卡牌数据和 Lua 脚本都有未保存的修改。"
            : cardDirty
                ? "当前卡牌数据有未保存的修改。"
                : "当前 Lua 脚本有未保存的修改。";
        DialogResult result = MessageBox.Show(
            this,
            content + "\n是否保存？",
            "未保存修改",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning,
            MessageBoxDefaultButton.Button1);

        return result switch
        {
            DialogResult.Yes => SaveAll(),
            DialogResult.No => true,
            _ => false
        };
    }

    private bool ConfirmScriptDiscardOrSave()
    {
        if (!scriptDirty) return true;
        DialogResult result = MessageBox.Show(
            this,
            "当前 Lua 脚本有未保存的修改。是否保存？",
            "未保存脚本",
            MessageBoxButtons.YesNoCancel,
            MessageBoxIcon.Warning);
        return result switch
        {
            DialogResult.Yes => SaveScriptFile(),
            DialogResult.No => true,
            _ => false
        };
    }

    private void ClearForm()
    {
        loading = true;
        cardIdInput.Value = 1;
        cardNameInput.Clear();
        oracleIdInput.Clear();
        setCodeInput.Clear();
        collectorInput.Clear();
        multipartInput.Clear();
        manaCostInput.Clear();
        subtypesInput.Clear();
        powerInput.Value = 0;
        toughnessInput.Value = 0;
        loyaltyInput.Value = 0;
        defenseInput.Value = 0;
        rulesTextInput.Clear();
        scriptPathInput.Clear();
        basicEditorView.SelectedTypes = CardTypeFlags.None;
        stringsGrid.Rows.Clear();
        preservedEffects.Clear();
        luaEditor.CodeText = string.Empty;
        loadedScriptFullPath = null;
        scriptCreatePromptDismissedForCardKey = null;
        loading = false;
        cardDirty = false;
        scriptDirty = false;
        UpdateCardHeader();
        UpdateCardImagePreview();
        UpdateScriptPathStatus();
    }

    private CardTypeFlags ReadTypes()
    {
        return basicEditorView.SelectedTypes;
    }

    private CardSupertypeFlags ReadSupertypes()
    {
        return basicEditorView.SelectedSupertypes;
    }

    private void UpdateStatFields(
     CardTypeFlags? typesOverride = null)
    {
        basicEditorView.RefreshStatFields(typesOverride);
    }


    private void AddPromptString()
    {
        int index = stringsGrid.Rows.Count;
        int rowIndex = stringsGrid.Rows.Add($"str{index + 1}", index, string.Empty);
        stringsGrid.CurrentCell = stringsGrid.Rows[rowIndex].Cells["StringText"];
        stringsGrid.BeginEdit(true);
        MarkCardDirty();
    }

    private void DeleteSelectedPromptString()
    {
        if (stringsGrid.CurrentRow == null) return;
        int index = stringsGrid.CurrentRow.Index;
        stringsGrid.Rows.RemoveAt(index);
        RenumberPromptStrings();
        if (stringsGrid.Rows.Count > 0)
        {
            int next = Math.Min(index, stringsGrid.Rows.Count - 1);
            stringsGrid.CurrentCell = stringsGrid.Rows[next].Cells["StringText"];
        }
        MarkCardDirty();
    }

    private void MovePromptString(int direction)
    {
        if (stringsGrid.CurrentRow == null) return;
        int current = stringsGrid.CurrentRow.Index;
        int target = current + direction;
        if (target < 0 || target >= stringsGrid.Rows.Count) return;

        object? currentText = stringsGrid.Rows[current].Cells["StringText"].Value;
        object? targetText = stringsGrid.Rows[target].Cells["StringText"].Value;
        stringsGrid.Rows[current].Cells["StringText"].Value = targetText;
        stringsGrid.Rows[target].Cells["StringText"].Value = currentText;
        stringsGrid.CurrentCell = stringsGrid.Rows[target].Cells["StringText"];
        RenumberPromptStrings();
        MarkCardDirty();
    }

    private void RenumberPromptStrings()
    {
        loading = true;
        for (int i = 0; i < stringsGrid.Rows.Count; i++)
        {
            stringsGrid.Rows[i].Cells["StringName"].Value = $"str{i + 1}";
            stringsGrid.Rows[i].Cells["StringIndex"].Value = i;
        }
        loading = false;
    }

    private string ContentRootDirectoryPath()
    {
        if (database == null) return AppContext.BaseDirectory;

        string databaseDirectory = Path.GetFullPath(database.DirectoryPath);
        string? directoryName = Path.GetFileName(databaseDirectory);
        if (directoryName is "数据库" or "Databases" or "Database")
            return Path.GetDirectoryName(databaseDirectory) ?? databaseDirectory;

        return databaseDirectory;
    }

    private string CardImagesDirectoryPath()
    {
        return Path.Combine(ContentRootDirectoryPath(), "卡图");
    }

    private string ScriptsDirectoryPath()
    {
        return Path.Combine(ContentRootDirectoryPath(), "脚本");
    }

    private string CurrentScriptDirectoryPath()
    {
        return Path.Combine(ScriptsDirectoryPath(), CurrentSetDirectoryName());
    }

    private string CurrentExistingScriptDirectoryPath()
    {
        string current = CurrentScriptDirectoryPath();
        return Directory.Exists(current) ? current : ScriptsDirectoryPath();
    }

    private string CurrentSetDirectoryName()
    {
        string setCode = setCodeInput.Text.Trim();
        return IsSafeCardImageToken(setCode) ? setCode : "CUSTOM";
    }

    private string CurrentCardImageId()
    {
        string collectorNumber = collectorInput.Text.Trim();
        return IsSafeCardImageToken(collectorNumber) ? collectorNumber : ((int)cardIdInput.Value).ToString();
    }

    private void UpdateCardImagePreview()
    {
        if (TryFindCardImagePath(out string? imagePath, out string expectedPath))
        {
            SetCardPreviewImage(LoadImageCopy(imagePath));
            string relativePath = database == null ? imagePath : Path.GetRelativePath(ContentRootDirectoryPath(), imagePath).Replace('\\', '/');
            cardImageStatusLabel.Text = $"已加载卡图：{relativePath}";
            cardImageStatusLabel.ForeColor = EditorTheme.Success;
            return;
        }

        SetCardPreviewImage(LoadDefaultCardBackImage());
        cardImageStatusLabel.Text = expectedPath.Length == 0
            ? "未选择卡牌；显示默认卡背。"
            : $"未找到卡图，显示默认卡背。\n正在查找：{expectedPath}";
        cardImageStatusLabel.ForeColor = expectedPath.Length == 0 ? EditorTheme.Muted : EditorTheme.Warning;
    }

    private bool TryFindCardImagePath(out string imagePath, out string expectedPath)
    {
        imagePath = string.Empty;
        expectedPath = string.Empty;
        if (database == null) return false;

        string setCode = setCodeInput.Text.Trim();
        string collectorNumber = collectorInput.Text.Trim();
        if (!IsSafeCardImageToken(setCode) || !IsSafeCardImageToken(collectorNumber)) return false;

        string cardImagesRoot = CardImagesDirectoryPath();
        expectedPath = $"{Path.Combine(cardImagesRoot, setCode, collectorNumber + ".png")} 或 {Path.Combine(cardImagesRoot, setCode + " " + collectorNumber + ".png")}";
        if (!Directory.Exists(cardImagesRoot)) return false;

        string[] supportedExtensions = { ".png", ".jpg", ".jpeg", ".webp" };
        string[] fileNameCandidates =
        {
            collectorNumber,
            $"{setCode} {collectorNumber}",
            $"{setCode}_{collectorNumber}",
            $"{setCode}-{collectorNumber}"
        };

        foreach (string directory in CandidateCardImageDirectories(cardImagesRoot, setCode))
        {
            foreach (string fileName in fileNameCandidates)
            {
                if (TryFindImageFile(directory, fileName, supportedExtensions, out imagePath))
                    return true;
            }
        }

        return false;
    }

    private static IEnumerable<string> CandidateCardImageDirectories(string cardImagesRoot, string setCode)
    {
        yield return Path.Combine(cardImagesRoot, setCode);
        yield return cardImagesRoot;

        foreach (string directory in Directory.EnumerateDirectories(cardImagesRoot))
        {
            if (Path.GetFileName(directory).Equals(setCode, StringComparison.OrdinalIgnoreCase))
                yield return directory;
        }
    }

    private static bool TryFindImageFile(
        string directory,
        string fileNameWithoutExtension,
        IEnumerable<string> supportedExtensions,
        out string imagePath)
    {
        imagePath = string.Empty;
        if (!Directory.Exists(directory)) return false;

        foreach (string extension in supportedExtensions)
        {
            string candidate = Path.Combine(directory, fileNameWithoutExtension + extension);
            if (File.Exists(candidate))
            {
                imagePath = candidate;
                return true;
            }
        }

        foreach (string file in Directory.EnumerateFiles(directory))
        {
            string extension = Path.GetExtension(file);
            if (!supportedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase)) continue;
            if (!Path.GetFileNameWithoutExtension(file).Equals(fileNameWithoutExtension, StringComparison.OrdinalIgnoreCase)) continue;

            imagePath = file;
            return true;
        }

        return false;
    }

    private static bool IsSafeCardImageToken(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        return value.IndexOfAny(Path.GetInvalidFileNameChars()) < 0
               && !value.Contains('/')
               && !value.Contains('\\');
    }

    private void SetCardPreviewImage(Image image)
    {
        Image? oldImage = currentCardPreviewImage;
        currentCardPreviewImage = image;
        cardImagePreview.Image = image;
        oldImage?.Dispose();
    }

    private static Image LoadImageCopy(string path)
    {
        using Image source = Image.FromFile(path);
        return new Bitmap(source);
    }

    private static Image LoadDefaultCardBackImage()
    {
        string path = Path.Combine(AppContext.BaseDirectory, "Assets", "default-card-back.png");
        if (File.Exists(path)) return LoadImageCopy(path);

        var bitmap = new Bitmap(265, 370);
        using Graphics graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.FromArgb(12, 18, 23));
        using var border = new Pen(EditorTheme.Border, 3);
        graphics.DrawRectangle(border, 8, 8, bitmap.Width - 16, bitmap.Height - 16);
        using var titleFont = new Font("Microsoft YaHei UI", 18f, FontStyle.Bold);
        using var bodyFont = new Font("Microsoft YaHei UI", 10f);
        TextRenderer.DrawText(graphics, "Magic\nThe Gathering", titleFont, new Rectangle(20, 120, 225, 80), EditorTheme.Text, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        TextRenderer.DrawText(graphics, "默认卡背", bodyFont, new Rectangle(20, 214, 225, 40), EditorTheme.Muted, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        return bitmap;
    }

    private void PromptCreateMissingScript()
    {
        if (database == null || scriptDirty) return;

        string storedPath = scriptPathInput.Text.Trim();
        bool hadStoredPath = storedPath.Length > 0;
        if (!hadStoredPath) storedPath = DefaultScriptStoredPath();

        if (!TryResolveScriptPath(storedPath, out string fullPath, out string error))
        {
            if (hadStoredPath)
                MessageBox.Show(this, error, "无法创建脚本", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (File.Exists(fullPath)) return;

        string promptKey = currentCardKey ?? $"new:{cardIdInput.Value}";
        if (scriptCreatePromptDismissedForCardKey == promptKey) return;

        string message = hadStoredPath
            ? $"关联的 Lua 脚本不存在。\n\n{storedPath}\n\n是否现在创建？"
            : $"这张卡还没有 Lua 脚本。\n\n是否创建 {storedPath}？";
        if (MessageBox.Show(this, message, "创建 Lua 脚本", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            scriptCreatePromptDismissedForCardKey = promptKey;
            return;
        }

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            string scriptText = BuildDefaultScriptText();
            File.WriteAllText(fullPath, scriptText, new UTF8Encoding(false));
            scriptPathInput.Text = storedPath;
            loadedScriptFullPath = fullPath;
            luaEditor.CodeText = scriptText;
            scriptDirty = false;
            scriptCreatePromptDismissedForCardKey = null;
            UpdateScriptPathStatus();
            UpdateStatus($"已创建 Lua 脚本：{storedPath}", cardDirty);
        }
        catch (Exception exception)
        {
            ShowError("无法创建脚本", exception);
        }
    }

    private string DefaultScriptStoredPath()
    {
        return $"脚本/{CurrentSetDirectoryName()}/{DefaultScriptFileName()}";
    }

    private string DefaultScriptFileName()
    {
        return $"m{CurrentCardImageId()}.lua";
    }

    private string BuildDefaultScriptText()
    {
        string cardName = cardNameInput.Text.Trim();
        var builder = new StringBuilder();
        if (cardName.Length > 0) builder.AppendLine($"-- {cardName}");
        builder.AppendLine("-- TODO: 在这里编写 MTGB 卡牌 Lua 脚本。");
        return builder.ToString();
    }

    private void BrowseExistingScript()
    {
        if (database == null)
        {
            MessageBox.Show(this, "请先打开数据库。", "没有数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ConfirmScriptDiscardOrSave()) return;

        using var dialog = new OpenFileDialog
        {
            Filter = "Lua 脚本 (*.lua)|*.lua|所有文件 (*.*)|*.*",
            InitialDirectory = CurrentExistingScriptDirectoryPath(),
            CheckFileExists = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        if (!TryMakeStoredScriptPath(dialog.FileName, out string storedPath, out string error))
        {
            MessageBox.Show(this, error, "无法选择脚本", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        scriptPathInput.Text = storedPath;
        LoadScriptFromCurrentPath(silent: false);
    }

    private void CreateNewScript()
    {
        if (database == null)
        {
            MessageBox.Show(this, "请先打开数据库。", "没有数据库", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        if (!ConfirmScriptDiscardOrSave()) return;

        Directory.CreateDirectory(CurrentScriptDirectoryPath());

        using var dialog = new SaveFileDialog
        {
            Filter = "Lua 脚本 (*.lua)|*.lua",
            DefaultExt = "lua",
            AddExtension = true,
            InitialDirectory = CurrentScriptDirectoryPath(),
            FileName = DefaultScriptFileName()
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;

        if (!TryMakeStoredScriptPath(dialog.FileName, out string storedPath, out string error))
        {
            MessageBox.Show(this, error, "无法新建脚本", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dialog.FileName)!);
            File.WriteAllText(dialog.FileName, string.Empty, new UTF8Encoding(false));
            scriptPathInput.Text = storedPath;
            loadedScriptFullPath = Path.GetFullPath(dialog.FileName);
            luaEditor.CodeText = string.Empty;
            scriptDirty = false;
            UpdateScriptPathStatus();
            UpdateStatus("已新建空白 Lua 脚本。", cardDirty);
            luaEditor.FocusEditor();
        }
        catch (Exception exception)
        {
            ShowError("无法新建脚本", exception);
        }
    }

    private void ReloadScript()
    {
        if (!ConfirmScriptDiscardOrSave()) return;
        LoadScriptFromCurrentPath(silent: false);
    }

    private void LoadScriptFromCurrentPath(bool silent)
    {
        loadedScriptFullPath = null;
        scriptDirty = false;

        string storedPath = scriptPathInput.Text.Trim();
        if (storedPath.Length == 0)
        {
            luaEditor.CodeText = string.Empty;
            UpdateScriptPathStatus();
            return;
        }

        if (!TryResolveScriptPath(storedPath, out string fullPath, out string error))
        {
            luaEditor.CodeText = string.Empty;
            scriptStatusLabel.Text = error;
            scriptStatusLabel.ForeColor = EditorTheme.Danger;
            if (!silent)
                MessageBox.Show(this, error, "无法加载脚本", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!File.Exists(fullPath))
        {
            luaEditor.CodeText = string.Empty;
            scriptStatusLabel.Text = $"文件不存在：{fullPath}";
            scriptStatusLabel.ForeColor = EditorTheme.Warning;
            if (!silent)
                MessageBox.Show(this, "指定的 Lua 文件不存在。可以直接编辑后点击“保存脚本”创建它。", "脚本不存在", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            luaEditor.CodeText = File.ReadAllText(fullPath);
            loadedScriptFullPath = fullPath;
            scriptDirty = false;
            UpdateScriptPathStatus();
            if (!silent) UpdateStatus("Lua 脚本已重新加载。", cardDirty);
        }
        catch (Exception exception)
        {
            ShowError("无法读取 Lua 脚本", exception);
        }
    }

    private bool SaveScriptFile()
    {
        if (database == null) return false;
        if (string.IsNullOrWhiteSpace(scriptPathInput.Text))
        {
            if (!ChooseScriptPathForSave()) return false;
        }

        if (!TryResolveScriptPath(scriptPathInput.Text.Trim(), out string fullPath, out string error))
        {
            MessageBox.Show(this, error, "无法保存脚本", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        try
        {
            string? directory = Path.GetDirectoryName(fullPath);
            if (!string.IsNullOrEmpty(directory)) Directory.CreateDirectory(directory);
            File.WriteAllText(fullPath, luaEditor.CodeText, new UTF8Encoding(false));
            loadedScriptFullPath = fullPath;
            scriptDirty = false;
            UpdateScriptPathStatus();
            UpdateStatus($"已保存 Lua 脚本：{Path.GetFileName(fullPath)}", cardDirty);
            return true;
        }
        catch (Exception exception)
        {
            ShowError("无法保存 Lua 脚本", exception);
            return false;
        }
    }

    private bool ChooseScriptPathForSave()
    {
        if (database == null) return false;
        Directory.CreateDirectory(CurrentScriptDirectoryPath());

        using var dialog = new SaveFileDialog
        {
            Filter = "Lua 脚本 (*.lua)|*.lua",
            DefaultExt = "lua",
            AddExtension = true,
            InitialDirectory = CurrentScriptDirectoryPath(),
            FileName = DefaultScriptFileName()
        };
        if (dialog.ShowDialog(this) != DialogResult.OK) return false;

        if (!TryMakeStoredScriptPath(dialog.FileName, out string storedPath, out string error))
        {
            MessageBox.Show(this, error, "无法保存脚本", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        scriptPathInput.Text = storedPath;
        return true;
    }

    private void OpenScriptFolder()
    {
        if (database == null) return;
        string folder = ScriptsDirectoryPath();
        if (TryResolveScriptPath(scriptPathInput.Text.Trim(), out string fullPath, out _))
            folder = Path.GetDirectoryName(fullPath) ?? folder;

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"\"{folder}\"",
                UseShellExecute = true
            });
        }
        catch (Exception exception)
        {
            ShowError("无法打开脚本目录", exception);
        }
    }

    private bool TryResolveScriptPath(string storedPath, out string fullPath, out string error)
    {
        fullPath = string.Empty;
        error = string.Empty;
        if (database == null)
        {
            error = "尚未打开数据库。";
            return false;
        }
        if (string.IsNullOrWhiteSpace(storedPath))
        {
            error = "尚未指定脚本路径。";
            return false;
        }
        if (Path.IsPathRooted(storedPath))
        {
            error = "script_path 必须是相对于 MTGB 根目录的路径，不能保存绝对路径。";
            return false;
        }

        try
        {
            string root = Path.GetFullPath(ContentRootDirectoryPath());
            string candidate = Path.GetFullPath(Path.Combine(root, storedPath.Replace('/', Path.DirectorySeparatorChar)));
            string rootPrefix = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                                + Path.DirectorySeparatorChar;
            if (!candidate.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
            {
                error = "当前资源结构不允许脚本路径离开 MTGB 根目录。";
                return false;
            }
            if (!Path.GetExtension(candidate).Equals(".lua", StringComparison.OrdinalIgnoreCase))
            {
                error = "脚本文件必须使用 .lua 扩展名。";
                return false;
            }

            fullPath = candidate;
            return true;
        }
        catch (Exception exception)
        {
            error = $"脚本路径无效：{exception.Message}";
            return false;
        }
    }

    private bool TryMakeStoredScriptPath(string fullPath, out string storedPath, out string error)
    {
        storedPath = string.Empty;
        error = string.Empty;
        if (database == null)
        {
            error = "尚未打开数据库。";
            return false;
        }

        try
        {
            string root = Path.GetFullPath(ContentRootDirectoryPath());
            string candidate = Path.GetFullPath(fullPath);
            string rootPrefix = root.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                                + Path.DirectorySeparatorChar;
            if (!candidate.StartsWith(rootPrefix, StringComparison.OrdinalIgnoreCase))
            {
                error = "该脚本位于 MTGB 根目录之外。当前运行端无法加载这个路径，因此编辑器不会写入。";
                return false;
            }
            if (!Path.GetExtension(candidate).Equals(".lua", StringComparison.OrdinalIgnoreCase))
            {
                error = "请选择 .lua 文件。";
                return false;
            }

            storedPath = Path.GetRelativePath(root, candidate).Replace('\\', '/');
            return true;
        }
        catch (Exception exception)
        {
            error = $"脚本路径无效：{exception.Message}";
            return false;
        }
    }

    private void UpdateScriptPathStatus()
    {
        if (database == null)
        {
            scriptStatusLabel.Text = "打开数据库后才能解析脚本路径。";
            scriptStatusLabel.ForeColor = EditorTheme.Muted;
        UpdateEditorPageButtons();
            return;
        }
        if (string.IsNullOrWhiteSpace(scriptPathInput.Text))
        {
            scriptStatusLabel.Text = "尚未指定 Lua 脚本。";
            scriptStatusLabel.ForeColor = EditorTheme.Muted;
        UpdateEditorPageButtons();
            return;
        }
        if (!TryResolveScriptPath(scriptPathInput.Text.Trim(), out string fullPath, out string error))
        {
            scriptStatusLabel.Text = error;
            scriptStatusLabel.ForeColor = EditorTheme.Danger;
            return;
        }

        scriptStatusLabel.Text = File.Exists(fullPath)
            ? $"已关联：{fullPath}"
            : $"保存脚本时将创建：{fullPath}";
        scriptStatusLabel.ForeColor = File.Exists(fullPath) ? EditorTheme.Success : EditorTheme.Warning;
    }

    private void MarkCardDirty()
    {
        if (loading) return;
        cardDirty = true;
        UpdateStatus(currentCardKey == null ? "新卡牌尚未保存。" : $"{currentCardKey} 的卡牌数据有未保存修改。", true);
    }

    private void MarkScriptDirty()
    {
        if (loading) return;
        scriptDirty = true;
        UpdateStatus("Lua 脚本有未保存修改。", true);
        UpdateScriptPathStatus();
    }

    private void UpdateStatus(string text, bool isDirty)
    {
        bool anyDirty = isDirty || cardDirty || scriptDirty;
        statusLabel.Text = anyDirty ? $"● {text}" : text;
        statusLabel.ForeColor = anyDirty ? EditorTheme.Warning : EditorTheme.Muted;
        UpdateWindowTitle();
    }

    private void UpdateWindowTitle()
    {
        string databaseName = database == null ? string.Empty : $" - {Path.GetFileName(database.Path)}";
        string dirtyMark = cardDirty || scriptDirty ? " *" : string.Empty;
        Text = $"MTGB 卡牌数据编辑器{databaseName}{dirtyMark}";
    }

    private void UpdateCardHeader()
    {
        string name = cardNameInput.Text.Trim();
        cardHeaderTitle.Text = name.Length == 0
            ? currentCardKey == null && database != null ? "新卡牌" : "未选择卡牌"
            : name;

        string setCode = setCodeInput.Text.Trim().ToUpperInvariant();
        string collector = collectorInput.Text.Trim();
        string printing = setCode.Length == 0 && collector.Length == 0
            ? "尚未设置系列与卡图编号"
            : $"{setCode}　·　卡图 {collector}";
        cardHeaderMeta.Text = printing;
    }

    private void MainFormKeyDown(object? sender, KeyEventArgs e)
    {
        if (!e.Control) return;
        switch (e.KeyCode)
        {
            case Keys.S:
                e.SuppressKeyPress = true;
                SaveAll();
                break;
            case Keys.N:
                e.SuppressKeyPress = true;
                NewCard();
                break;
            case Keys.O:
                e.SuppressKeyPress = true;
                OpenDatabaseDialog();
                break;
        }
    }

    private void ResizeCardListColumns()
    {
        if (cardList.Columns.Count != 2 || cardList.ClientSize.Width <= 0)
            return;

        const int printingWidth = 132;
        cardList.Columns[1].Width = printingWidth;
        cardList.Columns[0].Width = Math.Max(110, cardList.ClientSize.Width - printingWidth - 5);
    }

    private Button CreateEditorPageButton(string text, Action action)
    {
        var button = new Button
        {
            Text = text,
            Width = 132,
            Height = 36,
            Margin = new Padding(0, 0, 8, 0),
            FlatStyle = FlatStyle.Flat,
            TextAlign = ContentAlignment.MiddleCenter,
            Cursor = Cursors.Hand,
            BackColor = EditorTheme.Surface,
            ForeColor = EditorTheme.Muted,
            Font = new Font("Microsoft YaHei UI", 9.2f)
        };
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = EditorTheme.Border;
        button.FlatAppearance.MouseOverBackColor = EditorTheme.SurfaceRaised;
        button.FlatAppearance.MouseDownBackColor = EditorTheme.Input;
        button.Click += (_, _) => action();
        return button;
    }

    private void ShowEditorPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= editorPages.Count)
            return;


        if (pageIndex == 1)
            PromptCreateMissingScript();
        selectedEditorPage = pageIndex;
        for (int i = 0; i < editorPages.Count; i++)
            editorPages[i].Visible = i == pageIndex;
        editorPages[pageIndex].BringToFront();
        UpdateEditorPageButtons();
    }

    private void UpdateEditorPageButtons()
    {
        for (int i = 0; i < editorPageButtons.Count; i++)
        {
            bool selected = i == selectedEditorPage;
            Button button = editorPageButtons[i];
            button.BackColor = selected ? EditorTheme.SurfaceRaised : EditorTheme.Surface;
            button.ForeColor = selected ? Color.White : EditorTheme.Muted;
            button.FlatAppearance.BorderColor = selected ? EditorTheme.Accent : EditorTheme.Border;
        }
    }

    private static void UpdateTypeToggleAppearance(TypeToggle typeToggle)
    {
        bool selected = typeToggle.Toggle.Checked;
        typeToggle.Toggle.BackColor = selected ? EditorTheme.Selection : EditorTheme.Input;
        typeToggle.Toggle.ForeColor = selected ? Color.White : EditorTheme.Muted;
        typeToggle.Toggle.FlatAppearance.BorderColor = selected ? EditorTheme.Accent : EditorTheme.Border;
    }

    private void DrawCardListHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
    {
        using var background = new SolidBrush(EditorTheme.SurfaceRaised);
        using var border = new Pen(EditorTheme.Border);
        e.Graphics.FillRectangle(background, e.Bounds);
        e.Graphics.DrawRectangle(border, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
        using var headerFont = new Font("Microsoft YaHei UI", 9f, FontStyle.Bold);
        TextRenderer.DrawText(
            e.Graphics,
            e.Header?.Text ?? string.Empty,
            headerFont,
            new Rectangle(e.Bounds.X + 7, e.Bounds.Y, e.Bounds.Width - 10, e.Bounds.Height),
            EditorTheme.Text,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    private void DrawCardListSubItem(object? sender, DrawListViewSubItemEventArgs e)
    {
        bool selected = e.Item?.Selected == true;
        using var background = new SolidBrush(selected ? EditorTheme.Selection : EditorTheme.Input);
        using var border = new Pen(Color.FromArgb(42, 48, 57));
        e.Graphics.FillRectangle(background, e.Bounds);
        e.Graphics.DrawLine(border, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
        TextRenderer.DrawText(
            e.Graphics,
            e.SubItem?.Text ?? string.Empty,
            Font,
            new Rectangle(e.Bounds.X + 7, e.Bounds.Y, e.Bounds.Width - 10, e.Bounds.Height),
            selected ? Color.White : EditorTheme.Text,
            TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix);
    }

    private static bool Matches(CardSummary card, string filter)
    {
        if (filter.Length == 0) return true;
        return card.Name.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
               || card.CardId.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase)
               || card.SetCode.Contains(filter, StringComparison.OrdinalIgnoreCase)
               || card.CollectorNumber.Contains(filter, StringComparison.OrdinalIgnoreCase)
               || card.Types.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase);
    }

    private string NextCollectorNumber()
    {
        int number = 1;
        var existing = new HashSet<string>(
            summaries.Where(card => card.SetCode.Equals("CUSTOM", StringComparison.OrdinalIgnoreCase))
                .Select(card => card.CollectorNumber),
            StringComparer.OrdinalIgnoreCase);
        while (existing.Contains(number.ToString())) number++;
        return number.ToString();
    }

    private static string SafeFileName(string value)
    {
        string result = value.Trim();
        foreach (char invalid in Path.GetInvalidFileNameChars())
            result = result.Replace(invalid, '_');
        return result.Length == 0 ? "card" : result;
    }

    private static IEnumerable<(CardTypeFlags Flag, string Name)> CardTypeNames()
    {
        yield return (CardTypeFlags.Artifact, "神器");
        yield return (CardTypeFlags.Battle, "战役");
        yield return (CardTypeFlags.Creature, "生物");
        yield return (CardTypeFlags.Enchantment, "结界");
        yield return (CardTypeFlags.Instant, "瞬间");
        yield return (CardTypeFlags.Kindred, "亲族");
        yield return (CardTypeFlags.Land, "地");
        yield return (CardTypeFlags.Planeswalker, "鹏洛客");
        yield return (CardTypeFlags.Sorcery, "法术");
    }

    private TableLayoutPanel CreateFieldsTable(int rows)
    {
        var fields = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 4,
            RowCount = rows,
            BackColor = EditorTheme.Surface,
            Margin = new Padding(0)
        };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 128));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 128));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        for (int i = 0; i < rows; i++)
            fields.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rows));
        return fields;
    }

    private static Control CreateSection(string title, Control content)
    {
        var border = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = EditorTheme.Border,
            Padding = new Padding(1),
            Margin = new Padding(0, 0, 0, 12)
        };
        var body = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = EditorTheme.Surface,
            Padding = new Padding(16, 10, 16, 14)
        };
        var heading = new Label
        {
            Dock = DockStyle.Top,
            Height = 34,
            Text = title,
            ForeColor = EditorTheme.Text,
            Font = new Font("Microsoft YaHei UI", 10.5f, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft
        };
        content.Dock = DockStyle.Fill;
        body.Controls.Add(content);
        body.Controls.Add(heading);
        border.Controls.Add(body);
        return border;
    }

    private static Label AddSingleField(
        TableLayoutPanel table,
        string label,
        Control input,
        int row)
    {
        var fieldLabel = new Label
        {
            Text = label,
            Dock = DockStyle.Fill,
            ForeColor = EditorTheme.Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0, 0, 10, 0),
            AutoEllipsis = true
        };
        input.Dock = DockStyle.Fill;
        if (input.Margin == Padding.Empty)
            input.Margin = new Padding(0, 6, 10, 6);
        table.Controls.Add(fieldLabel, 0, row);
        table.Controls.Add(input, 1, row);
        return fieldLabel;
    }

    private static Label AddField(
        TableLayoutPanel table,
        string label,
        Control input,
        int row,
        int column,
        int span = 1)
    {
        var fieldLabel = new Label
        {
            Text = label,
            Dock = DockStyle.Fill,
            ForeColor = EditorTheme.Muted,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(0, 0, 8, 0),
            AutoEllipsis = true
        };
        input.Dock = DockStyle.Fill;
        input.Margin = new Padding(0, 6, 12, 6);
        table.Controls.Add(fieldLabel, column, row);
        table.Controls.Add(input, column + 1, row);
        if (span > 1) table.SetColumnSpan(input, span);
        return fieldLabel;
    }

    private static Button CreateButton(
        string text,
        Action action,
        int width,
        bool accent = false,
        bool danger = false)
    {
        var button = new Button
        {
            Text = text,
            Width = width,
            Height = 34,
            Margin = new Padding(4, 0, 4, 0),
            Tag = accent ? "accent" : danger ? "danger" : null,
            Font = new Font("Microsoft YaHei UI", 9f)
        };
        EditorTheme.StyleButton(button, accent, danger);
        button.Click += (_, _) => action();
        return button;
    }

    private static ToolStripButton CreateToolButton(
        string text,
        Action action,
        bool accent = false,
        bool danger = false)
    {
        var button = new ToolStripButton(text)
        {
            DisplayStyle = ToolStripItemDisplayStyle.Text,
            ForeColor = danger ? Color.FromArgb(255, 164, 164) : accent ? Color.FromArgb(148, 190, 255) : EditorTheme.Text,
            Margin = new Padding(4, 0, 4, 0),
            Padding = new Padding(8, 3, 8, 3),
            Font = new Font("Microsoft YaHei UI", 9.2f, accent ? FontStyle.Bold : FontStyle.Regular)
        };
        button.Click += (_, _) => action();
        return button;
    }

    private void ShowError(string title, Exception exception)
    {
        MessageBox.Show(this, exception.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        UpdateStatus(exception.Message, true);
    }

    private sealed record TypeToggle(CardTypeFlags Flag, CheckBox Toggle);
}
