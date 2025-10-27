using Chipsoft.EPD.BL.interfaces;
using Chipsoft.EPD.DAL.interfaces;
using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.BL.managers;

public class PhysicianService : IPhysicianService
{
    private readonly IPhysicianRepository _physicianRepository;

    public PhysicianService(IPhysicianRepository physicianRepository)
    {
        _physicianRepository = physicianRepository;
    }

    public Physician? GetPhysicianById(int physicianId)
    {
        return _physicianRepository.GetById(physicianId);
    }

    public IEnumerable<Physician> GetAllPhysicians()
    {
        return _physicianRepository.GetAll();
    }
    
    public Physician AddPhysician(string name, string email, string phoneNumber)
    {
        var physician = new Physician
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber
        };
        _physicianRepository.Add(physician);
        return physician;
    }

    public void DeletePhysician(int physicianId)
    {
        var physician = _physicianRepository.GetById(physicianId);
        if (physician == null) throw new ArgumentException($"No physician found with Id {physicianId}");
        
        _physicianRepository.DeleteById(physicianId);
    }
}