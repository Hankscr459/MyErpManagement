namespace MyErpManagement.Core.Dtos.Auth.Response
{
    /// <summary>
    /// 登入回傳物件
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// 使用者Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        /// <example>admin01</example>
        public string Account { get; set; } = default!;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = default!;

        /// <summary>
        /// 權杖(JWT Token)
        /// </summary>
        /// <example>eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiJhODcyMzU1NS01MjY0LTQ2OGUtOTZlOC0yZTU0MzQxNTFiOTIiLCJhY2NvdW50IjoiYWRtaW4iLCJqd3RSZWNvcmRJZCI6IjI3NzNkNzNmLTE2OGMtNDVlZS1iODY4LThmMGYwOTMwMTYwYSIsIm5iZiI6MTc2Nzg2MzgxMSwiZXhwIjoxNzY4NDY4NjExLCJpYXQiOjE3Njc4NjM4MTF9.bk2wJvCHjev0N29r50dHguKCIjut019NNjVsBz0GEh8imrxBeu3eGNevBFKd5iJTeOTXhoDlZpC-3oU8xCmuIg</example>
        public string Token { get; set; } = default!;
    }
}
