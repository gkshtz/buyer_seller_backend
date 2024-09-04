namespace BuyerSeller.Core.Utility
{
    /// <summary>
    /// Constants class
    /// </summary>
    public static class Constants
    {
        public const string EMAIL_TEMPLATE_STRING = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Email Template</title>\r\n</head>\r\n<body style=\"margin: 0; padding: 0;\">\r\n\r\n    <table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"border-collapse: collapse;\">\r\n        <tr>\r\n            <td align=\"center\" bgcolor=\"#70bbd9\" style=\"padding: 40px 0 30px 0;\">\r\n                <img src=\"https://via.placeholder.com/300x230\" alt=\"Creating Email Magic\" width=\"300\" height=\"230\" style=\"display: block;\">\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#ffffff\" style=\"padding: 40px 30px 40px 30px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                    <tr>\r\n                        <td style=\"color: #153643; font-family: Arial, sans-serif; font-size: 24px;\">\r\n                            <b>Subject: {0:subject}</b>\r\n                        </td>\r\n                    </tr>\r\n                    <tr>\r\n                        <td style=\"padding: 20px 0 30px 0; color: #153643; font-family: Arial, sans-serif; font-size: 16px; line-height: 20px;\">\r\n                            {1:body}\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td bgcolor=\"#ee4c50\" style=\"padding: 30px 30px 30px 30px;\">\r\n                <table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">\r\n                    <tr>\r\n                        <td style=\"color: #ffffff; font-family: Arial, sans-serif; font-size: 14px;\" align=\"center\">\r\n                            &reg; Someone, somewhere 2023<br/>\r\n                            <a href=\"#\" style=\"color: #ffffff;\">Unsubscribe</a> to this newsletter instantly\r\n                        </td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n    </table>\r\n\r\n</body>\r\n</html>";
        /// <summary>
        /// Name of API key in header
        /// </summary>
        public const string ApiKeyHeaderName = "X-API-Key";

        /// <summary>
        /// Name of actual valid API key
        /// </summary>
        public const string ApiKeyName = "ApiKey";
    }
}
