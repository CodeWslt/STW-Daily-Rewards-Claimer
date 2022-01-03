using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.IO;
using System.Threading;

namespace STW_Daily_Claimer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "STW Daily Rewards Claimer Created By Wslt#7904";
            if (!File.Exists("Accounts.json")) { File.WriteAllText("Accounts.json", "[]"); }
        Start:
            WriteAscii();
            Console.WriteLine("[1] Claim Daily Rewards\n[2] Add Account\n[3] Exit");
            switch (Console.ReadLine())
            {
                case "1":
                    {
                        WriteAscii();
                        int AccountCount = 1;
                        foreach (var Account in JArray.Parse(File.ReadAllText("Accounts.json")))
                        {
                            var client = new RestClient(string.Format("https://fortnite-public-service-prod11.ol.epicgames.com/fortnite/api/game/v2/profile/{0}/client/ClaimLoginReward?profileId=campaign", Account["AccountID"]));
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddHeader("Authorization", "Bearer " + Account["AccountToken"]);
                            request.AddJsonBody("{}");
                            string ReturnContent = client.Execute(request).Content;
                            if (ReturnContent.Contains("error"))
                            {
                                WriteAscii();
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine(JObject.Parse(ReturnContent)["errorMessage"]);
                                Thread.Sleep(4000);
                                goto Start;
                            }
                            else
                            {
                                Console.WriteLine(string.Format("Successfully Claimed STW Daily Rewards For Account {0}", AccountCount));
                            }
                        }
                        Thread.Sleep(2000);
                    }
                    goto Start;
                case "2":
                    {
                        WriteAscii();
                        Console.WriteLine("Sign In To Epic Games:\nVist This Link https://www.galaxyswapperv2.com/Redirects/EpicAuth.html\nThen Enter The Authorization Code Below:");
                        string AuthID = Console.ReadLine();
                        var client = new RestClient("https://account-public-service-prod.ol.epicgames.com/account/api/oauth/token");
                        var request = new RestRequest(Method.POST);
                        request.AddHeader("Authorization", "Basic MzQ0NmNkNzI2OTRjNGE0NDg1ZDgxYjc3YWRiYjIxNDE6OTIwOWQ0YTVlMjVhNDU3ZmI5YjA3NDg5ZDMxM2I0MWE=");
                        request.AddParameter("grant_type", (object)"authorization_code");
                        request.AddParameter("code", AuthID);
                        string ReturnContent = client.Execute(request).Content;
                        if (ReturnContent.Contains("error"))
                        {
                            WriteAscii();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(JObject.Parse(ReturnContent)["errorMessage"]);
                            Thread.Sleep(4000);
                            goto Start;
                        }
                        else
                        {
                            JObject AccountJson = JObject.FromObject(new
                            {
                                AccountID = JObject.Parse(ReturnContent)["account_id"],
                                AccountToken = JObject.Parse(ReturnContent)["access_token"]
                            });
                            JArray AccountFile = JArray.Parse(File.ReadAllText("Accounts.json"));
                            AccountFile.Add(AccountJson);
                            File.WriteAllText("Accounts.json", AccountFile.ToString());
                            WriteAscii();
                            Console.WriteLine("Successfully Added Account");
                            Thread.Sleep(2000);
                        }
                    }
                    goto Start;
                case "3":
                    return;
                default:
                    goto Start;
            }
        }
        static void WriteAscii()
        {
            string AsciiArt = " _____ _____ _    _  ______      _ _         _____ _       _                     \n/  ___|_   _| |  | | |  _  \\    (_) |       /  __ \\ |     (_)                    \n\\ `--.  | | | |  | | | | | |__ _ _| |_   _  | /  \\/ | __ _ _ _ __ ___   ___ _ __ \n `--. \\ | | | |/\\| | | | | / _` | | | | | | | |   | |/ _` | | '_ ` _ \\ / _ \\ '__|\n/\\__/ / | | \\  /\\  / | |/ / (_| | | | |_| | | \\__/\\ | (_| | | | | | | |  __/ |   \n\\____/  \\_/  \\/  \\/  |___/ \\__,_|_|_|\\__, |  \\____/_|\\__,_|_|_| |_| |_|\\___|_|   \n                                      __/ |                                      \n                                     |___/                                       \n=================================================================================================================";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(AsciiArt);
        }
    }
}
