using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EzCoreKit.MIME.Tools {
    public class MIMEData {
        public string Summary { get; set; }
        public string Name { get; set; }
        public string MIME { get; set; }

        public string Type => MIME.Split('/').First();
        public string[] FileExts { get; set; }
    }
}
