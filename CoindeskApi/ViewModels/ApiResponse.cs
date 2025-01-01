namespace CoindeskApi.Response
{
    public class ApiResponse<T>
    {
        public int? StatusCode { get; set; }

        public bool Success { get; set; } = false;

        public string? Detail { get; set; }

        public string? Message { get; set; }

        public List<string>? Errors { get; set; }

        public T? Data { get; set; }
    }
}
