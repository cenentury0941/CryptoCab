using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Org.BouncyCastle.Ocsp;


namespace CryptoCab;

public partial class ViewRequests : ContentPage
{
    public List<RequestData> Data;

	public ViewRequests()
	{
		InitializeComponent();
        Data = new List<RequestData>();
        GetRequests();


	}

    async void ViewDetailsHandler(object sender, EventArgs args)
    {
        App.RD = Data[ Data.Count -1 -Int32.Parse(((Button)sender).Text) ];
        await Navigation.PushAsync(new Requests());
    }


    async void GetRequests()
    {
        var privateKey = App.userID;

        var account = new Account(privateKey);

        var web3 = new Web3("http://192.168.43.61:8545"); //https://polygon-rpc.com

        string globalhub_abi = "[{\"inputs\":[{\"internalType\":\"int256\",\"name\":\"Lat\",\"type\":\"int256\"},{\"internalType\":\"int256\",\"name\":\"Long\",\"type\":\"int256\"}],\"name\":\"getLocalHubAddr\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"int256\",\"name\":\"Lat\",\"type\":\"int256\"},{\"internalType\":\"int256\",\"name\":\"Long\",\"type\":\"int256\"},{\"internalType\":\"string\",\"name\":\"Addr\",\"type\":\"string\"}],\"name\":\"setLocalHubAddr\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        string localhub_abi = "[{\"inputs\":[{\"internalType\":\"string\",\"name\":\"Request\",\"type\":\"string\"}],\"name\":\"addRequest\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"getRequests\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"}]";
        string global_addr = App.GlobalHub;

        var globalhub = web3.Eth.GetContract(globalhub_abi, global_addr);

        int lat = App.current.Latitude > 0 ? 1 : -1;
        int lon = App.current.Longitude > 0 ? 1 : -1;

        var localhub_addr = await globalhub.GetFunction("getLocalHubAddr").CallAsync<string>(lat, lon);
        var localhub = web3.Eth.GetContract(localhub_abi, localhub_addr);

        Location cur, tar;
        cur = App.current;
        tar = App.destination;

        //string req = account.Address + "|" + cur.Latitude + "|" + cur.Longitude + "|" + tar.Latitude + "|" + tar.Longitude + "|" + App.type;

        //await localhub.GetFunction("addRequest").SendTransactionAsync(account.Address, new HexBigInteger(700000), new HexBigInteger(0), req);

        string data = ( await localhub.GetFunction("getRequests").CallAsync<string>());//.SendTransactionAsync(account.Address, new HexBigInteger(700000), new HexBigInteger(0));
        Console.WriteLine(data);
        

        string[] data_list = data.Split( "," );
        int index = 0;
         foreach ( string s in data_list )
         {
            if ( s.Length < 2 )
            {
                continue;
            }
                string[] quantum = s.Split("|");
                RequestData rd = new RequestData( quantum[0] , quantum[1] , quantum[2] , quantum[3] , quantum[4] , quantum[5] , quantum[6] , index++);
                Data.Add(rd);
            }
            Data.Reverse();
            RequestList.ItemsSource = Data;

    }


}