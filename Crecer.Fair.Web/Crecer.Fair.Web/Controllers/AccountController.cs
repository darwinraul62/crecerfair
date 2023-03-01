using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Ecubytes.AspNetCore.WebUtilities.Api;
using Ecubytes.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Crecer.Fair.Web.Services;
using Crecer.Fair.Web.Models;
using Crecer.Fair.Web.Services.Models;
using Ecubytes.Data;
using Ecubytes.Identity.Models;
using JWT.Builder;
using JWT.Algorithms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Security;
using MailKit.Net.Smtp;
using System.Text;
using System.Web;

namespace Crecer.Fair.Web.Controllers
{
    public class AccountController : Ecubytes.AspNetCore.Mvc.Controllers.ControllerBase
    {
        private readonly ApiProfileManager apiProfileManager;
        private readonly IPartnertService partnertService;
        private readonly IConfiguration configuration;
        private readonly SmtpSettingsOptions smtpSettings;
        private readonly IFairService fairService;
        private readonly IProviderService providerService;
        private string backOfficeUrl;
        public AccountController(ApiProfileManager apiProfileManager,
            IPartnertService partnertService,
            IConfiguration configuration,
            IOptions<SmtpSettingsOptions> smtpOptions,
            IFairService fairService,
            IProviderService providerService)
        {
            this.apiProfileManager = apiProfileManager;
            this.partnertService = partnertService;
            this.configuration = configuration;
            this.smtpSettings = smtpOptions.Value;
            this.fairService = fairService;
            this.providerService = providerService;

            backOfficeUrl = configuration.GetValue<string>("FairSettings:BackOfficeUrl");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SignIn(string returnUrl = null, string Identification = null)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                if(!string.IsNullOrWhiteSpace(returnUrl))
                    return Redirect(returnUrl); 
                else
                    return RedirectToAction("Index", "Fair");
            }

