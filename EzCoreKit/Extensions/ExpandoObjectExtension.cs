using EzCoreKit.Reflection;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace EzCoreKit.Extensions {
#if NETCOREAPP2_0
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

                FieldBuilder field = tempTypeBuilder.DefineField("_" + keyvalue.Key, propertyType, FieldAttributes.Private);

                MethodAttributes getSetAttr = MethodAttributes.Public |
                    MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                MethodBuilder mbNumberGetAccessor = tempTypeBuilder.DefineMethod(
                    "get_" + keyvalue.Key,
                    getSetAttr,
                    propertyType,
                    Type.EmptyTypes);

                ILGenerator numberGetIL = mbNumberGetAccessor.GetILGenerator();
                numberGetIL.Emit(OpCodes.Ldarg_0);
                numberGetIL.Emit(OpCodes.Ldfld, field);
                numberGetIL.Emit(OpCodes.Ret);


                MethodBuilder mbNumberSetAccessor = tempTypeBuilder.DefineMethod(
                    "set_" + keyvalue.Key,
                    getSetAttr,
                    typeof(void),
                    new Type[] { typeof(object) });

                ILGenerator numberGetIL2 = mbNumberSetAccessor.GetILGenerator();
                numberGetIL2.Emit(OpCodes.Ldarg_0);
                numberGetIL2.Emit(OpCodes.Ldarg_1);
                numberGetIL2.Emit(OpCodes.Stfld, field);
                numberGetIL2.Emit(OpCodes.Ret);

                property.SetSetMethod(mbNumberSetAccessor);
                property.SetGetMethod(mbNumberGetAccessor);
            }


            var eqObject = (MethodInfo)new Object().GetMember(x => x.Equals(null));
            var eqMethod = tempTypeBuilder.DefineMethod("Equals", eqObject.Attributes, typeof(bool), new Type[] { typeof(object) });
            var eqMethodILG = eqMethod.GetILGenerator();
            eqMethodILG.Emit(OpCodes.Ldarg_0);
            eqMethodILG.Emit(OpCodes.Ldarg_1);
            eqMethodILG.Emit(OpCodes.Call, typeof(ExpandoObjectExtension).GetMethod("Equals", BindingFlags.Public | BindingFlags.Static));
            eqMethodILG.Emit(OpCodes.Ret);

            tempTypeBuilder.DefineMethodOverride(eqMethod, eqObject);


            var hashObject = typeof(object).GetMethod("GetHashCode");
            var hashMethod = tempTypeBuilder.DefineMethod("GetHashCode", hashObject.Attributes, typeof(int), Type.EmptyTypes);
            var hashMethodILG = hashMethod.GetILGenerator();
            hashMethodILG.Emit(OpCodes.Ldarg_0);
            hashMethodILG.Emit(OpCodes.Call, typeof(ExpandoObjectExtension).GetMethod("GetHashCode", BindingFlags.Public | BindingFlags.Static));
            hashMethodILG.Emit(OpCodes.Ret);

            tempTypeBuilder.DefineMethodOverride(hashMethod, hashObject);

            return tempTypeBuilder.CreateType();
        }

        public static int GetHashCode(object obj) {
            var values = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(x => x.GetValue(obj).GetHashCode()).ToArray();
            unchecked {
                int result = 0;
                foreach (int value in values) result += value;
                return result;
            }
        }

        public static bool Equals(object obj1, object obj2) {
            if (obj1 == null || obj2 == null) return false;

            var fields1 = obj1.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(x => x.GetValue(obj1)).ToArray();
            var fields2 = obj1.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Select(x => x.GetValue(obj2)).ToArray();

            if (fields1.Length != fields2.Length) return false;

            for (int i = 0; i < fields1.Length; i++) {
                if (!fields1[i].Equals(fields2[i])) return false;
            }

            return true;
        }
    }
#endif
}