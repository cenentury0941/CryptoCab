namespace CryptoCab;

public partial class App : Application
{
	public static string current_location, destination_location;
	public static Location current, destination;
	public static double cost;
	public static string type;
	public static string userID= "0xef0c182c519c817b5ee4ac9f01ab9d70d94a0aa318692ec20593655e75c69916";
	public static string GlobalHub = "0xC00e8e47214324c945Af98EbCf710B20D6f89653";
	public static string[][] requests ;

	public static RequestData RD;
	public App()
	{
		InitializeComponent();
		current = new Location( 1 , 1 );
		destination = new Location( 1 , 1 );
		MainPage = new NavigationPage(new MainPage());
	}
}
