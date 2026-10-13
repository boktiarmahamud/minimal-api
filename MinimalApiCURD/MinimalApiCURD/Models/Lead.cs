namespace MinimalApiCURD.Models
{
    public class Lead
    {
        public int id { get; set; }

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;

        public string Status { get; set; } = "New";

        public DateTime CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}