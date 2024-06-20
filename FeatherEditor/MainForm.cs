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
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"/\*(.|[\r\n])*?\*/", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.DarkSeaGreen, false, true));
            // single-line comments
            syntaxHighlighter.AddPattern(new PatternDefinition(new Regex(@"//.*?$", RegexOptions.Multiline | RegexOptions.Compiled)), new SyntaxStyle(Color.Green, false, true));
            // numbers
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\d+\.\d+|\d+"), new SyntaxStyle(Color.Purple));
            // double quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\""([^""]|\""\"")+\"""), new SyntaxStyle(Color.Red));
            // single quote strings
            syntaxHighlighter.AddPattern(new PatternDefinition(@"\'([^']|\'\')+\'"), new SyntaxStyle(Color.Salmon));
            // HTML tags
            syntaxHighlighter.AddPattern(new PatternDefinition("html", "body", "div", "span", "div", "button", "img", "image", "h1", "h2", "h3", "h4"), new SyntaxStyle(Color.Blue));
            // HTML attributes
            syntaxHighlighter.AddPattern(new CaseInsensitivePatternDefinition("class", "id", "style", "href"), new SyntaxStyle(Color.Navy, true, false));
            // operators
            syntaxHighlighter.AddPattern(new PatternDefinition("+", "-", ">", "<", "&", "|"), new SyntaxStyle(Color.Brown));

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
            // Load content from the RichTextBox into the WebView2 control
            webViewForm.LoadContent(editor.Text);
            webViewForm.Show();
        }
    }
}
