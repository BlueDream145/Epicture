using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using static epicture.Data.ClientData;

namespace epicture.View.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountImagesPage : ContentPage
    {

        #region "Variables"

        private Engine engine;

        private Image img;

        private Label statusLabel;

        private int position = 0;

        public bool running = false;

        public Task task;

        #endregion

        #region "Builder"

        public AccountImagesPage()
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
            }
            catch { }
        }

        private void initComponents()
        {
            try
            {
                img = (Image)FindByName("ViewerImage");
                statusLabel = (Label)FindByName("StatusLabel");
                loadImage();
            }
            catch { }
        }

        private void deleteImage()
        {
            try
            {
                if (running)
                    return;
                running = true;
                task = new Task(() => deleteImageProcess());
                task.Start();
            }
            catch { }
        }

        private void deleteImageProcess()
        {
            try
            {
                if (engine.Data.Images.Count == 0)
                {
                    running = false;
                    return;
                }
                string resp = engine.Network.Getter.GetData(DataEnum.DeleteImage, new List<string>() { engine.Data.Images[position].DeleteHash.ToString() });
                if (resp.Contains("success"))
                {
                    engine.Data.Images.Remove(engine.Data.Images[position]);
                    loadImageProcess();
                }
                else
                    Console.WriteLine("Can't remove image :(");
            }
            catch
            {
                running = false;
            }
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PreviousButton.IsVisible = true;
                    NextButton.IsVisible = true;
                    RemoveButton.IsVisible = true;
                });
            }
            catch { }
            running = false;
        }

        private void loadImage()
        {
            try
            {
                if (running)
                    return;
                running = true;
                task = new Task(() => loadImageProcess());
                task.Start();
            }
            catch { }
        }

        private void loadImageProcess()
        {
            try
            {
                if (engine.Data.Images.Count == 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        statusLabel.Text = "Vous n'avez aucune image.";
                    });
                    running = false;
                    return;
                }
                if (position > engine.Data.Images.Count - 1)
                    position = 0;
                else if (position < 0)
                    position = engine.Data.Images.Count - 1;
                if (!engine.Data.Images[position].Source.Contains("http"))
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        img.Source = ImageSource.FromFile(engine.Data.Images[position].Source);
                    });
                else
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        img.Source = ImageSource.FromStream(() =>
                        {
                            WebClient client = new WebClient();
                            Stream stream = client.OpenRead(engine.Data.Images[position].Source);
                            client.Dispose();
                            return (stream);
                        });
                    });
            } catch
            {
                running = false;
            }
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    PreviousButton.IsVisible = true;
                    NextButton.IsVisible = true;
                    RemoveButton.IsVisible = true;
                });
            }
            catch { }
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
                PreviousButton.IsVisible = false;
                NextButton.IsVisible = false;
                RemoveButton.IsVisible = false;
                position--;
                loadImage();
            }
            catch { }
        }

        private void onNextClicked(object obj, EventArgs args)
        {
            try
            {
                if (running)
                    return;
                PreviousButton.IsVisible = false;
                NextButton.IsVisible = false;
                RemoveButton.IsVisible = false;
                position++;
                loadImage();
            }
            catch { }
        }

        private void onDeleteClicked(object obj, EventArgs args)
        {
            try
            {
                if (running)
                    return;
                PreviousButton.IsVisible = false;
                NextButton.IsVisible = false;
                RemoveButton.IsVisible = false;
                deleteImage();
            }
            catch { }
        }

        private void onDataChanged(object sender, DataChangedEventArgs args)
        {
            try
            {
                if (args.type == DataEnum.AccountImages)
                {
                    if (engine.Data.Images.Count == 0)
                    {
                        statusLabel.Text = "Vous n'avez aucune image.";
                    }
                    else
                    {
                        if (position == 0)
                            loadImage();
                        statusLabel.Text = "";
                    }
                }
            }
            catch { }
        }

        #endregion
    }
}