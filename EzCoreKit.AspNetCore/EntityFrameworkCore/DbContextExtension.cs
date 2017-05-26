using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EzCoreKit.EntityFrameworkCore {
    public static class DbContextExtension {
        public static DbContextOptions<T> GetOptions<T>(this T dbcontext) where T:DbContext {
            var options = dbcontext.GetType().GetTypeInfo().BaseType.GetField("_options", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(dbcontext);
            return options as DbContextOptions<T>;
        }
    }
}
