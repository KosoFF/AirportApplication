using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Airport.SqlReference;

namespace Airport.SqlLinkService
{
    public class SqlLinkService : ISqlLinkService
    {
        readonly ServiceClient _sqlService = new ServiceClient();
        public async Task<Dictionary<string, string>> GetCityDictionaryAsync()
        {
            return await _sqlService.GetCitiesDictionaryAsync();
        }

        public async Task<bool> AddCityAsync(string name)
        {
            return await _sqlService.AddCityAsync(name);
        }

        public async Task<bool> AddRouteAsync(string departure, string arrival)
        {
            return await _sqlService.AddRouteAsync(departure, arrival);
        }

        public async Task<bool> AddAircraftAsync(string name, DateTime builtDate, int seatsNum)
        {
            return await _sqlService.AddAircraftAsync(name, builtDate, seatsNum);
        }

        public async Task<Dictionary<string, string>> GetAircraftDictionaryAsync()
        {
            return await _sqlService.GetAircraftDictionaryAsync();
        }

        public async Task<Dictionary<string, string>> GetRouteDictionaryAsync(string departure=null, string arrival=null)
        {
            return await _sqlService.GetRouteDictionaryAsync(departure, arrival);
        }

        public async Task<bool> AddFlightAsync(string routeId, string aircraftId)
        {
            return await _sqlService.AddFlightAsync(routeId, aircraftId);
        }

        public async Task<bool> AddPassengerAsync(string firstName, string lastName, long passportNum, bool gender,
            DateTime birthDate)
        {
            return await _sqlService.AddPassengerAsync(firstName, lastName, passportNum, gender, birthDate);
        }

        public async Task<Dictionary<string, string>> GetPassengerDictionaryAsync()
        {
            return await _sqlService.GetPassengerDictionaryAsync();
        }

        public async Task<bool> RemovePassengerAsync(string passengerId)
        {
            return await _sqlService.RemovePassengerAsync(passengerId);
        }

        public async Task<string> AddBoardingPass(string flightId, string passengerId, string managerId,
            DateTime flightDateTime)
        {
            return await _sqlService.AddBoardingPassAsync(flightId, passengerId, managerId, flightDateTime);
        }

        public async Task<string> AddLuggage(string boardingPassId, float weight)
        {
            return await _sqlService.AddLuggageAsync(boardingPassId, weight);
        }

        public async Task<Dictionary<string, string>> GetFlightDictionaryAsync(string departure = null, string arrival = null)
        {
            return await _sqlService.GetFlightDictionaryAsync(departure, arrival);
        }

    }
}
