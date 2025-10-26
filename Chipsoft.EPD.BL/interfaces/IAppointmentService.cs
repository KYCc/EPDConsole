using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.BL.interfaces;

public interface IAppointmentService
{
    public IEnumerable<Appointment> GetAllAppointments();
    public IEnumerable<Appointment> GetAppointmentsByPatient(int patientId);
    public IEnumerable<Appointment> GetAppointmentsByPhysician(int physicianId);
    public Appointment AddAppointment(int patientId, int physicianId, DateTime dateAndTime, string description);
    
}