using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.Test.TestModels {
    public class Student {
        private string TestPrivateField = "abc";
        internal string TestInternalField = "def";

        public int Id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }

        public static string TestMethod() {
            return "Test";
        }

        public override string ToString() {
            return Id + "," + Name + "," + Class;
        }
    }
}
