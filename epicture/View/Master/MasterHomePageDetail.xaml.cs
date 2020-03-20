using epicture.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static epicture.Data.ClientData;

namespace epicture.View.Master
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterHomePageDetail : ContentPage
    {

        #region "Variables"

        private Engine engine;

        private Label labelHeader;

        private Entry entrySearch;

        private Label titleLabel;

        private Label descriptionLabel;

        private Image img;

        private List<SearchImage> imgs;

        private Button prevButton;

        private Button nextButton;

        private Button searchButton;

        public bool running = false;

        public Task task;

        private int position;

        private int page;

        #endregion

        #region "Builder"

        public MasterHomePageDetail()
        {
            InitializeComponent();
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
            try
            {
                img = (Image)FindByName("ViewerImage");
                entrySearch = (Entry)FindByName("EntrySearch");
                labelHeader = (Label)FindByName("LabelHeader");
                prevButton = (Button)FindByName("PreviousButton");
                nextButton = (Button)FindByName("NextButton");
                titleLabel = (Label)FindByName("TitleLabel");
                descriptionLabel = (Label)FindByName("DescriptionLabel");
                searchButton = (Button)FindByName("SearchButton");
            } catch { }
        }

        private void loadNaviguation()
        {
            try
            {
                labelHeader.Text = "Bonjour " + engine.Data.AccountUsername + ", vous êtes connecté !";
            } catch { }
        }

        private bool isImage(string val)
        {
            try
            {
                string[] extensions = new string[] {
                    "jpg",
                    "jpeg",
                    "png",
                    "gif"
                };
                string v = val.ToLower();
                for (int i = 0; i < extensions.Length - 1; i++)
                    if (val.Contains(extensions[i]))
                        return (true);
                return (false);
            } catch { return (false);  }
        }

        private void search()
        {
            try
            {
                if (running)
                    return;
                titleLabel.IsVisible = true;
                descriptionLabel.IsVisible = true;
                running = true;
                task = new Task(() => searchProcess());
                task.Start();
            } catch { }
        }

        private void searchProcess()
        {
            running = true;
            try
            {
                string dat = engine.Network.Getter.GetData(DataEnum.GallerySearch, new List<string>() { page.ToString(), entrySearch.Text });
                imgs = ClientDataHandler.HandleSearchImagesCallBack(dat);
                position = 0;
                page = 0;
                loadImageProcess(1);
            } catch
            {
                running = false;
            }
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    searchButton.IsVisible = true;
                });
            } catch { }
            running = false;
        }

        private void favoriteImage()
        {
            try
            {
                if (running)
                    return;
                running = true;
                task = new Task(() => favoriteImageProcess());
                task.Start();
            } catch { }
        }

        private void favoriteImageProcess()
        {
            try
            {
                if (imgs.Count == 0)
                {
                    running = false;
                    return;
                }
                //string resp = engine.Network.Getter.GetData(DataEnum.FavoriteImage, new List<string>() { imgs[position].Id.ToString() });

                //using (var w = new WebClient())
                //{
                //    var values = new NameValueCollection
                //    {
                //        { "Authorization", "Bearer " + engine.Data.AccessToken },
                //    };
                //    w.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + engine.Data.AccessToken);
                //    byte[] response = w.UploadValues("https://api.imgur.com/3/image/", values);

                //};
                engine.Data.Favorites.Add(new FavoriteImage(imgs[position].Id, "", "", imgs[position].Source, "0", "0"));
                engine.Data.DataChanged(engine.Data, new DataChangedEventArgs(DataEnum.FavoriteImage));
            }
            catch
            {
                running = false;
            }
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    FavoriteButton.Text = "X";
                    PreviousButton.IsVisible = true;
                    NextButton.IsVisible = true;
                    FavoriteButton.IsVisible = true;
                });
            } catch { running = false;  }
            running = false;
        }

        private void loadImage(int val)
        {
            try
            {
                if (running)
                    return;
                running = true;
                task = new Task(() => loadImageProcess(val));
                task.Start();
            } catch { }
        }

        private void loadImageProcess(int val)
        {
            try
            {
                if (imgs.Count == 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        titleLabel.Text = "Il n'y a aucune image !";
                    });
                    running = false;
                    return;
                }
                if (position > imgs.Count - 1)
                {
                    page++;
                    string dat = engine.Network.Getter.GetData(DataEnum.GallerySearch, new List<string>() { page.ToString(), entrySearch.Text });
                    List<SearchImage> tmp = ClientDataHandler.HandleSearchImagesCallBack(dat);
                    imgs = imgs.Concat(tmp).ToList();
                    loadImageProcess(val);
                    return;
                }
                else if (position < 0)
                    position = imgs.Count - 1;
                Uri ur = new Uri(imgs[position].Source);
                if (!isImage(ur.AbsolutePath))
                {
                    position += val;
                    loadImageProcess(val);
                    return;
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        img.Source = ImageSource.FromStream(() =>
                        {
                            WebClient client = new WebClient();
                            Stream stream = client.OpenRead(imgs[position].Source);
                            client.Dispose();
                            return (stream);
                        });
                    }
                    catch
                    {
                        loadImageProcess(val);
                        return;
                    }
                    titleLabel.Text = "Titre: " + imgs[position].Title;
                    descriptionLabel.Text = imgs[position].Description;
                });
            } catch
            {
                running = false;
            }
            try
            {
                FavoriteImage fav = engine.Data.Favorites.FirstOrDefault(f => f.Id == imgs[position].Id);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    prevButton.IsVisible = true;
                    nextButton.IsVisible = true;
                    FavoriteButton.IsVisible = true;
                    if (fav == default(FavoriteImage))
                        FavoriteButton.Text = "Favoris";
                    else
                        FavoriteButton.Text = "X";
                });
            } catch { }
            running = false;
        }

        #endregion

        #region "Events"

        private void onPreviousClicked(object obj, EventArgs args)
        {
            try
            {
                if (running)
                    return;
                prevButton.IsVisible = false;
                nextButton.IsVisible = false;
                FavoriteButton.IsVisible = false;
                img.Source = ImageSource.FromStream(() =>
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(Constants.Src[0]);
                    client.Dispose();
                    return (stream);
                });
                position--;
                loadImage(-1);
            } catch { }
        }

        private void onNextClicked(object obj, EventArgs args)
        {
            try
            {
                if (running)
                    return;
                prevButton.IsVisible = false;
                nextButton.IsVisible = false;
                FavoriteButton.IsVisible = false;
                img.Source = ImageSource.FromStream(() =>
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(Constants.Src[0]);
                    client.Dispose();
                    return (stream);
                });
                position++;
                loadImage(1);
            } catch { }
        }

        private void onSearchClicked(object obj, EventArgs args)
        {
            try
            {
                if (entrySearch.Text == null || entrySearch.Text.Length == 0 || running)
                    return;
                prevButton.IsVisible = false;
                nextButton.IsVisible = false;
                searchButton.IsVisible = false;
                FavoriteButton.IsVisible = false;
                img.Source = ImageSource.FromStream(() =>
                {
                    WebClient client = new WebClient();
                    Stream stream = client.OpenRead(Constants.Src[0]);
                    client.Dispose();
                    return (stream);
                });
                search();
            } catch { }
        }

        private void onFavoriteClicked(object obj, EventArgs args)
        {
            try
            {
                if (running)
                    return;
                prevButton.IsVisible = false;
                nextButton.IsVisible = false;
                FavoriteButton.IsVisible = false;
                FavoriteImage img = engine.Data.Favorites.FirstOrDefault(f => f.Id == imgs[position].Id);
                if (img != default(FavoriteImage))
                {
                    engine.Data.Favorites.Remove(img);
                    engine.Data.DataChanged(engine.Data, new DataChangedEventArgs(DataEnum.Favorites));
                    FavoriteButton.Text = "Favoris";
                    prevButton.IsVisible = true;
                    nextButton.IsVisible = true;
                    FavoriteButton.IsVisible = true;
                    return;
                }
                favoriteImage();
            } catch { }
        }

        private void onDataChanged(object sender, DataChangedEventArgs args)
        {
            if (args.type == DataEnum.Authentification)
            {
                labelHeader.Text = "Bonjour " + engine.Data.AccountUsername + ", vous êtes connecté !";
            }
        }

        #endregion
    }
}