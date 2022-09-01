namespace ENI.Constants
{
    public class LoginSession
    {
        public const string LOGIN_SESSION = "LOGIN_SESSION";
        public const string LOGIN_COOKIE  = "LOGIN_COOKIE";
        public const string PAGE_URL_TO_REDIRECT = "PAGE_URL_TO_REDIRECT";
    }

    public class SMTPSetup
    {
        // Setup for SendMail Function in Util.cs
        public const string SMTP                = "smtp.umbler.com";
        public const int PORT                   = 587;
        public const string EMAIL_FROM          = "";
        public const string EMAIL_FROM_NAME     = "";
        public const string EMAIL_REPLYTO       = "";
        public const string CREDENTIAL_EMAIL    = "";
        public const string CREDENTIAL_PASSWORD = "";        
    }

    public enum userRole
    {
        Admin = 1,
        Padrao = 2
    }
    
}