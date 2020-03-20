using System;
using System.Collections.Generic;
using System.Text;

namespace epicture.Network
{
    public class ClientGetter
    {

        #region "Variables"

        private ClientNetwork network { get; set; }

        #endregion

        #region "Builder"

        public ClientGetter(ClientNetwork _network)
        {
            network = _network;
        }

        #endregion

        #region "Methods"

        public string GetData(DataEnum type, List<string> values = null)
        {
            try
            {
                string val = "";
                switch (type)
                {
                    case DataEnum.Authentification:
                        val = ClientHandler.HandleAuthorization(network.Engine);
                        break;
                    case DataEnum.AccountImages:
                        val = ClientHandler.HandleAccountImages(network.Engine);
                        break;
                    case DataEnum.GallerySearch:
                        val = ClientHandler.HandleGallerySearch(network.Engine, values);
                        break;
                    case DataEnum.Favorites:
                        val = ClientHandler.HandleFavoritesImage(network.Engine);
                        break;
                    case DataEnum.DeleteImage:
                        val = ClientHandler.HandleImageDeletion(network.Engine, values);
                        break;
                    case DataEnum.FavoriteImage:
                        val = ClientHandler.HandleImageFavorite(network.Engine, values);
                        break;
                    case DataEnum.Upload:
                        val = ClientHandler.HandleImageUpload(network.Engine, values);
                        break;
                    default:
                        break;
                }
                return (val);
            } catch { return (null); }
        }

        public void Dispose()
        {
            network = null;
        }

        #endregion

    }
}
