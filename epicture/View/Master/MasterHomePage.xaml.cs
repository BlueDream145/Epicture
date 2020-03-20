using epicture.View.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static epicture.Data.ClientData;

namespace epicture.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterHomePage : MasterDetailPage
    {

        #region "Variables"

        private Engine engine;

        #endregion

        #region "Builder"

        public MasterHomePage()
        {
            engine = App.engine;
            engine.Data.OnDataChanged += onDataChanged;
            InitializeComponent();
            initComponents();
            loadNaviguation();
        }

        #endregion

        #region "Methods"

        private void initComponents()
        {
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void loadNaviguation()
        {

        }

        #endregion

        #region "Events"

        private void onDataChanged(object sender, DataChangedEventArgs args)
        {
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var item = e.SelectedItem as MasterHomePageMasterMenuItem;
                if (item == null)
                    return;

                if (((NavigationPage)Detail).RootPage.GetType() == typeof(AccountUploadPage))
                {
                    AccountUploadPage uploadPage = (AccountUploadPage)(((NavigationPage)Detail).RootPage);
                    if (uploadPage.task != null)
                    {
                        uploadPage.task.Wait();
                        uploadPage.task.Dispose();
                    }
                    uploadPage.running = false;
                }
                if (((NavigationPage)Detail).RootPage.GetType() == typeof(AccountFavoritesPage))
                {
                    AccountFavoritesPage favPage = (AccountFavoritesPage)(((NavigationPage)Detail).RootPage);
                    if (favPage.task != null)
                    {
                        favPage.task.Wait();
                        favPage.task.Dispose();
                    }
                    favPage.running = false;
                }
                if (((NavigationPage)Detail).RootPage.GetType() == typeof(AccountImagesPage))
                {
                    AccountImagesPage imgPage = (AccountImagesPage)(((NavigationPage)Detail).RootPage);
                    if (imgPage.task != null)
                    {
                        imgPage.task.Wait();
                        imgPage.task.Dispose();
                    }
                    imgPage.running = false;
                }
                if (((NavigationPage)Detail).RootPage.GetType() == typeof(MasterHomePageDetail))
                {
                    MasterHomePageDetail detailPage = (MasterHomePageDetail)(((NavigationPage)Detail).RootPage);
                    if (detailPage.task != null)
                    {
                        detailPage.task.Wait();
                        detailPage.task.Dispose();
                    }
                    detailPage.running = false;
                }
                if (((NavigationPage)Detail).RootPage.GetType() == item.TargetType)
                    return;
                var page = (Page)Activator.CreateInstance(item.TargetType);
                page.Title = item.Title;

                Detail = new NavigationPage(page);
                IsPresented = false;

                MasterPage.ListView.SelectedItem = null;
            }
            catch { }
        }

        #endregion
    }
}