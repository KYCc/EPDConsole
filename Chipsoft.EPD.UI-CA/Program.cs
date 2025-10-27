using Chipsoft.EPD.BL.managers;
using Chipsoft.EPD.DAL;
using Chipsoft.EPD.DAL.interfaces;

namespace Chipsoft.EPD.UI.CA
{
    public class Program
    {
        //Don't create EF migrations, use the reset db option
        //This deletes and recreates the db, this makes sure all tables exist
        private static PatientService _patientService;
        private static PhysicianService _physicianService;
        private static AppointmentService _appointmentService;

        static void Main(string[] args)
        {
            ConfigureServices();
            while (ShowMenu())
            {
                //Continue
            }
        }
        
        public static bool ShowMenu()
        {
            Console.Clear();
            foreach (var line in File.ReadAllLines("logo.txt"))
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("");
            Console.WriteLine("1 - Patient toevoegen");
            Console.WriteLine("2 - Patienten verwijderen");
            Console.WriteLine("3 - Arts toevoegen");
            Console.WriteLine("4 - Arts verwijderen");
            Console.WriteLine("5 - Afspraak toevoegen");
            Console.WriteLine("6 - Afspraken inzien");
            Console.WriteLine("7 - Sluiten");
            Console.WriteLine("8 - Reset db");

            if (int.TryParse(Console.ReadLine(), out int option))
            {
                switch (option)
                {
                    case 1:
                        AddPatient();
                        return true;
                    case 2:
                        DeletePatient();
                        return true;
                    case 3:
                        AddPhysician();
                        return true;
                    case 4:
                        DeletePhysician();
                        return true;
                    case 5:
                        AddAppointment();
                        return true;
                    case 6:
                        ShowAppointment();
                        return true;
                    case 7:
                        return false;
                    case 8:
                        EPDDbContext dbContext = new EPDDbContext();
                        dbContext.Database.EnsureDeleted();
                        dbContext.Database.EnsureCreated();
                        return true;
                    default:
                        return true;
                }
            }
            return true;
        }

        private static void ConfigureServices()
        {
            var epdDbContext = new EPDDbContext();
            epdDbContext.Database.EnsureCreated();
            
            IPatientRepository patientRepository = new PatientRepository(epdDbContext);
            IPhysicianRepository physicianRepository = new PhysicianRepository(epdDbContext);
            IAppointmentRepository appointmentRepository = new AppointmentRepository(epdDbContext);

            _patientService = new PatientService(patientRepository);
            _physicianService = new PhysicianService(physicianRepository);
            _appointmentService = new AppointmentService(appointmentRepository, patientRepository, physicianRepository);
        }

        private static string PromptUserAndReturnString(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? "";
        }

        private static void AddPatient()
        {
            Console.Clear();
            Console.WriteLine("== PATIENT TOEVOEGEN ==");
            string name = PromptUserAndReturnString("Naam:");
            string email = PromptUserAndReturnString("Email:");
            string phoneNumber = PromptUserAndReturnString("Telefoonnr:");
            string country = PromptUserAndReturnString("Land:");
            string city = PromptUserAndReturnString("Stad:");
            string postalCode = PromptUserAndReturnString("Postcode:");
            string street = PromptUserAndReturnString("Straat:");
            int houseNumber;
            while (!int.TryParse(PromptUserAndReturnString("Huisnummer: "), out houseNumber))
            {
                Console.WriteLine("Gelieve een correct huisnummer in te geven!");
            }
            
            _patientService.AddPatient(name, email, phoneNumber, country, city, postalCode, street, houseNumber);
            Console.WriteLine("Patient succesvol toegevoegd!");
            Console.WriteLine("Druk op een knop om terug te keren.");
            Console.ReadKey();
        }