            LoginViewModel model = new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                Identification = Identification,
                IsRegister = Identification != null
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginViewModel requestModel)
        {
            ViewData["ReturnUrl"] = requestModel.ReturnUrl;
            try
            {
                if (ModelState.IsValid)
                {
                    var responseLogin = await TrySignInAsync(GetIdentityClientInstance(), requestModel.LogonName, requestModel.Password);

                    if (responseLogin.IsValid)
                    {
                        if (!string.IsNullOrWhiteSpace(requestModel.ReturnUrl))
                            return Redirect(requestModel.ReturnUrl);
                        else
                            return await RedirectFair();
                    }
                    else
                    {
                        this.AddMessages(responseLogin.Messages);
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddErrorMessage(ex);
            }

            return View(requestModel);
        }

        private async Task<IActionResult> RedirectFair()
        {
            var fairMain = await fairService.GetMainAsync();
            
            return RedirectToAction(nameof(FairController.Stand), "Fair", new { id = fairMain.HostProviderId });
        }


        [HttpGet, ActionName("SignOut")]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> BeginRegister(LoginViewModel model)
        {
            try
            {
                var partnert = await partnertService.GetByIdentificationAsync(model.Identification);

                if (partnert == null)
                {
                    return RedirectToAction(nameof(Register), new { identification = model.Identification });
                }
                else
                {
                    if (partnert.HasUser)
                    {
                        return this.AddErrorMessage("Ya existe un registro para la identificación ingresada. Inicie sesión con su usuario y contraseña").
                                                RedirectToAction(nameof(SignIn), new { Identification = model.Identification });
                    }
                    else
                        return RedirectToAction(nameof(Register), new { identification = model.Identification });
                }
            }
            catch (Exception ex)
            {
                return this.AddDefaultErrorMessage(ex).
                    RedirectToAction(nameof(SignIn), new { Identification = model.Identification });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string identification = null)
        {
            if (!string.IsNullOrWhiteSpace(identification))
            {
                bool allow = await partnertService.AllowRegisterAsync(identification);
                if (!allow)
                    this.AddWarningMessage("Su Identificación no consta en nuestro registro de socios. Puede continuar con su registro pero lo invitamos a ponerse en contacto con la Cooperativa para que pueda ser parte.");
            }

            RegisterViewModel model = new RegisterViewModel()
            {
                Identification = identification,
                Step = "FORM"
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            IdentityClient client = null;
            try
            {
                if (ModelState.IsValid)
                {
                    if (model.Step == "FORM")
                    {
                        if (string.IsNullOrWhiteSpace(model.Password))
                            return this.AddMessageRequiredField("Password").View(model);

                        if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
                            return this.AddMessageRequiredField("Confirmación de Password").View(model);

                        if (model.Password != model.ConfirmPassword)
                            return this.AddErrorMessage("Contraseñas no coinciden").View(model);
                        
                        if (model.Password.Length < 8)
                            return this.AddErrorMessage("La contraseña debe tener una longitud mínima de 8 carácteres").View(model);
                    }
                    else
                    {
                        model.Password = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(model.Pwd));
                    }

                    if (model.Identification != null && (model.Identification.Length != 10 && model.Identification.Length != 13))
                        return this.AddErrorMessage("La Identificación es inválida").View(model);

                    var partnert = await partnertService.GetByIdentificationAsync(model.Identification);

                    if (partnert != null && partnert.HasUser)
                        return this.AddErrorMessage("Ya existe un registro para la identificación ingresada. Realice un Inicio de Sesión").View(model);

                    client = GetIdentityClientInstance();

                    if ((await client.CheckEmailUsedAsync(model.Email)) || (await client.CheckLogonNameUsedAsync(model.Email)))
                        return this.AddErrorMessage("El Email ingresado ya fue utilizado").View(model);

                    if (model.Step == "VALEMAIL")
                    {
                        if (string.IsNullOrWhiteSpace(model.OtpEmailValidation))
                            return this.AddErrorMessage("Debe ingresar el código de verificación enviado a su Email")
                                .View(model);

                        var responseValidation = await client.ActivateOtpAsync(model.Email, "EMAILVERIFICATION", model.OtpEmailValidation);

                        if (!responseValidation.IsValid)
                        {
                            model.OtpEmailValidation = null;
                            return this.AddErrorMessage("El código de verificación del Email es inválido. Intente generar uno nuevo.").View(model);
                        }

                        if (IsValidState)
                        {
                            PartnertRegisterRequestDTO modelPartnertRequest = new PartnertRegisterRequestDTO()
                                {
                                    Address = model.Address,
                                    Email = model.Email,
                                    HasUser = false,
                                    Identification = model.Identification,
                                    LastNames = model.LastNames,
                                    Names = model.Names,
                                    PhoneNumber = model.PhoneNumber                                    
                                };

                            Guid partnertId = Guid.Empty;
                            if (partnert == null)
                            {
                                //Create Partnert
                                var partnertCreateResponse = await partnertService.CreateAsync(modelPartnertRequest);
                                partnertId = partnertCreateResponse.Data.PartnertId;

                                if (!partnertCreateResponse.IsValid)
                                    this.AddMessages(partnertCreateResponse);
                                else
                                    partnertId = partnertCreateResponse.Data.PartnertId;
                            }
                            else
                            {
                                partnertId = partnert.PartnertId;

                                modelPartnertRequest.Address = model.Address;
                                modelPartnertRequest.Email = model.Email;
                                modelPartnertRequest.Identification = model.Identification;
                                modelPartnertRequest.LastNames = model.LastNames;
                                modelPartnertRequest.Names = model.Names;
                                modelPartnertRequest.PhoneNumber = model.PhoneNumber;
                            }

                            //Create User
                            if (this.IsValidState)
                            {
                                var responseUser = await client.CreateUserAsync(new UserCreateRequest()
                                {
                                    BlockLogin = false,
                                    Email = model.Email,
                                    LastNames = model.LastNames,
                                    LogonName = model.Email,
                                    Names = model.Names,
                                    Password = model.Password,
                                    UserId = partnertId,
                                    RegisterDate = DateTime.UtcNow,                                    
                                    Attributes = new Dictionary<string, string>()
                                    {
                                        ["Identification"] = model.Identification,
                                        ["IsPartnert"] = "true"
                                    }
                                });

                                if (!responseUser.IsValid)
                                {
                                    this.AddMessages(responseUser);                                   
                                }
                                else
                                {
                                    modelPartnertRequest.HasUser = true;                                    
                                    await partnertService.UpdateAsync(partnertId, modelPartnertRequest);
                                }
                                
                                //Succes and try signin
                                if (this.IsValidState)
                                {
                                    await TrySignInAsync(client, model.Email, model.Password);

                                    return RedirectToAction("Index", "Fair");
                                }
                            }
                        }
                    }
                    else
                    {
                        //continue next step email validation
                        var otpResponse = await client.GenerateOtpAsync(new OtpGenerateRequest()
                        {
                            Challenge = model.Email,
                            TargetId = "EMAILVERIFICATION"
                        });

                        //Send Email
                        await SendRegisterActivationCodeEmailAsync(model.Email, otpResponse.Data.Password);

                        model.Step = "VALEMAIL";
                        model.Pwd = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(model.Password));
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddErrorMessage(ex, "Lo sentimos, no pudimos registrate. Intente más tarde");
            }
            finally
            {
                client?.Dispose();
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Profile()
        {
            string name = User.Identity.Name;
            string logonName = User.FindFirstValue("username");

            // var identityClient = GetIdentityClientInstance();
            // var userModel = await identityClient.GetAsync(logonName);

            return View(new UserProfileViewModel()
            {
                Name = name,
                LogonName = logonName
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel viewModel)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.CurrentPassword))
                    this.AddErrorMessage("La contraseña actual es requerida");

                if (string.IsNullOrWhiteSpace(viewModel.NewPassword))
                    this.AddErrorMessage("La nueva contraseña es requerida");

                if (string.IsNullOrWhiteSpace(viewModel.ConfirmNewPassword))
                    this.AddErrorMessage("La confirmación de la contraseña es requerida");
                
                if (viewModel.NewPassword.Length < 8)
                    return this.AddErrorMessage("La contraseña debe tener una longitud mínima de 8 carácteres").View(viewModel);

                if (IsValidState)
                {
                    var identityClient = GetIdentityClientInstance();

                    var response = await identityClient.LoginAsync(new LoginRequest()
                    {
                        LogonName = User.FindFirstValue("username"),
                        Password = viewModel.CurrentPassword
                    });

                    if (!response.IsValid)
                        this.AddErrorMessage("La contraseña ingresada es inválida");

                    if (IsValidState)
                    {
                        if (viewModel.NewPassword != viewModel.ConfirmNewPassword)
                        {
                            this.AddErrorMessage("Contraseñas no coinciden");
                        }

                        if (IsValidState)
                        {
                            await identityClient.ChangePasswordAsync(Guid.Parse(User.FindFirstValue("sub")), viewModel.NewPassword);

                            this.AddDefaultSuccessMessage();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }

            return JsonModelResult();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordRecovery()
        {
            PasswordRecoveryRequestViewModel viewModel = new PasswordRecoveryRequestViewModel();
            return View(viewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordRecovery(PasswordRecoveryRequestViewModel viewModel)
        {
            IdentityClient client = null;
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.Email))
                    return this.AddMessageRequiredField("Email").View();

                client = GetIdentityClientInstance();

                var user = await client.GetAsync(viewModel.Email);
                if (user != null)
                {
                    var otpResult = await client.GenerateOtpAsync(new OtpGenerateRequest()
                    {
                        TargetId = "REQUESTRESETPASSWORD",
                        UserId = user.UserId
                    });

                    if (!otpResult.IsValid)
                        this.AddMessages(otpResult);

                    string code = $"{user.UserId}~~~{otpResult.Data.Password}";
                    string encode = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(code));

                    await SendPasswordRecoveryEmailAsync(viewModel.Email, encode);

                    return RedirectToAction(nameof(SendedPasswordRecovery));

                }
                else
                    this.AddErrorMessage("El email ingresado no esta registrado como usuario");

            }
            catch (Exception ex)
            {
                this.AddErrorMessage(ex, "No pudimos completar el proceso. Intente más tarde.");
            }
            finally
            {
                client?.Dispose();
            }

            return View(viewModel);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult SendedPasswordRecovery()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string code)
        {
            IdentityClient client = null;
            try
            {
                string decode = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(code));

                string[] parts = decode.Split("~~~");

                client = GetIdentityClientInstance();
                Guid userId = Guid.Parse(parts[0]);

                var responseValidation = await client.ActivateOtpAsync(userId, "REQUESTRESETPASSWORD", parts[1]);

                //Validate request
                if (responseValidation.IsValid)
                {
                    //request new OTP
                    var newOtp = await client.GenerateOtpAsync(new OtpGenerateRequest()
                    {
                        UserId = userId,
                        TargetId = "RESETPASSWORD"
                    });

                    if (newOtp.IsValid)
                    {
                        var userModel = await client.GetAsync(userId);

                        ResetPasswordViewModel model = new ResetPasswordViewModel()
                        {
                            UserId = userId,
                            Email = userModel.Email,
                            Name = userModel.FullName,
                            Otp = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(newOtp.Data.Password))
                        };

                        return View(model);
                    }
                    else
                    {
                        this.AddMessages(newOtp);
                    }
                }
                else
                {
                    this.AddErrorMessage("El Link es inválido o ha expirado");
                }

            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }
            finally
            {
                client?.Dispose();
            }

            return View("ErrorPasswordRecovery");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel viewModel)
        {
            IdentityClient client = null;
            try
            {
                if (string.IsNullOrWhiteSpace(viewModel.Password))
                    return this.AddMessageRequiredField("Password").View(viewModel);

                if (string.IsNullOrWhiteSpace(viewModel.ConfirmPassword))
                    return this.AddMessageRequiredField("Confirmación de Password").View(viewModel);

                if (viewModel.Password != viewModel.ConfirmPassword)
                    return this.AddErrorMessage("Contraseñas no coinciden").View(viewModel);

                if (viewModel.Password.Length < 8)
                    return this.AddErrorMessage("La contraseña debe tener una longitud mínima de 8 carácteres").View(viewModel);

                client = GetIdentityClientInstance();

                string otp = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(viewModel.Otp));

                var responseValidation = await client.ActivateOtpAsync(viewModel.UserId, "RESETPASSWORD", otp);

                //Validate request
                if (responseValidation.IsValid)
                {
                    var userModel = await client.GetAsync(viewModel.UserId);

                    var response = await client.ChangePasswordAsync(userModel.UserId, viewModel.Password);

                    if (response.IsValid)
                    {
                        this.AddSuccessMessage("La contraseña fue modificada con éxito. Utiliza la nueva contraseña para ingresar.");
                        return RedirectToAction("SignIn");
                    }
                    else
                        this.AddMessages(response);
                }
                else
                {
                    this.AddErrorMessage("No se pudo realizar la actualización. Inicie el proceso de recuperación de contraseña nuevamente.");
                }

            }
            catch (Exception ex)
            {
                this.AddDefaultErrorMessage(ex);
            }
            finally
            {
                client?.Dispose();
            }

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetChatToken()
        {
            string id = User.FindFirstValue("sub");
            string name = User.FindFirstValue("name");
            string providerName = User.FindFirstValue("provider_name");
            string providerId = User.FindFirstValue("provider_id");

            if (!string.IsNullOrWhiteSpace(providerName))
                name = $"{providerName} - {name}";

            if (!string.IsNullOrEmpty(providerId))
                id = providerId;

            var token = new JwtBuilder()
           .WithAlgorithm(new HMACSHA256Algorithm())
           .AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(3600).ToUnixTimeSeconds())
           .AddClaim("iss", "2c52b093-29b7-497b-aade-2c1e6d9b302e")
           .AddClaim("sub", User.FindFirstValue("sub"))
           .AddClaim("name", name)
           .AddClaim("email", User.FindFirstValue("email"))
           .AddClaim("username", User.FindFirstValue("username"))
           .AddClaim("picture", $"{backOfficeUrl}/{User.FindFirstValue("picture")}")
           .AddClaim("client_id", "2c52b093-29b7-497b-aade-2c1e6d9b302e")
           .WithSecret("mxWyLXiqpHk@XrVjFzBvCwkgdlYKiyC8")
           .Encode();

            return Json(token);
        }

        [NonAction]
        private async Task<ModelResult<LoginResponse>> TrySignInAsync(IdentityClient client, string logonName, string password)
        {
            var responseLogin = await client.LoginAsync(new Ecubytes.Identity.Models.LoginRequest()
            {
                LogonName = logonName,
                Password = password
            });

            if (responseLogin.IsValid)
            {
                List<Claim> claims = new List<Claim>
                        {
                            new Claim("username", responseLogin.Data.LogonName),
                            new Claim("name", responseLogin.Data.Name),
                            new Claim("sub", responseLogin.Data.UserId.ToString())
                        };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme, "name", "role");

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity));
            }

            return responseLogin;

        }

