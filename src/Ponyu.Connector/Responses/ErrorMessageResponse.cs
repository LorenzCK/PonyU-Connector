namespace Ponyu.Connector.Responses
{
    internal class ErrorMessageResponse
    {
        public string Message { get; set; }

        public class ErrorDetail
        {
            public string Code { get; set; }

            public string Message { get; set; }
        }

        public ErrorDetail[] Errors { get; set; } = Array.Empty<ErrorDetail>();
    }
}
