using System.Collections;
using System.Collections.Generic;

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.IO;

using System.Net.Sockets;

using UnityEngine;
using UnityEngine.Networking;

using OmniGiovanni.Cryptography;

namespace OmniGiovanni.Web
{

	[Serializable]
	public class Authentication
	{
		
		[Flags]
		public enum TwitchOAuthScope
		{
			None = 0,
			channelBot = 1 << 0,         
			channelModerate = 1 << 1,   
			chatEdit = 1 << 2,         
			chatRead = 1 << 3,  
			userBot = 1 << 4,    
			userReadChat = 1 << 5,   
			whispersRead = 1 << 6,      
			whispersEdit = 1 << 7,		
			All = ~0
		}
		
		public TwitchOAuthScope Scopes;
	
		private Thread listenerThread;
		private HttpListener httpListener;
		private bool stopListening = false;
		
		[SerializeField] private string clientID = "Client_Application_ID";  //Client ID obtained in the dev.twitch.tv.
		[SerializeField] private string webRedirect = "http://www.example.com/";
	
		[SerializeField]Response Data = new Response();
		
		public void Request()
		{		
			string authorizationURL = $"https://id.twitch.tv/oauth2/authorize?response_type=code&client_id={(clientID)}&redirect_uri={webRedirect}&scope={GetScopesString(Scopes)}";
			Application.OpenURL(authorizationURL);
			StartListener();
		}
	
	
		private static Dictionary<TwitchOAuthScope, string> ScopeMappings = new Dictionary<TwitchOAuthScope, string>
		{
			{ TwitchOAuthScope.channelBot, "channel:bot" },
			{ TwitchOAuthScope.channelModerate, "channel:moderate" },
			{ TwitchOAuthScope.chatEdit, "chat:edit" },
			{ TwitchOAuthScope.chatRead, "chat:read" },
			{ TwitchOAuthScope.userBot, "user:bot" },
			{ TwitchOAuthScope.userReadChat, "user:read:chat" },
			{ TwitchOAuthScope.whispersRead, "whispers:read" },
			{ TwitchOAuthScope.whispersEdit, "whispers:edit" }
		};
	 
		public static string GetScopesString(TwitchOAuthScope scopes)
		{
			if(scopes == TwitchOAuthScope.None)
				return null;
				
			string result = string.Join("+", ScopeMappings
			.Where(kv => scopes.HasFlag(kv.Key))
			.Select(kv => kv.Value));

			return result;
		}

		private void StartListener()
		{
			httpListener = new HttpListener();
			httpListener.Prefixes.Add("http://localhost:3000/"); // Set your desired prefix
			httpListener.Start();

			listenerThread = new Thread(Listen);
			listenerThread.Start();
		}
	
		void Listen()
		{
			try
			{
				httpListener.Start();	
				while (!stopListening)
				{
					// Wait for a request and process it
					HttpListenerContext context = httpListener.GetContext();
					ProcessRequest(context);
				}
			}
			catch (HttpListenerException e)
			{
				Debug.LogError("HttpListenerException: " + e.Message);
			}
		}
	
	
	
		private void StopListener()
		{
			stopListening = true;
			listenerThread.Join(); // Wait for the listener thread to finish
			if (httpListener.IsListening)
			{
				httpListener.Stop();
			}

			httpListener.Close();
			
			
		}

		private void ProcessRequest(HttpListenerContext context)
		{
			// Handle the request here
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;
		
			// Get query parameters
			string encryptedData = request.QueryString["E"];
			if (encryptedData != null) {
			 encryptedData = System.Web.HttpUtility.UrlDecode(encryptedData); // Decode the URL
			 encryptedData = encryptedData.Replace('-', '+').Replace('_', '/');

			} else {
			 Debug.LogError("Missing or null 'E' parameter in the request.");
				return; // Exit the method or take appropriate action.
			}
			
			string key = "your_secret_key_here"; // Same key used in the acess.php script
			//
			key = key.PadRight(32, '\0').Substring(0, 32);
			byte[] iv = new byte[16];
			byte[] encryptedBytes = Convert.FromBase64String(encryptedData);

			string decryptedData = Crypto.DecryptData(encryptedBytes, key, iv);
	
			// Do something with the parameters.
			//Debug.Log($"Decrypted Data: {decryptedData}");
		
			Data = JsonUtility.FromJson<Response>(decryptedData);

			// Send a response back to the browser.
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(HTML.Page);

			// Write the response.
			response.ContentLength64 = buffer.Length;
			Stream output = response.OutputStream;
			output.Write(buffer, 0, buffer.Length);
			output.Close();
		
	
		}
	
	
		void OnDestroy()
		{
			// Stop the listener when the script is destroyed
			if (httpListener != null && httpListener.IsListening)
			{
				httpListener.Stop();
				Debug.Log("Server stopped.");
			}
		}
	
	}	
}



