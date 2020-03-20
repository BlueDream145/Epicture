using System;
using System.Collections.Generic;
using System.Text;

namespace epicture
{
    public class Constants
    {

        public static string APIClientId = "91cb5e70c2f40df";

        public static string APIClientSecret = "eaa0c55355703749c3afa6f245a1ab106286b227";

        public static string[] ClientsHosts =
        {
            "https://api.imgur.com/oauth2/authorize", // Authorization
            "https://api.imgur.com/3/account/me/images", // AccountImages
            "https://api.imgur.com/3/gallery/search/time/all/", // GallerySearch
            "https://api.imgur.com/3/upload", // Upload
            "https://api.imgur.com/3/account/me/favorites", // Favorites
            "https://api.imgur.com/3/account/me/image/", // DeleteImage
            "https://api.imgur.com/3/image/" // FavoriteImage

        };

        public static string[] CallBackHosts =
        {
            "https://app.getpostman.com/oauth2/" // Authorization Call Back
        };

        public static string[] Src =
        {
            "http://www.veloclub-chatenay.fr/images/interfaces/chargement.gif" // Loader
        };
    }
}
