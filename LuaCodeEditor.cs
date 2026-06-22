using System.Drawing;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MTGB.CardDatabaseEditor;

internal sealed class LuaCodeEditor : UserControl
{
    private const int WmSetRedraw = 0x000B;
    private const int EmGetFirstVisibleLine = 0x00CE;
    private const int EmLineScroll = 0x00B6;

    private static readonly Regex KeywordRegex = new(
        @"\b(and|break|do|else|elseif|end|false|for|function|goto|if|in|local|nil|not|or|repeat|return|then|true|until|while)\b",
        RegexOptions.Compiled);
    private static readonly Regex BuiltinRegex = new(
        @"\b(assert|collectgarbage|dofile|error|getmetatable|ipairs|load|loadfile|next|pairs|pcall|print|rawequal|rawget|rawlen|rawset|require|select|setmetatable|tonumber|tostring|type|xpcall)\b",
        RegexOptions.Compiled);
    private static readonly Regex NumberRegex = new(
        @"(?<![\w.])(?:0[xX][0-9a-fA-F]+|\d+(?:\.\d+)?)(?![\w.])",
        RegexOptions.Compiled);
    private static readonly Regex StringRegex = new(
        @"(?:""(?:\\.|[^""\\])*""|'(?:\\.|[^'\\])*')",
        RegexOptions.Compiled);
    private static readonly Regex LongStringRegex = new(
        @"\[\[(?:.|\r|\n)*?\]\]",
        RegexOptions.Compiled);
    private static readonly Regex CommentRegex = new(
        @"--(?:\[\[(?:.|\r|\n)*?\]\]|[^\r\n]*)",
        RegexOptions.Compiled);

    private readonly Panel lineNumberPanel = new();
    private readonly RichTextBox editor = new();
    private readonly System.Windows.Forms.Timer highlightTimer = new();
    private bool settingText;
    private bool highlighting;

    public LuaCodeEditor()
    {
        BackColor = EditorTheme.Input;
        Padding = new Padding(0);

        lineNumberPanel.Dock = DockStyle.Left;
        lineNumberPanel.Width = 54;
        lineNumberPanel.BackColor = Color.FromArgb(25, 29, 35);
        lineNumberPanel.Paint += DrawLineNumbers;

        editor.Dock = DockStyle.Fill;
        editor.BorderStyle = BorderStyle.None;
        editor.BackColor = EditorTheme.Input;
        editor.ForeColor = Color.FromArgb(220, 225, 233);
        editor.Font = new Font("Consolas", 11f);
        editor.AcceptsTab = true;
        editor.WordWrap = false;
        editor.DetectUrls = false;
        editor.HideSelection = false;
        editor.ScrollBars = RichTextBoxScrollBars.Both;
        editor.TextChanged += EditorTextChanged;
        editor.VScroll += (_, _) => lineNumberPanel.Invalidate();
        editor.Resize += (_, _) => lineNumberPanel.Invalidate();
        editor.SelectionChanged += (_, _) => lineNumberPanel.Invalidate();
        editor.KeyDown += EditorKeyDown;

        editor.ContextMenuStrip = BuildContextMenu();

        highlightTimer.Interval = 300;
        highlightTimer.Tick += (_, _) =>
        {
            highlightTimer.Stop();
            ApplySyntaxHighlighting();
        };

        Controls.Add(editor);
        Controls.Add(lineNumberPanel);
    }

    public event EventHandler? CodeChanged;
    public event EventHandler? SaveRequested;

    public string CodeText
    {
        get => editor.Text;
        set
        {
            settingText = true;
            editor.Text = value ?? string.Empty;
            editor.SelectionStart = 0;
            editor.SelectionLength = 0;
            settingText = false;
            ApplySyntaxHighlighting();
            lineNumberPanel.Invalidate();
        }
    }

    public bool EditorReadOnly
    {
        get => editor.ReadOnly;
        set => editor.ReadOnly = value;
    }

    public void FocusEditor() => editor.Focus();

    private ContextMenuStrip BuildContextMenu()
    {
        var menu = new ContextMenuStrip
        {
            BackColor = EditorTheme.Surface,
            ForeColor = EditorTheme.Text,
            Renderer = new ToolStripProfessionalRenderer(new DarkColorTable())
        };
        menu.Items.Add("撤销", null, (_, _) => editor.Undo());
        menu.Items.Add("重做", null, (_, _) => editor.Redo());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("剪切", null, (_, _) => editor.Cut());
        menu.Items.Add("复制", null, (_, _) => editor.Copy());
        menu.Items.Add("粘贴", null, (_, _) => editor.Paste());
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add("全选", null, (_, _) => editor.SelectAll());
        return menu;
    }

    private void EditorTextChanged(object? sender, EventArgs e)
    {
        lineNumberPanel.Invalidate();
        if (!settingText && !highlighting)
        {
            highlightTimer.Stop();
            highlightTimer.Start();
            CodeChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void EditorKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Control && e.KeyCode == Keys.S)
        {
            e.SuppressKeyPress = true;
            SaveRequested?.Invoke(this, EventArgs.Empty);
            return;
        }

        if (e.KeyCode == Keys.Tab && !e.Control && !e.Alt)
        {
            e.SuppressKeyPress = true;
            if (e.Shift)
                UnindentSelection();
            else
                IndentSelection();
        }
    }

