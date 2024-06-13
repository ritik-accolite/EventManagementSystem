namespace WebApplicationServer.Email
{
    public class UpdateEventEmail
    {
        public static string EmailBody(string eventname,string firstname, string lastname, string date, string time,string location, string description)
        {
            return $@"
            <html>
            <head></head>
            <body>
                <h4>Event Updated: {eventname}</h4>
                <br>
                <p>Dear {firstname} {lastname}, </p>
                
                <p>We would like to inform you that the event {eventname} you booked has been updated.</p>
                <br>
                <p>Here are the new details:</p>
                
                <p>Date : {date}</p>
                
                <p>Time : {time}</p>
                
                <p>Location : {location}</p>
                
                <p>Description : {description}</p>
                <br>
                <p>If you have any questions, please contact us on 7856023589.</p>
                <br>
                <p>Regards,</p>
               
                <p>EventHub Team</p>

            </body>
            </html>";
        }
    }
}


