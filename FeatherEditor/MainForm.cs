using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI;
using DarkUI.Forms;
using FeatherEditor.Forms;
using WinFormsSyntaxHighlighter;

namespace FeatherEditor
{
    public partial class MainForm : DarkForm
    {
        public MainForm()
        {
            InitializeComponent();
            var syntaxHighlighter = new SyntaxHighlighter(editor);

            // -------- HTML --------
            // multi-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.FromArgb(117, 113, 94), false, true)); //  brownish-grey for comments

            // single-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"//.*?$", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.FromArgb(117, 113, 94), false, true)); // Same brownish-grey as multi-line comments

            // numbers
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\d+\.\d+|\d+"), new SyntaxStyle(Color.FromArgb(174, 129, 255))); // shade of purple for numbers

            // double quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\""([^""]|\""\"")+\"""), new SyntaxStyle(Color.FromArgb(230, 219, 116))); //  yellow color for strings

            // single quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\'([^']|\'\')+\'"), new SyntaxStyle(Color.FromArgb(230, 219, 116))); // Same yellow as double quote strings

            // HTML tags
            syntaxHighlighter.AddPattern(new PatternDefinition(
                "a", "abbr", "acronym", "address", "applet", "area", "article", "aside", "audio",
                "b", "base", "basefont", "bdi", "bdo", "big", "blockquote", "body", "br", "button",
                "canvas", "caption", "center", "cite", "code", "col", "colgroup", "data", "datalist",
                "dd", "del", "details", "dfn", "dialog", "dir", "div", "dl", "dt", "em", "embed",
                "fieldset", "figcaption", "figure", "font", "footer", "form", "frame", "frameset",
                "h1", "h2", "h3", "h4", "h5", "h6", "head", "header", "hr", "html",
                "i", "iframe", "img", "input", "ins", "kbd", "label", "legend", "li", "link",
                "main", "map", "mark", "meta", "meter", "nav", "noframes", "noscript", "object",
                "ol", "optgroup", "option", "output", "p", "param", "picture", "pre", "progress",
                "q", "rp", "rt", "ruby",
                "s", "samp", "script", "section", "select", "small", "source", "span", "strike", "strong", "style", "sub", "summary", "sup", "svg",
                "table", "tbody", "td", "template", "textarea", "tfoot", "th", "thead", "time", "title", "tr", "track", "tt",
                "u", "ul",
                "var", "video",
                "wbr"
            ), new SyntaxStyle(Color.FromArgb(249, 38, 114))); //  red for HTML tags

            // HTML attributes (excluding href)
            syntaxHighlighter.AddPattern(new CaseInsensitivePatternDefinition("class", "id", "style"), new SyntaxStyle(Color.FromArgb(102, 217, 239), true, false)); // cyan for attributes

            // href attribute
            syntaxHighlighter.AddPattern(new CaseInsensitivePatternDefinition("href"), new SyntaxStyle(Color.FromArgb(166, 226, 46), true, false)); //  green for href

            // paths/stylesheet references
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\.\/[\w\/]*\.\w+"), new SyntaxStyle(Color.FromArgb(230, 219, 116))); // yellow for paths/stylesheet references

            // operators
            syntaxHighlighter.AddPattern(new PatternDefinition("+", "-", ">", "<", "&", "|"), new SyntaxStyle(Color.FromArgb(192, 192, 192))); // silver for operators




        }

        private string currentFilePath;
        private string htmlContent;
        private string cssContent;
        private string jsContent;

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            editor.Paste();
        }

        private void pasteHTML5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string htmlFilePath = Path.Combine(Application.StartupPath, "./Boilertemplates/template.html");


            try
            {
                // Read the content of the HTML file
                string htmlContent = File.ReadAllText(htmlFilePath);

                // Set the content to your control (e.g., a text box)
                editor.Text = htmlContent;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading HTML file: " + ex.Message);
            }
        }

        private void windowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm newForm = new MainForm();
            newForm.Show();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the file filters
                openFileDialog.Filter = "Feather Files (*.fethr)|*.fethr|HTML Files (*.html;*.htm)|*.html;*.htm|Text Files (*.txt)|*.txt|CSS Files (*.css)|*.css|JavaScript Files (*.js)|*.js|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        currentFilePath = openFileDialog.FileName;

                        // Read the content of the file
                        string fileContent = File.ReadAllText(currentFilePath);

                        // Display the content in RichTextBox
                        editor.Text = fileContent;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private string GetFileContent(string filePath)
        {
            try
            {
                return File.ReadAllText(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error reading file {filePath}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }


        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set the default file extension and filter
                saveFileDialog.DefaultExt = "fethr";
                saveFileDialog.Filter = "Feather Files (*.fethr)|*.fethr|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        currentFilePath = saveFileDialog.FileName;

                        // Write content to the file in the custom format
                        using (StreamWriter writer = new StreamWriter(currentFilePath, false, System.Text.Encoding.UTF8))
                        {
                            writer.Write(editor.Text);
                        }

                        MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void aboutFeatherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void whatsNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Whats_New wnForm = new Whats_New();
            wnForm.Show();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                SaveAs();
            }
            else
            {
                SaveFile(currentFilePath);
            }
        }

        private void SaveAs()
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.DefaultExt = "fethr";
                saveFileDialog.Filter = "Feather Files (*.fethr)|*.fethr|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog.FileName;
                    SaveFile(currentFilePath);
                }
            }
        }

        private void SaveFile(string filePath)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8))
                {
                    writer.Write(editor.Text);
                }

                MessageBox.Show("File saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void runHTMLPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Open a new form with WebView2 control
            Preview webViewForm = new Preview();

            // Combine the CSS and JS content with the HTML content
            string combinedHtml = editor.Text;

            // Inject CSS
            if (!string.IsNullOrEmpty(cssContent))
            {
                combinedHtml = Regex.Replace(combinedHtml, @"<head>", $"<head>{Environment.NewLine}<style>{cssContent}</style>");
            }

            // Inject JS
            if (!string.IsNullOrEmpty(jsContent))
            {
                combinedHtml = Regex.Replace(combinedHtml, @"</body>", $"{Environment.NewLine}<script>{jsContent}</script></body>");
            }

            // Load content into the WebView2 control
            webViewForm.LoadContent(combinedHtml);
            webViewForm.Show();
        }



        private void projectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject npForm = new NewProject();
            npForm.Show();
        }

        public void OpenProject(string projectFolderPath)
        {
            this.Show();

            // Scan for .css and .js files in the project folder
            var cssFiles = Directory.GetFiles(projectFolderPath, "*.css", SearchOption.AllDirectories);
            var jsFiles = Directory.GetFiles(projectFolderPath, "*.js", SearchOption.AllDirectories);

            // Combine the content of all CSS and JS files
            cssContent = string.Join(Environment.NewLine, cssFiles.Select(GetFileContent));
            jsContent = string.Join(Environment.NewLine, jsFiles.Select(GetFileContent));

            // Optionally, you might want to process the HTML content to inject the CSS and JS references
        }

    }
}
