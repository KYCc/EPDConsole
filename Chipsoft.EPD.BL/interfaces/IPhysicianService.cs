using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.BL.interfaces;

public interface IPhysicianService
{
    public Physician AddPhysician(string name, string email, string phoneNumber);
    public void DeletePhysician(int physicianId);
}