using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using ShiftManagementFE.DTOs;

namespace ShiftManagementFE.Services
{
    public class UserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<UserDto>?> GetUsersAsync(string? search = null, int page = 1, int pageSize = 50)
        {
            var url = $"/api/Users?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={search}";
            return await _http.GetFromJsonAsync<List<UserDto>>(url);
        }

        public async Task<UserDto?> GetUserAsync(int id)
        {
            return await _http.GetFromJsonAsync<UserDto>($"/api/Users/{id}");
        }

        public async Task<ApiResult<int>> CreateUserAsync(UserCreateDto input)
        {
            var res = await _http.PostAsJsonAsync("/api/Users", input);
            if (res.IsSuccessStatusCode)
            {
                var id = await res.Content.ReadFromJsonAsync<int>();
                return new ApiResult<int>(true, id, null);
            }
            var err = await res.Content.ReadAsStringAsync();
            return new ApiResult<int>(false, 0, err);
        }

        public async Task<ApiResult> UpdateUserAsync(int id, UserUpdateDto input)
        {
            var res = await _http.PutAsJsonAsync($"/api/Users/{id}", input);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> DeleteUserAsync(int id)
        {
            var res = await _http.DeleteAsync($"/api/Users/{id}");
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> ChangePasswordAsync(int id, ChangePasswordDto dto)
        {
            var res = await _http.PatchAsJsonAsync($"/api/Users/{id}/changepassword", dto);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> ToggleLockAsync(int id)
        {
            var res = await _http.PatchAsync($"/api/Users/{id}/lock", null);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> AddRoleAsync(int userId, int roleId)
        {
            var res = await _http.PostAsync($"/api/Users/{userId}/roles?roleId={roleId}", null);
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<ApiResult> RemoveRoleAsync(int userId, int roleId)
        {
            var res = await _http.DeleteAsync($"/api/Users/{userId}/roles/{roleId}");
            if (res.IsSuccessStatusCode)
                return ApiResult.Success();
            var err = await res.Content.ReadAsStringAsync();
            return ApiResult.Fail(err);
        }

        public async Task<List<string>?> GetUserRolesAsync(int userId)
        {
            return await _http.GetFromJsonAsync<List<string>>($"/api/Users/{userId}/roles");
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var res = await _http.PostAsJsonAsync("/api/Users/login", dto);
            if (res.IsSuccessStatusCode)
                return await res.Content.ReadAsStringAsync();
            return null;
        }
    }

    public class ApiResult
    {
        public bool IsSuccess { get; }
        public string? Error { get; }

        public ApiResult(bool isSuccess, string? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static ApiResult Success() => new ApiResult(true, null);
        public static ApiResult Fail(string? error) => new ApiResult(false, error);
    }

    public class ApiResult<T> : ApiResult
    {
        public T Data { get; }
        public ApiResult(bool isSuccess, T data, string? error) : base(isSuccess, error)
        {
            Data = data;
        }
    }
}