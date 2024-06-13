namespace WebApplicationServer.Email
{
    public class ForgetPasswordEmail
    {
        public static string ForgetPasswordEmailBody(string firstName, string lastName , string token, string username)
        {
            return $@"
            <html>
            <head></head>
            <body>
                <p>Dear {firstName} {lastName},</p> 
                <p>We have received a request to change password for {username}.</p>
                <p>Please use the following Token to set new password. The Token is only valid for 30 minutes. 
                <br>
                <p>Your token is : {token}.<p>
                <br>
                <p>If you have not initiated this request, please contact us on 1800329432 immediately.</p>
                <br>
                <p>Regards</p>
                <p>EventHub Team</p>
            </body>
            </html>";
        }
    }
}
