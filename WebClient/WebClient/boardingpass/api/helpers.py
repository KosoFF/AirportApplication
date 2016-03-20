from WebClient.boardingpass.models import BoardingPass 

def BoardingPassConvert(passenger): 

    bp=BoardingPass(); 
    bp.boardingpass_id=passenger['BoardingPassId']; 
    bp.passenger_id=passenger['PassengerId']; 
    bp.checkin_manager_id=passenger['CheckInManagerId']; 
    bp.flight_id=passenger['FlightId']; 
    bp.flight_time=passenger['FlightTime']; 
    return bp; 
def _url(path): 
    return 'http://localhost:54822/Service.svc/rest/' + path