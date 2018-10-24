using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoPacket
{
    class VersionModel
    {
        public string LastTime { set; get; }
        public string Version { set; get; }
        public string PubNumber { set; get; }
        public string ForceUpdate { set; get; }
        public string WholeUpdate { set; get; }
        public Dictionary<string, FileDetails> fileDic { set; get; }

        public VersionModel()
        {
            fileDic = new Dictionary<string, FileDetails>();
            LastTime = "";
            Version = "0";
            PubNumber = "0";
            ForceUpdate = "0";
            WholeUpdate = "0";
        }
        
        public class FileDetails
        {
            public string version;
            public string lastime;
            public string pubnumber;
        }
    }
}
