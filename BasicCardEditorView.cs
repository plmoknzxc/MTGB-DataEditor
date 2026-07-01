namespace MTGB.CardDatabaseEditor;

public partial class BasicCardEditorView : UserControl
{
    private readonly List<TypeToggle> cardTypeToggles = new();
    private readonly List<SupertypeToggle> cardSupertypeToggles = new();

    public event EventHandler? ViewChanged;
    public event EventHandler? AddPromptRequested;
    public event EventHandler? DeletePromptRequested;
    public event Action<int>? MovePromptRequested;

    public BasicCardEditorView()
    {
        InitializeComponent();
        InitializeRuntimeBindings();

        EditorTheme.Apply(this);
        EditorTheme.StyleGrid(stringsGrid);

        if (IsDesignerHosted())
            ShowAllCharacteristicFieldsForDesigner();
        else
            UpdateStatFields();
    }

    private void InitializeRuntimeBindings()
    {
        RegisterTypeToggle(CardTypeFlags.Artifact, artifactTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Battle, battleTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Creature, creatureTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Enchantment, enchantmentTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Instant, instantTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Kindred, kindredTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Land, landTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Planeswalker, planeswalkerTypeToggle);
        RegisterTypeToggle(CardTypeFlags.Sorcery, sorceryTypeToggle);

        RegisterSupertypeToggle(CardSupertypeFlags.Basic, basicSupertypeToggle);
        RegisterSupertypeToggle(CardSupertypeFlags.Legendary, legendarySupertypeToggle);
        RegisterSupertypeToggle(CardSupertypeFlags.Ongoing, ongoingSupertypeToggle);
        RegisterSupertypeToggle(CardSupertypeFlags.Snow, snowSupertypeToggle);
        RegisterSupertypeToggle(CardSupertypeFlags.World, worldSupertypeToggle);

        addPromptButton.Click += (_, _) =>
            AddPromptRequested?.Invoke(this, EventArgs.Empty);

        deletePromptButton.Click += (_, _) =>
            DeletePromptRequested?.Invoke(this, EventArgs.Empty);

        movePromptUpButton.Click += (_, _) =>
            MovePromptRequested?.Invoke(-1);

        movePromptDownButton.Click += (_, _) =>
            MovePromptRequested?.Invoke(1);

        InitializeManaSymbolMenu();
    }

    private void InitializeManaSymbolMenu()
    {
        components ??= new System.ComponentModel.Container();

        var menu = new ContextMenuStrip(components)
        {
            BackColor = EditorTheme.Surface,
            ForeColor = EditorTheme.Text,
            Renderer = new ToolStripProfessionalRenderer(new DarkColorTable())
        };

        AddManaSymbolCategory(menu, "数字通用 {1}",
            "{0}", "{1}", "{2}", "{3}", "{4}", "{5}", "{6}",
            "{7}", "{8}", "{9}", "{10}", "{11}", "{12}", "{13}",
            "{14}", "{15}", "{16}", "{17}", "{18}", "{19}", "{20}");
        AddManaSymbolCategory(menu, "变量费用 {X}", "{X}", "{Y}", "{Z}");
        AddManaSymbolCategory(menu, "五色法术力 {W}", "{W}", "{U}", "{B}", "{R}", "{G}");
        AddManaSymbolCategory(menu, "无色法术力 {C}", "{C}");
        AddManaSymbolCategory(menu, "雪法术力 {S}", "{S}");
        AddManaSymbolCategory(menu, "双色混血 {W/U}",
            "{W/U}", "{W/B}", "{U/B}", "{U/R}", "{B/R}",
            "{B/G}", "{R/G}", "{R/W}", "{G/W}", "{G/U}");
        AddManaSymbolCategory(menu, "二费混血 {2/W}", "{2/W}", "{2/U}", "{2/B}", "{2/R}", "{2/G}");
        AddManaSymbolCategory(menu, "非瑞克西亚 {W/P}", "{W/P}", "{U/P}", "{B/P}", "{R/P}", "{G/P}");
        AddManaSymbolCategory(menu, "混血非瑞 {W/U/P}",
            "{W/U/P}", "{W/B/P}", "{U/B/P}", "{U/R/P}", "{B/R/P}",
            "{B/G/P}", "{R/G/P}", "{R/W/P}", "{G/W/P}", "{G/U/P}");
        AddManaSymbolCategory(menu, "无色混血 {C/W}", "{C/W}", "{C/U}", "{C/B}", "{C/R}", "{C/G}");

        manaSymbolMenuButton.Click += (_, _) =>
            menu.Show(manaSymbolMenuButton, new Point(0, manaSymbolMenuButton.Height));

        fieldToolTip.SetToolTip(manaSymbolMenuButton, "插入 {W}、{2/W} 等法术力符号到光标位置");
    }

