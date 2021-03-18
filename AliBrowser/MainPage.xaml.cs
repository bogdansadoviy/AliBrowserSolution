using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AliBrowser
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DataAccess dataAccess = new DataAccess();
            dataAccess.CreateSettingsFile();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            if (webBrowser.CanGoBack)
            {
                webBrowser.GoBack();
            }
        }

        private void frdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (webBrowser.CanGoForward)
            {
                webBrowser.GoForward();
            }
        }

        private void SearchBar_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if(e.Key == Windows.System.VirtualKey.Enter)
            {
                Search();
            }
        }

        private void Search()
        {
            webBrowser.Source = new Uri("https://www.google.com/search?q=" + SearchBar.Text);
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            webBrowser.Refresh();
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void MainBrowserWindow_Loading(FrameworkElement sender, object args)
        {
        }

        private void webBrowser_Loading(FrameworkElement sender, object args)
        {
            statusText.Text = webBrowser.Source.AbsoluteUri;
        }

        private void webBrowser_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            bool isSSl = false;

            browserProgress.IsActive = false;

            try
            {
                statusText.Text = webBrowser.Source.AbsoluteUri;
                AppTitle.Text = "Sadoviy Browser" + " | " + webBrowser.DocumentTitle;

                DataTransfer dataTransfer = new DataTransfer();
                if (!string.IsNullOrEmpty(SearchBar.Text))
                {
                    dataTransfer.SaveSearchTerm(SearchBar.Text, webBrowser.DocumentTitle, webBrowser.Source.AbsoluteUri);

                }
            }
            catch 
            {

               
            }

            if (webBrowser.Source.AbsoluteUri.Contains("https"))
            {
                isSSl = true;
                sslIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
                sslIcon.Glyph = "\xE72E";

                ToolTip toolTip = new ToolTip();
                toolTip.Content = "This website has a SSl certifacate.";

                ToolTipService.SetToolTip(sslButton, toolTip);
            }
            else
            {
                isSSl = false;
                sslIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
                sslIcon.Glyph = "\xE785";

                ToolTip toolTip = new ToolTip();
                toolTip.Content = "This website is unsafe and doesn't have a SSL sertificate.";

                ToolTipService.SetToolTip(sslButton, toolTip);
            }
        }

        private void webBrowser_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            browserProgress.IsActive = true;
        }


        private void TabControl_AddTabButtonClick(muxc.TabView sender, object args)
        {
            var newTab = new muxc.TabViewItem();
            newTab.IconSource = new muxc.SymbolIconSource() { Symbol = Symbol.Document };
            newTab.Header = "Blank Page";

            WebView webView = new WebView();

            string path = $"ms-appx-web:///Assets/BlankPage.html";


            newTab.Content = webView;
            webView.Navigate(new Uri(path));

            sender.TabItems.Add(newTab);

            sender.SelectedItem = newTab;
        }
        private void TabControl_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
        {
            sender.TabItems.Remove(args.Tab);
        }

        private void SearchButtom_Click(object sender, RoutedEventArgs e)
        {
            Search();
        }
    }
}
