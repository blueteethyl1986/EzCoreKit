using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace EzCoreKit.Extensions {
    public static class ExpandoObjectExtension {
        public static Type CreateAnonymousType(this ExpandoObject obj) {
            //建構組件
            AssemblyBuilder tempAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName() {
                Name = "TempAssembly"
            }, AssemblyBuilderAccess.RunAndCollect);

            //建構模組
            ModuleBuilder tempModuleBuilder = tempAssemblyBuilder.DefineDynamicModule("TempModule");

            //建構實作介面類型
            TypeBuilder tempTypeBuilder = tempModuleBuilder.DefineType(
                $"Anon_{Guid.NewGuid().ToString().Replace("-", "_")}",
                TypeAttributes.Class,
                typeof(object), Type.EmptyTypes);

            var dict = obj as IDictionary<string, object>;
            if (dict == null) return null;

            foreach (var keyvalue in dict) {
                var propertyType = keyvalue.Value?.GetType() ?? typeof(object);
                PropertyBuilder property = tempTypeBuilder.DefineProperty(
                    keyvalue.Key,
                    PropertyAttributes.HasDefault,
                    propertyType,
                    null);

                MethodAttributes getSetAttr = MethodAttributes.Public |
                    MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                MethodBuilder mbNumberGetAccessor = tempTypeBuilder.DefineMethod(
                    "get_" + keyvalue.Key,
                    getSetAttr,
                    propertyType,
                    Type.EmptyTypes);

                ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
                numberGetIL.Emit(OpCodes.Ldnull);
                numberGetIL.Emit(OpCodes.Ret);

                property.SetGetMethod(mbNumberGetAccessor);
            }

            return tempTypeBuilder.CreateType();
        }
    }
}
