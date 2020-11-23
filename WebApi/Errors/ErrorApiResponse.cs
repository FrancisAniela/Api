using Newtonsoft.Json;

namespace WebApi.Errors
{

    public class ErrorApiResponse
    {
        public int StatusCode { get; set; }
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string InternalMessage { get; set; }

        public ErrorApiResponse(int statusCode, int errorCode, string internalErrorMessage = "")
        {
            this.StatusCode = statusCode;
            this.ErrorCode = errorCode;
            this.Message = ErrorCodes.ErrorsDescription[this.ErrorCode];
            this.InternalMessage = internalErrorMessage;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

}
