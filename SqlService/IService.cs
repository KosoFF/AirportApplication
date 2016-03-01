using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace SqlService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IService1" в коде и файле конфигурации.
    /* [ServiceContract]
     public interface IService1
     {

         [OperationContract]
         string GetData(int value);

         [OperationContract]
         CompositeType GetDataUsingDataContract(CompositeType composite);

         // TODO: Добавьте здесь операции служб
     }*/

    [ServiceContract]
    public interface IService
    {

        #region Login
        [OperationContract]
        bool SignUp(Check_in manager);

        [OperationContract]
        AccountInformation Login(string login, string passwordHash);
        #endregion //login

        #region Route
        [OperationContract]
        bool AddCity(string name);
        [OperationContract]
        bool AddRoute(string departure, string arrival);
        [OperationContract]
        Dictionary<string, string> GetCitiesDictionary();
        #endregion //route

        #region Aircraft
        [OperationContract]
        bool AddAircraft(string name, DateTime builtDate, int seatsNum);
        #endregion //Aircraft

        #region Flight
        [OperationContract]
        Dictionary<string, string> GetAircraftDictionary();
        [OperationContract]
        Dictionary<string, string> GetRouteDictionary(string departure, string arrival);
        [OperationContract]
        bool AddFlight(string routeId, string aircraftId);
        #endregion //Flight

        #region Passenger
        [OperationContract]
        bool AddPassenger(string firstName, string lastName, long passportNum, bool gender, DateTime birthDate);
        [OperationContract]
        bool RemovePassenger(string passengerId);
        [OperationContract]
        Dictionary<string, string> GetPassengerDictionary();

        #endregion

        #region Checkin
        [OperationContract]
        string AddBoardingPass(string flightId, string passengerId, string managerId, DateTime flightDateTime);

        [OperationContract]
        string AddLuggage(string passengerId, float weight);

        [OperationContract]
        Dictionary<string, string> GetFlightDictionary(string departure, string arrival);

        #endregion

    }


    [DataContract]
    public class Credentials //removable
    {
        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }
       
    }

    [DataContract]
    public class AccountInformation //For IAcoountService needs
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Login { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
    }

}