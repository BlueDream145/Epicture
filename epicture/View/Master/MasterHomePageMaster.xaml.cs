using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static epicture.Data.ClientData;

namespace epicture.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterHomePageMaster : ContentPage
    {

        #region "Variables"

        private Engine engine;

        public ListView ListView;

        #endregion

        #region "Builder"

        public MasterHomePageMaster()
        {
            InitializeComponent();

            BindingContext = new MasterHomePageMasterViewModel();
            ListView = MenuItemsListView;
            init(App.engine);
        }

        #endregion

        #region "Methods"

        public void init(Engine _engine)
        {
            try
            {
                engine = _engine;
                engine.Data.OnDataChanged += onDataChanged;
                initComponents();
                loadNaviguation();
            } catch { }
        }

        private void initComponents()
        {
        }

        private void loadNaviguation()
        {

        }

        #endregion

        #region "Events"

        private void onDataChanged(object sender, DataChangedEventArgs args)
        {
        }

        #endregion

        class MasterHomePageMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterHomePageMasterMenuItem> MenuItems { get; set; }

            public MasterHomePageMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterHomePageMasterMenuItem>(new[]
                {
                    new MasterHomePageMasterMenuItem { Id = 5, Title = "Galerie", TargetType = typeof(MasterHomePageDetail) },
                    new MasterHomePageMasterMenuItem { Id = 0, Title = "Mes Images", TargetType = typeof(Account.AccountImagesPage) },
                    new MasterHomePageMasterMenuItem { Id = 1, Title = "Mes Favoris", TargetType = typeof(Account.AccountFavoritesPage) },
                    new MasterHomePageMasterMenuItem { Id = 2, Title = "Ajouter une photo", TargetType = typeof(Account.AccountUploadPage) }
                }); ;
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}