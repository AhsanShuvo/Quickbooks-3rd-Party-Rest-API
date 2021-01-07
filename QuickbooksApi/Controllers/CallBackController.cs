using QuickbooksCommon.Logger;
using QuickbooksDAL;
using QuickbooksDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuickbooksWeb.Controllers
{
    public class CallBackController : Controller
    {
        private IUserRepository _repository;

        public CallBackController(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActionResult> Index()
        {
            Logger.WriteDebug("Successfully redirected to callback url.");
            string code = Request.QueryString["code"] ?? "none";
            string realmId = Request.QueryString["realmId"] ?? "none";
            await GetAuthTokensAsync(code, realmId);
            return RedirectToAction("Index", "Home");
        }

        private async Task GetAuthTokensAsync(string code, string realmId)
        {
            if (realmId != null)
            {
                Session["realmId"] = realmId;
            }
            var tokenResponse = await AppController.auth2Client.GetBearerTokenAsync(code);
            var claims = new List<Claim>();

            if (Session["realmId"] != null)
            {
                claims.Add(new Claim("realmId", Session["realmId"].ToString()));
            }

            if (!string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
            {
                claims.Add(new Claim("access_token", tokenResponse.AccessToken));
                claims.Add(new Claim("access_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.AccessTokenExpiresIn)).ToString()));
            }

            if (!string.IsNullOrWhiteSpace(tokenResponse.RefreshToken))
            {
                claims.Add(new Claim("refresh_token", tokenResponse.RefreshToken));
                claims.Add(new Claim("refresh_token_expires_at", (DateTime.Now.AddSeconds(tokenResponse.RefreshTokenExpiresIn)).ToString()));
            }

            var id = new ClaimsIdentity(claims, "Cookies");
            Request.GetOwinContext().Authentication.SignIn(id);

            UserInfo user = new UserInfo()
            {
                RealmId = claims[0].Value,
                AccessToken = claims[1].Value,
                RefreshToken = claims[3].Value,
                AccessTokenExpiresIn = Convert.ToDateTime(claims[2].Value),
                RefreshTokenExpiresIn = Convert.ToDateTime(claims[4].Value)
            };
            _repository.SaveUserInfo(user);
        }
    }
}