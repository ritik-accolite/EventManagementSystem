namespace WebApplicationServer.Email
{
    public class IssueResolveEmail
    {
        public static string IssueResolveEmailBody(string email,int reviewid,int eventid,string description)
        {
            return $@"
            <html>
            <head></head>
            <body>
                <p>Dear { email },</p> 
                <p>We are pleased to inform you that the issue you reported has been successfully resolved.</p>
                <p>Issue Details:</p>
                <p>Issue ID : { reviewid }</p>
                <p>Event ID : { eventid }</p>
                <p>Issue Description : { description }</p>
                <br>
                <p>If you have any further questions or need additional assistance, please do not hesitate to contact our support team.</p>
                <p>Thank you for your patience and understanding.</p>
                <br>
                <p>Regards</p>
                <p>EventHub Team</p>
            </body>
            </html>";
        }
    }
}