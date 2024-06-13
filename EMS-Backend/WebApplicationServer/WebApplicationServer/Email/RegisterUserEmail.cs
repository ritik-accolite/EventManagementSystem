namespace WebApplicationServer.Email
{
    public class RegisterUserEmail
    {
        public static string RegisterEmailBody(string email, string confirmationlink)
        {
            return $@"
            <html>
            <head></head>
            <body>
                <p>Hello {email},</p>
                <p>Your Account has been Successfully Created, Please Confirm Your Account By Copying Below Link in the browser : </p> 
                <br>
                <p>https://eventhubfusion.azurewebsites.net{confirmationlink}</p>
                <br>
                <p>Regards</p>
                <p>EventHub Team</p>
               

            </body>
            </html>";
        }
    }
}
