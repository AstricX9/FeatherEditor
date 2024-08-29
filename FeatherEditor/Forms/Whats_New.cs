using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DarkUI;
using DarkUI.Forms;
using Microsoft.Web.WebView2;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace FeatherEditor.Forms
{
    public partial class Whats_New : DarkForm
    {
        public Whats_New()
        {
            InitializeComponent();
            InitializeWebView();
        }

        private async void InitializeWebView()
        {
            await webView21.EnsureCoreWebView2Async(null);
            webView21.CoreWebView2.Settings.AreDevToolsEnabled = false; // Disable developer tools
            string htmlFilePath = Path.Combine(Application.StartupPath, "./pages/whatsnew.html");
            webView21.CoreWebView2.Navigate(htmlFilePath);

            // Inject JavaScript to disable right-click context menu
            webView21.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        }

        private void CoreWebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            string script = "document.addEventListener('contextmenu', event => event.preventDefault());";
            webView21.CoreWebView2.ExecuteScriptAsync(script);
        }

        private void CoreWebView2_ContextMenuRequested(object sender, CoreWebView2ContextMenuRequestedEventArgs e)
        {
            // Suppress the default context menu
            e.Handled = true;
        }
    }
}
