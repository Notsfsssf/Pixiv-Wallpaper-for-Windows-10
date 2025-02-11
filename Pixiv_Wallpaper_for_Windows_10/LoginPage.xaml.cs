﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Web.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http.Filters;
using System.Threading.Tasks;
using Pixiv_Wallpaper_for_Windows_10.Model;
using Pixiv_Wallpaper_for_Windows_10.Util;
using Windows.ApplicationModel.Resources;
using Windows.Storage;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Pixiv_Wallpaper_for_Windows_10
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        private static PixivCS.PixivBaseAPI baseAPI;
        private static LoginPage lp;
        private static Conf conf;
        private static Pixiv pixiv;
        private static Frame frame;
        private ResourceLoader loader;
        public LoginPage()
        {
            this.InitializeComponent();
            lp = this;
            frame = Window.Current.Content as Frame;
            loader = ResourceLoader.GetForCurrentView("Resources");
            baseAPI = new PixivCS.PixivBaseAPI();
            baseAPI.ExperimentalConnection = true;
            conf = new Conf();
            lp.webView.ScriptNotify += WebView_ScriptNotify;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoginMethod();
        }

        public static async Task LoginMethod()
        {
            PixivCS.Objects.AuthResult res = null;
            string refreshToken = conf.RefreshToken;
            try
            {
                if (refreshToken != null && !refreshToken.Equals("Invalidation"))
                {
                    res = await baseAPI.AuthAsync(refreshToken);
                    conf.RefreshToken = res.Response.RefreshToken;
                    var currentUser = res.Response.User;
                    pixiv = new Pixiv(baseAPI, currentUser);
                    frame.Navigate(typeof(MainPage), new ValueTuple<Pixiv,Conf>(pixiv, conf));
                }
                else
                {
                    lp.webView.Navigate(baseAPI.GenerateWebViewUri());
                    lp.webView.Visibility = Visibility.Visible;
                }
            }
            catch (Exception)
            {
                //refreshToken失效或是代理+去除SNI导致的认证失败
                try
                {
                    baseAPI.ExperimentalConnection = false;
                    res = await baseAPI.AuthAsync(refreshToken);
                    conf.RefreshToken = res.Response.RefreshToken;
                    var currentUser = res.Response.User;
                    pixiv = new Pixiv(baseAPI, currentUser);
                    frame.Navigate(typeof(MainPage), new ValueTuple<Pixiv, Conf>(pixiv, conf));
                }
                catch(Exception)
                {
                    conf.RefreshToken = "Invalidation";
                    lp.webView.Navigate(baseAPI.GenerateWebViewUri());
                    lp.webView.Visibility = Visibility.Visible;
                }
            }
        }

        /*private async void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(args.Uri.ToString());
            await ClientLogOutput(bytes);
            if (args.Uri.Scheme == "pixiv")
                await GetToken(args.Uri.ToString());
        }*/


        private async void WebView_ScriptNotify(object sender, NotifyEventArgs e)
        {
            Windows.UI.Popups.MessageDialog dialog;
            string[] messageArray = e.Value.Split(':');
            string message;
            string type;
            if (messageArray.Length > 1)
            {
                message = messageArray[1];
                type = messageArray[0];
            }
            else
            {
                message = e.Value;
                type = "typeAlert";
            }
            dialog = new Windows.UI.Popups.MessageDialog(message);
            Console.WriteLine("type=" + type + " ,message=" + message);

            if (type.Equals("typeConfirm"))
            {
                dialog.Commands.Add(new UICommand("Yes"));
                dialog.Commands.Add(new UICommand("Cancel"));
                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;
            }
            var result = await dialog.ShowAsync();
            if (result.Label.Equals("Yes"))
            {
                // do something you want, maybe invoke a script
                //await webView1.InvokeScriptAsync("eval", new string[] { functionString });
            }
            else
            {
                // do something you want, maybe invoke a script
                //await webView1.InvokeScriptAsync("eval", new string[] { functionString });
            }
        }

        public static async Task GetToken(string uri)
        {
            Console.WriteLine(uri);
            try
            {
                string[] uriSplit = uri.Split('=', '&');
                string monitor = uriSplit[1];
                try
                {
                    var res = await baseAPI.Code2Token(uriSplit[1]);
                    var currentUser = res.Response.User;
                    pixiv = new Pixiv(baseAPI, currentUser);
                    conf.RefreshToken = res.Response.RefreshToken;
                    frame.Navigate(typeof(MainPage), new ValueTuple<Pixiv, Conf>(pixiv, conf));
                    lp.webView.Stop();
                }
                catch (Exception)
                {
                    string title = lp.loader.GetString("FailToLoginTry");
                    ToastMessage message = new ToastMessage(title, "", ToastMessage.ToastMode.OtherMessage);
                    message.ToastPush(30);
                    baseAPI.ExperimentalConnection = false;
                    lp.webView.Navigate(baseAPI.GenerateWebViewUri());
                }
                
            }
            catch(Exception)
            {
                string title = lp.loader.GetString("FailToLogin");
                ToastMessage message = new ToastMessage(title, "", ToastMessage.ToastMode.OtherMessage);
                message.ToastPush(30);
            }
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void registration_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
