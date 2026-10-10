namespace MinimalApiCURD.Models
{
    public class Lead
    {
        public int id { get; set; }
        public string FisrtName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comapany { get; set; }
        public string Status { get; set; } = "New";
        public DateTime CreateAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
