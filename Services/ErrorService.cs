using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ShiftManagementFE.Services
{
    public class ErrorDetail
    {
        public string Title { get; set; } = "Đã xảy ra lỗi!";
        public string? Message { get; set; }
        public string? Detail { get; set; }
        public DateTime OccurredAt { get; set; } = DateTime.Now;
        public bool IsTechnical { get; set; }
    }

    public class ErrorService
    {
        private readonly HttpClient _http;

        public ErrorService(HttpClient http)
        {
            _http = http;
        }

        public event Action<ErrorDetail>? OnError;
        public event Action? OnClear;

        public ErrorDetail? LastError { get; private set; }

        public void SetError(
            string? message,
            string? title = null,
            string? detail = null,
            bool isTechnical = false
        )
        {
            LastError = new ErrorDetail
            {
                Title = title ?? (isTechnical ? "Lỗi hệ thống" : "Lỗi"),
                Message = message,
                Detail = detail,
                IsTechnical = isTechnical,
                OccurredAt = DateTime.Now
            };
            OnError?.Invoke(LastError);
        }

        public void ClearError()
        {
            LastError = null;
            OnClear?.Invoke();
        }

        /// <summary>
        /// Helper method để wrap try-catch, cho phép truyền message chung & chi tiết kỹ thuật nếu có.
        /// Nếu có url, sẽ thực hiện HTTP call và xử lý lỗi tập trung.
        /// </summary>
        public async Task<T?> TryCatchHttpAsync<T>(
            Func<Task<T>> httpAction,
            string? userMessage = null,
            bool showDetail = false
        )
        {
            try
            {
                ClearError();
                return await httpAction();
            }
            catch (HttpRequestException ex)
            {
                SetError(
                    userMessage ?? "Không thể kết nối đến máy chủ. Vui lòng thử lại sau!",
                    isTechnical: true,
                    detail: showDetail ? ex.ToString() : null
                );
                return default;
            }
            catch (NotSupportedException ex)
            {
                SetError(
                    "Định dạng phản hồi không được hỗ trợ.",
                    isTechnical: true,
                    detail: showDetail ? ex.ToString() : null
                );
                return default;
            }
            catch (Exception ex)
            {
                SetError(
                    userMessage ?? "Có lỗi hệ thống. Vui lòng thử lại sau!",
                    isTechnical: true,
                    detail: showDetail ? ex.ToString() : null
                );
                return default;
            }
        }
    }
}