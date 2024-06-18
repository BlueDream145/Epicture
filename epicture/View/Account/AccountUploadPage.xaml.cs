using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static epicture.Data.ClientData;

namespace epicture.View.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountUploadPage : ContentPage
    {
        #region "Variables"

        private Engine engine;

        private MediaFile file = null;

        public bool running = false;

        public Task task;

        #endregion

        #region "Builder"

        public AccountUploadPage()
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
                initComponents();
            }
            catch { }
        }

        private void initComponents()
        {
        }

        private void uploadImage()
        {
            if (running)
                return;
            try
            {
                running = true;
                task = new Task(() => uploadImageProcess());
                task.Start();
            } catch { }
        }

        private async void uploadImageProcess()
        {
            try
            {
                string image64 = "";
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    file.GetStream().CopyTo(memoryStream);
                    byte[] result = memoryStream.ToArray();
                    image64 = Convert.ToBase64String(result);
                };

                using (var w = new WebClient())
                {
                    var values = new NameValueCollection
                    {
                        { "Authorization", "Bearer " + engine.Data.AccessToken },
                        { "image", image64 },
                        { "title", TitleEntry.Text }
                    };
                    w.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + engine.Data.AccessToken);
                    byte[] response = w.UploadValues("https://api.imgur.com/3/image", values);
                    engine.Data.Images.Add(new Data.ClientImage("", "", "", file.Path, "0", "0", ""));
                };
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
                running = false;
            }
            running = false;
        }

        private void loadImage()
        {
            try {
                if (running)
                    return;
                running = true;
                task = new Task(() => loadImageProcess());
                task.Start();
            } catch { }
        }

        private async void loadImageProcess()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Error", "This is not support on your device.", "OK");
                    running = false;
                    return;
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                    if (file == null)
                    {
                        running = false;
                        return;
                    }
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        ViewerImage.Source = ImageSource.FromStream(() => file.GetStream());
                    });
                }
            }
            catch
            {
                running = false;
            }
            running = false;
        }

        #endregion

        #region "Events"
        private void onSelectClicked(object obj, EventArgs args)
        {
            try
            {
                if (running)
                    return;
                loadImage();

            }
            catch { }
        }

        private void onUploadClicked(object obj, EventArgs args)
        {
            try
            {
                if (running || file == null)
                    return;
                uploadImage();
            }
            catch { }
        }

        #endregion
    }
}