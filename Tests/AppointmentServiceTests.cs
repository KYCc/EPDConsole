using Chipsoft.EPD.BL.managers;
using Chipsoft.EPD.DAL.interfaces;
using Chipsoft.EPD.Domain;
using Moq;
using Xunit;

namespace Tests;

public class AppointmentServiceTests
{
    private readonly Mock<IAppointmentRepository> _mockAppointmentRepository;
    private readonly Mock<IPatientRepository> _mockPatientRepository;
    private readonly Mock<IPhysicianRepository> _mockPhysicianRepository;
    private readonly AppointmentService _appointmentService;

    public AppointmentServiceTests()
    {
        _mockAppointmentRepository = new Mock<IAppointmentRepository>();
        _mockPatientRepository = new Mock<IPatientRepository>();
        _mockPhysicianRepository = new Mock<IPhysicianRepository>();
        
        _appointmentService = new AppointmentService(
            _mockAppointmentRepository.Object,
            _mockPatientRepository.Object,
            _mockPhysicianRepository.Object
        );
    }

    [Fact]
    public void GetAllAppointments_ReturnsAllAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, Description = "Checkup" },
            new Appointment { Id = 2, Description = "Follow-up" }
        };
        _mockAppointmentRepository.Setup(r => r.GetAll()).Returns(appointments);

        // Act
        var result = _appointmentService.GetAllAppointments().ToList();

        // Assert
        Assert.Equal(2, result.Count);
        _mockAppointmentRepository.Verify(r => r.GetAll(), Times.Once);
    }

    [Fact]
    public void GetAppointmentsByPatient_ReturnsPatientAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 5, Description = "Checkup" }
        };
        _mockAppointmentRepository.Setup(r => r.GetByPatientId(5)).Returns(appointments);

        // Act
        var result = _appointmentService.GetAppointmentsByPatient(5).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(5, result[0].PatientId);
    }

    [Fact]
    public void GetAppointmentsByPhysician_ReturnsPhysicianAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PhysicianId = 3, Description = "Consultation" }
        };
        _mockAppointmentRepository.Setup(r => r.GetByPhysicianId(3)).Returns(appointments);

        // Act
        var result = _appointmentService.GetAppointmentsByPhysician(3).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal(3, result[0].PhysicianId);
    }

    [Fact]
    public void AddAppointment_WithValidData_CreatesAppointmentSuccessfully()
    {
        // Arrange
        var patient = new Patient { Id = 1, Name = "John Doe" };
        var physician = new Physician { Id = 2, Name = "Dr. Smith" };
        var dateTime = new DateTime(2025, 11, 15, 10, 30, 0);
        
        _mockPatientRepository.Setup(r => r.GetById(1)).Returns(patient);
        _mockPhysicianRepository.Setup(r => r.GetById(2)).Returns(physician);

        Appointment? capturedAppointment = null;
        _mockAppointmentRepository
            .Setup(r => r.Add(It.IsAny<Appointment>()))
            .Callback<Appointment>(a => capturedAppointment = a);

        // Act
        _appointmentService.AddAppointment(1, 2, dateTime, "Annual checkup");

        // Assert
        Assert.NotNull(capturedAppointment);
        Assert.Equal(dateTime, capturedAppointment.DateAndTime);
        Assert.Equal("Annual checkup", capturedAppointment.Description);
        Assert.Same(patient, capturedAppointment.Patient);
        Assert.Same(physician, capturedAppointment.Physician);
        _mockAppointmentRepository.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Once);
    }

    [Fact]
    public void AddAppointment_WhenPatientDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        _mockPatientRepository.Setup(r => r.GetById(999)).Returns((Patient?)null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _appointmentService.AddAppointment(999, 1, DateTime.Now, "Test"));
        
        Assert.Contains("No patient found with Id 999", exception.Message);
        _mockAppointmentRepository.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Never);
    }

    [Fact]
    public void AddAppointment_WhenPhysicianDoesNotExist_ThrowsArgumentException()
    {
        // Arrange
        var patient = new Patient { Id = 1, Name = "John Doe" };
        _mockPatientRepository.Setup(r => r.GetById(1)).Returns(patient);
        _mockPhysicianRepository.Setup(r => r.GetById(999)).Returns((Physician?)null);

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _appointmentService.AddAppointment(1, 999, DateTime.Now, "Test"));
        
        Assert.Contains("No physician found with Id 999", exception.Message);
        _mockAppointmentRepository.Verify(r => r.Add(It.IsAny<Appointment>()), Times.Never);
    }
}