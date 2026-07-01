#nullable enable

namespace MTGB.CardDatabaseEditor;

partial class BasicCardEditorView
{
    private System.ComponentModel.IContainer? components = null;

    private Panel rootScrollPanel = null!;
    private Panel rootCanvas = null!;

    private Panel previewSectionBorder = null!;
    private Panel previewSectionBody = null!;
    private Label previewSectionHeading = null!;
    private PictureBox cardImagePreview = null!;
    private Label cardImageStatusLabel = null!;

    private Panel identitySectionBorder = null!;
    private Panel identitySectionBody = null!;
    private Label identitySectionHeading = null!;
    private Label cardNameLabel = null!;
    private Label setCodeLabel = null!;
    private Label collectorLabel = null!;
    private Label oracleIdLabel = null!;
    private Label multipartLabel = null!;
    private TextBox cardNameInput = null!;
    private TextBox setCodeInput = null!;
    private TextBox collectorInput = null!;
    private TextBox oracleIdInput = null!;
    private TextBox multipartInput = null!;

    private Panel cardTypesSectionBorder = null!;
    private Panel cardTypesSectionBody = null!;
    private Label cardTypesSectionHeading = null!;

    private Panel gameplaySectionBorder = null!;
    private Panel gameplaySectionBody = null!;
    private Label gameplaySectionHeading = null!;
    private Label manaCostLabel = null!;
    private TextBox manaCostInput = null!;
    private Button manaSymbolMenuButton = null!;
    private Label cardTypesLabel = null!;
    private CheckBox artifactTypeToggle = null!;
    private CheckBox battleTypeToggle = null!;
    private CheckBox creatureTypeToggle = null!;
    private CheckBox enchantmentTypeToggle = null!;
    private CheckBox instantTypeToggle = null!;
    private CheckBox kindredTypeToggle = null!;
    private CheckBox landTypeToggle = null!;
    private CheckBox planeswalkerTypeToggle = null!;
    private CheckBox sorceryTypeToggle = null!;
    private Label cardSupertypesLabel = null!;
    private CheckBox basicSupertypeToggle = null!;
    private CheckBox legendarySupertypeToggle = null!;
    private CheckBox ongoingSupertypeToggle = null!;
    private CheckBox snowSupertypeToggle = null!;
    private CheckBox worldSupertypeToggle = null!;
    private Label cardSubtypesLabel = null!;
    private TextBox cardSubtypesInput = null!;
    private Label typeNote = null!;
    private Label characteristicsLabel = null!;
    private Panel powerStatHost = null!;
    private Label powerLabel = null!;
    private NumericUpDown powerInput = null!;
    private Panel toughnessStatHost = null!;
    private Label toughnessLabel = null!;
    private NumericUpDown toughnessInput = null!;
    private Panel loyaltyStatHost = null!;
    private Label loyaltyLabel = null!;
    private NumericUpDown loyaltyInput = null!;
    private Panel defenseStatHost = null!;
    private Label defenseLabel = null!;
    private NumericUpDown defenseInput = null!;
    private Label noCharacteristicsLabel = null!;

    private Panel rulesSectionBorder = null!;
    private Panel rulesSectionBody = null!;
    private Label rulesSectionHeading = null!;
    private TextBox rulesTextInput = null!;

    private Panel stringsSectionBorder = null!;
    private Panel stringsSectionBody = null!;
    private Label stringsSectionHeading = null!;
    private Label stringsHelpLabel = null!;
    private Button addPromptButton = null!;
    private Button deletePromptButton = null!;
    private Button movePromptUpButton = null!;
    private Button movePromptDownButton = null!;
    private DataGridView stringsGrid = null!;
    private DataGridViewTextBoxColumn stringNameColumn = null!;
    private DataGridViewTextBoxColumn stringIndexColumn = null!;
    private DataGridViewTextBoxColumn stringTextColumn = null!;

