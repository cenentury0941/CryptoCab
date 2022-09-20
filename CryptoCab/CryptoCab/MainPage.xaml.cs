using Nethereum.Web3;
using System;
using Nethereum.Web3.Accounts;
using System.Threading.Tasks;

namespace CryptoCab;

public partial class MainPage : ContentPage
{
	int count = 0;
	Button C;
	public MainPage()
	{
		InitializeComponent();
        
    }

    protected override void OnAppearing()
    {
		CurrentLocation.Text = App.current_location;
    }

    async void OnOpenMap(object sender, EventArgs args)
    {
		await Navigation.PushAsync( new MapPage() );
    }

    async void OnViewRequests(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new ViewRequests());
    }

    async void OnLocalRequests(object sender, EventArgs args)
    {
        await Navigation.PushAsync(new LocalRequests());
    }

    private async void OnCounterClicked(object sender, EventArgs e)
	{
		//CounterBtn.Text = (await GetBlockNumber());
	}

	async Task<string> GetBlockNumber()
	{
        var privateKey = "0x78fe9e50268c3d2ee3ced03900046755e74486b9bd6162ccb11a6f0fb062421f";
        var account = new Account(privateKey);

        var web3 = new Web3("http://192.168.43.61:8545"); //https://polygon-rpc.com
        var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        string abi = "[{\"inputs\":[],\"name\":\"get\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"y\",\"type\":\"uint256\"}],\"name\":\"set\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        var contract = web3.Eth.GetContract(abi, "0x6D0Eb582E295c8D8Cff917D7cA008Fae9Be40222");
        var data = await contract.GetFunction("get").CallAsync<int>();

        return $"Latest Value is: {data}";

	}

}

