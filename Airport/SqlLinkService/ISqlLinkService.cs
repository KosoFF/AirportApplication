using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace Airport.SqlLinkService
{
    public interface ISqlLinkService
    {
        Task<Dictionary<string, string>> GetCityDictionaryAsync();
        Task<bool> AddCityAsync(string name);
        Task<bool> AddRouteAsync(string departure, string arrival);
        Task<bool> AddAircraftAsync(string name, DateTime builtDate, int seatsNum);
        Task<Dictionary<string, string>> GetRouteDictionaryAsync(string departure = null, string arrival = null);
        Task<Dictionary<string, string>> GetAircraftDictionaryAsync();
        Task<bool> AddFlightAsync(string routeId, string aircraftId);
        Task<bool> AddPassengerAsync(string firstName, string lastName, long passportNum, bool gender,
            DateTime birthDate);
        Task<bool> RemovePassengerAsync(string passengerId);
        Task<Dictionary<string, string>> GetPassengerDictionaryAsync();
        Task<string> AddLuggage(string boardingPassId, float weight);

        Task<string> AddBoardingPass(string flightId, string passengerId, string managerId,
            DateTime flightDateTime);

        Task<Dictionary<string, string>> GetFlightDictionaryAsync(string departure=null, string arrival=null);
    }
}
