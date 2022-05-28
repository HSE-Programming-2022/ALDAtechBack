using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace AldaTech_api.BotCore;

public class TelegramAPI
{
	const string TgApiUrl = "https://api.telegram.org/bot{0}/{1}";

	public string Token { get; set; } = "5001678276:AAH0MgT6aPNR7pkOGeD5eYFxEsJI3sgE4WA";

	public async void GetUpdates()
	{
		const string MethodName = "getUpdates";
		string url = string.Format(TgApiUrl, Token, MethodName);
		var request = (HttpWebRequest)WebRequest.Create(url);
		var response = (HttpWebResponse) await Task.Factory
    		.FromAsync<WebResponse>(request.BeginGetResponse,
                            request.EndGetResponse,
                            null);
		//Console.WriteLine(response.);
	}
}