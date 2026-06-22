using System.Drawing;

namespace MTGB.CardDatabaseEditor;

internal static class EditorTheme
{
    public static readonly Color Window = Color.FromArgb(22, 25, 30);
    public static readonly Color Surface = Color.FromArgb(29, 33, 40);
    public static readonly Color SurfaceRaised = Color.FromArgb(36, 41, 49);
    public static readonly Color Input = Color.FromArgb(18, 21, 26);
    public static readonly Color Border = Color.FromArgb(62, 70, 82);
    public static readonly Color Text = Color.FromArgb(232, 236, 242);
    public static readonly Color Muted = Color.FromArgb(153, 164, 179);
    public static readonly Color Accent = Color.FromArgb(79, 139, 245);
    public static readonly Color AccentHover = Color.FromArgb(96, 153, 250);
    public static readonly Color Selection = Color.FromArgb(53, 94, 158);
    public static readonly Color Warning = Color.FromArgb(242, 183, 76);
    public static readonly Color Danger = Color.FromArgb(224, 94, 94);
    public static readonly Color Success = Color.FromArgb(95, 196, 132);

    public static void Apply(Control root)
    {
        if (root is Form)
            root.BackColor = Window;
        else if (root is TabPage)
            root.BackColor = Window;
        root.ForeColor = Text;

        switch (root)
        {
            case TextBoxBase textBox:
                textBox.BackColor = Input;
                textBox.ForeColor = Text;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                break;
            case NumericUpDown numeric:
                numeric.BackColor = Input;
                numeric.ForeColor = Text;
                numeric.BorderStyle = BorderStyle.FixedSingle;
                break;
            case CheckedListBox checkedList:
                checkedList.BackColor = Input;
                checkedList.ForeColor = Text;
                checkedList.BorderStyle = BorderStyle.FixedSingle;
                break;
            case ListView list:
                list.BackColor = Input;
                list.ForeColor = Text;
                list.BorderStyle = BorderStyle.FixedSingle;
                break;
            case DataGridView grid:
                StyleGrid(grid);
                break;
            case Button button:
                StyleButton(
                    button,
                    accent: Equals(button.Tag, "accent"),
                    danger: Equals(button.Tag, "danger"));
                break;
            case Label label when label.ForeColor == SystemColors.ControlText:
                label.ForeColor = Text;
                break;
        }

        foreach (Control child in root.Controls)
            Apply(child);
    }

    public static void StyleButton(Button button, bool accent = false, bool danger = false)
    {
        button.FlatStyle = FlatStyle.Flat;
        button.FlatAppearance.BorderSize = 1;
        button.FlatAppearance.BorderColor = accent ? Accent : danger ? Danger : Border;
        button.FlatAppearance.MouseOverBackColor = accent ? AccentHover : SurfaceRaised;
        button.FlatAppearance.MouseDownBackColor = accent ? Accent : Input;
        button.BackColor = accent ? Accent : SurfaceRaised;
        button.ForeColor = danger ? Color.FromArgb(255, 170, 170) : Color.White;
        button.Cursor = Cursors.Hand;
        button.Padding = new Padding(8, 0, 8, 0);
    }

    public static void StyleGrid(DataGridView grid)
    {
        grid.BackgroundColor = Input;
        grid.BorderStyle = BorderStyle.FixedSingle;
        grid.GridColor = Border;
        grid.EnableHeadersVisualStyles = false;
        grid.RowHeadersVisible = false;
        grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        grid.ColumnHeadersHeight = 34;
        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = SurfaceRaised,
            ForeColor = Text,
            SelectionBackColor = SurfaceRaised,
            SelectionForeColor = Text,
            Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
            Padding = new Padding(5, 0, 5, 0)
        };
        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = Input,
            ForeColor = Text,
            SelectionBackColor = Selection,
            SelectionForeColor = Color.White,
            Padding = new Padding(5, 2, 5, 2),
            NullValue = string.Empty
        };
        grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(23, 27, 33);
        grid.RowTemplate.Height = 32;
    }
}

internal sealed class DarkColorTable : ProfessionalColorTable
{
    public override Color ToolStripGradientBegin => EditorTheme.Surface;
    public override Color ToolStripGradientMiddle => EditorTheme.Surface;
    public override Color ToolStripGradientEnd => EditorTheme.Surface;
    public override Color ToolStripBorder => EditorTheme.Border;
    public override Color ToolStripDropDownBackground => EditorTheme.Surface;
    public override Color ImageMarginGradientBegin => EditorTheme.Surface;
    public override Color ImageMarginGradientMiddle => EditorTheme.Surface;
    public override Color ImageMarginGradientEnd => EditorTheme.Surface;
    public override Color MenuItemSelected => EditorTheme.SurfaceRaised;
    public override Color MenuItemBorder => EditorTheme.Border;
    public override Color MenuBorder => EditorTheme.Border;
    public override Color ButtonSelectedBorder => EditorTheme.Border;
    public override Color ButtonSelectedGradientBegin => EditorTheme.SurfaceRaised;
    public override Color ButtonSelectedGradientMiddle => EditorTheme.SurfaceRaised;
    public override Color ButtonSelectedGradientEnd => EditorTheme.SurfaceRaised;
    public override Color ButtonPressedGradientBegin => EditorTheme.Input;
    public override Color ButtonPressedGradientMiddle => EditorTheme.Input;
    public override Color ButtonPressedGradientEnd => EditorTheme.Input;
    public override Color SeparatorDark => EditorTheme.Border;
    public override Color SeparatorLight => EditorTheme.Border;
}
