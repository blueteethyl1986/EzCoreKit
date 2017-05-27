using EzCoreKit.AspNetCore.Mvc;
using EzCoreKit.AspNetCore.Ruleflow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.AspNetCore.Ruleflow {
    public class RuleflowController<T>: EzController 
        where T : DbContext, IRuleflowDbContext {

        public T Database { get; private set; }

        public RuleflowController(T dbContext) {
            this.Database = dbContext;
            this.ExecutingCheckList.Insert(0, LaunchRuleEngine);
        }

        internal void LaunchRuleEngine(ActionExecutingContext context) {
            var engine = new AccessRuleEngine<T>(Database);
            
            if (!engine.Vaild(context.HttpContext).GetAwaiter().GetResult()) {
                throw new UnauthorizedAccessException("deny connect by rule engine");
            }
        }
    }
}
