using Microsoft.Maui.Controls;
using Microsoft.Maui.Dispatching;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace CryptoCab;

public partial class Post : ContentPage
{
	public Post()
    {
        InitializeComponent();
        LoadingAnim.IsAnimationPlaying = false;
        LoadingAnim.IsAnimationPlaying = true;

        PostRequest();

	}

	async public void popToRoot()
	{
        await Navigation.PopToRootAsync();
    }

    async void PostRequest()
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

        var localhub_addr = await globalhub.GetFunction("getLocalHubAddr").CallAsync<string>( lat , lon );
        var localhub = web3.Eth.GetContract(localhub_abi, localhub_addr);

        Location cur, tar;
        cur = App.current;
        tar = App.destination;

        string req = account.Address+"|"+cur.Latitude+"|"+cur.Longitude+"|"+tar.Latitude+"|"+tar.Longitude+"|"+App.type+"|None";

        await localhub.GetFunction("addRequest").SendTransactionAsync( account.Address , new HexBigInteger(700000), new HexBigInteger(0) , req );

        popToRoot();

    }

}