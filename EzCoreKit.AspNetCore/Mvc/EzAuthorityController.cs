using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace EzCoreKit.AspNetCore.Mvc {
    /// <summary>
    /// 支援權限管理的進階控制器
    /// </summary>
    /// <typeparam name="TAuthorityEnum"></typeparam>
    public abstract class EzAuthorityController<TAuthorityEnum> : EzController
        where TAuthorityEnum : struct, IConvertible {

        /// <summary>
        /// 取得目前使用者權限
        /// </summary>
        public abstract TAuthorityEnum UserAuthority { get; }

        public EzAuthorityController() : base() {
            
        }

        public override void OnActionExecuting(ActionExecutingContext context) {
            var controllerAttributes = context.Controller.GetType().GetTypeInfo().GetCustomAttributes<AuthorityAttribute>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) {
                base.OnActionExecuting(context);
                return;
            }

            var methodInfo = controllerActionDescriptor.MethodInfo;
            var attributes = controllerAttributes.Concat(
                methodInfo.GetCustomAttributes<AuthorityAttribute>());
            if (attributes.Count() == 0) {
                base.OnActionExecuting(context);
                return;
            }
            var now = DateTime.Now;
            foreach (var attribute in attributes) {
                if (Convert.ToInt32(UserAuthority) < Convert.ToInt32(attribute.Minimum)) {
                    OnException(context,null, new Exception("authority problem"));
                    return;
                }
            }

            //下一階段
            base.OnActionExecuting(context);
        }
    }
}