    private void IndentSelection()
    {
        if (editor.SelectionLength == 0)
        {
            editor.SelectedText = "    ";
            return;
        }

        int start = editor.SelectionStart;
        int originalLength = editor.SelectionLength;
        int end = start + originalLength;
        int firstLine = editor.GetLineFromCharIndex(start);
        int lastLine = editor.GetLineFromCharIndex(Math.Max(start, end - 1));
        for (int line = lastLine; line >= firstLine; line--)
        {
            int index = editor.GetFirstCharIndexFromLine(line);
            editor.Select(index, 0);
            editor.SelectedText = "    ";
        }

        int added = (lastLine - firstLine + 1) * 4;
        editor.Select(start + 4, originalLength + added - 4);
    }

    private void UnindentSelection()
    {
        int start = editor.SelectionStart;
        int originalLength = editor.SelectionLength;
        int end = start + Math.Max(originalLength, 1);
        int firstLine = editor.GetLineFromCharIndex(start);
        int lastLine = editor.GetLineFromCharIndex(Math.Max(start, end - 1));
        int removedBeforeStart = 0;
        int removedTotal = 0;

        for (int line = lastLine; line >= firstLine; line--)
        {
            int index = editor.GetFirstCharIndexFromLine(line);
            if (index < 0) continue;
            int count = 0;
            while (count < 4 && index + count < editor.TextLength && editor.Text[index + count] == ' ')
                count++;
            if (count == 0) continue;

            editor.Select(index, count);
            editor.SelectedText = string.Empty;
            removedTotal += count;
            if (index < start) removedBeforeStart += count;
        }

        editor.Select(
            Math.Max(0, start - removedBeforeStart),
            Math.Max(0, originalLength - removedTotal + removedBeforeStart));
    }

    private void DrawLineNumbers(object? sender, PaintEventArgs e)
    {
        e.Graphics.Clear(lineNumberPanel.BackColor);
        using var numberBrush = new SolidBrush(Color.FromArgb(112, 123, 139));
        using var separatorPen = new Pen(EditorTheme.Border);
        e.Graphics.DrawLine(separatorPen, lineNumberPanel.Width - 1, 0, lineNumberPanel.Width - 1, lineNumberPanel.Height);

        int firstChar = editor.GetCharIndexFromPosition(new Point(0, 0));
        int firstLine = Math.Max(0, editor.GetLineFromCharIndex(firstChar));
        int lastChar = editor.GetCharIndexFromPosition(new Point(editor.ClientSize.Width, editor.ClientSize.Height));
        int lastLine = Math.Max(firstLine, editor.GetLineFromCharIndex(lastChar));
        lastLine = Math.Min(lastLine + 1, Math.Max(0, editor.Lines.Length - 1));

        for (int line = firstLine; line <= lastLine; line++)
        {
            int charIndex = editor.GetFirstCharIndexFromLine(line);
            if (charIndex < 0) continue;
            Point position = editor.GetPositionFromCharIndex(charIndex);
            string number = (line + 1).ToString();
            SizeF size = e.Graphics.MeasureString(number, editor.Font);
            e.Graphics.DrawString(
                number,
                editor.Font,
                numberBrush,
                lineNumberPanel.Width - size.Width - 9,
                position.Y + 1);
        }
    }

    private void ApplySyntaxHighlighting()
    {
        if (highlighting || editor.TextLength == 0)
            return;

        highlighting = true;
        int selectionStart = editor.SelectionStart;
        int selectionLength = editor.SelectionLength;
        int firstVisibleLine = (int)SendMessage(editor.Handle, EmGetFirstVisibleLine, IntPtr.Zero, IntPtr.Zero);

        SendMessage(editor.Handle, WmSetRedraw, IntPtr.Zero, IntPtr.Zero);
        try
        {
            editor.SelectAll();
            editor.SelectionColor = Color.FromArgb(220, 225, 233);

            ApplyColor(KeywordRegex, Color.FromArgb(218, 142, 255));
            ApplyColor(BuiltinRegex, Color.FromArgb(104, 190, 255));
            ApplyColor(NumberRegex, Color.FromArgb(247, 191, 103));
            ApplyColor(LongStringRegex, Color.FromArgb(151, 205, 135));
            ApplyColor(StringRegex, Color.FromArgb(151, 205, 135));
            ApplyColor(CommentRegex, Color.FromArgb(104, 132, 108));

            editor.Select(Math.Min(selectionStart, editor.TextLength),
                Math.Min(selectionLength, Math.Max(0, editor.TextLength - selectionStart)));
        }
        finally
        {
            int currentFirstVisibleLine = (int)SendMessage(
                editor.Handle,
                EmGetFirstVisibleLine,
                IntPtr.Zero,
                IntPtr.Zero);
            SendMessage(
                editor.Handle,
                EmLineScroll,
                IntPtr.Zero,
                new IntPtr(firstVisibleLine - currentFirstVisibleLine));
            SendMessage(editor.Handle, WmSetRedraw, new IntPtr(1), IntPtr.Zero);
            editor.Invalidate();
            lineNumberPanel.Invalidate();
            highlighting = false;
        }
    }

    private void ApplyColor(Regex regex, Color color)
    {
        foreach (Match match in regex.Matches(editor.Text))
        {
            editor.Select(match.Index, match.Length);
            editor.SelectionColor = color;
        }
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);
}