        private IdentityClient GetIdentityClientInstance()
        {
            var profile = apiProfileManager.Get("Identity.User");

            IdentityClient client = new IdentityClient(
                profile.BaseAddress,
                profile.ClientId,
                profile.ClientSecret,
                GlobalOptions.DefaultTenantId);

            return client;
        }

        private async Task SendRegisterActivationCodeEmailAsync(string to, string activationCode)
        {
            FairViewModel fair = (await fairService.GetMainAsync()).ToViewModel();
            var provider = await providerService.GetAsync(fair.HostProviderId);

            string filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "email", "register_activation_code.html");
            StringBuilder template = new StringBuilder(System.IO.File.ReadAllText(filePath));

            template.Replace("{{activationCode}}", activationCode);
            template.Replace("{{logoUrl}}", Url.AboluteUrl("/img/logo_crecer.png"));
            //template.Replace("{{imageUrlPath}}", Url.AboluteUrl("/img/vpn_key-48-primary.png"));
            template.Replace("{{websiteUrl}}", provider.Website);

            await SendMailAsync(to, "Código de Activación de Registro - Feria Virtual Crecer", template.ToString());
        }

        private async Task SendPasswordRecoveryEmailAsync(string to, string code)
        {
            FairViewModel fair = (await fairService.GetMainAsync()).ToViewModel();
            var provider = await providerService.GetAsync(fair.HostProviderId);

            string filePath = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "wwwroot", "email", "password_recovery.html");
            StringBuilder template = new StringBuilder(System.IO.File.ReadAllText(filePath));

            template.Replace("{{logoUrl}}", Url.AboluteUrl("/img/logo_crecer.png"));
            template.Replace("{{imageUrlPath}}", Url.AboluteUrl("/img/vpn_key-48-primary.png"));
            template.Replace("{{resetPasswordUrl}}", Url.AboluteUrl($"/Account/ResetPassword?code={HttpUtility.UrlEncode(code)}"));
            template.Replace("{{websiteUrl}}", provider.Website);

            await SendMailAsync(to, "Recuperar Contraseña - Feria Virtual Crecer", template.ToString());
        }

        private async Task SendMailAsync(
            string to,
            string subject,
            string content)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(smtpSettings.SenderName));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = content };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(smtpSettings.Server, smtpSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(smtpSettings.UserName, smtpSettings.Password);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

    }
}
