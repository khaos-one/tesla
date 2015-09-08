using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Tesla.Collections;
using Tesla.IO;

namespace Tesla.SocialApi.Vk
{
    public sealed class VkApi
        : IVkApi
    {
        public const string ApiVersion = "5.34";
        private static readonly Regex _authFormRegex = new Regex(@"form\smethod=""post""\saction=""(.+?)"".+name=""_origin""\svalue=""(.+?)"".+name=""ip_h""\svalue=""(.+?)"".+name=""lg_h""\svalue=""(.+?)"".+name=""to""\svalue=""(.+?)""", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex _authFormCredentialsErrorCheckRegex = new Regex(@"service_msg\sservice_msg_warning");
        private static readonly Regex _authFormNextRegex = new Regex(@"form\smethod=""post""\saction=""([^""]+)"">.+?name=""email_denied""\svalue=""([\d])""", RegexOptions.Singleline | RegexOptions.Compiled);
        private static readonly Regex _authFinalRegex = new Regex(@"oauth.vk.com/blank.html#access_token=([a-f0-9]+)&expires_in=(\d+)&user_id=(\d+)&secret=([a-f0-9]+)", RegexOptions.Compiled);

        public ulong AppId { get; private set; }
        public ulong UserId { get; private set; }
        public AccessScope Scope { get; private set; }
        public string AccessToken { get; private set; }
        public string ApiSecret { get; private set; }
        public TimeSpan ExpiresIn { get; private set; }

        public VkApi(ulong appId, AccessScope scope)
        {
            AppId = appId;
            Scope = scope;
        }

        public AuthorizationResult Authorize(string username, string password)
        {
            var scope = Scope.GetUrlString();
            var authorizationUrl = $"https://oauth.vk.com/authorize?client_id={AppId}&scope={scope}&redirect_uri=https://oauth.vk.com/blank.html &display=wap&response_type=token&v={ApiVersion}";

            // Initial request.
            var web = new HttpClient();
            var response = web.Get(authorizationUrl);
            var match = _authFormRegex.Match(response.Content);
            var formAction = match.Groups[1].Value;

            if (string.IsNullOrWhiteSpace(formAction))
                return AuthorizationResult.UnknownFailure;

            // Parse login form and fill the data.
            var inputs = new Dictionary<string, string>(6);
            inputs.Add("_origin", match.Groups[2].Value);
            inputs.Add("ip_h", match.Groups[3].Value);
            inputs.Add("lg_h", match.Groups[4].Value);
            inputs.Add("to", match.Groups[5].Value);
            inputs.Add("email", username);
            inputs.Add("pass", password);

            // Finally parse accept form.
            response = web.Post(formAction, inputs, response.Encoding, followRedirect: true);

            match = _authFormCredentialsErrorCheckRegex.Match(response.Content);

            if (match.Success)
                return AuthorizationResult.CredentialsIncorrect;

            match = _authFormNextRegex.Match(response.Content);
            formAction = match.Groups[1].Value;

            if (string.IsNullOrWhiteSpace(formAction))
            {
                if (!RetrieveAuthParametersFromUri(response.Uri.ToString()))
                    return AuthorizationResult.UnknownFailure;

                return AuthorizationResult.Ok;
            }

            inputs.Clear();
            inputs.Add("email_denied", match.Groups[2].Value);

            // And finally get access_token.
            response = web.Post(formAction, inputs, response.Encoding, followRedirect: false);

            if (response.Status != HttpStatusCode.Found)
                return AuthorizationResult.UnknownFailure;

            if (!RetrieveAuthParametersFromUri(response.Headers["Location"]))
                return AuthorizationResult.UnknownFailure;

            return AuthorizationResult.Ok;
        }

        private bool RetrieveAuthParametersFromUri(string uri)
        {
            var match = _authFinalRegex.Match(uri);

            if (!match.Success)
                return false;

            AccessToken = match.Groups[1].Value;
            ExpiresIn = new TimeSpan(0, 0, int.Parse(match.Groups[2].Value));
            UserId = ulong.Parse(match.Groups[3].Value);

            if ((Scope & AccessScope.NoHttps) == AccessScope.NoHttps)
                ApiSecret = match.Groups[4].Value;

            return true;
        }

        public dynamic Raw(string method, Dictionary<string, string> parameters)
        {
            var parametersString = parameters
                .Select(x => $"{x.Key}={x.Value}")
                .JoinString("&");
            var requestUri = $"/method/{method}?{parametersString}&v={ApiVersion}&access_token={AccessToken}";

            if (!string.IsNullOrEmpty(ApiSecret))
            {
                var hash = MD5Extensions.HexDigest(requestUri + ApiSecret);
                requestUri = "https://api.vk.com" + requestUri + $"&sig={hash}";
            }
            else
                requestUri = "https://api.vk.com" + requestUri;

            using (var web = new HttpClient())
            {
                var response = web.Get(requestUri);
                return JObject.Parse(response.Content);
            }
        }
    }
}
