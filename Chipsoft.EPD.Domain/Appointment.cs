using System.ComponentModel.DataAnnotations;

namespace Chipsoft.EPD.Domain
{
    public class Appointment
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Description { get; set; }
        
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        
        public int PhysicianId { get; set; }
        public Physician Physician { get; set; }
    }
}