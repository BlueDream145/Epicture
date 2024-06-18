using System;
using System.Collections.Generic;
using System.Text;

namespace epicture.Data
{
    public class FavoriteImage
    {

        #region "Variables"

        public string Id { get; set; }

        public string Type { get; set; }

        public DateTime Time { get; set; }

        public string Source { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        #endregion

        #region "Builder"

        public FavoriteImage()
        { }

        public FavoriteImage(string _id, string _type, string _time, string _source, string _width, string _height)
        {
            Id = _id;
            Type = _type;
            Source = _source;
            try
            {
                if (_time.Length != 0)
                    Time = new DateTime(Convert.ToInt32(_time));
                Width = Convert.ToInt32(_width);
                Height = Convert.ToInt32(_height);
            } catch { }
        }

        #endregion
    }
}
