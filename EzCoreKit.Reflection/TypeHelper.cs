using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EzCoreKit.Reflection {
    /// <summary>
    /// 類型幫助類別
    /// </summary>
    public static class TypeHelper {
        /// <summary>
        /// 取得指定命名空間內所有類型陣列
        /// </summary>
        /// <param name="ns">指定命名空間</param>
        /// <returns>指定命名空間內所有類型陣列</returns>
        public static Type[] GetNamespaceTypes(string ns) {
            return (from t in Assembly.GetExecutingAssembly().GetTypes()
                    where t.IsClass && t.Namespace == ns
                    select t).ToArray();
        }
    }
}
