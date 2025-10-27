using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.DAL.interfaces;

public interface IPhysicianRepository
{
    public Physician? GetById(int id);
    public IEnumerable<Physician> GetAll();
    public void Add(Physician physician);
    public void DeleteById(int id);
}