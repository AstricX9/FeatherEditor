using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI.Forms;
using System.IO;

namespace FeatherEditor.Forms
{
    public partial class NewProject : DarkForm
    {
        public NewProject()
        {
            InitializeComponent();
        }

        private void darkButton1_Click(object sender, EventArgs e)
        {
            // Get the project name from the textbox
            string projectName = txtProjectName.Text.Trim();
            if (string.IsNullOrEmpty(projectName))
            {
                MessageBox.Show("Please enter a project name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select the location for the new project";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = folderBrowserDialog.SelectedPath;

                    try
                    {
                        // Creating project folder and .feathr file code
                        string newProjectFolder = Path.Combine(selectedPath, projectName); // Updated folder name
                        Directory.CreateDirectory(newProjectFolder);

                        string feathrFilePath = Path.Combine(newProjectFolder, projectName + ".fethr"); // Updated file name
                        using (StreamWriter writer = new StreamWriter(feathrFilePath, false, System.Text.Encoding.UTF8))
                        {
                            writer.Write(""); // Write an empty file or initial content if needed
                        }

                        MessageBox.Show("Project created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Close the current application instance
                        Application.Exit();

                        // Start a new instance of the application and pass the newly created project file as an argument
                        System.Diagnostics.Process.Start(Application.ExecutablePath, $"\"{feathrFilePath}\"");
                    }
                    catch (Exception ex)
                    {
                        // Error handling code
                        MessageBox.Show("Error creating project: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }



        private void darkButton3_Click(object sender, EventArgs e)
        {
            // Close the current application instance
            Application.Exit();
        }
    }
}
