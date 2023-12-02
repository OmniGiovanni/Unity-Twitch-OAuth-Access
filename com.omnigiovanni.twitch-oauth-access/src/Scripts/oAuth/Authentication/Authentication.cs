using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Text;

namespace OmniGiovanni.Web
{

	[Serializable]
	public class Authentication
	{
	
	
				
		[SerializeField] private const string HostURL = "https://example.com/oauth/authenticate?";
		
		private Scopes scopes = new Scopes();
		[SerializeField] private Scopes.TwitchOAuthScope scopesList;
	
		private AuthThread listenerThread;
		private HttpListener httpListener;
		
		public bool stopListening = false;

		[SerializeField]AccessTokenResponse Response = new AccessTokenResponse();	
		
		public delegate void RequestEvent();
		public event RequestEvent requestFinalCallBackEvent;

		
		public void Request()
		{		
			
			//Create a provide the localhost a random port to listen on and set it to the expected endpoint URL param.
			int localport = UnityEngine.Random.Range(25002,60339);
			string url = $"{HostURL}state={Uri.EscapeDataString(Convert.ToBase64String(System.Guid.NewGuid().ToByteArray()))}&scope={Uri.EscapeDataString(scopes.ConstuctToString(scopesList))}&endpoint={localport}";
			Application.OpenURL(url);
			//StartListener(localport);
			
		}


		private void ProcessRequest(HttpListenerContext context)
		{
			// Handle the request here
			HttpListenerRequest request = context.Request;
			HttpListenerResponse response = context.Response;
			
			//CORS headers to allow sites to access resources from any origin "*", with allowed targted "GET, POST, OPTIONS" method requests, and the allow header 'Content-Type'.
			response.AddHeader("Access-Control-Allow-Origin", "*");
			response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
			response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

			try
			{
				
				if(request.QueryString["data"]!=null)
				{
				
					string jsonData = request.QueryString["data"];
					Response = JsonUtility.FromJson<AccessTokenResponse>(jsonData);
					
					if (context.Response.StatusCode == 200)
					{
						
						LocalSuccessResponse LocalSuccessResponce = new LocalSuccessResponse
						{
							success = true,
							message = "Data received and processed successfully."
						};
						
						// Convert the response object to JSON
						string jsonResponse = JsonUtility.ToJson(LocalSuccessResponce);
						
						// Send the JSON response back to the client
						byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);
						response.ContentLength64 = buffer.Length;
						Stream output = response.OutputStream;
						output.Write(buffer, 0, buffer.Length);
						output.Close();
						
						//StopListener
						
						//TODO: Fix the Double-Calls & Make it so theat the thead joins the main.
						//For now the StopListener() method acts too early can stops the response OutputStream for the localhost, leaving the webpage dead in its tracks waiting..
						requestFinalCallBackEvent?.Invoke();
						
					}
					 
				}else{
					Debug.Log("invalid data....");	
					return;
				}
	
			}//
    catch (Exception ex)
    {
	    // Handle the exception, you can log it or take appropriate action.
	    Debug.LogError($"An error occurred: {ex.Message}");
    }
    // You can add finally{} if you want some logic passed after the Response is waiting or something.
		}
		
		private void OnDestroy()
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