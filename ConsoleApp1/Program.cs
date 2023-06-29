using Nethereum.Web3;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;
using Nethereum.Util;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Contracts;
using Nethereum.Contracts.Extensions;
using System.Numerics;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

var url = "https://polygon-mumbai.g.alchemy.com/v2/demo";
var web3 = new Web3(url);
var erc20TokenContractAddress = "0x97A7c8bDE3ABe65D919E1eD9d8918f46f311De94";
string abi = File.ReadAllText("../../../abi.json");
var contract = web3.Eth.GetContract(abi, erc20TokenContractAddress);
var transferEventHandler = web3.Eth.GetEvent<TransferEventDTO>(erc20TokenContractAddress);
var filter = transferEventHandler.CreateFilterInput(
    fromBlock: new BlockParameter(37307896),
    toBlock: new BlockParameter(37308442));
var logs = await transferEventHandler.GetAllChangesAsync(filter);
var getSymbolFunction = contract.GetFunction("symbol");
var result = await getSymbolFunction.CallAsync<string>();
Console.WriteLine($"Contract symbol: {result}");
Console.WriteLine($"Token Transfer Events for ERC20 Token at Contract Address: {erc20TokenContractAddress}");
Console.WriteLine($"Total of logs: {logs.Count()}");
foreach (var logItem in logs)
    Console.WriteLine(
        $"tx:{logItem.Log.TransactionHash} from:{logItem.Event.From} to:{logItem.Event.To} value:{logItem.Event.Value}");