namespace CryptoCab;

public partial class CabSelect : ContentPage
{
	public CabSelect()
	{
		InitializeComponent();
        Destination.Text = $" ------Target Location------  Lat:{App.destination.Latitude.ToString().Substring(0, 6)} Long:{App.destination.Longitude.ToString().Substring(0, 6)}";
        Current.Text = $"------Current Location------  Lat:{App.current.Latitude.ToString().Substring(0, 6)} Long:{App.current.Longitude.ToString().Substring(0, 6)}";
        CabType.Text = "Hatchback";
        Seats.Text = "4 Seats";
        Desc.Text = "A two or four-door vehicle with a tailgate that would flip upwards called a hatch.";
        double cost = Location.CalculateDistance(App.current.Latitude, App.current.Longitude, App.destination, DistanceUnits.Kilometers);
        RideCost.Text = $"{cost:00.0000} ETH";
        App.type = "Hatchback";
    }

    public void TypeSelectHandler(object sender, EventArgs args)
    {
        Hatchback.IsVisible = false;
        Sedan.IsVisible = false;
        SUV.IsVisible = false;
        if ( sender == HatchbackButton )
        {
            Hatchback.IsVisible = true;
            CabType.Text = "Hatchback";
            Seats.Text = "4 Seats";
            Desc.Text = "A two or four-door vehicle with a tailgate that would flip upwards called a hatch.";
            double cost = Location.CalculateDistance( App.current.Latitude , App.current.Longitude , App.destination , DistanceUnits.Kilometers );
            RideCost.Text = $"{cost:00.0000} ETH";
            App.type = "Hatchback";
        }
        if ( sender == SedanButton )
        {
            Sedan.IsVisible = true;
            CabType.Text = "Sedan";
            Seats.Text = "5 Seats";
            Desc.Text = "A 4-door passenger car with a trunk that is separate from the passengers.";
            double cost = Location.CalculateDistance(App.current.Latitude, App.current.Longitude, App.destination, DistanceUnits.Kilometers)*1.39;
            RideCost.Text = $"{cost:00.0000} ETH";
            App.type = "Sedan";
        }
        if ( sender == SUVButton )
        {
            SUV.IsVisible = true;
            CabType.Text = "SUV";
            Seats.Text = "7 Seats";
            Desc.Text = "Sits high off the ground and which often has four-wheel drive and rugged styling.";
            double cost = Location.CalculateDistance(App.current.Latitude, App.current.Longitude, App.destination, DistanceUnits.Kilometers)*1.75;
            RideCost.Text = $"{cost:00.0000} ETH";
            App.type = "SUV";
        }
    }


    public void PostRequestHandler(object sender, EventArgs args)
    {
        Navigation.PushAsync( new Post() );
    }
}