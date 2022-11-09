using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Tutorial8.DTOs.Requests;
using Tutorial8.DTOs.Responses;
using Tutorial8.Models;

namespace Tutorial8.Services
{
    public interface IDatabaseService
    {
        Task<GetDoctorResponseDTO> GetDoctorsAsync(int idDoctor);
        bool AddDoctor(AddDoctorRequestDTO doctor);
        Task<bool> ModifyDoctorAsync(AddDoctorRequestDTO doctor, int idDoctor);
        Task<bool> DeleteDoctorAsync(int idDoctor);
        Task<GetPrescriptionResponseDTO> GetPrescriptionAsync(int idPrescription);
    }

    public class MedDatabaseService : IDatabaseService
    {
        private readonly MedsDbContext _context;

        public MedDatabaseService(MedsDbContext context)
        {
            _context = context;
        }

        public async Task<GetDoctorResponseDTO> GetDoctorsAsync(int idDoctor)
        {
            var doctor = await _context.Doctors.Where(d => d.IdDoctor == idDoctor).Select(d => new GetDoctorResponseDTO
            {
                IdDoctor = d.IdDoctor,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Email = d.Email
            }).FirstOrDefaultAsync();

            return doctor;
        }

        public bool AddDoctor(AddDoctorRequestDTO doctor)
        {
            var _doctor = new Doctor
            {
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Email = doctor.Email
            };
          

            _context.Doctors.Add(_doctor);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> ModifyDoctorAsync(AddDoctorRequestDTO doctorRequest, int idDoctor)
        {
            var doctor = await _context.Doctors.SingleOrDefaultAsync(d => d.IdDoctor == idDoctor);

            if (doctor is null)
            {
                return false;
            }

            doctor.FirstName = doctorRequest.FirstName;
            doctor.LastName = doctorRequest.LastName;
            doctor.Email = doctorRequest.Email;

            _context.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteDoctorAsync(int idDoctor)
        {
            var doctor = await _context.Doctors.SingleOrDefaultAsync(d => d.IdDoctor == idDoctor);

            if (doctor is null)
            {
                return false;
            }
            _context.Remove(doctor);

            _context.SaveChanges();

            return true;
        }

        public async Task<GetPrescriptionResponseDTO> GetPrescriptionAsync(int idPrescription)
        {
            var prescriptionResponse = await _context.Prescriptions.Where(p => p.IdPrescription == idPrescription)
                .Include(p => p.Prescription_Medicaments).ThenInclude(pm => pm.IdMedicamentNavigation)
                .Select(prescription => new GetPrescriptionResponseDTO
                {
                    IdPrescription = idPrescription,
                    Date = prescription.Date,
                    DueDate = prescription.DueDate,
                    Patient = new GetPatientResponseDTO
                    {
                        IdPatient = prescription.IdPatientNavigation.IdPatient,
                        FirstName = prescription.IdPatientNavigation.FirstName,
                        LastName = prescription.IdPatientNavigation.LastName,
                        Birthdate = prescription.IdPatientNavigation.Birthdate
                    },
                    Doctor = new GetDoctorResponseDTO
                    {
                        IdDoctor = prescription.IdDoctorNavigation.IdDoctor,
                        FirstName = prescription.IdDoctorNavigation.FirstName,
                        LastName = prescription.IdDoctorNavigation.LastName,
                        Email = prescription.IdDoctorNavigation.Email
                    },
                    Prescriptions_Medicaments = prescription.Prescription_Medicaments.Select(pm => new GetPrescriptions_MedicamentsResponseDTO
                    {
                        Dose = pm.Dose,
                        Details = pm.Details,
                        Medicament = new GetMedicamentsResponse
                        {
                            IdMedicament = pm.IdMedicamentNavigation.IdMedicament,
                            Name = pm.IdMedicamentNavigation.Name,
                            Type = pm.IdMedicamentNavigation.Type,
                            Description = pm.IdMedicamentNavigation.Description
                        }
                    })
                    
                })
                .FirstOrDefaultAsync();

            return prescriptionResponse;
        }
    }
}
