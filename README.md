Recaptcha.Mvc
=============
Project contains:
  An ActionFilter for validating the recaptcha response.
  A HtmlHelper for generating the recaptcha form field.

Installation
============
Available from Nuget as the Recaptcha.Mvc package. 
Install-Package Recaptcha.Mvc 
https://www.nuget.org/packages/Recaptcha.Mvc/

The installer will add two keys to your app settings section of Web.Config that you will need to fill in with your public and private key.
  <add key="recaptcha.privateKey" value="" />
  <add key="recaptcha.publicKey" value="" />


Example Usage
=============

In the view use @Html.GenerateCaptcha() to generate the necessary form fields.
  GenerateCaptcha has two optional arguments allowing you to set the ID and theme of the form field.

In your controller use the attribute [ValidateCaptcha] on the HttpPost method to validate the captcha response.
  [HttpPost]
  [ValidateCaptcha]
  public ActionResult FormPost(FormViewModel form)

By default, the action filter will set the key captchaIsValid in the controllers parameters, and also add a ModelState error for the key.
The ValidateCaptchaAttribute has three optional arguments allowing you to specify the key the result is stored in, whether or not to set a ModelState error, and the text of the error.
