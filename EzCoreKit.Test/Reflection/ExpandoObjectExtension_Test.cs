using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using EzCoreKit.Reflection;
using Xunit;
using System.Linq;
using System.Diagnostics;

namespace EzCoreKit.Test.Reflection {
    public class ExpandoObjectExtension_Test {
        [Fact(DisplayName = "Reflection.CreateAnonymousType")]
        public void CreateAnonymousType_Test() {
            dynamic obj1 = new ExpandoObject();
            obj1.Id = 12;
            obj1.Name = "name";
            obj1.Class = "class";

            var expObj = (ExpandoObject)obj1;

            Assert.Equal(
                expObj.CreateAnonymousType().GetProperties().Select(x => x.Name).ToArray(),
                new string[] { "Id", "Name", "Class" });
        }
        public interface IEcho {
            string Echo(string str);
        }

        [Fact(DisplayName = "Reflection.CreateAnonymousType_Interface")]
        public void CreateAnonymousType_Interface_Test() {
            dynamic obj1 = new ExpandoObject();
            obj1.Echo = new Func<object, string, string>((THIS, x) => x);

            var expObj = (ExpandoObject)obj1;
            var anonType = expObj.CreateAnonymousType<IEcho>();
            var obj2 = (IEcho)Activator.CreateInstance(anonType);
            Console.WriteLine(">>>>>>" + string.Join(",", anonType.GetMethods().Select(X => X.Name)));

            Assert.Equal(obj2.Echo("G"), "G");
        }
    }
}
