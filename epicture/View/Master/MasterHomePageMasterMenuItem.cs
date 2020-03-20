using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epicture.View.Master
{

    public class MasterHomePageMasterMenuItem
    {
        public MasterHomePageMasterMenuItem()
        {
            TargetType = typeof(MasterHomePageMasterMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}