    private void AddManaSymbolCategory(ContextMenuStrip menu, string categoryName, params string[] symbols)
    {
        var categoryItem = CreateManaMenuItem(categoryName);

        foreach (string symbol in symbols)
            categoryItem.DropDownItems.Add(CreateManaMenuItem(symbol, symbol));

        menu.Items.Add(categoryItem);
    }

    private ToolStripMenuItem CreateManaMenuItem(string text, string? symbol = null)
    {
        var item = new ToolStripMenuItem(text)
        {
            BackColor = EditorTheme.Surface,
            ForeColor = EditorTheme.Text
        };

        if (symbol is not null)
            item.Click += (_, _) => InsertManaSymbol(symbol);

        return item;
    }

    private void InsertManaSymbol(string symbol)
    {
        int start = manaCostInput.SelectionStart;
        int length = manaCostInput.SelectionLength;

        manaCostInput.Text = manaCostInput.Text
            .Remove(start, length)
            .Insert(start, symbol);
        manaCostInput.SelectionStart = start + symbol.Length;
        manaCostInput.SelectionLength = 0;
        manaCostInput.Focus();

        ViewChanged?.Invoke(this, EventArgs.Empty);
    }
    private void RegisterTypeToggle(CardTypeFlags flag, CheckBox toggle)
    {
        var typeToggle = new TypeToggle(flag, toggle);
        cardTypeToggles.Add(typeToggle);

        toggle.CheckedChanged += (_, _) =>
        {
            UpdateTypeToggleAppearance(typeToggle);
            UpdateStatFields();
            ViewChanged?.Invoke(this, EventArgs.Empty);
        };

        UpdateTypeToggleAppearance(typeToggle);
    }

    private static void UpdateTypeToggleAppearance(TypeToggle typeToggle)
    {
        bool selected = typeToggle.Toggle.Checked;
        typeToggle.Toggle.BackColor = selected
            ? EditorTheme.Selection
            : EditorTheme.Input;
        typeToggle.Toggle.ForeColor = selected
            ? Color.White
            : EditorTheme.Muted;
        typeToggle.Toggle.FlatAppearance.BorderColor = selected
            ? EditorTheme.Accent
            : EditorTheme.Border;
    }

    private void UpdateStatFields(CardTypeFlags? typesOverride = null)
    {
        CardTypeFlags types = typesOverride ?? ReadTypes();
        bool isCreature = types.HasFlag(CardTypeFlags.Creature);
        bool isPlaneswalker = types.HasFlag(CardTypeFlags.Planeswalker);
        bool isBattle = types.HasFlag(CardTypeFlags.Battle);

        powerStatHost.Visible = isCreature;
        toughnessStatHost.Visible = isCreature;
        loyaltyStatHost.Visible = isPlaneswalker;
        defenseStatHost.Visible = isBattle;
        noCharacteristicsLabel.Visible =
            !isCreature && !isPlaneswalker && !isBattle;
    }

