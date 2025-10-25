using System.ComponentModel.DataAnnotations;

namespace Chipsoft.EPD.Domain
{
    public class Physician
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        
        public ICollection<Appointment> Appointments { get; set; }
    }
}