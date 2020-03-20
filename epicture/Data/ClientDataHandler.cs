using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace epicture.Data
{
    public class ClientDataHandler
    {

        public static void HandleAuthorizationCallBack(ClientData data, string value)
        {
            try
            {
                Uri myUri = new Uri(value);
                string query = myUri.Fragment.Remove(0, 1);
                data.AccessToken = HttpUtility.ParseQueryString(query).Get("access_token");
                data.RefreshToken = HttpUtility.ParseQueryString(query).Get("refresh_token");
                data.AccountUsername = HttpUtility.ParseQueryString(query).Get("account_username");
                data.AccountId = HttpUtility.ParseQueryString(query).Get("account_id");
                int expiration = Convert.ToInt32(HttpUtility.ParseQueryString(query).Get("expires_in"));
                data.AccessExpiration = DateTime.Now.AddSeconds(expiration);
                data.TokenType = HttpUtility.ParseQueryString(query).Get("token_type");
            } catch { }
        }

        public static void HandleAccountImagesCallBack(ClientData data, string value)
        {
            try
            {
                string[] entries = value.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries);
                if (entries.Length == 0 || !entries[entries.Length - 1].Contains("success"))
                {
                    Console.WriteLine("Can't handle account images.");
                    return;
                }
                for (int i = 1; i < entries.Length - 1; i += 2)
                {
                    var jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes("{ " + entries[i] + " }"),
                        new System.Xml.XmlDictionaryReaderQuotas());
                    var root = XElement.Load(jsonReader);
                    ClientImage img = new ClientImage(root.XPathSelectElement("//id").Value,
                        root.XPathSelectElement("//type").Value,
                        root.XPathSelectElement("//datetime").Value,
                        root.XPathSelectElement("//link").Value,
                        root.XPathSelectElement("//width").Value,
                        root.XPathSelectElement("//height").Value,
                        root.XPathSelectElement("//deletehash").Value);
                    data.Images.Add(img);
                }
            } catch { }
        }

        private static FavoriteImage favoriteImageArray(XElement root)
        {
            string link = "";
            bool found = false;
            FavoriteImage img = new FavoriteImage();
            try
            {
                IEnumerable<XElement> items = root.XPathSelectElement("//images").Elements();
                foreach (XElement item in items)
                {
                    foreach (XElement attr in items.Elements())
                    {
                        if ((attr.Name == "link" || attr.Value.StartsWith("https://i.imgur.com/")) && !attr.Value.Contains("."))
                            break;
                        if (attr.Name == "link" || attr.Value.StartsWith("https://i.imgur.com/"))
                        {
                            img.Source = attr.Value;
                            found = true;
                        }
                        if (attr.Name == "id")
                            img.Id = attr.Value;
                        if (attr.Name == "datetime")
                            img.Time = new DateTime(Convert.ToInt32(attr.Value));
                        if (attr.Name == "type")
                            img.Type = attr.Value;
                    }
                    if (found)
                        break;
                }
            }
            catch
            {
                found = false;
            }
            return (img);
        }


        public static void HandleAccountFavoritesCallBack(ClientData data, string value)
        {
            try
            {
                if (!value.Contains("success"))
                {
                    Console.WriteLine("Can't handle favorites images.");
                    return;
                }
                value = value.Remove(0, 37);
                value = value.Remove(value.Length - 4, 4);
                string[] entries = value.Split(new string[] { ",\"size\":0}," }, StringSplitOptions.RemoveEmptyEntries);
                if (entries.Length == 0)
                {
                    Console.WriteLine("Can't handle favorites images.");
                    return;
                }
                for (int i = 0; i < entries.Length; i++)
                {
                    XmlDictionaryReader jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(entries[i] + "}"),
                        new System.Xml.XmlDictionaryReaderQuotas());
                    XElement root = XElement.Load(jsonReader);
                    FavoriteImage img = favoriteImageArray(root);
                    if ((img == null || img.Source == null || img.Source == string.Empty) && entries[i].Contains("cover"))
                    {
                        string url = "https://i.imgur.com/" + root.XPathSelectElement("//cover").Value + ".png";
                        img = new FavoriteImage(root.XPathSelectElement("//id").Value,
                        root.XPathSelectElement("//type").Value,
                        root.XPathSelectElement("//datetime").Value,
                        url,
                        root.XPathSelectElement("//width").Value,
                        root.XPathSelectElement("//height").Value);
                    }
                    if (img.Source == null)
                        continue;
                    data.Favorites.Add(img);
                }
            } catch { }
        }

        private static SearchImage searchImageArray(XElement root)
        {
            string link = "";
            bool found = false;
            SearchImage img = new SearchImage();
            try
            {
                IEnumerable<XElement> items = root.XPathSelectElement("//images").Elements();
                foreach (XElement item in items)
                {
                    foreach (XElement attr in items.Elements())
                    {
                        if (attr.Name == "link" && !attr.Value.Contains("."))
                            break;
                        if (attr.Name == "link")
                        {
                            img.Source = attr.Value;
                            found = true;
                        }
                        if (attr.Name == "id")
                            img.Id = attr.Value;
                        if (attr.Name == "title")
                            img.Title = attr.Value;
                        if (attr.Name == "description")
                            img.Description = attr.Value;
                        if (attr.Name == "datetime")
                            img.Time = new DateTime(Convert.ToInt32(attr.Value));
                        if (attr.Name == "favorite")
                        {
                            if (attr.Value == "true")
                                img.Favorite = true;
                            else
                                img.Favorite = false;
                        }
                        if (attr.Name == "type")
                            img.Type = attr.Value;
                    }
                    if (found)
                        break;
                }
            }
            catch
            {
                found = false;
            }
            return (img);
        }

        public static List<SearchImage> HandleSearchImagesCallBack(string value)
        {
            try
            {
                List<SearchImage> imgs = new List<SearchImage>();
                JsonConvert.DeserializeObject(value);

                value = value.Remove(0, 9);
                string[] entries = value.Split(new string[] { ",\"showsAds\":false}},", ",\"showsAds\":true}},", ",\"is_album\":false},", ",\"is_album\":true}," }, StringSplitOptions.RemoveEmptyEntries);
                if (entries.Length == 0 || !entries[entries.Length - 1].Contains("success"))
                {
                    Console.WriteLine("Can't handle search images.");
                    return (imgs);
                }
                for (int i = 1; i < entries.Length - 1; i++)
                {
                    string tmp = entries[i] + "}";
                    XmlDictionaryReader jsonReader = JsonReaderWriterFactory.CreateJsonReader(Encoding.UTF8.GetBytes(tmp),
                        new System.Xml.XmlDictionaryReaderQuotas());
                    XElement root = XElement.Load(jsonReader);
                    SearchImage img = searchImageArray(root);
                    if (img == null)
                    {
                        img = new SearchImage(root.XPathSelectElement("//id").Value,
                            root.XPathSelectElement("//title").Value,
                            root.XPathSelectElement("//description").Value,
                            root.XPathSelectElement("//datetime").Value,
                            root.XPathSelectElement("//link").Value,
                            root.XPathSelectElement("//favorite").Value,
                            root.XPathSelectElement("//type").Value);
                    }
                    imgs.Add(img);
                }
                return (imgs);
            }
            catch { return (null); }
        }

    }
}
