﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EzCoreKit.AspNetCore.Mvc {
    /// <summary>
    /// 針對ASP.net Core 2.0之增強<see cref="Controller"/>
    /// </summary>
    public class EzController : Controller {
        /// <summary>
        /// 執行階段檢查表
        /// </summary>
        public List<Action<ActionExecutingContext>> ActionExecutingCheckList { get; set; }
            = new List<Action<ActionExecutingContext>>();

        /// <summary>
        /// 建立並建立<see cref="Controller"/>實例
        /// </summary>
        public EzController() {
            ActionExecutingCheckList.Add(DefaultActionExcutingCheckMain);
        }


        /// <summary>
        /// 當Request管線進入Action前執行的操作
        /// </summary>
        /// <param name="context">Action執行內容</param>
        public override void OnActionExecuting(ActionExecutingContext context) {
            try {
                //檢查控制器或方法是否有停用
                DisableCheck(context);
                DisableInTimeCheck(context);
                EnableInTimeCheck(context);

                //執行方法前檢查
                foreach (var check in ActionExecutingCheckList) {
                    check(context);
                }

                //設定ViewData
                SetViewData(context);
                //設定ViewBag
                SetViewBag(context);
                //設定TempData
                SetTempData(context);

                //檢查無問題執行下一階段
                base.OnActionExecuting(context);
            } catch (Exception e) {
                OnException(context, null, e);
            }
        }

        #region 方法開放存取檢查
        private void DisableCheck(ActionExecutingContext context) {
            var controllerAttributes = context.Controller.GetType().GetTypeInfo().GetCustomAttributes<DisableAttribute>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var methodInfo = controllerActionDescriptor.MethodInfo;
            var attributes = methodInfo.GetCustomAttributes<DisableAttribute>();
            if (controllerAttributes.Concat(attributes).Count() == 0) return;
            throw new NotSupportedException("disabled method");
        }

        private void DisableInTimeCheck(ActionExecutingContext context) {
            var controllerAttributes = context.Controller.GetType().GetTypeInfo().GetCustomAttributes<DisableInTimeAttribute>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var methodInfo = controllerActionDescriptor.MethodInfo;
            var attributes = controllerAttributes.Concat(
                methodInfo.GetCustomAttributes<DisableInTimeAttribute>());
            if (attributes.Count() == 0) return;
            var now = DateTime.Now;
            foreach (var attribute in attributes) {
                if (now >= attribute.Start && now < attribute.End) {
                    throw new NotSupportedException("disabled method");
                }
            }
        }

        private void EnableInTimeCheck(ActionExecutingContext context) {
            var controllerAttributes = context.Controller.GetType().GetTypeInfo().GetCustomAttributes<EnableInTimeAttribute>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var methodInfo = controllerActionDescriptor.MethodInfo;
            var attributes = controllerAttributes.Concat(methodInfo.GetCustomAttributes<EnableInTimeAttribute>());
            if (attributes.Count() == 0) return;
            var now = DateTime.Now;
            foreach (var attribute in attributes) {
                if (now < attribute.Start && now >= attribute.End) {
                    throw new NotSupportedException("disabled method");
                }
            }
        }
        #endregion

        #region 常用屬性檢查
        private void DefaultActionExcutingCheckMain(ActionExecutingContext context) {
            DefaultActionExcutingCheck<RequiredAttribute>(context);
            DefaultActionExcutingCheck<RangeAttribute>(context);
            DefaultActionExcutingCheck<MaxLengthAttribute>(context);
            DefaultActionExcutingCheck<MinLengthAttribute>(context);
            DefaultActionExcutingCheck<RegularExpressionAttribute>(context);
        }

        private void DefaultActionExcutingCheck<TAttribute>(ActionExecutingContext context) where TAttribute : ValidationAttribute {
            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var methodInfo = controllerActionDescriptor.MethodInfo;

            var requiredParameters = methodInfo.GetParameters()
                 .Select(x => new {
                     parameter = x,
                     attribute = x.GetCustomAttribute<TAttribute>()
                 })
                 .Where(x => x.attribute != null);
            var requiredErrorMessages = requiredParameters
                .Where(x => !x.attribute.IsValid(context.ActionArguments[x.parameter.Name]))
                .Select(x => x.attribute.ErrorMessage ?? x.parameter.Name);
            if (requiredErrorMessages.Count() > 0) {
                throw new ArgumentNullException(string.Join(",", requiredErrorMessages));
            }
        }
        #endregion

        #region Data預設作業
        private void SetViewData(ActionExecutingContext context) {
            var controllerAttributes = context.Controller.GetType().GetTypeInfo().GetCustomAttributes<SetViewDataAttribute>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var methodInfo = controllerActionDescriptor.MethodInfo;
            var attributes = methodInfo.GetCustomAttributes<SetViewDataAttribute>();
            if (attributes.Count() == 0) return;
            foreach (var attribute in controllerAttributes.Concat(attributes)) {
                ViewData[attribute.Key] = attribute.Value;
            }
        }
        private void SetViewBag(ActionExecutingContext context) {
            var controllerAttributes = context.Controller.GetType().GetTypeInfo().GetCustomAttributes<SetViewBagAttribute>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var methodInfo = controllerActionDescriptor.MethodInfo;
            var attributes = methodInfo.GetCustomAttributes<SetViewBagAttribute>();
            if (attributes.Count() == 0) return;

            IDictionary<string, object> viewBagData = ViewBag as IDictionary<string, object>;
            foreach (var attribute in controllerAttributes.Concat(attributes)) {
                viewBagData[attribute.Key] = attribute.Value;
            }
        }
        private void SetTempData(ActionExecutingContext context) {
            var controllerAttributes = context.Controller.GetType().GetTypeInfo().GetCustomAttributes<SetTempDataAttribute>();

            var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var methodInfo = controllerActionDescriptor.MethodInfo;
            var attributes = methodInfo.GetCustomAttributes<SetTempDataAttribute>();
            if (attributes.Count() == 0) return;
            foreach (var attribute in controllerAttributes.Concat(attributes)) {
                TempData[attribute.Key] = attribute.Value;
            }
        }
        #endregion

        public override void OnActionExecuted(ActionExecutedContext context) {
            if (context.Exception != null) {
                OnException(null, context, context.Exception);
            }
            base.OnActionExecuted(context);
        }

        /// <summary>
        /// 執行Action時候發生的例外處理
        /// </summary>
        /// <param name="executingContext">執行前發生例外</param>
        /// <param name="executedContext">執行中發生例外</param>
        /// <param name="exception">例外</param>
        [NonAction]
        public virtual void OnException(
            ActionExecutingContext executingContext,
            ActionExecutedContext executedContext,
            Exception exception) {
            if (executingContext != null && executingContext.Result == null) executingContext.Result = new EmptyResult();
        }
    }
}
