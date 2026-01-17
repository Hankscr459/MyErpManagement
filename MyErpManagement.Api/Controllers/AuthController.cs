using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Attributes;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Examples.Auth;
using MyErpManagement.Api.Examples.Shared;
using MyErpManagement.Core.Dtos.Auth.Request;
using MyErpManagement.Core.Dtos.Auth.Response;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.JwtModule.Entities;
using MyErpManagement.Core.Modules.JwtModule.IServices;
using MyErpManagement.Core.Modules.JwtModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Constants;
using MyErpManagement.Core.Modules.UsersModule.IServices;
using Swashbuckle.AspNetCore.Filters;
using System.Net;

namespace MyErpManagement.Api.Controllers
{
    public class AuthController(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IPasswordService passwordHasher
    ) : BaseApiController
    {
        /// <summary>
        /// 登入
        /// </summary>
        /// <param name="loginDto"></param>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status400BadRequest)]
        [SwaggerRequestExample(typeof(LoginRequestDto), typeof(RequestBodyLoginExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestLoginResponseExample))]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginDto)
        {
            // 驗證使用者帳號
            //var user = await unitOfWork.UserRepository.GetUserByUserAccountAsync(loginDto.Account);
            var user = await unitOfWork.UserRepository
                    .GetFirstOrDefaultAsync(u => u.Account == loginDto.Account);
            if (user is null)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.InvalidAccount));
            }

            // 驗證密碼
            var hashedPassword = passwordHasher.HashPassword(loginDto.Password, user.PasswordSalt);
            if (hashedPassword != user.PasswordHash)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.InvalidPassword));
            }
            // 產生 JWT (建議修改 jwtService 使其產生一個包含 Jti 的物件)
            // 這裡我們預設產生一個 Guid 作為 Jti
            var jti = Guid.NewGuid();
            TokenResultModel jwtResult = jwtService.CreateLogInToken(user, jti.ToString()); // 傳入 jti 寫入 Claim

            var jwtRecord = new JwtRecord
            {
                Id = jti,
                UserId = user.Id,
                TokenValue = jwtResult.Token, // 或存 Hash 值
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = jwtResult.ExpiresAt, // 與 JWT 設定同步
                IsRevoked = false,
                RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = Request.Headers["User-Agent"].ToString()
            };

            await unitOfWork.JwtRecordRepository.AddAsync(jwtRecord);
            var saveResult = await unitOfWork.Complete();
            if (!saveResult)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToSaveDb));
            }

            return Ok(new LoginResponseDto
            {
                Id = user.Id,
                Account = user.Account,
                Token = jwtResult.Token
            });
        }

        [HttpGet("secret")]
        [HasPermission(PermissionKeysConstant.Auth.Secret.Key)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SuccessResponseExample))]
        public ActionResult<ApiResponseDto> Secret()
        {
            return Ok(new ApiResponseDto(HttpStatusCode.OK, "secret"));
        }

        [HttpGet("secret1")]
        [Authorize]
        [HasPermission(PermissionKeysConstant.Auth.Secret1.Key)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SuccessResponseExample))]
        public ActionResult<ApiResponseDto> Secret1()
        {
            return Ok(new ApiResponseDto(HttpStatusCode.OK, "secret1"));
        }

        [HttpGet("secret2")]
        [Authorize]
        [HasPermission(PermissionKeysConstant.Auth.Secret2.Key)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SuccessResponseExample))]
        public ActionResult<ApiResponseDto> Secret2()
        {
            return Ok(new ApiResponseDto(HttpStatusCode.OK, "secret2"));
        }

        [HttpGet("secret3")]
        [HasQueryToken]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(SuccessResponseExample))]
        public ActionResult<ApiResponseDto> Secret3()
        {
            return Ok(new ApiResponseDto(HttpStatusCode.OK, "secret3"));
        }
    }
}