        private static void ShowAppointment()
        {
            Console.Clear();
            Console.WriteLine("== AFSPRAKEN ==");
            var appointments = _appointmentService.GetAllAppointments().ToList();
            if (appointments.Count == 0)
            {
                Console.WriteLine("Er zijn nog geen afspraken");
                Console.WriteLine("Druk op een knop om terug te keren.");
                Console.ReadKey();
                return;
            }
            
            var physicianIds = appointments.Select(a => a.PhysicianId).ToHashSet();
            var patientIds  = appointments.Select(a => a.PatientId).ToHashSet();
            
            Console.WriteLine("1 - Alle afspraken");
            Console.WriteLine("2 - Filter op arts");
            Console.WriteLine("3 - Filter op patient");
            int choice;
            while (true)
            {
                Console.Write("\nMaak een keuze: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    var choiceExists = (choice == 1 ||  choice == 2 || choice == 3);
                    if (choiceExists) break;
                }

                Console.WriteLine("Gelieve een correcte keuze in te geven!");
            }

            if (choice == 1)
            {
                Console.WriteLine("Alle afspraken:");
                foreach (var appointment in appointments)
                {
                    Console.WriteLine($"{appointment.Id} - " +
                                      $"{appointment.Description} - " +
                                      $"{appointment.DateAndTime.ToShortDateString()} - " +
                                      $"{appointment.DateAndTime.ToShortTimeString()} - " +
                                      $"Arts: {appointment.Physician.Name} - " +
                                      $"Patient: {appointment.Patient.Name}");
                }
            }

            if (choice == 2)
            {
                Console.WriteLine("Artsen:");
                foreach (var physician in physicianIds)
                {
                    Console.WriteLine($"{physician} - {_physicianService.GetPhysicianById(physician)!.Name}");
                }
                int physicianId;
                while (true)
                {
                    Console.Write("\nGeef een ID in: ");

                    if (int.TryParse(Console.ReadLine(), out physicianId))
                    {
                        var correctId = physicianIds.Contains(physicianId);
                        if (correctId) break;
                    }

                    Console.WriteLine("Gelieve een correcte ID in te geven!");
                }
                
                Console.WriteLine("Alle afspraken van gekozen arts:");
                var appointmentsOfPhysician = _appointmentService.GetAppointmentsByPhysician(physicianId);
                foreach (var appointment in appointmentsOfPhysician)
                {
                    Console.WriteLine($"{appointment.Id} - " +
                                      $"{appointment.Description} - " +
                                      $"{appointment.DateAndTime.ToShortDateString()} - " +
                                      $"{appointment.DateAndTime.ToShortTimeString()} - " +
                                      $"Arts: {appointment.Physician.Name} - " +
                                      $"Patient: {appointment.Patient.Name}");
                }
            }
            
            if (choice == 3)
            {
                Console.WriteLine("Patienten:");
                foreach (var patient in patientIds)
                {
                    Console.WriteLine($"{patient} - {_patientService.GetPatientById(patient)!.Name}");
                }
                int patientId;
                while (true)
                {
                    Console.Write("\nGeef een ID in: ");

                    if (int.TryParse(Console.ReadLine(), out patientId))
                    {
                        var correctId = patientIds.Contains(patientId);
                        if (correctId) break;
                    }

                    Console.WriteLine("Gelieve een correcte ID in te geven!");
                }
                
                Console.WriteLine("Alle afspraken van gekozen patient:");
                var appointmentsOfPatient = _appointmentService.GetAppointmentsByPatient(patientId);
                foreach (var appointment in appointmentsOfPatient)
                {
                    Console.WriteLine($"{appointment.Id} - " +
                                      $"{appointment.Description} - " +
                                      $"{appointment.DateAndTime.ToShortDateString()} - " +
                                      $"{appointment.DateAndTime.ToShortTimeString()} - " +
                                      $"Arts: {appointment.Physician.Name} - " +
                                      $"Patient: {appointment.Patient.Name}");
                }
            }
            
            Console.WriteLine("Druk op een knop om terug te keren.");
            Console.ReadKey();
        }

