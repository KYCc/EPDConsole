using Chipsoft.EPD.BL.interfaces;
using Chipsoft.EPD.DAL.interfaces;
using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.BL.managers;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository)
    {
        _patientRepository = patientRepository;
    }
    
    public Patient AddPatient(string name, string email, string phoneNumber, string country, string city, string postalCode,
        string street, int houseNumber)
    {
        var patient = new Patient
        {
            Name = name,
            Email = email,
            PhoneNumber = phoneNumber,
            Country = country,
            City = city,
            PostalCode = postalCode,
            Street = street,
            HouseNumber = houseNumber
        };
        _patientRepository.Add(patient);
        return patient;
    }

    public void DeletePatient(int patientId)
    {
        var patient = _patientRepository.GetById(patientId);
        if (patient == null) throw new ArgumentException($"No patient found with Id {patientId}");
        
        _patientRepository.DeleteById(patientId);
    }
}