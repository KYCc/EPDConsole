using System.ComponentModel.DataAnnotations;

namespace Chipsoft.EPD.Domain
{
    public class Patient
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public int HouseNumber { get; set; }
        
        public ICollection<Appointment> Appointments { get; set; }
    }
}