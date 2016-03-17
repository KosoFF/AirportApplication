
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SqlService
{
    [ServiceContract]
    public interface IWebService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "BoardingPass")]
        List<BoardingPassInformation> GetBoardingPass();

        [OperationContract]
        [WebInvoke(Method = "GET",
        ResponseFormat = WebMessageFormat.Json,
        RequestFormat = WebMessageFormat.Json,
        BodyStyle = WebMessageBodyStyle.Wrapped,
        UriTemplate = "boardingpass/{passId}")]
        BoardingPassFullInformation GetBoardingPassById(string passId);
    }

    [DataContract]
    public class BoardingPassInformation
    {

        [DataMember]
        public string BoardingPassId { get; set; }
        [DataMember]
        public string FlightId { get; set; }
        [DataMember]
        public string PassengerId { get; set; }
        [DataMember]
        public string CheckInManagerId { get; set; }
        [DataMember]
        public string FlightTime { get; set; }
    }

    public class BoardingPassFullInformation
    {
        [DataMember]
        public string BoardingPassId { get; set; }

        [DataMember]
        public PassengerInformation PassengerInformation { get; set; }

        [DataMember]
        public FlightInformation FlightInformation { get; set; }

        [DataMember]
        public ManagerInformation ManagerInformation { get; set; }

        [DataMember]
        public string FlightTime { get; set; }
    }

    [DataContract]
    public class PassengerInformation //For IAcoountService needs 
    {

        [DataMember]
        public string PassengerId { get; set; }
        [DataMember]
        public long PassportNum { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public bool Gender { get; set; }
        [DataMember]
        public string BirthDate { get; set; }
    }
    [DataContract]
    public class FlightInformation //For IAcoountService needs 
    {

        [DataMember]
        public string FlightId { get; set; }
        [DataMember]
        public RouteInformation RouteInformation { get; set; }
        [DataMember]
        public AircraftInformation AircraftInformation { get; set; }
    }
    [DataContract]
    public class RouteInformation //For IAcoountService needs 
    {
        [DataMember]
        public string RouteId { get; set; }
        [DataMember]
        public CityInformation DepartureCity { get; set; }
        [DataMember]
        public CityInformation ArrivalCity { get; set; }
    }
    [DataContract]
    public class AircraftInformation //For IAcoountService needs 
    {

        [DataMember]
        public string AircraftId { get; set; }
        [DataMember]
        public string AircraftName { get; set; }
        [DataMember]
        public int SeatsNum { get; set; }
        [DataMember]
        public string YearBuilt { get; set; }
    }
    [DataContract]
    public class CityInformation
    {
        [DataMember]
        public string CityId { get; set; }
        [DataMember]
        public string CityName { get; set; }
    }
    [DataContract]
    public class ManagerInformation //For IAcoountService needs 
    {

        [DataMember]
        public string ManagerId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }

    }

}