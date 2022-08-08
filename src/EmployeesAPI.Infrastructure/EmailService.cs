using RestSharp;
using RestSharp.Authenticators;

namespace EmployeesAPI.Infrastructure;

public static class EmailService
{
    private const string Domain = "sandbox645aee9d7e0049b8bb17e6f0efc4339b.mailgun.org";
    private const string APIKey = "f730f9e1553fb2f7ea7356092928b829-1b3a03f6-4412b043";

    public static RestResponse SendEmail(string sendToEmail, string code)
    {
        var client = new RestClient("https://api.mailgun.net/v3");

        client.Authenticator = new HttpBasicAuthenticator("api", APIKey);
        var request = new RestRequest
        {
            Resource = $"{Domain}/messages"
        };

        request.AddParameter("from", "Excited User <andreythedeveloper@gmail.com>");
        request.AddParameter("to", sendToEmail);
        request.AddParameter("subject", "Verification");
        request.AddParameter("text", $"Your verification code is: {code}");
        request.Method = Method.Post;

        return client.Execute(request);
    }
}