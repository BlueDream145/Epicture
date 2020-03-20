using epicture.Network;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace epicture
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {

        #region "Variables"

        private Engine engine;

        private Label stateLabel;

        private WebView loginViewer;

        #endregion

        #region "Builder"

        public MainPage()
        {
            engine = App.engine;
            InitializeComponent();
            initComponents();
            loadNaviguation();
        }

        #endregion

        #region "Methods"

        private void initComponents()
        {
            loginViewer = (WebView)FindByName("LoginViewer");
            loginViewer.Navigating += webviewNavigating;
            LoginViewer.Navigated += webviewNavigated;
        }

        public void loadNaviguation()
        {
            string val = engine.Network.Getter.GetData(DataEnum.Authentification);
            if (!val.Contains("<!DOCTYPE html PUBLIC "))
                return;
            var values = new Dictionary<string, string>
                {
                    { "client_id", Constants.APIClientId },
                    { "response_type", "token" }
                };
            loginViewer.Source = engine.Network.ConcatUri(values, Constants.ClientsHosts[(int)DataEnum.Authentification]);
        }

        #endregion

        #region "Events"

        void webviewNavigating(object sender, WebNavigatingEventArgs e)
        {

        }

        void webviewNavigated(object sender, WebNavigatedEventArgs e)
        {
            UrlWebViewSource src = (UrlWebViewSource)loginViewer.Source;
            if (src.Url.StartsWith(Constants.CallBackHosts[(int)DataEnum.Authentification]))
            {
                engine.Data.HandleData(DataEnum.Authentification, src.Url);
                engine.State = EngineState.Connected;
                Console.WriteLine("Connected !");
            }
            else if (src.Url.StartsWith(Constants.ClientsHosts[(int)DataEnum.Authentification]))
                engine.State = EngineState.Authentification;
            else
                loadNaviguation();
        }

        #endregion

    }
}
