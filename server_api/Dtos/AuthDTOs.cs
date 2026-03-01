using server_api.Dtos;

namespace Chinese_sale_Api.Dtos;



public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; }
    public GetUsers User { get; set; } = null!;
}
