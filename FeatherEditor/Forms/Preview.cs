using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2;
using System.IO;

namespace FeatherEditor.Forms
{
    public partial class Preview : Form
    {
        public Preview()
        {
            InitializeComponent();
            InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error initializing WebView2: " + ex.Message);
            }
        }

        public async void LoadContent(string htmlContent)
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
                webView21.NavigateToString(htmlContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading content: " + ex.Message);
            }
        }
    }
}
