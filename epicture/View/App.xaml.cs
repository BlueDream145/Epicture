using epicture.View;
using epicture.View.Master;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static epicture.Data.ClientData;
using static epicture.Engine;

namespace epicture
{
    public partial class App : Application
    {
        public static Engine engine;

        public static MasterHomePage masterPage;

        public static MainPage mainPage;

        public App()
        {
            try
            {
                InitializeComponent();
                engine = new Engine();
                engine.OnStateChanged += OnStateChanged;
                engine.Data.OnDataChanged += onDataChanged;
                mainPage = new MainPage();
                masterPage = new MasterHomePage();
            } catch { }
        }

        protected override void OnStart()
        {
            MainPage = mainPage;
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void OnStateChanged(object sender, StateChangedEventArgs eventArgs)
        {
            try
            {
                switch (engine.State)
                {
                    case EngineState.Connected:
                        MainPage = masterPage;
                        break;
                    default:
                        break;
                }
            }
            catch { }
        }

        private void onDataChanged(object sender, DataChangedEventArgs args)
        {
            try
            {
                if (args.type == DataEnum.Authentification)
                {
                    engine.Data.HandleData(DataEnum.AccountImages, engine.Network.Getter.GetData(DataEnum.AccountImages)); // Update Images
                    engine.Data.HandleData(DataEnum.Favorites, engine.Network.Getter.GetData(DataEnum.Favorites)); // Favorites Images
                }
            }
            catch { }
        }
    }
}
