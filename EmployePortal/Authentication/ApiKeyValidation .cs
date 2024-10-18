namespace EmployePortal.Authentication
{
    public class ApiKeyValidation : IApiKeyValidation
    {
        private readonly IConfiguration _configuration;
        public ApiKeyValidation(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool IsValidApiKey(string userApiKey)
        {
            if (string.IsNullOrWhiteSpace(userApiKey))
                return false;
            var apiKey = _configuration.GetValue<string>(Constants.Constants.ApiKeyName);
            if (apiKey == null || apiKey != userApiKey)
                return false;
            return true;
        }
    }
}
