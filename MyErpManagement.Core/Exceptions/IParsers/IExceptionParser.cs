using MyErpManagement.Core.Dtos.Shared;

namespace MyErpManagement.Core.Exceptions.IParsers
{
    public interface IExceptionParser
    {
        ApiResponseDto? Parser(Exception ex);
    }
}
