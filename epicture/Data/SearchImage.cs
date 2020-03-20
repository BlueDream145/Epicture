using System;
using System.Collections.Generic;
using System.Text;

namespace epicture.Data
{
    public class SearchImage
    {

        #region "Variables"

        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime Time { get; set; }

        public string Source { get; set; }

        public string Type { get; set; }

        public bool Favorite { get; set; }

        #endregion

        #region "Builder"

        public SearchImage()
        { }

        public SearchImage(string _id, string _title, string _description, string _time, string _link, string _favorite, string _type)
        {
            Id = _id;
            Title = _title;
            Description = _description;
            Source = _link;
            Type = _type;
            try
            {
                if (_favorite == "true")
                    Favorite = true;
                else if (_favorite == "false")
                    Favorite = false;
                Time = new DateTime(Convert.ToInt32(_time));
            } catch { }
        }

        #endregion
    }
}
