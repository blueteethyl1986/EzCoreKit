using System;
using System.Collections.Generic;
using System.Reflection;
using EzCoreKit.Rest.Attributes;
using System.Reflection.Emit;
using System.Linq;
using System.Threading.Tasks;

namespace EzCoreKit.Rest {
    //這個部分類別用以實作interface類別，並串接至RestClientBuilder.RestMethods
    public partial class RestClientBuilder<T> {
        /// <summary>
        /// 實作指定的interface類別
        /// </summary>
        /// <returns>實作指定的interface匿名類別</returns>
        private Type ImplementInterface() => ImplementInterface(typeof(T));

        /// <summary>
        /// 實作指定的interface類別
        /// </summary>
        /// <param name="interfaceType">指定的interface類別</param>
        /// <returns>實作指定的interface匿名類別</returns>
        private Type ImplementInterface(Type interfaceType) {
            if (interfaceType == null) {
                throw new ArgumentNullException(nameof(interfaceType));
            }
            if (!interfaceType.IsInterface) {
                throw new ArgumentException($"{nameof(interfaceType)}必須為interface");
            }

            //建構組件
            AssemblyBuilder tempAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName() {
                Name = "TempAssembly"
            }, AssemblyBuilderAccess.RunAndCollect);

            //建構模組
            ModuleBuilder tempModuleBuilder = tempAssemblyBuilder.DefineDynamicModule("TempModule");

            //建構實作介面類別
            TypeBuilder tempTypeBuilder = tempModuleBuilder.DefineType(
                $"Anon_{Guid.NewGuid().ToString().Replace("-", "_")}",
                TypeAttributes.Class,
                typeof(object), Type.EmptyTypes);

            //實作介面類別
            tempTypeBuilder.AddInterfaceImplementation(interfaceType);

            //建立儲存欄位
            FieldBuilder tempFieldBuilder_baseUri = tempTypeBuilder.DefineField("baseUri", typeof(string), FieldAttributes.Private);
            FieldBuilder tempFieldBuilder_auth_username = tempTypeBuilder.DefineField("auth_username", typeof(string), FieldAttributes.Private);
            FieldBuilder tempFieldBuilder_auth_password = tempTypeBuilder.DefineField("auth_password", typeof(string), FieldAttributes.Private);
            FieldBuilder tempFieldBuilder_requestFormat = tempTypeBuilder.DefineField("requestFormat", typeof(string), FieldAttributes.Private);
            //建構子
            ConstructorBuilder tempConstructorBuilder = tempTypeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis, Type.EmptyTypes);

            //設定欄位內容
            var ctorIL = tempConstructorBuilder.GetILGenerator();
            if (baseUri != null) {
                ctorIL.Emit(OpCodes.Ldarg_0);
                ctorIL.Emit(OpCodes.Ldstr, baseUri);
                ctorIL.Emit(OpCodes.Stfld, tempFieldBuilder_baseUri);
            }
            if (auth_username != null) {
                ctorIL.Emit(OpCodes.Ldarg_0);
                ctorIL.Emit(OpCodes.Ldstr, auth_username);
                ctorIL.Emit(OpCodes.Stfld, tempFieldBuilder_auth_username);
            }
            if (auth_password != null) {
                ctorIL.Emit(OpCodes.Ldarg_0);
                ctorIL.Emit(OpCodes.Ldstr, auth_password);
                ctorIL.Emit(OpCodes.Stfld, tempFieldBuilder_auth_password);
            }
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldstr, (int)requestFormat);
            ctorIL.Emit(OpCodes.Stfld, tempFieldBuilder_requestFormat);

            ctorIL.Emit(OpCodes.Ret);

            //取得interfaceType的所有Methods
            var methods = interfaceType?.GetMethods() ?? new MethodInfo[0];

            //循環建立Method Body
            foreach (var method in methods) {
                //取得欲實作的方法參數列表
                var parameters = method.GetParameters();

                //建構方法
                MethodBuilder tempMethodBuilder =
                    tempTypeBuilder.DefineMethod(method.Name,
                    MethodAttributes.Public | MethodAttributes.Virtual,
                    method.ReturnType,
                    parameters.Select(x => x.ParameterType).ToArray());

                ILGenerator il = tempMethodBuilder.GetILGenerator();

                il.Emit(OpCodes.Ldarg, 0);//Push Sender to Parameter 1

                //Push MethodBase.GetCurrentMethod() to Parameter 2
                il.Emit(OpCodes.Call, typeof(MethodBase).GetMethod(nameof(MethodBase.GetCurrentMethod), BindingFlags.Public | BindingFlags.Static));

                //Createn and push object[] to Parameter 3
                il.Emit(OpCodes.Ldc_I4, parameters.Length);
                il.Emit(OpCodes.Newarr, typeof(object));

                for (int i = 0; i < parameters.Length; i++) {
                    il.Emit(OpCodes.Dup);
                    il.Emit(OpCodes.Ldc_I4, i);
                    il.Emit(OpCodes.Ldarg, i + 1);
                    il.Emit(OpCodes.Box, parameters[i].ParameterType);
                    il.Emit(OpCodes.Stelem_Ref);
                }

                if (method.ReturnType?.BaseType == typeof(Task)) {
                    il.Emit(OpCodes.Call, typeof(RestClientBuilder<T>).GetMethod(nameof(RestClientBuilder<T>.RestRequestProcessAsync), BindingFlags.Public | BindingFlags.Static));
                } else {
                    il.Emit(OpCodes.Call, typeof(RestClientBuilder<T>).GetMethod(nameof(RestClientBuilder<T>.RestRequestProcess), BindingFlags.Public | BindingFlags.Static));
                }
                il.Emit(OpCodes.Ret); ;


                //Override Interface Define Method
                tempTypeBuilder.DefineMethodOverride(
                    tempMethodBuilder,
                    method
                );
            }

            return tempTypeBuilder.CreateTypeInfo();
        }
    }
}