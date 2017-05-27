using EzCoreKit.AspNetCore.Ruleflow.Exceptions;
using EzCoreKit.AspNetCore.Ruleflow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzCoreKit.AspNetCore.Ruleflow {
    public class AccessRuleEngine<T>
        where T : DbContext, IRuleflowDbContext {
        public T Source { get; set; }

        public AccessRuleEngine(T Source) {
            this.Source = Source;
        }

        public async Task<bool> Vaild(HttpContext httpContext) {
            var appId = httpContext.Request.Query["appId"];
            if (httpContext.Request.Method == HttpMethods.Post) {
                appId = httpContext.Request.Form["appId"];
            }                                
            var instance = Source.AccessInstances
                .FirstOrDefault(x => x.Id == (string)appId);

            if (instance == null) throw new AccessInstanceNotFoundException();

            string ruleIdString = instance.RuleEnterId;
            List<string> executedRules = new List<string>();
            do {
                Guid ruleId = default(Guid);
                AccessRule rule = null;
                if (Guid.TryParse(ruleIdString, out ruleId)) {
                    rule = Source.AccessRules.FirstOrDefault(x => x.Id == ruleId);
                } else {
                    rule = Source.AccessRules.FirstOrDefault(x => x.Name == ruleIdString);
                }
                if (rule == null) throw new AccessRuleNotFoundException();
                
                ruleIdString = await rule.Run(httpContext, instance.Id, instance.Key, executedRules);
                executedRules.Add(rule.Id.ToString());
            } while (ruleIdString != null && 
                     ruleIdString != Guid.Empty.ToString() &&
                     ruleIdString != "OK");

            return ruleIdString != null;
        }
    }
}
