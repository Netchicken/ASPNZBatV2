namespace RazorHtmlEmails.RazorClassLib.Views.Emails.RebookSessions
{
    public class RebookSessionsEmailViewModel
    {
        public RebookSessionsEmailViewModel(string confirmEmailUrl)
        {
            ConfirmEmailUrl = confirmEmailUrl;
        }

        public string ConfirmEmailUrl { get; set; }
    }
}
