using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace EzCoreKit.Test.TestModels {
     [XmlType("ReservoirsInformation")]
    public class Reservoirs {
        public string ReservoirName { get; set; }
        public string Location { get; set; }
    }
}