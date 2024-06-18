using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static epicture.Data.ClientData;

namespace epicture.View.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountAlbumsPage : ContentPage
    {

        #region "Variables"

        private Engine engine;

        #endregion

        #region "Builder"

        public AccountAlbumsPage()
        {
            InitializeComponent();
            init(App.engine);
        }

        #endregion

        #region "Methods"

        public void init(Engine _engine)
        {
            engine = _engine;
            engine.Data.OnDataChanged += onDataChanged;
            initComponents();
        }

        private void initComponents()
        {

        }


        #endregion

        #region "Events"

        private void onDataChanged(object sender, DataChangedEventArgs args)
        {
        }

        #endregion
    }
}