        private static void AddAppointment()
        {
            Console.Clear();
            Console.WriteLine("== AFSPRAAK TOEVOEGEN ==");
            var patients = _patientService.GetAllPatients().ToList();
            if (patients.Count == 0)
            {
                Console.WriteLine("Er zijn nog geen patienten");
                Console.WriteLine("Druk op een knop om terug te keren.");
                Console.ReadKey();
                return;
            }
            
            var physicians = _physicianService.GetAllPhysicians().ToList();
            if (physicians.Count == 0)
            {
                Console.WriteLine("Er zijn nog geen artsen");
                Console.WriteLine("Druk op een knop om terug te keren.");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("Selecteer een arts:");
            foreach (var physician in physicians)
            {
                Console.WriteLine($"{physician.Id} - {physician.Name}");
            }
            int physicianId;
            while (true)
            {
                Console.Write("\nGeef een ID in: ");

                if (int.TryParse(Console.ReadLine(), out physicianId))
                {
                    var physicianExists = physicians.Any(p => p.Id == physicianId);
                    if (physicianExists) break;
                }

                Console.WriteLine("Gelieve een correcte ID in te geven!");
            }
            
            Console.WriteLine("Selecteer een patient:");
            foreach (var patient in patients)
            {
                Console.WriteLine($"{patient.Id} - {patient.Name}");
            }
            
            int patientId;
            while (true)
            {
                Console.Write("\nGeef een ID in: ");

                if (int.TryParse(Console.ReadLine(), out patientId))
                {
                    var patientExists = patients.Any(p => p.Id == patientId);
                    if (patientExists) break;
                }

                Console.WriteLine("Gelieve een correcte ID in te geven!");
            }
            
            DateTime dateTime;
            while (true)
            {
                Console.Write("\nGeef de datum en tijd (formaat: yyyy-MM-dd HH:mm): ");
                if (DateTime.TryParse(Console.ReadLine(), out dateTime))
                    break;

                Console.WriteLine("Gelieve het correcte formaat te gebruiken.");
            }
            
            string description = PromptUserAndReturnString("Beschrijving:");

            _appointmentService.AddAppointment(patientId, physicianId, dateTime, description);
            Console.WriteLine("Afspraak succesvol toegevoegd!");
            Console.WriteLine("Druk op een knop om terug te keren.");
            Console.ReadKey();
        }

        private static void DeletePhysician()
        {
            Console.Clear();
            Console.WriteLine("== ARTS VERWIJDEREN ==");
            var physicians = _physicianService.GetAllPhysicians().ToList();
            if (physicians.Count == 0)
            {
                Console.WriteLine("Er zijn nog geen artsen");
                Console.WriteLine("Druk op een knop om terug te keren.");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("Selecteer een arts:");
            foreach (var physician in physicians)
            {
                Console.WriteLine($"{physician.Id} - {physician.Name}");
            }
            
            int physicianId;
            while (true)
            {
                Console.Write("\nGeef een ID in: ");

                if (int.TryParse(Console.ReadLine(), out physicianId))
                {
                    var physicianExists = physicians.Any(p => p.Id == physicianId);
                    if (physicianExists) break;
                }

                Console.WriteLine("Gelieve een correcte ID in te geven!");
            }

            _physicianService.DeletePhysician(physicianId);
            Console.WriteLine("Arts succesvol verwijderd!");
            Console.WriteLine("Druk op een knop om terug te keren.");
            Console.ReadKey();
        }

        private static void AddPhysician()
        {
            Console.Clear();
            Console.WriteLine("== ARTS TOEVOEGEN ==");
            string name = PromptUserAndReturnString("Naam:");
            string email = PromptUserAndReturnString("Email:");
            string phoneNumber = PromptUserAndReturnString("Telefoonnr:");
            
            _physicianService.AddPhysician(name, email, phoneNumber);
            Console.WriteLine("Arts succesvol toegevoegd!");
            Console.WriteLine("Druk op een knop om terug te keren.");
            Console.ReadKey();
        }

        private static void DeletePatient()
        {
            Console.Clear();
            Console.WriteLine("== PATIENT VERWIJDEREN ==");
            var patients = _patientService.GetAllPatients().ToList();
            if (patients.Count == 0)
            {
                Console.WriteLine("Er zijn nog geen patienten");
                Console.WriteLine("Druk op een knop om terug te keren.");
                Console.ReadKey();
                return;
            }
            
            Console.WriteLine("Selecteer een patient:");
            foreach (var patient in patients)
            {
                Console.WriteLine($"{patient.Id} - {patient.Name}");
            }
            
            int patientId;
            while (true)
            {
                Console.Write("\nGeef een ID in: ");

                if (int.TryParse(Console.ReadLine(), out patientId))
                {
                    var patientExists = patients.Any(p => p.Id == patientId);
                    if (patientExists) break;
                }

                Console.WriteLine("Gelieve een correcte ID in te geven!");
            }

            _patientService.DeletePatient(patientId);
            Console.WriteLine("Patient succesvol verwijderd!");
            Console.WriteLine("Druk op een knop om terug te keren.");
            Console.ReadKey();
        }
    }
}