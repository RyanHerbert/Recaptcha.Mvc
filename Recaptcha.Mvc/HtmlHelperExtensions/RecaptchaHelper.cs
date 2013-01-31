using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.Mvc;
using System.IO;
using System.Configuration;
using System.Web;

namespace Recaptcha.Mvc.HtmlHelperExtensions
{
    public static class RecaptchaHelper
    {
        public static IHtmlString GenerateCaptcha(this HtmlHelper helper, string id = "recaptcha", string theme = "blackglass")
        {
            var publicKey = ConfigurationManager.AppSettings["recaptcha.publicKey"];
            var privateKey = ConfigurationManager.AppSettings["recaptcha.privateKey"];

            if (String.IsNullOrEmpty(publicKey))
                throw new ConfigurationErrorsException("The key 'recaptcha.publicKey' must be defined in your config file");

            if (String.IsNullOrEmpty(privateKey))
                throw new ConfigurationErrorsException("The key 'recaptcha.privateKey' must be defined in your config file");

            var captchaControl = new Recaptcha.RecaptchaControl
            {
                ID = id,
                Theme = theme,
                PublicKey = publicKey,
                PrivateKey = privateKey
            };

            var htmlWriter = new HtmlTextWriter(new StringWriter());

            captchaControl.RenderControl(htmlWriter);

            return helper.Raw(htmlWriter.InnerWriter.ToString());
        }
    }
}
