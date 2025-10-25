using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.DAL.interfaces;

public interface IAppointmentRepository
{
    public Appointment? GetById(int id);
    public IEnumerable<Appointment> GetAll();
    public IEnumerable<Appointment> GetByPhysicianId(int physicianId);
    public IEnumerable<Appointment> GetByPatientId(int patientId);
    public void Add(Appointment appointment);
    public void DeleteById(int id);
}