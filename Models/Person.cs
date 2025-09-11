namespace MyApi.Models
{
    public class Person
    {
        public int PersonID { get; set; }  // PK
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}