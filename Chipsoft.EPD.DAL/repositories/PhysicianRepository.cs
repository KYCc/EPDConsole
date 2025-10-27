using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.DAL.interfaces;

public class PhysicianRepository : IPhysicianRepository
{
    private readonly EPDDbContext _epdDbContext;

    public PhysicianRepository(EPDDbContext epdDbContext)
    {
        _epdDbContext = epdDbContext;
    }
    
    public Physician? GetById(int id)
    {
        return _epdDbContext.Physicians.Find(id);
    }

    public IEnumerable<Physician> GetAll()
    {
        return _epdDbContext.Physicians;
    }

    public void Add(Physician physician)
    {
        _epdDbContext.Physicians.Add(physician);
        _epdDbContext.SaveChanges();
    }

    public void DeleteById(int id)
    {
        var physician = _epdDbContext.Physicians.Find(id);
        if (physician == null) return;
        _epdDbContext.Physicians.Remove(physician);
        _epdDbContext.SaveChanges();
    }
}