using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class AuthService
    {
        private readonly HttpClient _http;
        private readonly ErrorService _errorService;

        public AuthService(HttpClient http, ErrorService errorService)
        {
            _http = http;
            _errorService = errorService;
        }

        public async Task<ApiResult<AuthResultDto>> LoginAsync(LoginDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Users/login", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var result = await res.Content.ReadFromJsonAsync<AuthResultDto>();
                        return new ApiResult<AuthResultDto>(true, result!, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<AuthResultDto>(false, null!, err);
                },
                userMessage: "Đăng nhập thất bại!"
            ) ?? new ApiResult<AuthResultDto>(false, null!, "Lỗi hệ thống.");
        }

        public async Task<ApiResult<int>> RegisterAsync(RegisterDto dto)
        {
            return await _errorService.TryCatchHttpAsync(
                async () =>
                {
                    var res = await _http.PostAsJsonAsync("api/Users/register", dto);
                    if (res.IsSuccessStatusCode)
                    {
                        var userId = await res.Content.ReadFromJsonAsync<int>();
                        return new ApiResult<int>(true, userId, null);
                    }
                    var err = await res.Content.ReadAsStringAsync();
                    return new ApiResult<int>(false, 0, err);
                },
                userMessage: "Đăng ký thất bại!"
            ) ?? new ApiResult<int>(false, 0, "Lỗi hệ thống.");
        }

        public async Task<AuthUserDto?> GetMeAsync(int userId)
        {
            return await _errorService.TryCatchHttpAsync(
                async () => await _http.GetFromJsonAsync<AuthUserDto>($"api/Users/{userId}"),
                userMessage: "Không thể lấy thông tin người dùng!"
            );
        }
    }

    // AuthResultDto nên chứa token và user info, bạn cần bổ sung DTO này nếu chưa có
    public class AuthResultDto
    {
        public string? Token { get; set; }
        public AuthUserDto? User { get; set; }
    }
}