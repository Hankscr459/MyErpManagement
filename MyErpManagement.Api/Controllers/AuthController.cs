using MapsterMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; // statusCode lint
using Microsoft.AspNetCore.Mvc;
using MyErpManagement.Api.Constants;
using MyErpManagement.Api.Examples.Auth;
using MyErpManagement.Core.Dtos.Auth.Request;
using MyErpManagement.Core.Dtos.Auth.Response;
using MyErpManagement.Core.Dtos.Shared;
using MyErpManagement.Core.IRepositories;
using MyErpManagement.Core.Modules.CacheModule.IServices;
using MyErpManagement.Core.Modules.EmailModule.IServices;
using MyErpManagement.Core.Modules.JwtModule.Entities;
using MyErpManagement.Core.Modules.JwtModule.IServices;
using MyErpManagement.Core.Modules.JwtModule.Models;
using MyErpManagement.Core.Modules.MessageBusModule.IServices;
using MyErpManagement.Core.Modules.MessageBusModule.Models;
using MyErpManagement.Core.Modules.UsersModule.Entities;
using MyErpManagement.Core.Modules.UsersModule.IServices;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Security.Cryptography;
using TGolla.Swashbuckle.AspNetCore.SwaggerGen;

namespace MyErpManagement.Api.Controllers
{
    [Tags("認證相關")]
    [SwaggerControllerOrder(1)]
    public class AuthController(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IPasswordService passwordHasher,
        IEmailService emailService,
        ICachService cachService,
        IMapper mapper
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
            var user = await unitOfWork.UserRepository
                    .GetFirstOrDefaultAsync(u => u.Account == loginDto.Account || u.Email == loginDto.Account);
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
                Email = user.Email,
                Token = jwtResult.Token
            });
        }

        /// <summary>
        /// 發送Email驗證碼（註冊用）
        /// </summary>
        /// <param name="verifyEmailRequestDto"></param>
        /// <param name="mqService"></param>
        /// <returns></returns>
        [HttpPost("verify-email/register")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status409Conflict)]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ConflictVerifyEmailRegisterExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestVerifyEmailRegisterExample))]
        public async Task<ActionResult<VerifyEmailResponseDto>> VerifyEmail([FromBody] VerifyEmailRequestDto verifyEmailRequestDto, [FromServices] IRabbitMqService mqService)
        {
            var existingUser = await unitOfWork.UserRepository
                .GetFirstOrDefaultAsync(u => u.Email == verifyEmailRequestDto.Email);
            if (existingUser is not null)
            {
                return Conflict(new ApiResponseDto(HttpStatusCode.Conflict, ResponseTextConstant.Conflict.EmailAlreadyInUse));
            }
            var emailCode = await cachService.GetRegistCodeAsync(verifyEmailRequestDto.Email);
            if (emailCode is not null)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToReSendEmailCode));
            }
            string verificationCode = new Random().Next(100000, 999999).ToString();
            var resendIntervalMinutes = await cachService.SaveRegisterCodeAsync(verifyEmailRequestDto.Email, verificationCode);

            // 將寄信任務派發到 RabbitMQ
            var emailTask = new EmailMessage { Email = verifyEmailRequestDto.Email, VerificationCode = verificationCode };
            await mqService.PublishEmailTaskAsync(emailTask);

            return Ok(new VerifyEmailResponseDto
            {
                Message = ResponseTextConstant.Ok.SuccessSendEmailCode,
                ResendIntervalMinutes = resendIntervalMinutes
            });
        }

        /// <summary>
        /// 註冊
        /// </summary>
        /// <param name="registerRequestDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status409Conflict)]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(BadRequestRegisterExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(ConflictRegisterExample))]
        public async Task<ActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var emailCode = await cachService.GetRegistCodeAsync(registerRequestDto.Email);
            if (emailCode is null || emailCode != registerRequestDto.VerificationCode)
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.InvalidEmailCode));
            }
            var bytes = new byte[8];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            var account = Convert.ToBase64String(bytes);
            var userEntity = mapper.Map<User>(registerRequestDto);
            userEntity.Account = account;
            userEntity.PasswordSalt = passwordHasher.GenerateSalt();
            userEntity.PasswordHash = passwordHasher.HashPassword(registerRequestDto.Password, userEntity.PasswordSalt);
            unitOfWork.UserRepository.Add(userEntity);
            if (!await unitOfWork.Complete())
            {
                return BadRequest(new ApiResponseDto(HttpStatusCode.BadRequest, ResponseTextConstant.BadRequest.FailToCreateUser));
            }
            return NoContent();
        }
    }
}