    private ToolTip fieldToolTip = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
            components?.Dispose();

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        rootScrollPanel = new Panel();
        rootCanvas = new Panel();
        previewSectionBorder = new Panel();
        previewSectionBody = new Panel();
        previewSectionHeading = new Label();
        cardImagePreview = new PictureBox();
        cardImageStatusLabel = new Label();
        identitySectionBorder = new Panel();
        identitySectionBody = new Panel();
        identitySectionHeading = new Label();
        cardNameLabel = new Label();
        cardNameInput = new TextBox();
        setCodeLabel = new Label();
        setCodeInput = new TextBox();
        collectorLabel = new Label();
        collectorInput = new TextBox();
        oracleIdLabel = new Label();
        oracleIdInput = new TextBox();
        multipartLabel = new Label();
        multipartInput = new TextBox();
        cardTypesSectionBorder = new Panel();
        cardTypesSectionBody = new Panel();
        cardTypesSectionHeading = new Label();
        gameplaySectionBorder = new Panel();
        gameplaySectionBody = new Panel();
        gameplaySectionHeading = new Label();
        manaCostLabel = new Label();
        manaCostInput = new TextBox();
        manaSymbolMenuButton = new Button();
        cardTypesLabel = new Label();
        artifactTypeToggle = new CheckBox();
        battleTypeToggle = new CheckBox();
        creatureTypeToggle = new CheckBox();
        enchantmentTypeToggle = new CheckBox();
        instantTypeToggle = new CheckBox();
        kindredTypeToggle = new CheckBox();
        landTypeToggle = new CheckBox();
        planeswalkerTypeToggle = new CheckBox();
        sorceryTypeToggle = new CheckBox();
        typeNote = new Label();
        cardSupertypesLabel = new Label();
        basicSupertypeToggle = new CheckBox();
        legendarySupertypeToggle = new CheckBox();
        ongoingSupertypeToggle = new CheckBox();
        snowSupertypeToggle = new CheckBox();
        worldSupertypeToggle = new CheckBox();
        cardSubtypesLabel = new Label();
        cardSubtypesInput = new TextBox();
        characteristicsLabel = new Label();
        powerStatHost = new Panel();
        powerLabel = new Label();
        powerInput = new NumericUpDown();
        toughnessStatHost = new Panel();
        toughnessLabel = new Label();
        toughnessInput = new NumericUpDown();
        loyaltyStatHost = new Panel();
        loyaltyLabel = new Label();
        loyaltyInput = new NumericUpDown();
        defenseStatHost = new Panel();
        defenseLabel = new Label();
        defenseInput = new NumericUpDown();
        noCharacteristicsLabel = new Label();
        rulesSectionBorder = new Panel();
        rulesSectionBody = new Panel();
        rulesSectionHeading = new Label();
        rulesTextInput = new TextBox();
        stringsSectionBorder = new Panel();
        stringsSectionBody = new Panel();
        stringsSectionHeading = new Label();
        stringsHelpLabel = new Label();
        addPromptButton = new Button();
        deletePromptButton = new Button();
        movePromptUpButton = new Button();
        movePromptDownButton = new Button();
        stringsGrid = new DataGridView();
        stringNameColumn = new DataGridViewTextBoxColumn();
        stringIndexColumn = new DataGridViewTextBoxColumn();
        stringTextColumn = new DataGridViewTextBoxColumn();
        fieldToolTip = new ToolTip(components);
        rootScrollPanel.SuspendLayout();
        rootCanvas.SuspendLayout();
        previewSectionBorder.SuspendLayout();
        previewSectionBody.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)cardImagePreview).BeginInit();
        identitySectionBorder.SuspendLayout();
        identitySectionBody.SuspendLayout();
        cardTypesSectionBorder.SuspendLayout();
        cardTypesSectionBody.SuspendLayout();
        gameplaySectionBorder.SuspendLayout();
        gameplaySectionBody.SuspendLayout();
        powerStatHost.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)powerInput).BeginInit();
        toughnessStatHost.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)toughnessInput).BeginInit();
        loyaltyStatHost.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)loyaltyInput).BeginInit();
        defenseStatHost.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)defenseInput).BeginInit();
        rulesSectionBorder.SuspendLayout();
        rulesSectionBody.SuspendLayout();
        stringsSectionBorder.SuspendLayout();
        stringsSectionBody.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)stringsGrid).BeginInit();
        SuspendLayout();
        // 
        // rootScrollPanel
        // 
        rootScrollPanel.AutoScroll = true;
        rootScrollPanel.AutoScrollMinSize = new Size(1485, 1345);
        rootScrollPanel.BackColor = Color.FromArgb(22, 25, 30);
        rootScrollPanel.Controls.Add(rootCanvas);
        rootScrollPanel.Dock = DockStyle.Fill;
        rootScrollPanel.Location = new Point(0, 0);
        rootScrollPanel.Name = "rootScrollPanel";
        rootScrollPanel.Padding = new Padding(12, 10, 12, 12);
        rootScrollPanel.Size = new Size(1500, 1360);
        rootScrollPanel.TabIndex = 0;
        // 
        // rootCanvas
        // 
        rootCanvas.BackColor = Color.FromArgb(22, 25, 30);
        rootCanvas.Controls.Add(previewSectionBorder);
        rootCanvas.Controls.Add(identitySectionBorder);
        rootCanvas.Controls.Add(cardTypesSectionBorder);
        rootCanvas.Controls.Add(gameplaySectionBorder);
        rootCanvas.Controls.Add(rulesSectionBorder);
        rootCanvas.Controls.Add(stringsSectionBorder);
        rootCanvas.Location = new Point(12, 0);
        rootCanvas.Name = "rootCanvas";
        rootCanvas.Size = new Size(1473, 1345);
        rootCanvas.TabIndex = 0;
        // 
        // previewSectionBorder
        // 
        previewSectionBorder.BackColor = Color.FromArgb(62, 70, 82);
        previewSectionBorder.Controls.Add(previewSectionBody);
        previewSectionBorder.Location = new Point(0, 0);
        previewSectionBorder.Name = "previewSectionBorder";
        previewSectionBorder.Padding = new Padding(1);
        previewSectionBorder.Size = new Size(300, 560);
        previewSectionBorder.TabIndex = 0;
        // 
        // previewSectionBody
        // 
        previewSectionBody.BackColor = Color.FromArgb(28, 33, 41);
        previewSectionBody.Controls.Add(previewSectionHeading);
        previewSectionBody.Controls.Add(cardImagePreview);
        previewSectionBody.Controls.Add(cardImageStatusLabel);
        previewSectionBody.Dock = DockStyle.Fill;
        previewSectionBody.Location = new Point(1, 1);
        previewSectionBody.Name = "previewSectionBody";
        previewSectionBody.Size = new Size(298, 558);
        previewSectionBody.TabIndex = 0;
        // 
        // previewSectionHeading
        // 
        previewSectionHeading.AutoSize = true;
        previewSectionHeading.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Bold);
        previewSectionHeading.ForeColor = Color.FromArgb(232, 236, 242);
        previewSectionHeading.Location = new Point(14, 11);
        previewSectionHeading.Name = "previewSectionHeading";
        previewSectionHeading.Size = new Size(106, 31);
        previewSectionHeading.TabIndex = 0;
        previewSectionHeading.Text = "卡图预览";
        // 
        // cardImagePreview
        // 
        cardImagePreview.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        cardImagePreview.BackColor = Color.FromArgb(13, 17, 22);
        cardImagePreview.BorderStyle = BorderStyle.FixedSingle;
        cardImagePreview.Location = new Point(12, 46);
        cardImagePreview.Name = "cardImagePreview";
        cardImagePreview.Size = new Size(274, 438);
        cardImagePreview.SizeMode = PictureBoxSizeMode.Zoom;
        cardImagePreview.TabIndex = 1;
        cardImagePreview.TabStop = false;
        // 
        // cardImageStatusLabel
        // 
        cardImageStatusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        cardImageStatusLabel.ForeColor = Color.FromArgb(172, 183, 198);
        cardImageStatusLabel.Location = new Point(12, 506);
        cardImageStatusLabel.Name = "cardImageStatusLabel";
        cardImageStatusLabel.Size = new Size(274, 52);
        cardImageStatusLabel.TabIndex = 2;
        cardImageStatusLabel.Text = "尚未加载卡图";
        cardImageStatusLabel.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // identitySectionBorder
        // 
        identitySectionBorder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        identitySectionBorder.BackColor = Color.FromArgb(62, 70, 82);
        identitySectionBorder.Controls.Add(identitySectionBody);
        identitySectionBorder.Location = new Point(907, 1);
        identitySectionBorder.Name = "identitySectionBorder";
        identitySectionBorder.Padding = new Padding(1);
        identitySectionBorder.Size = new Size(540, 240);
        identitySectionBorder.TabIndex = 1;
        // 
        // identitySectionBody
        // 
        identitySectionBody.BackColor = Color.FromArgb(28, 33, 41);
        identitySectionBody.Controls.Add(identitySectionHeading);
        identitySectionBody.Controls.Add(cardNameLabel);
        identitySectionBody.Controls.Add(cardNameInput);
        identitySectionBody.Controls.Add(setCodeLabel);
        identitySectionBody.Controls.Add(setCodeInput);
        identitySectionBody.Controls.Add(collectorLabel);
        identitySectionBody.Controls.Add(collectorInput);
        identitySectionBody.Controls.Add(oracleIdLabel);
        identitySectionBody.Controls.Add(oracleIdInput);
        identitySectionBody.Controls.Add(multipartLabel);
        identitySectionBody.Controls.Add(multipartInput);
        identitySectionBody.Dock = DockStyle.Fill;
        identitySectionBody.Location = new Point(1, 1);
        identitySectionBody.Name = "identitySectionBody";
        identitySectionBody.Size = new Size(538, 195);
        identitySectionBody.TabIndex = 0;
        // 
        // identitySectionHeading
        // 
        identitySectionHeading.AutoSize = true;
        identitySectionHeading.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Bold);
        identitySectionHeading.ForeColor = Color.FromArgb(232, 236, 242);
        identitySectionHeading.Location = new Point(17, 17);
        identitySectionHeading.Name = "identitySectionHeading";
        identitySectionHeading.Size = new Size(106, 31);
        identitySectionHeading.TabIndex = 0;
        identitySectionHeading.Text = "卡牌标识";
        // 
        // cardNameLabel
        // 
        cardNameLabel.ForeColor = Color.FromArgb(172, 183, 198);
        cardNameLabel.Location = new Point(27, 62);
        cardNameLabel.Name = "cardNameLabel";
        cardNameLabel.Size = new Size(58, 30);
        cardNameLabel.TabIndex = 1;
        cardNameLabel.Text = "卡名";
        cardNameLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cardNameInput
        // 
        cardNameInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        cardNameInput.BackColor = Color.FromArgb(15, 19, 24);
        cardNameInput.BorderStyle = BorderStyle.FixedSingle;
        cardNameInput.ForeColor = Color.FromArgb(232, 236, 242);
        cardNameInput.Location = new Point(146, 60);
        cardNameInput.Name = "cardNameInput";
        cardNameInput.PlaceholderText = "例如 Training Island";
        cardNameInput.Size = new Size(177, 32);
        cardNameInput.TabIndex = 2;
        cardNameInput.TextChanged += cardNameInput_TextChanged;
        // 
        // setCodeLabel
        // 
        setCodeLabel.ForeColor = Color.FromArgb(172, 183, 198);
        setCodeLabel.Location = new Point(25, 101);
        setCodeLabel.Name = "setCodeLabel";
        setCodeLabel.Size = new Size(98, 30);
        setCodeLabel.TabIndex = 3;
        setCodeLabel.Text = "系列代号";
        setCodeLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // setCodeInput
        // 
        setCodeInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        setCodeInput.BackColor = Color.FromArgb(15, 19, 24);
        setCodeInput.BorderStyle = BorderStyle.FixedSingle;
        setCodeInput.ForeColor = Color.FromArgb(232, 236, 242);
        setCodeInput.Location = new Point(146, 101);
        setCodeInput.Name = "setCodeInput";
        setCodeInput.PlaceholderText = "例如 MTGB";
        setCodeInput.Size = new Size(131, 32);
        setCodeInput.TabIndex = 4;
        fieldToolTip.SetToolTip(setCodeInput, "卡图路径中的系列文件夹，例如 CardImages/MTGB/1.png");
        // 
        // collectorLabel
        // 
        collectorLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        collectorLabel.ForeColor = Color.FromArgb(172, 183, 198);
        collectorLabel.Location = new Point(288, 101);
        collectorLabel.Name = "collectorLabel";
        collectorLabel.Size = new Size(98, 30);
        collectorLabel.TabIndex = 5;
        collectorLabel.Text = "卡图编号";
        collectorLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // collectorInput
        // 
        collectorInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        collectorInput.BackColor = Color.FromArgb(15, 19, 24);
        collectorInput.BorderStyle = BorderStyle.FixedSingle;
        collectorInput.ForeColor = Color.FromArgb(232, 236, 242);
        collectorInput.Location = new Point(393, 101);
        collectorInput.Name = "collectorInput";
        collectorInput.PlaceholderText = "例如 1、45a";
        collectorInput.Size = new Size(133, 32);
        collectorInput.TabIndex = 6;
        fieldToolTip.SetToolTip(collectorInput, "卡图文件名，不含扩展名");
        collectorInput.TextChanged += collectorInput_TextChanged;
        // 
        // oracleIdLabel
        // 
        oracleIdLabel.ForeColor = Color.FromArgb(172, 183, 198);
        oracleIdLabel.Location = new Point(25, 141);
        oracleIdLabel.Name = "oracleIdLabel";
        oracleIdLabel.Size = new Size(117, 30);
        oracleIdLabel.TabIndex = 7;
        oracleIdLabel.Text = "Oracle ID";
        oracleIdLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // oracleIdInput
        // 
        oracleIdInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        oracleIdInput.BackColor = Color.FromArgb(15, 19, 24);
        oracleIdInput.BorderStyle = BorderStyle.FixedSingle;
        oracleIdInput.ForeColor = Color.FromArgb(232, 236, 242);
        oracleIdInput.Location = new Point(146, 141);
        oracleIdInput.Name = "oracleIdInput";
        oracleIdInput.PlaceholderText = "同一规则卡牌的共享标识，可留空";
        oracleIdInput.Size = new Size(238, 32);
        oracleIdInput.TabIndex = 8;
        fieldToolTip.SetToolTip(oracleIdInput, "同一规则卡牌的不同印刷版本可共享；自制卡可留空");
        // 
        // multipartLabel
        // 
        multipartLabel.ForeColor = Color.FromArgb(172, 183, 198);
        multipartLabel.Location = new Point(25, 181);
        multipartLabel.Name = "multipartLabel";
        multipartLabel.Size = new Size(117, 30);
        multipartLabel.TabIndex = 9;
        multipartLabel.Text = "关联特征";
        multipartLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // multipartInput
        // 
        multipartInput.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        multipartInput.BackColor = Color.FromArgb(15, 19, 24);
        multipartInput.BorderStyle = BorderStyle.FixedSingle;
        multipartInput.ForeColor = Color.FromArgb(232, 236, 242);
        multipartInput.Location = new Point(146, 181);
        multipartInput.Name = "multipartInput";
        multipartInput.PlaceholderText = "";
        multipartInput.Size = new Size(238, 32);
        multipartInput.TabIndex = 10;
        // 
        // cardTypesSectionBorder
        // 
        cardTypesSectionBorder.BackColor = Color.FromArgb(62, 70, 82);
        cardTypesSectionBorder.Controls.Add(cardTypesSectionBody);
        cardTypesSectionBorder.Location = new Point(312, 0);
        cardTypesSectionBorder.Name = "cardTypesSectionBorder";
        cardTypesSectionBorder.Padding = new Padding(1);
        cardTypesSectionBorder.Size = new Size(583, 560);
        cardTypesSectionBorder.TabIndex = 2;
        // 
        // cardTypesSectionBody
        // 
        cardTypesSectionBody.BackColor = Color.FromArgb(28, 33, 41);
        cardTypesSectionBody.Controls.Add(cardTypesSectionHeading);
        cardTypesSectionBody.Controls.Add(cardTypesLabel);
        cardTypesSectionBody.Controls.Add(artifactTypeToggle);
        cardTypesSectionBody.Controls.Add(battleTypeToggle);
        cardTypesSectionBody.Controls.Add(creatureTypeToggle);
        cardTypesSectionBody.Controls.Add(enchantmentTypeToggle);
        cardTypesSectionBody.Controls.Add(instantTypeToggle);
        cardTypesSectionBody.Controls.Add(kindredTypeToggle);
        cardTypesSectionBody.Controls.Add(landTypeToggle);
        cardTypesSectionBody.Controls.Add(planeswalkerTypeToggle);
        cardTypesSectionBody.Controls.Add(sorceryTypeToggle);
        cardTypesSectionBody.Controls.Add(typeNote);
        cardTypesSectionBody.Controls.Add(cardSupertypesLabel);
        cardTypesSectionBody.Controls.Add(basicSupertypeToggle);
        cardTypesSectionBody.Controls.Add(legendarySupertypeToggle);
        cardTypesSectionBody.Controls.Add(ongoingSupertypeToggle);
        cardTypesSectionBody.Controls.Add(snowSupertypeToggle);
        cardTypesSectionBody.Controls.Add(worldSupertypeToggle);
        cardTypesSectionBody.Controls.Add(cardSubtypesLabel);
        cardTypesSectionBody.Controls.Add(cardSubtypesInput);
        cardTypesSectionBody.Dock = DockStyle.Fill;
        cardTypesSectionBody.Location = new Point(1, 1);
        cardTypesSectionBody.Name = "cardTypesSectionBody";
        cardTypesSectionBody.Size = new Size(581, 558);
        cardTypesSectionBody.TabIndex = 0;
        // 
        // cardTypesSectionHeading
        // 
        cardTypesSectionHeading.AutoSize = true;
        cardTypesSectionHeading.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Bold);
        cardTypesSectionHeading.ForeColor = Color.FromArgb(232, 236, 242);
        cardTypesSectionHeading.Location = new Point(18, 17);
        cardTypesSectionHeading.Name = "cardTypesSectionHeading";
        cardTypesSectionHeading.Size = new Size(106, 31);
        cardTypesSectionHeading.TabIndex = 0;
        cardTypesSectionHeading.Text = "卡牌类型";
        // 
        // gameplaySectionBorder
        // 
        gameplaySectionBorder.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        gameplaySectionBorder.BackColor = Color.FromArgb(62, 70, 82);
        gameplaySectionBorder.Controls.Add(gameplaySectionBody);
        gameplaySectionBorder.Location = new Point(907, 242);
        gameplaySectionBorder.Name = "gameplaySectionBorder";
        gameplaySectionBorder.Padding = new Padding(1);
        gameplaySectionBorder.Size = new Size(540, 328);
        gameplaySectionBorder.TabIndex = 3;
        // 
        // gameplaySectionBody
        // 
        gameplaySectionBody.BackColor = Color.FromArgb(28, 33, 41);
        gameplaySectionBody.Controls.Add(gameplaySectionHeading);
        gameplaySectionBody.Controls.Add(manaCostLabel);
        gameplaySectionBody.Controls.Add(manaCostInput);
        gameplaySectionBody.Controls.Add(manaSymbolMenuButton);
        gameplaySectionBody.Controls.Add(characteristicsLabel);
        gameplaySectionBody.Controls.Add(powerStatHost);
        gameplaySectionBody.Controls.Add(toughnessStatHost);
        gameplaySectionBody.Controls.Add(loyaltyStatHost);
        gameplaySectionBody.Controls.Add(defenseStatHost);
        gameplaySectionBody.Controls.Add(noCharacteristicsLabel);
        gameplaySectionBody.Dock = DockStyle.Fill;
        gameplaySectionBody.Location = new Point(1, 1);
        gameplaySectionBody.Name = "gameplaySectionBody";
        gameplaySectionBody.Size = new Size(538, 356);
        gameplaySectionBody.TabIndex = 0;
        gameplaySectionBody.Paint += gameplaySectionBody_Paint;
        // 
        // gameplaySectionHeading
        // 
        gameplaySectionHeading.AutoSize = true;
        gameplaySectionHeading.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Bold);
        gameplaySectionHeading.ForeColor = Color.FromArgb(232, 236, 242);
        gameplaySectionHeading.Location = new Point(17, 17);
        gameplaySectionHeading.Name = "gameplaySectionHeading";
        gameplaySectionHeading.Size = new Size(106, 31);
        gameplaySectionHeading.TabIndex = 0;
        gameplaySectionHeading.Text = "数值 / 法术力";
        // 
        // manaCostLabel
        // 
        manaCostLabel.ForeColor = Color.FromArgb(172, 183, 198);
        manaCostLabel.Location = new Point(27, 66);
        manaCostLabel.Name = "manaCostLabel";
        manaCostLabel.Size = new Size(90, 30);
        manaCostLabel.TabIndex = 1;
        manaCostLabel.Text = "法术力费用";
        manaCostLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // manaCostInput
        // 
        manaCostInput.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        manaCostInput.BackColor = Color.FromArgb(15, 19, 24);
        manaCostInput.BorderStyle = BorderStyle.FixedSingle;
        manaCostInput.ForeColor = Color.FromArgb(232, 236, 242);
        manaCostInput.Location = new Point(120, 66);
        manaCostInput.Name = "manaCostInput";
        manaCostInput.PlaceholderText = "例如 {2}{U}、{G/W}";
        manaCostInput.Size = new Size(260, 32);
        manaCostInput.TabIndex = 2;
        // 
        // manaSymbolMenuButton
        // 
        manaSymbolMenuButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        manaSymbolMenuButton.BackColor = Color.FromArgb(36, 41, 49);
        manaSymbolMenuButton.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        manaSymbolMenuButton.FlatStyle = FlatStyle.Flat;
        manaSymbolMenuButton.ForeColor = Color.FromArgb(232, 236, 242);
        manaSymbolMenuButton.Location = new Point(390, 65);
        manaSymbolMenuButton.Name = "manaSymbolMenuButton";
        manaSymbolMenuButton.Size = new Size(118, 34);
        manaSymbolMenuButton.TabIndex = 3;
        manaSymbolMenuButton.Text = "插入符号 ▾";
        manaSymbolMenuButton.UseVisualStyleBackColor = false;
        // 
        // cardTypesLabel
        // 
        cardTypesLabel.ForeColor = Color.FromArgb(172, 183, 198);
        cardTypesLabel.Location = new Point(22, 68);
        cardTypesLabel.Name = "cardTypesLabel";
        cardTypesLabel.Size = new Size(520, 30);
        cardTypesLabel.TabIndex = 3;
        cardTypesLabel.Text = "选择一个或多个类别";
        cardTypesLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // artifactTypeToggle
        // 
        artifactTypeToggle.Appearance = Appearance.Button;
        artifactTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        artifactTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        artifactTypeToggle.FlatStyle = FlatStyle.Flat;
        artifactTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        artifactTypeToggle.Location = new Point(22, 108);
        artifactTypeToggle.Name = "artifactTypeToggle";
        artifactTypeToggle.Size = new Size(72, 34);
        artifactTypeToggle.TabIndex = 4;
        artifactTypeToggle.Text = "神器";
        artifactTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        artifactTypeToggle.UseVisualStyleBackColor = false;
        // 
        // battleTypeToggle
        // 
        battleTypeToggle.Appearance = Appearance.Button;
        battleTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        battleTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        battleTypeToggle.FlatStyle = FlatStyle.Flat;
        battleTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        battleTypeToggle.Location = new Point(100, 108);
        battleTypeToggle.Name = "battleTypeToggle";
        battleTypeToggle.Size = new Size(72, 34);
        battleTypeToggle.TabIndex = 5;
        battleTypeToggle.Text = "战役";
        battleTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        battleTypeToggle.UseVisualStyleBackColor = false;
        // 
        // creatureTypeToggle
        // 
        creatureTypeToggle.Appearance = Appearance.Button;
        creatureTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        creatureTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        creatureTypeToggle.FlatStyle = FlatStyle.Flat;
        creatureTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        creatureTypeToggle.Location = new Point(178, 108);
        creatureTypeToggle.Name = "creatureTypeToggle";
        creatureTypeToggle.Size = new Size(72, 34);
        creatureTypeToggle.TabIndex = 6;
        creatureTypeToggle.Text = "生物";
        creatureTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        creatureTypeToggle.UseVisualStyleBackColor = false;
        // 
        // enchantmentTypeToggle
        // 
        enchantmentTypeToggle.Appearance = Appearance.Button;
        enchantmentTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        enchantmentTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        enchantmentTypeToggle.FlatStyle = FlatStyle.Flat;
        enchantmentTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        enchantmentTypeToggle.Location = new Point(256, 108);
        enchantmentTypeToggle.Name = "enchantmentTypeToggle";
        enchantmentTypeToggle.Size = new Size(72, 34);
        enchantmentTypeToggle.TabIndex = 7;
        enchantmentTypeToggle.Text = "结界";
        enchantmentTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        enchantmentTypeToggle.UseVisualStyleBackColor = false;
        // 
        // instantTypeToggle
        // 
        instantTypeToggle.Appearance = Appearance.Button;
        instantTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        instantTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        instantTypeToggle.FlatStyle = FlatStyle.Flat;
        instantTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        instantTypeToggle.Location = new Point(334, 108);
        instantTypeToggle.Name = "instantTypeToggle";
        instantTypeToggle.Size = new Size(72, 34);
        instantTypeToggle.TabIndex = 8;
        instantTypeToggle.Text = "瞬间";
        instantTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        instantTypeToggle.UseVisualStyleBackColor = false;
        // 
        // kindredTypeToggle
        // 
        kindredTypeToggle.Appearance = Appearance.Button;
        kindredTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        kindredTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        kindredTypeToggle.FlatStyle = FlatStyle.Flat;
        kindredTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        kindredTypeToggle.Location = new Point(412, 108);
        kindredTypeToggle.Name = "kindredTypeToggle";
        kindredTypeToggle.Size = new Size(72, 34);
        kindredTypeToggle.TabIndex = 9;
        kindredTypeToggle.Text = "亲族";
        kindredTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        kindredTypeToggle.UseVisualStyleBackColor = false;
        // 
        // landTypeToggle
        // 
        landTypeToggle.Appearance = Appearance.Button;
        landTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        landTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        landTypeToggle.FlatStyle = FlatStyle.Flat;
        landTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        landTypeToggle.Location = new Point(22, 152);
        landTypeToggle.Name = "landTypeToggle";
        landTypeToggle.Size = new Size(72, 34);
        landTypeToggle.TabIndex = 10;
        landTypeToggle.Text = "地";
        landTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        landTypeToggle.UseVisualStyleBackColor = false;
        // 
        // planeswalkerTypeToggle
        // 
        planeswalkerTypeToggle.Appearance = Appearance.Button;
        planeswalkerTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        planeswalkerTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        planeswalkerTypeToggle.FlatStyle = FlatStyle.Flat;
        planeswalkerTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        planeswalkerTypeToggle.Location = new Point(100, 152);
        planeswalkerTypeToggle.Name = "planeswalkerTypeToggle";
        planeswalkerTypeToggle.Size = new Size(86, 34);
        planeswalkerTypeToggle.TabIndex = 11;
        planeswalkerTypeToggle.Text = "鹏洛客";
        planeswalkerTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        planeswalkerTypeToggle.UseVisualStyleBackColor = false;
        // 
        // sorceryTypeToggle
        // 
        sorceryTypeToggle.Appearance = Appearance.Button;
        sorceryTypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        sorceryTypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        sorceryTypeToggle.FlatStyle = FlatStyle.Flat;
        sorceryTypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        sorceryTypeToggle.Location = new Point(192, 152);
        sorceryTypeToggle.Name = "sorceryTypeToggle";
        sorceryTypeToggle.Size = new Size(72, 34);
        sorceryTypeToggle.TabIndex = 12;
        sorceryTypeToggle.Text = "法术";
        sorceryTypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        sorceryTypeToggle.UseVisualStyleBackColor = false;
        // 
        // typeNote
        // 
        typeNote.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        typeNote.ForeColor = Color.FromArgb(172, 183, 198);
        typeNote.Location = new Point(22, 208);
        typeNote.Name = "typeNote";
        typeNote.Size = new Size(520, 96);
        typeNote.TabIndex = 13;
        typeNote.Text = "可组合多种类别；编辑器会同时显示每种类别需要的数值。亲族必须与另一种类别并存。";
        typeNote.TextAlign = ContentAlignment.TopLeft;
        // 
        // characteristicsLabel
        // 
        characteristicsLabel.ForeColor = Color.FromArgb(172, 183, 198);
        characteristicsLabel.Location = new Point(27, 122);
        characteristicsLabel.Name = "characteristicsLabel";
        characteristicsLabel.Size = new Size(90, 30);
        characteristicsLabel.TabIndex = 14;
        characteristicsLabel.Text = "类别数值";
        characteristicsLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // powerStatHost
        // 
        powerStatHost.BackColor = Color.FromArgb(15, 19, 24);
        powerStatHost.BorderStyle = BorderStyle.FixedSingle;
        powerStatHost.Controls.Add(powerLabel);
        powerStatHost.Controls.Add(powerInput);
        powerStatHost.Location = new Point(120, 122);
        powerStatHost.Name = "powerStatHost";
        powerStatHost.Size = new Size(170, 48);
        powerStatHost.TabIndex = 15;
        // 
        // powerLabel
        // 
        powerLabel.ForeColor = Color.FromArgb(232, 236, 242);
        powerLabel.Location = new Point(10, 8);
        powerLabel.Name = "powerLabel";
        powerLabel.Size = new Size(72, 28);
        powerLabel.TabIndex = 0;
        powerLabel.Text = "力量";
        powerLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // powerInput
        // 
        powerInput.BackColor = Color.FromArgb(15, 19, 24);
        powerInput.BorderStyle = BorderStyle.FixedSingle;
        powerInput.ForeColor = Color.FromArgb(232, 236, 242);
        powerInput.Location = new Point(86, 8);
        powerInput.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        powerInput.Minimum = new decimal(new int[] { 9999, 0, 0, int.MinValue });
        powerInput.Name = "powerInput";
        powerInput.Size = new Size(72, 32);
        powerInput.TabIndex = 1;
        // 
        // toughnessStatHost
        // 
        toughnessStatHost.BackColor = Color.FromArgb(15, 19, 24);
        toughnessStatHost.BorderStyle = BorderStyle.FixedSingle;
        toughnessStatHost.Controls.Add(toughnessLabel);
        toughnessStatHost.Controls.Add(toughnessInput);
        toughnessStatHost.Location = new Point(300, 122);
        toughnessStatHost.Name = "toughnessStatHost";
        toughnessStatHost.Size = new Size(170, 48);
        toughnessStatHost.TabIndex = 16;
        // 
        // toughnessLabel
        // 
        toughnessLabel.ForeColor = Color.FromArgb(232, 236, 242);
        toughnessLabel.Location = new Point(10, 8);
        toughnessLabel.Name = "toughnessLabel";
        toughnessLabel.Size = new Size(72, 28);
        toughnessLabel.TabIndex = 0;
        toughnessLabel.Text = "防御力";
        toughnessLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // toughnessInput
        // 
        toughnessInput.BackColor = Color.FromArgb(15, 19, 24);
        toughnessInput.BorderStyle = BorderStyle.FixedSingle;
        toughnessInput.ForeColor = Color.FromArgb(232, 236, 242);
        toughnessInput.Location = new Point(86, 8);
        toughnessInput.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        toughnessInput.Minimum = new decimal(new int[] { 9999, 0, 0, int.MinValue });
        toughnessInput.Name = "toughnessInput";
        toughnessInput.Size = new Size(72, 32);
        toughnessInput.TabIndex = 1;
        // 
        // loyaltyStatHost
        // 
        loyaltyStatHost.BackColor = Color.FromArgb(15, 19, 24);
        loyaltyStatHost.BorderStyle = BorderStyle.FixedSingle;
        loyaltyStatHost.Controls.Add(loyaltyLabel);
        loyaltyStatHost.Controls.Add(loyaltyInput);
        loyaltyStatHost.Location = new Point(120, 180);
        loyaltyStatHost.Name = "loyaltyStatHost";
        loyaltyStatHost.Size = new Size(170, 48);
        loyaltyStatHost.TabIndex = 17;
        // 
        // loyaltyLabel
        // 
        loyaltyLabel.ForeColor = Color.FromArgb(232, 236, 242);
        loyaltyLabel.Location = new Point(10, 8);
        loyaltyLabel.Name = "loyaltyLabel";
        loyaltyLabel.Size = new Size(72, 28);
        loyaltyLabel.TabIndex = 0;
        loyaltyLabel.Text = "初始忠诚";
        loyaltyLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // loyaltyInput
        // 
        loyaltyInput.BackColor = Color.FromArgb(15, 19, 24);
        loyaltyInput.BorderStyle = BorderStyle.FixedSingle;
        loyaltyInput.ForeColor = Color.FromArgb(232, 236, 242);
        loyaltyInput.Location = new Point(86, 8);
        loyaltyInput.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        loyaltyInput.Minimum = new decimal(new int[] { 9999, 0, 0, int.MinValue });
        loyaltyInput.Name = "loyaltyInput";
        loyaltyInput.Size = new Size(72, 32);
        loyaltyInput.TabIndex = 1;
        // 
        // defenseStatHost
        // 
        defenseStatHost.BackColor = Color.FromArgb(15, 19, 24);
        defenseStatHost.BorderStyle = BorderStyle.FixedSingle;
        defenseStatHost.Controls.Add(defenseLabel);
        defenseStatHost.Controls.Add(defenseInput);
        defenseStatHost.Location = new Point(300, 180);
        defenseStatHost.Name = "defenseStatHost";
        defenseStatHost.Size = new Size(170, 48);
        defenseStatHost.TabIndex = 18;
        // 
        // defenseLabel
        // 
        defenseLabel.ForeColor = Color.FromArgb(232, 236, 242);
        defenseLabel.Location = new Point(10, 8);
        defenseLabel.Name = "defenseLabel";
        defenseLabel.Size = new Size(72, 28);
        defenseLabel.TabIndex = 0;
        defenseLabel.Text = "布防值";
        defenseLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // defenseInput
        // 
        defenseInput.BackColor = Color.FromArgb(15, 19, 24);
        defenseInput.BorderStyle = BorderStyle.FixedSingle;
        defenseInput.ForeColor = Color.FromArgb(232, 236, 242);
        defenseInput.Location = new Point(86, 8);
        defenseInput.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
        defenseInput.Minimum = new decimal(new int[] { 9999, 0, 0, int.MinValue });
        defenseInput.Name = "defenseInput";
        defenseInput.Size = new Size(72, 32);
        defenseInput.TabIndex = 1;
        // 
        // noCharacteristicsLabel
        // 
        noCharacteristicsLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        noCharacteristicsLabel.ForeColor = Color.FromArgb(172, 183, 198);
        noCharacteristicsLabel.Location = new Point(120, 245);
        noCharacteristicsLabel.Name = "noCharacteristicsLabel";
        noCharacteristicsLabel.Size = new Size(390, 66);
        noCharacteristicsLabel.TabIndex = 19;
        noCharacteristicsLabel.Text = "当前类别没有需要填写的力量、防御力、忠诚或布防数值。";
        // 
        // cardSupertypesLabel
        // 
        cardSupertypesLabel.ForeColor = Color.FromArgb(172, 183, 198);
        cardSupertypesLabel.Location = new Point(22, 320);
        cardSupertypesLabel.Name = "cardSupertypesLabel";
        cardSupertypesLabel.Size = new Size(520, 30);
        cardSupertypesLabel.TabIndex = 14;
        cardSupertypesLabel.Text = "选择一个或多个超类别";
        cardSupertypesLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // basicSupertypeToggle
        // 
        basicSupertypeToggle.Appearance = Appearance.Button;
        basicSupertypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        basicSupertypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        basicSupertypeToggle.FlatStyle = FlatStyle.Flat;
        basicSupertypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        basicSupertypeToggle.Location = new Point(22, 360);
        basicSupertypeToggle.Name = "basicSupertypeToggle";
        basicSupertypeToggle.Size = new Size(72, 34);
        basicSupertypeToggle.TabIndex = 15;
        basicSupertypeToggle.Text = "基本";
        basicSupertypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        basicSupertypeToggle.UseVisualStyleBackColor = false;
        // 
        // legendarySupertypeToggle
        // 
        legendarySupertypeToggle.Appearance = Appearance.Button;
        legendarySupertypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        legendarySupertypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        legendarySupertypeToggle.FlatStyle = FlatStyle.Flat;
        legendarySupertypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        legendarySupertypeToggle.Location = new Point(100, 360);
        legendarySupertypeToggle.Name = "legendarySupertypeToggle";
        legendarySupertypeToggle.Size = new Size(72, 34);
        legendarySupertypeToggle.TabIndex = 16;
        legendarySupertypeToggle.Text = "传奇";
        legendarySupertypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        legendarySupertypeToggle.UseVisualStyleBackColor = false;
        // 
        // ongoingSupertypeToggle
        // 
        ongoingSupertypeToggle.Appearance = Appearance.Button;
        ongoingSupertypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        ongoingSupertypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        ongoingSupertypeToggle.FlatStyle = FlatStyle.Flat;
        ongoingSupertypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        ongoingSupertypeToggle.Location = new Point(178, 360);
        ongoingSupertypeToggle.Name = "ongoingSupertypeToggle";
        ongoingSupertypeToggle.Size = new Size(72, 34);
        ongoingSupertypeToggle.TabIndex = 17;
        ongoingSupertypeToggle.Text = "长效";
        ongoingSupertypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        ongoingSupertypeToggle.UseVisualStyleBackColor = false;
        // 
        // snowSupertypeToggle
        // 
        snowSupertypeToggle.Appearance = Appearance.Button;
        snowSupertypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        snowSupertypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        snowSupertypeToggle.FlatStyle = FlatStyle.Flat;
        snowSupertypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        snowSupertypeToggle.Location = new Point(256, 360);
        snowSupertypeToggle.Name = "snowSupertypeToggle";
        snowSupertypeToggle.Size = new Size(72, 34);
        snowSupertypeToggle.TabIndex = 18;
        snowSupertypeToggle.Text = "雪境";
        snowSupertypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        snowSupertypeToggle.UseVisualStyleBackColor = false;
        // 
        // worldSupertypeToggle
        // 
        worldSupertypeToggle.Appearance = Appearance.Button;
        worldSupertypeToggle.BackColor = Color.FromArgb(15, 19, 24);
        worldSupertypeToggle.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        worldSupertypeToggle.FlatStyle = FlatStyle.Flat;
        worldSupertypeToggle.ForeColor = Color.FromArgb(172, 183, 198);
        worldSupertypeToggle.Location = new Point(334, 360);
        worldSupertypeToggle.Name = "worldSupertypeToggle";
        worldSupertypeToggle.Size = new Size(72, 34);
        worldSupertypeToggle.TabIndex = 19;
        worldSupertypeToggle.Text = "普世";
        worldSupertypeToggle.TextAlign = ContentAlignment.MiddleCenter;
        worldSupertypeToggle.UseVisualStyleBackColor = false;
        // 
        // cardSubtypesLabel
        // 
        cardSubtypesLabel.ForeColor = Color.FromArgb(172, 183, 198);
        cardSubtypesLabel.Location = new Point(22, 420);
        cardSubtypesLabel.Name = "cardSubtypesLabel";
        cardSubtypesLabel.Size = new Size(520, 30);
        cardSubtypesLabel.TabIndex = 20;
        cardSubtypesLabel.Text = "输入副类别";
        cardSubtypesLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // cardSubtypesInput
        // 
        cardSubtypesInput.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        cardSubtypesInput.BackColor = Color.FromArgb(15, 19, 24);
        cardSubtypesInput.BorderStyle = BorderStyle.FixedSingle;
        cardSubtypesInput.ForeColor = Color.FromArgb(232, 236, 242);
        cardSubtypesInput.Location = new Point(22, 465);
        cardSubtypesInput.Name = "cardSubtypesInput";
        cardSubtypesInput.PlaceholderText = "";
        cardSubtypesInput.Size = new Size(370, 36);
        cardSubtypesInput.TabIndex = 21;
        // 
        // rulesSectionBorder
        // 
        rulesSectionBorder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        rulesSectionBorder.BackColor = Color.FromArgb(62, 70, 82);
        rulesSectionBorder.Controls.Add(rulesSectionBody);
        rulesSectionBorder.Location = new Point(0, 572);
        rulesSectionBorder.Name = "rulesSectionBorder";
        rulesSectionBorder.Padding = new Padding(1);
        rulesSectionBorder.Size = new Size(1447, 350);
        rulesSectionBorder.TabIndex = 3;
        // 
        // rulesSectionBody
        // 
        rulesSectionBody.BackColor = Color.FromArgb(28, 33, 41);
        rulesSectionBody.Controls.Add(rulesSectionHeading);
        rulesSectionBody.Controls.Add(rulesTextInput);
        rulesSectionBody.Dock = DockStyle.Fill;
        rulesSectionBody.Location = new Point(1, 1);
        rulesSectionBody.Name = "rulesSectionBody";
        rulesSectionBody.Size = new Size(1445, 348);
        rulesSectionBody.TabIndex = 0;
        rulesSectionBody.Paint += rulesSectionBody_Paint;
        // 
        // rulesSectionHeading
        // 
        rulesSectionHeading.AutoSize = true;
        rulesSectionHeading.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Bold);
        rulesSectionHeading.ForeColor = Color.FromArgb(232, 236, 242);
        rulesSectionHeading.Location = new Point(14, 10);
        rulesSectionHeading.Name = "rulesSectionHeading";
        rulesSectionHeading.Size = new Size(106, 31);
        rulesSectionHeading.TabIndex = 0;
        rulesSectionHeading.Text = "规则文本";
        // 
        // rulesTextInput
        // 
        rulesTextInput.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        rulesTextInput.BackColor = Color.FromArgb(15, 19, 24);
        rulesTextInput.BorderStyle = BorderStyle.FixedSingle;
        rulesTextInput.ForeColor = Color.FromArgb(232, 236, 242);
        rulesTextInput.Location = new Point(23, 47);
        rulesTextInput.Multiline = true;
        rulesTextInput.Name = "rulesTextInput";
        rulesTextInput.ScrollBars = ScrollBars.Vertical;
        rulesTextInput.Size = new Size(1422, 288);
        rulesTextInput.TabIndex = 1;
        // 
        // stringsSectionBorder
        // 
        stringsSectionBorder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        stringsSectionBorder.BackColor = Color.FromArgb(62, 70, 82);
        stringsSectionBorder.Controls.Add(stringsSectionBody);
        stringsSectionBorder.Location = new Point(0, 934);
        stringsSectionBorder.Name = "stringsSectionBorder";
        stringsSectionBorder.Padding = new Padding(1);
        stringsSectionBorder.Size = new Size(1447, 390);
        stringsSectionBorder.TabIndex = 4;
        // 
        // stringsSectionBody
        // 
        stringsSectionBody.BackColor = Color.FromArgb(28, 33, 41);
        stringsSectionBody.Controls.Add(stringsSectionHeading);
        stringsSectionBody.Controls.Add(stringsHelpLabel);
        stringsSectionBody.Controls.Add(addPromptButton);
        stringsSectionBody.Controls.Add(deletePromptButton);
        stringsSectionBody.Controls.Add(movePromptUpButton);
        stringsSectionBody.Controls.Add(movePromptDownButton);
        stringsSectionBody.Controls.Add(stringsGrid);
        stringsSectionBody.Dock = DockStyle.Fill;
        stringsSectionBody.Location = new Point(1, 1);
        stringsSectionBody.Name = "stringsSectionBody";
        stringsSectionBody.Size = new Size(1445, 388);
        stringsSectionBody.TabIndex = 0;
        // 
        // stringsSectionHeading
        // 
        stringsSectionHeading.AutoSize = true;
        stringsSectionHeading.Font = new Font("Microsoft YaHei UI", 11.5F, FontStyle.Bold);
        stringsSectionHeading.ForeColor = Color.FromArgb(232, 236, 242);
        stringsSectionHeading.Location = new Point(14, 10);
        stringsSectionHeading.Name = "stringsSectionHeading";
        stringsSectionHeading.Size = new Size(106, 31);
        stringsSectionHeading.TabIndex = 0;
        stringsSectionHeading.Text = "提示文本";
        // 
        // stringsHelpLabel
        // 
        stringsHelpLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        stringsHelpLabel.ForeColor = Color.FromArgb(172, 183, 198);
        stringsHelpLabel.Location = new Point(14, 43);
        stringsHelpLabel.Name = "stringsHelpLabel";
        stringsHelpLabel.Size = new Size(1419, 38);
        stringsHelpLabel.TabIndex = 1;
        stringsHelpLabel.Text = "为 Lua 效果准备可复用的提示文本。str1 对应索引 0，str2 对应索引 1。";
        stringsHelpLabel.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // addPromptButton
        // 
        addPromptButton.BackColor = Color.FromArgb(77, 142, 234);
        addPromptButton.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        addPromptButton.FlatStyle = FlatStyle.Flat;
        addPromptButton.ForeColor = Color.White;
        addPromptButton.Location = new Point(14, 86);
        addPromptButton.Name = "addPromptButton";
        addPromptButton.Size = new Size(82, 34);
        addPromptButton.TabIndex = 2;
        addPromptButton.Text = "添加";
        addPromptButton.UseVisualStyleBackColor = false;
        // 
        // deletePromptButton
        // 
        deletePromptButton.BackColor = Color.FromArgb(130, 52, 62);
        deletePromptButton.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        deletePromptButton.FlatStyle = FlatStyle.Flat;
        deletePromptButton.ForeColor = Color.White;
        deletePromptButton.Location = new Point(102, 86);
        deletePromptButton.Name = "deletePromptButton";
        deletePromptButton.Size = new Size(82, 34);
        deletePromptButton.TabIndex = 3;
        deletePromptButton.Text = "删除";
        deletePromptButton.UseVisualStyleBackColor = false;
        // 
        // movePromptUpButton
        // 
        movePromptUpButton.BackColor = Color.FromArgb(15, 19, 24);
        movePromptUpButton.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        movePromptUpButton.FlatStyle = FlatStyle.Flat;
        movePromptUpButton.ForeColor = Color.White;
        movePromptUpButton.Location = new Point(190, 86);
        movePromptUpButton.Name = "movePromptUpButton";
        movePromptUpButton.Size = new Size(82, 34);
        movePromptUpButton.TabIndex = 4;
        movePromptUpButton.Text = "上移";
        movePromptUpButton.UseVisualStyleBackColor = false;
        // 
        // movePromptDownButton
        // 
        movePromptDownButton.BackColor = Color.FromArgb(15, 19, 24);
        movePromptDownButton.FlatAppearance.BorderColor = Color.FromArgb(62, 70, 82);
        movePromptDownButton.FlatStyle = FlatStyle.Flat;
        movePromptDownButton.ForeColor = Color.White;
        movePromptDownButton.Location = new Point(278, 86);
        movePromptDownButton.Name = "movePromptDownButton";
        movePromptDownButton.Size = new Size(82, 34);
        movePromptDownButton.TabIndex = 5;
        movePromptDownButton.Text = "下移";
        movePromptDownButton.UseVisualStyleBackColor = false;
        // 
        // stringsGrid
        // 
        stringsGrid.AllowUserToAddRows = false;
        stringsGrid.AllowUserToDeleteRows = false;
        stringsGrid.AllowUserToResizeRows = false;
        stringsGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        stringsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        stringsGrid.BackgroundColor = Color.FromArgb(13, 17, 22);
        stringsGrid.ColumnHeadersHeight = 34;
        stringsGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        stringsGrid.Columns.AddRange(new DataGridViewColumn[] { stringNameColumn, stringIndexColumn, stringTextColumn });
        stringsGrid.Location = new Point(14, 128);
        stringsGrid.MultiSelect = false;
        stringsGrid.Name = "stringsGrid";
        stringsGrid.RowHeadersVisible = false;
        stringsGrid.RowHeadersWidth = 62;
        stringsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        stringsGrid.Size = new Size(1419, 246);
        stringsGrid.TabIndex = 6;
        // 
        // stringNameColumn
        // 
        stringNameColumn.FillWeight = 18F;
        stringNameColumn.HeaderText = "标识";
        stringNameColumn.MinimumWidth = 8;
        stringNameColumn.Name = "stringNameColumn";
        stringNameColumn.ReadOnly = true;
        stringNameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // stringIndexColumn
        // 
        stringIndexColumn.FillWeight = 18F;
        stringIndexColumn.HeaderText = "Lua 索引";
        stringIndexColumn.MinimumWidth = 8;
        stringIndexColumn.Name = "stringIndexColumn";
        stringIndexColumn.ReadOnly = true;
        stringIndexColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // stringTextColumn
        // 
        stringTextColumn.HeaderText = "提示文本";
        stringTextColumn.MinimumWidth = 8;
        stringTextColumn.Name = "stringTextColumn";
        stringTextColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        // 
        // BasicCardEditorView
        // 
        AutoScaleDimensions = new SizeF(144F, 144F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(22, 25, 30);
        Controls.Add(rootScrollPanel);
        Font = new Font("Microsoft YaHei UI", 9.8F);
        ForeColor = Color.FromArgb(232, 236, 242);
        Name = "BasicCardEditorView";
        Size = new Size(1500, 1360);
        rootScrollPanel.ResumeLayout(false);
        rootCanvas.ResumeLayout(false);
        previewSectionBorder.ResumeLayout(false);
        previewSectionBody.ResumeLayout(false);
        previewSectionBody.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)cardImagePreview).EndInit();
        identitySectionBorder.ResumeLayout(false);
        identitySectionBody.ResumeLayout(false);
        identitySectionBody.PerformLayout();
        cardTypesSectionBorder.ResumeLayout(false);
        cardTypesSectionBody.ResumeLayout(false);
        cardTypesSectionBody.PerformLayout();
        gameplaySectionBorder.ResumeLayout(false);
        gameplaySectionBody.ResumeLayout(false);
        gameplaySectionBody.PerformLayout();
        powerStatHost.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)powerInput).EndInit();
        toughnessStatHost.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)toughnessInput).EndInit();
        loyaltyStatHost.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)loyaltyInput).EndInit();
        defenseStatHost.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)defenseInput).EndInit();
        rulesSectionBorder.ResumeLayout(false);
        rulesSectionBody.ResumeLayout(false);
        rulesSectionBody.PerformLayout();
        stringsSectionBorder.ResumeLayout(false);
        stringsSectionBody.ResumeLayout(false);
        stringsSectionBody.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)stringsGrid).EndInit();
        ResumeLayout(false);
    }

}
