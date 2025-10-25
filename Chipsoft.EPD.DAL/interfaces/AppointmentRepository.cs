using Chipsoft.EPD.Domain;
using Microsoft.EntityFrameworkCore;

namespace Chipsoft.EPD.DAL.interfaces;

public class AppointmentRepository : IAppointmentRepository
{
    private EPDDbContext _epdDbContext;

    public AppointmentRepository(EPDDbContext epdDbContext)
    {
        _epdDbContext = epdDbContext;
    }
    
    public Appointment? GetById(int id)
    {
        return _epdDbContext.Appointments
                .Include(appointment => appointment.Patient)
                .Include(appointment => appointment.Physician)
                .SingleOrDefault(appointment => appointment.Id == id);
    }

    public IEnumerable<Appointment> GetAll()
    {
        return _epdDbContext.Appointments
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Physician)
            .ToList();
    }

    public IEnumerable<Appointment> GetByPhysicianId(int physicianId)
    {
        return _epdDbContext.Appointments
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Physician)
            .Where(appointment => appointment.PhysicianId == physicianId)
            .ToList();
    }

    public IEnumerable<Appointment> GetByPatientId(int patientId)
    {
        return _epdDbContext.Appointments
            .Include(appointment => appointment.Patient)
            .Include(appointment => appointment.Physician)
            .Where(appointment => appointment.PatientId == patientId)
            .ToList();
    }

    public void Add(Appointment appointment)
    {
        _epdDbContext.Appointments.Add(appointment);
        _epdDbContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var appointment = _epdDbContext.Appointments.Find(id);
        if (appointment == null) return;
        _epdDbContext.Appointments.Remove(appointment);
        _epdDbContext.SaveChanges();
    }
}