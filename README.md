# CryptoCab
[Polygon] CryptoCab
Submission for web3athon

The application is designed to run on a localy hosted block chain using ganache-cli
once installed, run the block chain using ganache-cli using command

> ganache-cli -h "<this-pcs-IP-address>"

The Ip address has to be mentioned as it defaults to local host locking out the mobile app.

Take one of the private keys and update the value in utils.js.
Also update the IP address mentioned in utils.js

Once done execute deploy.js using
> node deploy.js

If executed successfully the Smart Contract tree would be successfully deployed.
The output should've printed a "global address"

Take it's value and update the address in interact.js in the line:
let globalhub = new web3.eth.Contract( JSON.parse(globalhub_abi) , "<global address>" )

then open the project in visual studio and update the file App.xaml.cs lines 9 and 10 with the private key and global address you got:
	public static string userID= "<private key>";
	public static string GlobalHub = "<global-address>";
  
Update the IP address in:
post.xaml.cs -> line 32
viewrequests.xaml.cs -> line 35
localrequests.xaml.cs -> line 132
