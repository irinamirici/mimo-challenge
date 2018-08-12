using Newtonsoft.Json;

namespace Mimo.Api.Dtos
{
    public class ResultError
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; private set; }

        [JsonProperty("errorSource")]
        public string ErrorSource { get; private set; }

        [JsonProperty("originalValue")]
        public object OriginalValue { get; private set; }

        [JsonProperty("errorDescription")]
        public string ErrorDescription { get; private set; }

        public ResultError()
        {

        }

        public ResultError(string errorCode, string errorDescription)
        {
            ErrorCode = errorCode;
            ErrorDescription = errorDescription;
        }

        public ResultError(string errorCode, string errorDescription, string errorSource) : this(errorCode, errorDescription)
        {
            ErrorSource = errorSource;
        }

        public ResultError(string errorCode, string errorDescription, string errorSource, object originalValue)
            : this(errorCode, errorDescription, errorSource)
        {
            OriginalValue = originalValue;
        }
    }
}