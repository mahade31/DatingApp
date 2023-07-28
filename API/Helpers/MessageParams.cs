namespace API.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; }
        public string Conatiner { get; set; } = "Unread";
    }
}