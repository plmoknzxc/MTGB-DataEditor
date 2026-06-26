namespace MTGB.CardDatabaseEditor;

public partial class BasicCardEditorView : UserControl
{
    private readonly List<TypeToggle> cardTypeToggles = new();

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

        addPromptButton.Click += (_, _) =>
            AddPromptRequested?.Invoke(this, EventArgs.Empty);

        deletePromptButton.Click += (_, _) =>
            DeletePromptRequested?.Invoke(this, EventArgs.Empty);

        movePromptUpButton.Click += (_, _) =>
            MovePromptRequested?.Invoke(-1);

        movePromptDownButton.Click += (_, _) =>
            MovePromptRequested?.Invoke(1);
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
    internal TextBox SetCodeInput => setCodeInput;
    internal TextBox CollectorInput => collectorInput;
    internal TextBox ManaCostInput => manaCostInput;
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

    internal void RefreshStatFields(CardTypeFlags? types = null)
    {
        UpdateStatFields(types);
    }

    private void cardImagePreview_Click(object sender, EventArgs e)
    {

    }

    private void defenseInput_ValueChanged(object sender, EventArgs e)
    {

    }

    private void gameplaySectionBody_Paint(object sender, PaintEventArgs e)
    {

    }

    private void rulesSectionBody_Paint(object sender, PaintEventArgs e)
    {

    }

    private void collectorInput_TextChanged(object sender, EventArgs e)
    {

    }

    private void cardNameInput_TextChanged(object sender, EventArgs e)
    {

    }

    private sealed record TypeToggle(
        CardTypeFlags Flag,
        CheckBox Toggle);
}
