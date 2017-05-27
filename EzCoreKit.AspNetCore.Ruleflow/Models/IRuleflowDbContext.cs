using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EzCoreKit.AspNetCore.Ruleflow.Models {
    public interface IRuleflowDbContext {
        DbSet<AccessInstance> AccessInstances { get; set; }
        DbSet<AccessRule> AccessRules { get; set; }
    }
}
