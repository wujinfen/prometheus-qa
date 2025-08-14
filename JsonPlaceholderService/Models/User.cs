namespace JsonPlaceholderService.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Address address { get; set; } = new Address();
        public string Phone { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public Company company { get; set; } = new Company();
    }

    public class Address
    {
        public string Street { get; set; } = string.Empty;
        public string Suite { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Zipcode { get; set; } = string.Empty;
        public Geo geo { get; set; } = new Geo();
    }

    public class Geo
    {
        public string Lat { get; set; } = string.Empty;
        public string Lng { get; set; } = string.Empty;
    }

    public class Company
    {
        public string Name { get; set; } = string.Empty;
        public string CatchPhrase { get; set; } = string.Empty;
        public string Bs { get; set; } = string.Empty;
    }

}