using Chipsoft.EPD.BL.interfaces;
using Chipsoft.EPD.DAL.interfaces;
using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.BL.managers;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly IPhysicianRepository _physicianRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository, 
        IPatientRepository patientRepository,
        IPhysicianRepository physicianRepository)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _physicianRepository = physicianRepository;
    }
    
    public IEnumerable<Appointment> GetAllAppointments()
    {
        return _appointmentRepository.GetAll();
    }

    public IEnumerable<Appointment> GetAppointmentsByPatient(int patientId)
    {
        return _appointmentRepository.GetByPatientId(patientId);
    }

    public IEnumerable<Appointment> GetAppointmentsByPhysician(int physicianId)
    {
        return _appointmentRepository.GetByPhysicianId(physicianId);
    }

    public Appointment AddAppointment(int patientId, int physicianId, DateTime dateAndTime, string description)
    {
        var patient = _patientRepository.GetById(patientId);
        if (patient == null) throw new ArgumentException($"No patient found with Id {patientId}");
        
        var physician = _physicianRepository.GetById(physicianId);
        if (physician == null) throw new ArgumentException($"No physician found with Id {physicianId}");
        
        var appointment = new Appointment
        {
            DateAndTime = dateAndTime,
            Description = description,
            Patient = patient,
            Physician = physician
        };
        _appointmentRepository.Add(appointment);
        return appointment;
    }
}