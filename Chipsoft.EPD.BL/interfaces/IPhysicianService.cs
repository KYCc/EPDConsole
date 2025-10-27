using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.BL.interfaces;

public interface IPhysicianService
{
    public Physician? GetPhysicianById(int physicianId);
    public IEnumerable<Physician> GetAllPhysicians();
    public Physician AddPhysician(string name, string email, string phoneNumber);
    public void DeletePhysician(int physicianId);
}