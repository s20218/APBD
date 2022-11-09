using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.DTOs;
using Test.Models;

namespace Test.Service
{
    public interface IDatabaseService
    {
        Task<IEnumerable<GetFlightsDTO>> GetFlights(int idPassenger);
        void EnrollPassenger(int idPassenger, int idFlight);
        Task<bool> FlightExists(int idFlight);
        Task<bool> PassengerExists(int idPassenger);
        Task<bool> PassengerAssigned(int idFlight, int idPassenger);
    }

    public class FlightsService : IDatabaseService
    {
        private readonly FlightDbContext _context;

        public FlightsService(FlightDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetFlightsDTO>> GetFlights(int idPassenger)
        {
            var ifExists = await _context.Passengers.AnyAsync(c => c.IdPassenger == idPassenger);

            if (ifExists is false)
            {
                return null;
            }

            var ifEmpty = await _context.Flight_Passengers.AnyAsync(ct => ct.IdPassenger == idPassenger);


            if (ifEmpty is false)
            {
                return Enumerable.Empty<GetFlightsDTO>();
            }

            var flights = await _context.Passengers.Where(p => p.IdPassenger == idPassenger)
                .Include(e => e.Flight_Passengers).ThenInclude(e => e.IdFlightNavigation)
                .Select(c => new GetFlightsDTO
                {
                    NameOfCity = _context.Flight_Passengers.Select(fp => fp.IdFlightNavigation.IdCityDictNavigation.City).FirstOrDefault(),
                    Plane = new PlaneDTO
                    {
                        IdPlane = _context.Flight_Passengers.Select(fp => fp.IdFlightNavigation.IdPlaneNavigation.IdPlane).FirstOrDefault(),
                        Name = _context.Flight_Passengers.Select(fp => fp.IdFlightNavigation.IdPlaneNavigation.Name).FirstOrDefault()
                    }
                }).ToListAsync();


            return flights;
        }
        public void EnrollPassenger(int idPassenger, int idFlight)
        {
            var newFlight_Passenger = new Flight_Passenger
            {
                IdFlight = idFlight,
                IdPassenger = idPassenger
            };

            _context.Flight_Passengers.Add(newFlight_Passenger);
            _context.SaveChanges();
        }

        public async Task<bool> FlightExists(int idFlight)
        {
            var result = await _context.Flights.AnyAsync(f => f.IdFlight == idFlight);

            return result;
        }

        public async Task<bool> PassengerExists(int idPassenger)
        {
            var result = await _context.Passengers.AnyAsync(f => f.IdPassenger == idPassenger);

            return result;
        }

        public async Task<bool> PassengerAssigned(int idFlight, int idPassenger)
        {
            var result = await _context.Flight_Passengers.AnyAsync(f => f.IdFlight == idFlight && f.IdPassenger == idPassenger);

            return !result;
        }

        //public async Task<bool> FlightHasNotTakenPlace(int idFlight)
        //{
        //    var result = await _context.Flights.AnyAsync(f => f.IdFlight == idFlight);

        //    return result;
        //}

        //public async Task<bool> NumOfSeatsNotExceeded(int idFlight)
        //{
        //    var numOfSeats = await _context.Flights.Where(f => f.IdFlight == idFlight);

        //    return result;
        //}
    }
}
