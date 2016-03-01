namespace Airport.ViewModels
{
    public class ViewModelLocator
    {
        public LoginViewModel LoginViewModel => new LoginViewModel();
        public ControlViewModel ControlViewModel => new ControlViewModel();
        public CheckInViewModel CheckInViewModel => new CheckInViewModel();
        public PassengerRegisterViewModel PassengerRegisterViewModel => new PassengerRegisterViewModel();
        public AddFlightViewModel AddFlightViewModel => new AddFlightViewModel();
        public AddRouteViewModel  AddRouteViewModel => new AddRouteViewModel();
        public AddAircraftViewModel AddAircraftViewModel => new AddAircraftViewModel();
        public SignupViewModel SignupViewModel => new SignupViewModel();
        public BoardingPassViewModel BoardingPassViewModel => new BoardingPassViewModel();
        public AboutViewModel AboutViewModel => new AboutViewModel();

        
        
    }
}