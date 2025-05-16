namespace backend.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
    }
}