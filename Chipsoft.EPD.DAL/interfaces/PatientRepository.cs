using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.DAL.interfaces;

public class PatientRepository : IPatientRepository
{
    private readonly EPDDbContext _epdDbContext;

    public PatientRepository(EPDDbContext epdDbContext)
    {
        _epdDbContext = epdDbContext;
    }
    
    public Patient? GetById(int id)
    {
        return _epdDbContext.Patients.Find(id);
    }

    public IEnumerable<Patient> GetAll()
    {
        return _epdDbContext.Patients;
    }

    public void Add(Patient patient)
    {
        _epdDbContext.Patients.Add(patient);
        _epdDbContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var patient = _epdDbContext.Patients.Find(id);
        if (patient == null) return;
        _epdDbContext.Patients.Remove(patient);
        _epdDbContext.SaveChanges();
    }
}