using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.DAL.interfaces;

public interface IPatientRepository
{
    public Patient? GetById(int id);
    public IEnumerable<Patient> GetAll();
    public void Add(Patient patient);
    public void DeleteById(int id);
}