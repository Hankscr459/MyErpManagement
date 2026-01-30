using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyErpManagement.Api.Constants;
using MyErpManagement.Core.Dtos.Auth.Request;
using MyErpManagement.Core.Dtos.Auth.Response;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.DataBase;
using MyErpManagement.IntegrationTests.Constants;
using MyErpManagement.IntegrationTests.Fixtures;
using MyErpManagement.IntegrationTests.SeedData;
using System.Net;
using System.Net.Http.Json;

namespace MyErpManagement.IntegrationTests.Tests.Controllers
{
    public class AuthControllerTests : IClassFixture<ApiWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly ApiWebApplicationFactory _factory;
        protected IConfiguration _config { get; }

        public AuthControllerTests(ApiWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
            _factory = factory;
            var projectDir = Directory.GetCurrentDirectory();
            // 根據 appsettings.json 的位置調整路徑
            _config = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.Testing.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<AuthControllerTests>(optional: true)
                .Build();
        }

        [Fact]
        public async Task Login_WithCorrectCredentials_ShouldReturn200AndToken()
        {
            await TestUserSeed.Initialize(_factory);

            var loginDto = new LoginRequestDto
            {
                Account = UserConstant.Account,
                Password = UserConstant.Password
            };

            var response = await _client.PostAsJsonAsync(ApiUrlConstant.Auth.Login, loginDto);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                throw new Exception($"API 崩潰詳細資訊: {errorDetail}");
            }

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            // 假設你的 LoginResponseDto 裡面有 Token 欄位
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            if (result?.Token == null)
            {
                throw new Exception("成功登入測試的回傳Token是空值");
            }

            // 檢查Token是否存入Db Memory
            using var scope = _factory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.JwtRecords.FirstOrDefault(j => j.TokenValue == result.Token).Should().NotBeNull();
        }

        [Fact]
        public async Task Login_WithWrongPassword_ShouldReturn400BadRequest()
        {
            var loginDto = new LoginRequestDto { Account = UserConstant.Account, Password = "wrongpassword" };

            var response = await _client.PostAsJsonAsync(ApiUrlConstant.Auth.Login, loginDto);
            if (response.StatusCode != HttpStatusCode.BadRequest)
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                throw new Exception($"API 崩潰詳細資訊: {errorDetail}");
            }

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>() ?? throw new Exception("登入密碼錯誤測試的回傳Token是空值");
            result.Message.Should().Be(ResponseTextConstant.BadRequest.InvalidPassword);
        }

        [Fact]
        public async Task Login_WithWrongAccount_ShouldReturn400BadRequest()
        {
            var loginDto = new LoginRequestDto { Account = "WrongAccount", Password = "wrongpassword" };

            var response = await _client.PostAsJsonAsync(ApiUrlConstant.Auth.Login, loginDto);
            if (response.StatusCode != HttpStatusCode.BadRequest)
            {
                var errorDetail = await response.Content.ReadAsStringAsync();
                throw new Exception($"API 崩潰詳細資訊: {errorDetail}");
            }

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = await response.Content.ReadFromJsonAsync<ApiResponseDto>() ?? throw new Exception("登入帳號錯誤測試的回傳Token是空值");
            result.Message.Should().Be(ResponseTextConstant.BadRequest.InvalidAccount);
        }

        [Fact]
        public async Task VerifyRegistEmail_Ok()
        {
            var verifyEmailDto = new VerifyEmailRequestDto { Email = _config["Test_Email"] };
            var response = await _client.PostAsJsonAsync(ApiUrlConstant.Auth.VerifyEmail, verifyEmailDto);
            var result = await response.Content.ReadFromJsonAsync<VerifyEmailResponseDto>() ?? throw new Exception("verifyEmail回傳是空值");
            Console.WriteLine($"VerifyRegistEmail_Ok Message: {result.Message}");
            Console.WriteLine($"VerifyRegistEmail_Ok ResendIntervalMinutes: {result.ResendIntervalMinutes}");
            result.ResendIntervalMinutes.Should().Be(int.Parse(_config["Mail_Verify_Register_Code_Expiry"]));
        }
    }
}
