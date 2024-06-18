using System;
using System.Collections.Generic;
using System.Text;

namespace epicture.Data
{
    public class ClientData
    {

        #region "Variables"

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string TokenType { get; set; }
        public string AccountUsername { get; set; }
        public string AccountId { get; set; }
        public DateTime AccessExpiration { get; set; }
        public List<ClientImage> Images { get; set; }
        public List<FavoriteImage> Favorites { get; set; }

        public delegate void MessageEventHandler(object sender, DataChangedEventArgs args);

        public event MessageEventHandler OnDataChanged;

        #endregion

        #region "Builder"

        public ClientData()
        {
            Images = new List<ClientImage>();
            Favorites = new List<FavoriteImage>();
        }

        #endregion

        #region "Methods"

        public void Clear()
        {
            AccessToken = string.Empty;
            RefreshToken = string.Empty;
            TokenType = string.Empty;
            AccountUsername = string.Empty;
            AccountId = string.Empty;
            Images.Clear();
            Favorites.Clear();
        }

        public void HandleData(DataEnum type, string data)
        {
            try
            {
                switch (type)
                {
                    case DataEnum.Authentification:
                        ClientDataHandler.HandleAuthorizationCallBack(this, data);
                        break;
                    case DataEnum.AccountImages:
                        ClientDataHandler.HandleAccountImagesCallBack(this, data);
                        break;
                    case DataEnum.Favorites:
                        ClientDataHandler.HandleAccountFavoritesCallBack(this, data);
                        break;
                    default:
                        return;
                }
                DataChanged(this, new DataChangedEventArgs(type));
            } catch { }
        }

        public void Dispose()
        {
        }

        #endregion

        #region "Events"

        public class DataChangedEventArgs : EventArgs
        {
            public DataEnum type;

            public DataChangedEventArgs(DataEnum _type)
            { type = _type; }
        };
        public void DataChanged(Object obj, DataChangedEventArgs eventArgs)
        {
            if (OnDataChanged != null)
            {
                OnDataChanged(obj, eventArgs);
            }
        }

        #endregion

    }
}
