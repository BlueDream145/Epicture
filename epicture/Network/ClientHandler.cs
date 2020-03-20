using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace epicture.Network
{
    public class ClientHandler
    {

        public static string HandleAuthorization(Engine engine)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "client_id", Constants.APIClientId },
                    { "response_type", "token" }
                };
                string resp = engine.Network.MakeGetRequest(values, Constants.ClientsHosts[(int)DataEnum.Authentification], true);
                return (resp);
            } catch { return ("");  }
        }

        public static string HandleFavoritesImage(Engine engine)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + engine.Data.AccessToken }
                };
                string resp = engine.Network.MakeGetRequest(values, Constants.ClientsHosts[(int)DataEnum.Favorites]);
                return (resp);
            } catch { return ("");  }
}

        public static string HandleAccountImages(Engine engine)
        {
            try
            {
                var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + engine.Data.AccessToken }
                };
                string resp = engine.Network.MakeGetRequest(values, Constants.ClientsHosts[(int)DataEnum.AccountImages]);
                return (resp);
            } catch { return ("");  }
}

        public static string HandleGallerySearch(Engine engine, List<string> args)
        {
            try
            {
                if (args.Count < 2)
                    return (null);
                var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + engine.Data.AccessToken }
                };
                string url = Constants.ClientsHosts[(int)DataEnum.GallerySearch] + args[0] + "?q=" + args[1];
                string resp = engine.Network.MakeGetRequest(values, url);
                return (resp);
            }
            catch { return (""); }
        }

        public static string HandleImageDeletion(Engine engine, List<string> args)
        {
            try
            {
                if (args.Count < 1)
                    return (null);
                var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + engine.Data.AccessToken }
                };
                string url = Constants.ClientsHosts[(int)DataEnum.DeleteImage] + args[0];
                string resp = engine.Network.MakeGetRequest(values, url);
                return (resp);
            }
            catch { return (""); }
        }

        public static string HandleImageFavorite(Engine engine, List<string> args)
        {
            try
            {
                if (args.Count < 1)
                    return (null);
                var values = new Dictionary<string, string>
                {
                    { "Authorization", "Bearer " + engine.Data.AccessToken }
                };
                string url = Constants.ClientsHosts[(int)DataEnum.FavoriteImage] + args[0] + "/favorite";
                string resp = engine.Network.MakeGetRequest(values, url);
                return (resp);
            }
            catch { return (""); }
        }

        public static string HandleImageUpload(Engine engine, List<string> args)
        {
            if (args.Count < 2)
                return (null);
            //var values = new NameValueCollection
            //{
            //    { "Authorization", "Bearer " + engine.Data.AccessToken },
            //    { "title", args[1] }
            //};

            //var jsonData = JsonConvert.SerializeObject(new
            //{
            //    Authorization = "Bearer " + engine.Data.AccessToken,
            //    image = args[0]
            //});
            //var jsonContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            //string url = Constants.ClientsHosts[(int)DataEnum.Upload];
            //engine.Network.MakePostRequest(jsonContent, url);
            return ("");
        }


    }
}
