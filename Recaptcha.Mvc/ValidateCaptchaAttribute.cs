using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Configuration;

namespace Recaptcha.Mvc
{
    public class ValidateCaptchaActionFilterAttribute : ActionFilterAttribute
    {
        private const string ChallengeFieldKey = "recaptcha_challenge_field";
        private const string ResponseFieldKey = "recaptcha_response_field";

        private readonly string _resultKey ;
        private readonly bool _setModelStateError ;
        private readonly string _errorMessage ;

        /// <summary>
        /// This attribute will validate a recaptcha response OnActionExecuting and add the result to the ActionParameters and ModelState
        /// </summary>
        /// <param name="resultKey">The name of the ActionParameter and ModelState key to set</param>
        /// <param name="setModelStateError">Set to false to disable setting a modelstate error</param>
        /// <param name="errorMessage">The error message to set in the model state if setModelStateError is true</param>
        public ValidateCaptchaActionFilterAttribute(string resultKey = "captchaIsValid", bool setModelStateError = true, string errorMessage = "Your response did not match the captcha.")
        {
            _resultKey = resultKey;
            _setModelStateError = setModelStateError;
            _errorMessage = errorMessage;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var privateKey = ConfigurationManager.AppSettings["recaptcha.privateKey"];

            if (String.IsNullOrEmpty(privateKey))
                throw new ConfigurationErrorsException("The key 'recaptcha.privateKey' must be defined in your config file");

            var captchaValidator = new Recaptcha.RecaptchaValidator
            {
                PrivateKey = privateKey,
                RemoteIP = filterContext.HttpContext.Request.UserHostAddress,
                Challenge = filterContext.HttpContext.Request.Form[ChallengeFieldKey] ?? String.Empty,
                Response = filterContext.HttpContext.Request.Form[ResponseFieldKey] ?? String.Empty
            };

            var captchaResponse = captchaValidator.Validate();

            // Push the result value into a parameter in our Action
            filterContext.ActionParameters[_resultKey] = captchaResponse.IsValid;

            // Set the Model State error
            if (!captchaResponse.IsValid && _setModelStateError)
            {
                filterContext.Controller.ViewData.ModelState.AddModelError(_resultKey, _errorMessage);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