    private void RegisterSupertypeToggle(CardSupertypeFlags flag, CheckBox toggle)
    {
        var typeToggle = new SupertypeToggle(flag, toggle);
        cardSupertypeToggles.Add(typeToggle);
        
        toggle.CheckedChanged += (_, _) =>
        {
            UpdateSupertypeToggleAppearance(typeToggle);
            ViewChanged?.Invoke(this, EventArgs.Empty);
        };
        
        UpdateSupertypeToggleAppearance(typeToggle);
    }
    private static void UpdateSupertypeToggleAppearance(SupertypeToggle typeToggle)
    {
        bool selected = typeToggle.Toggle.Checked;
        typeToggle.Toggle.BackColor = selected
        ? EditorTheme.Selection
        : EditorTheme.Input;
        typeToggle.Toggle.ForeColor = selected
        ? Color.White
        : EditorTheme.Muted;
        typeToggle.Toggle.FlatAppearance.BorderColor = selected
        ? EditorTheme.Accent
        : EditorTheme.Border;
    }

    private void ShowAllCharacteristicFieldsForDesigner()
    {
        powerStatHost.Visible = true;
        toughnessStatHost.Visible = true;
        loyaltyStatHost.Visible = true;
        defenseStatHost.Visible = true;
        noCharacteristicsLabel.Visible = false;
    }

    private CardTypeFlags ReadTypes()
    {
        CardTypeFlags result = CardTypeFlags.None;

        foreach (TypeToggle typeToggle in cardTypeToggles)
        {
            if (typeToggle.Toggle.Checked)
                result |= typeToggle.Flag;
        }

        return result;
    }
    private CardSupertypeFlags ReadSupertypes()
    {
        CardSupertypeFlags result = CardSupertypeFlags.None;

        foreach (SupertypeToggle typeToggle in cardSupertypeToggles)
        {
            if (typeToggle.Toggle.Checked)
                result |= typeToggle.Flag;
        }

        return result;
    }

    private bool IsDesignerHosted()
    {
        return System.ComponentModel.LicenseManager.UsageMode ==
               System.ComponentModel.LicenseUsageMode.Designtime
               || DesignMode
               || (Site?.DesignMode ?? false);
    }

    internal PictureBox CardImagePreview => cardImagePreview;
    internal Label CardImageStatusLabel => cardImageStatusLabel;

    internal TextBox CardNameInput => cardNameInput;
    internal TextBox OracleIdInput => oracleIdInput;
    internal TextBox MultipartInput => multipartInput;
    internal TextBox SetCodeInput => setCodeInput;
    internal TextBox CollectorInput => collectorInput;
    internal TextBox ManaCostInput => manaCostInput;
    internal TextBox SubtypesInput => cardSubtypesInput;
    internal TextBox RulesTextInput => rulesTextInput;

    internal NumericUpDown PowerInput => powerInput;
    internal NumericUpDown ToughnessInput => toughnessInput;
    internal NumericUpDown LoyaltyInput => loyaltyInput;
    internal NumericUpDown DefenseInput => defenseInput;

    internal DataGridView StringsGrid => stringsGrid;

    internal CardTypeFlags SelectedTypes
    {
        get => ReadTypes();
        set
        {
            foreach (TypeToggle typeToggle in cardTypeToggles)
            {
                typeToggle.Toggle.Checked =
                    value.HasFlag(typeToggle.Flag);
            }

            UpdateStatFields(value);
        }
    }

    internal CardSupertypeFlags SelectedSupertypes
    {
        get => ReadSupertypes();
        set
        {
            foreach (SupertypeToggle typeToggle in cardSupertypeToggles)
            {
                typeToggle.Toggle.Checked =
                value.HasFlag(typeToggle.Flag);
            }

        }
    }

    internal void RefreshStatFields(CardTypeFlags? types = null)
    {
        UpdateStatFields(types);
    }

    private void cardImagePreview_Click(object? sender, EventArgs e)
    {

    }

    private void defenseInput_ValueChanged(object? sender, EventArgs e)
    {

    }

    private void gameplaySectionBody_Paint(object? sender, PaintEventArgs e)
    {

    }

    private void rulesSectionBody_Paint(object? sender, PaintEventArgs e)
    {

    }

    private void collectorInput_TextChanged(object? sender, EventArgs e)
    {

    }

    private void cardNameInput_TextChanged(object? sender, EventArgs e)
    {

    }

    private sealed record TypeToggle(
        CardTypeFlags Flag,
        CheckBox Toggle);

    private sealed record SupertypeToggle(
        CardSupertypeFlags Flag,
        CheckBox Toggle);

}
