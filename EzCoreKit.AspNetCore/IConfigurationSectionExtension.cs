using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace EzCoreKit.AspNetCore {
    /// <summary>
    /// 針對<see cref="IConfigurationSection"/>的擴充方法
    /// </summary>
    public static class IConfigurationSectionExtension {
        /// <summary>
        /// 將IConfiguration物件轉換為<see cref="ExpandoObject"/>物件
        /// </summary>
        /// <param name="configureation">IConfiguration實例</param>
        /// <returns><see cref="ExpandoObject"/>物件</returns>
        public static object ToDynamicObject(this IConfiguration configureation) {
            if (configureation == null) return null;

            dynamic result = new ExpandoObject();
            IDictionary<string, object> setting = (IDictionary<string, object>)result;

            var attributes = configureation.GetChildren();

            var firstKey = attributes.FirstOrDefault()?.Key;
            int temp = 0;
            if (int.TryParse(firstKey, out temp)) {
                result = new object[attributes.Count()];
                foreach (var attr in attributes) {
                    if (attr.GetChildren().Count() > 0) {
                        result[temp++] = attr.ToDynamicObject();
                    } else {
                        result[temp++] = attr.Value?.Length > 0 ? attr.Value : null;
                    }
                }
            } else {
                foreach (var attr in attributes) {
                    if (attr.GetChildren().Count() > 0) {
                        setting[attr.Key] = attr.ToDynamicObject();
                    } else {
                        setting[attr.Key] = attr.Value?.Length > 0 ? attr.Value : null;
                    }
                }
            }
            return result;
        }
    }
}
