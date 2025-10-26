using Chipsoft.EPD.Domain;

namespace Chipsoft.EPD.BL.interfaces;

public interface IPatientService
{
    public Patient AddPatient(
        string name, 
        string email, 
        string phoneNumber, 
        string country, 
        string city, 
        string postalCode, 
        string street, 
        int houseNumber);
    
    public void DeletePatient(int patientId);
}