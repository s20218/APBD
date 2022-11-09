using Assignment7.Models;
using Assignment7.Models.DTO;
using Assignment7.Models.DTO.Responses;
using Assignment7.Models.DTOs.Requests;
using Assignment7.Models.DTOs.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment7.Service
{    

    public interface IDatabaseService
    {
      
        Task<IEnumerable<GetTripsResponseDTO>> GetTripsAsync();
        Task<bool> DeleteClientAsync(int idClient);
        Task<bool> AddClientToTourAsync(AddClientsRequestDTO request, int idTrip);
    }
    public class DatabaseService : IDatabaseService
    {
        private readonly TripsDbContext _context;

        public DatabaseService(TripsDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetTripsResponseDTO>> GetTripsAsync()
        {
            var trips = await _context.Trips
                                      .Select(t => new GetTripsResponseDTO
                                      {
                                          Name = t.Name,
                                          Description = t.Description,
                                          DateFrom = t.DateFrom,
                                          DateTo = t.DateTo,
                                          MaxPeople = t.MaxPeople,
                                          Countries = _context.Countries
                                                                        .Where(c => t.CountryTrips.Select(ct => ct.IdCountry).Contains(c.IdCountry))
                                                                        .Select(c => new GetCountriesResponseDTO
                                                                        {
                                                                            Name = c.Name
                                                                        }).ToList(),
                                          Clients = _context.Clients
                                                                    .Where(c => t.ClientTrips.Select(ct => ct.IdClient).Contains(c.IdClient))
                                                                    .Select(c => new GetClientsResponseDTO
                                                                    {
                                                                            FirstName = c.FirstName,
                                                                            LastName = c.LastName
                                                                    }).ToList()
                                      }).OrderBy(t => t.DateFrom).ToListAsync();
            return trips;
        }

        public async Task<bool> DeleteClientAsync(int idClient)
        {
            bool hasTours;
            hasTours = await ClientHasTours(idClient);

            if (hasTours)
            {
                return false;
            }

            var client = _context.Clients.First(c => c.IdClient == idClient);

            _context.Clients.Remove(client);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> ClientHasTours(int idClient)
        {
            return await _context.ClientTrips.Select(ct => ct.IdClient)
                                             .ContainsAsync(idClient);
        }

        public async Task<bool> AddClientToTourAsync(AddClientsRequestDTO request, int idTrip)
        {
            bool peselExists, clientAssigned, tripExists;
            int idClient;

            idClient = GetClientId(request);

            peselExists = await IfPeselExists(request);

            tripExists = await IfTripExists(request);

            clientAssigned = await IfClientAssigned(idClient);

            if(idTrip != request.IdTrip)
            {
                return false;
            }

            if (!tripExists || clientAssigned)
            {
                return false;
            }

            if (!peselExists)
            {
                idClient = AddClientIfNoPesel(idClient, request);
            }

            var client_trip = new ClientTrip
            {
                IdClient = idClient,
                IdTrip = request.IdTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate
            };

            _context.ClientTrips.Add(client_trip);
            _context.SaveChanges();
            
            
            return true;
        }
        
        public int GetClientId(AddClientsRequestDTO request)
        {
            return _context.Clients.Where(c => c.Pesel == request.Pesel)
                                   .Select(c => c.IdClient).FirstOrDefault();
        }

        public async Task<bool> IfPeselExists(AddClientsRequestDTO request)
        {
            return await _context.Clients.Select(c => c.Pesel)
                                         .ContainsAsync(request.Pesel);
        }

        public async Task<bool> IfTripExists(AddClientsRequestDTO request)
        {
            return await _context.Trips.Select(t => t.IdTrip)
                                 .ContainsAsync(request.IdTrip);
        }

        public async Task<bool> IfClientAssigned(int idClient)
        {
            return await _context.ClientTrips.Select(ct => ct.IdClient)
                                             .ContainsAsync(idClient);
        }

        public int AddClientIfNoPesel(int idClient, AddClientsRequestDTO request)
        {
            var client = new Client
            {
                IdClient = _context.Clients.Max(c => c.IdClient) + 1,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Telephone = request.Telephone,
                Pesel = request.Pesel
            };
            _context.Clients.Add(client);

            return client.IdClient;
        }
    }
}
