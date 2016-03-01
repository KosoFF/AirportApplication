using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SqlService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service.svc или Service.svc.cs в обозревателе решений и начните отладку.
    public class Service1 : IService
    {
        #region Login
        public bool SignUp(Check_in userInsert)
        {
            using (var authData = new AirportEntities())
            {
                try
                {
                    var newUser = userInsert;
                    if (authData.Check_in.Any(c => c.Login == newUser.Login /* || c.UserName == newUser.UserName*/))
                    {
                        return false;
                    }


                    newUser.ManagerID = Guid.NewGuid().ToString("D");
                    //TODO: Make guid check?..
                    authData.Check_in.Add(newUser);
                    //authData.SaveChanges();
                    authData.SaveChanges();
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }


        public AccountInformation Login(string login, string passwordHash)
        {
            try
            {
                using (var airportEntities = new AirportEntities())
                {
                    var manager = airportEntities.Check_in.SingleOrDefault(a => a.Login == login);
                    if (manager != null)
                    {
                        if (manager.PasswordHash == passwordHash)
                            return new AccountInformation()
                            {
                                Login = manager.Login,
                                Id = manager.ManagerID,
                                FirstName = manager.FirstName,
                                LastName = manager.LastName,
                                PasswordHash = manager.PasswordHash

                            };
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Service.Login: " + ex.Message);
                return new AccountInformation() { Login = "Connection error" };
            }
        }
        #endregion  //Login

        #region Route
        public bool AddCity(string name)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    if (!context.City.Any(c => c.CityName == name))
                    {
                        context.City.Add(new City()
                        {
                            CityID = Guid.NewGuid().ToString("D"),
                            CityName = name
                        });
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to add city: " + ex.Message);
                return false;
            }
        }
        public Dictionary<string, string> GetCitiesDictionary()
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    if (context.City.Any())
                        return context.City.ToDictionary(c => c.CityID, c => c.CityName);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to get cities: " + ex.Message);
                return new Dictionary<string, string>() { { "Error", "Connection error" } };
            }
        }
        public bool AddRoute(string departure, string arrival)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    Debug.Assert(context.City.Any(c => c.CityID == arrival));
                    Debug.Assert(context.City.Any(c => c.CityID == departure));
                    if (context.AircraftRoute.Any(c => c.City.CityID == departure && c.City1.CityID == arrival))
                    {
                        return false;
                    }
                    context.AircraftRoute.Add(new AircraftRoute()
                    {
                        RouteID = Guid.NewGuid().ToString("D"),
                        DeparturePointID = departure,
                        ArrivalPointID = arrival
                    });
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to add route: " + ex.Message);
                return false;
            }
        }
        #endregion //route

        #region Aircraft
        public bool AddAircraft(string name, DateTime builtDate, int seatsNum)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    //if (context.Aircraft.Any(c => c.AircraftName==name))
                    //{
                    //    return false;
                    //}
                    context.Aircraft.Add(new Aircraft()
                    {
                        AircraftID = Guid.NewGuid().ToString("D"),
                        AircraftName = name,
                        SeatsNum = seatsNum,
                        YearBuilt = builtDate
                    });
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to add aircraft: " + ex.Message);
                return false;
            }
        }
        #endregion //aircraft

        #region Flight
        public Dictionary<string, string> GetAircraftDictionary()
        {
            try
            {

                using (var context = new AirportEntities())
                {
                    if (context.Aircraft.Any())
                        return context.Aircraft.ToDictionary(a => a.AircraftID, a => a.AircraftName);
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to get aircraft dictionary: " + ex.Message);
                return null;
            }
        }
        public Dictionary<string, string> GetRouteDictionary(string departure, string arrival)
        {
            try
            {

                using (var context = new AirportEntities())
                {
                    if (context.AircraftRoute.Any())
                        if(departure != null||arrival != null)
                        return context.AircraftRoute
                            .Where(a=>a.DeparturePointID==(departure ?? a.DeparturePointID)&&a.ArrivalPointID==(arrival ?? a.ArrivalPointID))
                            .ToDictionary(a => a.RouteID, a => 
                            context.City.Single(c => c.CityID == a.DeparturePointID).CityName +
                            "  -  " +
                            context.City.Single(c => c.CityID == a.ArrivalPointID).CityName);
                        else
                        {
                            return context.AircraftRoute
                                .ToDictionary(a => a.RouteID, a =>
                            context.City.Single(c => c.CityID == a.DeparturePointID).CityName +
                            "  -  " +
                            context.City.Single(c => c.CityID == a.ArrivalPointID).CityName);
                        }

                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to get aircraft dictionary: " + ex.Message);
                return null;
            }

        }

        public bool AddFlight(string routeId, string aircraftId)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    if (context.Flight.Any(f => f.AircraftID == aircraftId && f.RouteID == routeId))
                        return false;
                    Debug.Assert(context.AircraftRoute.Any(r=>r.RouteID==routeId));
                    Debug.Assert(context.Aircraft.Any(r => r.AircraftID == aircraftId));
                    context.Flight.Add(new Flight()
                    {
                       RouteID = routeId,
                       AircraftID = aircraftId,
                       FlightID = Guid.NewGuid().ToString("D")
                    });
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        #endregion //Flight

        #region Passenger

        public bool AddPassenger(string firstName, string lastName, long passportNum, bool gender, DateTime birthDate)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    if (context.Passenger.Any(p => p.PassportNum == passportNum))
                    {
                        return false;
                    }
                    else
                    {
                        context.Passenger.Add(new Passenger()
                        {
                            BirthDate = birthDate,
                            FirstName = firstName,
                            LastName = lastName,
                            Gender = gender,
                            PassportNum = passportNum,
                            PassengerID = Guid.NewGuid().ToString("D")
                        });
                        context.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Something probably went wrong: " + ex.Message);
                return false;
            }
        }

        public Dictionary<string, string> GetPassengerDictionary()
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    if (context.Passenger.Any())
                        return context.Passenger.ToDictionary(c => c.PassengerID,
                            c => c.FirstName + " " + c.LastName + " <" + c.PassportNum + ">");
                     return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to get passenger dictionary: " + ex.Message);
                return null;
            }
        }
        public bool RemovePassenger(string passengerId)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    if (context.Passenger.Any(c => c.PassengerID == passengerId))
                    {
                        context.Passenger.Remove(context.Passenger.Single(c => c.PassengerID == passengerId));
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to remove passenger: " + ex.Message);
                return false;
            }
        }
        #endregion //Passenger
        #region BoardingPass
        public string AddBoardingPass(string flightId, string passengerId, string managerId, DateTime flightDateTime)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    var bp = new BoardingPass()
                    {
                        BoardingPassID = Guid.NewGuid().ToString("D"),
                        CheckInManagerID = managerId,
                        FlightID = flightId,
                        PassengerID = passengerId,
                        FlightTime = flightDateTime

                    };
                    context.BoardingPass.Add(bp);
                    context.SaveChanges();
                    return bp.BoardingPassID;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to add luggage: " + ex.Message);
                return null;
            }
        }

        public string AddLuggage(string boardingPassId, float weight)
        {
            try
            {
                using (var context = new AirportEntities())
                {
                    var lug = new Luggage()
                    {
                        BoardingPassID = boardingPassId,
                        Weight = weight,
                        LuggageID = Guid.NewGuid().ToString("D")
                    };
                    context.Luggage.Add(lug);
                    context.SaveChanges();
                    return lug.LuggageID;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to add luggage: " + ex.Message);
                return null;
            }
        }

        public Dictionary<string, string> GetFlightDictionary(string departure, string arrival)
        {
            try
            {

                using (var context = new AirportEntities())
                {
                    if (context.Flight.Any())
                    {
                        if (departure != null || arrival != null)
                            return context.Flight
                                .Where(
                                    a =>
                                        a.AircraftRoute.DeparturePointID ==
                                        (departure ?? a.AircraftRoute.DeparturePointID)
                                        && a.AircraftRoute.ArrivalPointID == (arrival ?? a.AircraftRoute.ArrivalPointID))
                                .ToDictionary(a => a.FlightID, a =>
                                    "<" +
                                    context.City.Single(c => c.CityID == a.AircraftRoute.DeparturePointID).CityName +
                                    " - " +
                                    context.City.Single(c => c.CityID == a.AircraftRoute.ArrivalPointID).CityName +
                                    ">  " +
                                    a.Aircraft.AircraftName);

                        else
                        {
                            return context.Flight
                                .ToDictionary(a => a.FlightID, a =>
                                    "<" +
                                    context.City.Single(c => c.CityID == a.AircraftRoute.DeparturePointID).CityName +
                                    " - " +
                                    context.City.Single(c => c.CityID == a.AircraftRoute.ArrivalPointID).CityName +
                                    ">  " +
                                    a.Aircraft.AircraftName);
                        }
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.Print("Failed to get flight dictionary: " + ex.Message);
                return null;
            }
        }
        #endregion //Boarding pass
    }
}