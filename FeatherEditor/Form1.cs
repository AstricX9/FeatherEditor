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
using WinFormsSyntaxHighlighter;

namespace FeatherEditor
{
    public partial class Form1 : DarkForm
    {
        public Form1()
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
            string htmlFilePath = Path.Combine(Application.StartupPath, "template.html");


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
            Form1 newForm = new Form1();
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
                        string fileName = openFileDialog.FileName;

                        // Check if the selected file is a .fethr file
                        string ext = Path.GetExtension(fileName).ToLower();
                        if (ext == ".fethr")
                        {
                            // Read the content of the file
                            string fileContent = File.ReadAllText(fileName);

                            // Display the content in RichTextBox
                            editor.Text = fileContent;
                        }
                        else
                        {
                            // Handle other file types
                            // For simplicity, you can use the existing code to handle other file types
                            string fileContent = File.ReadAllText(fileName);
                            editor.Text = fileContent;
                        }
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
                        string fileName = saveFileDialog.FileName;

                        // Write content to the file in the custom format
                        using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
                        {
                            // Write your custom format here
                            // For example, you can write the content of the RichTextBox
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
    }
}
