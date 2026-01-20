using Microsoft.Extensions.Configuration;
using MyErpManagement.Core.Modules.EmailModule.IServices;
using RestSharp;
using RestSharp.Authenticators;

namespace MyErpManagement.Core.Modules.EmailModule.Services
{
    public class EmailService(IConfiguration config) : IEmailService
    {
        public async Task<bool> SendRegisterCode(string email, string verificationRegistCode)
        {
            var options = new RestClientOptions("https://api.mailgun.net")
            {
                Authenticator = new HttpBasicAuthenticator("api", config["Mail_Gun_API_key"] ?? "API_KEY")
            };

            var client = new RestClient(options);
            var request = new RestRequest($"/v3/{config["Mail_Gun_Domain"]}/messages", Method.Post);
            request.AlwaysMultipartFormData = true;
            request.AddParameter("from", "Mailgun Sandbox <postmaster@" + config["Mail_Gun_Domain"] + ">");
            request.AddParameter("to", email);
            request.AddParameter("subject", "My-Erp Proj Invite Email Code");
            request.AddParameter("text", "My-Erp 會員註冊驗證碼: " + verificationRegistCode);
            var response = await client.ExecuteAsync(request);
            if (response.ErrorMessage is not null)
            {
                Console.WriteLine("Error sending email: " + response.ErrorMessage);
                return false;
            }
            return true;
        }
    }
}
