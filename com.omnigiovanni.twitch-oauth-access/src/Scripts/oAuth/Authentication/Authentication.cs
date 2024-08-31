using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace OmniGiovanni.Web
{
    public class Authentication
    {
        private HttpListener httpListener;
        private Thread listenerThread;
        public event Action<string> OnAuthorizationCodeReceived;

        private string redirectUri = "http://localhost:3400/";
        private string authorizationEndpoint = "https://codemune.dx.am/oauth/authenticate.php"; // Replace with actual authorization URL

        public Authentication()
        {
            InitializeListener();
        }

        private void InitializeListener()
        {
            if (httpListener != null)
            {
                httpListener.Close(); // Ensure any previous listener is fully closed
            }

            httpListener = new HttpListener();
            httpListener.Prefixes.Add(redirectUri);
        }

        public void Start(string scope)
        {
            // Ensure any previous listener is stopped
            Stop();

            // Initialize a new listener
            InitializeListener();

            listenerThread = new Thread(Listen);
            listenerThread.IsBackground = true;
            listenerThread.Start();

            // Construct the OAuth URL with parameters
            string authUrl = $"{authorizationEndpoint}?state={Uri.EscapeDataString(Convert.ToBase64String(Guid.NewGuid().ToByteArray()))}&scope={Uri.EscapeDataString(scope)}&endpoint=3400";

            // Open the URL in the default web browser
            Application.OpenURL(authUrl);

            Debug.Log($"Opened URL: {authUrl}");
        }

        private void Listen()
        {
            try
            {
                httpListener.Start();
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.Log("Listening for incoming HTTP requests...");
                });

                while (httpListener.IsListening)
                {
                    try
                    {
                        var context = httpListener.GetContext();
                        ProcessRequest(context);
                    }
                    catch (Exception ex)
                    {
                        if (httpListener.IsListening)
                        {
                            UnityMainThreadDispatcher.Instance().Enqueue(() =>
                            {
                                Debug.LogError($"Listener exception: {ex.Message}");
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.LogError($"Failed to start HttpListener: {ex.Message}");
                });
            }
        }

        public void Stop()
        {
            if (httpListener != null && httpListener.IsListening)
            {
                new Thread(() =>
                {
                    try
                    {
                        httpListener.Stop();
                        httpListener.Close();
                    }
                    catch (Exception ex)
                    {
                        UnityMainThreadDispatcher.Instance().Enqueue(() =>
                        {
                            Debug.LogError($"Error stopping HttpListener: {ex.Message}");
                        });
                    }
                }).Start();
            }

            if (listenerThread != null && listenerThread.IsAlive)
            {
                listenerThread.Join();
                listenerThread = null;
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            // Set CORS headers
            response.AddHeader("Access-Control-Allow-Origin", "*");
            response.AddHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.AddHeader("Access-Control-Allow-Headers", "Content-Type");

            try
            {
                if (request.HttpMethod == "OPTIONS")
                {
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.Close(); // Close the response for OPTIONS requests
                    return;
                }

                if (request.QueryString["data"] != null)
                {
                    string data = WebUtility.UrlDecode(request.QueryString["data"]);

                    // Enqueue Unity tasks on the main thread.
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        try
                        {
                            // Invoke the event
                            OnAuthorizationCodeReceived?.Invoke(data);

                            // Prepare the success response
                            LocalSuccessResponse localSuccessResponse = new LocalSuccessResponse
                            {
                                success = true,
                                message = "Token received successfully."
                            };

                            string jsonResponse = JsonUtility.ToJson(localSuccessResponse);
                            byte[] buffer = Encoding.UTF8.GetBytes(jsonResponse);

                            // Send the response on the main thread
                            response.ContentLength64 = buffer.Length;
                            using (Stream output = response.OutputStream)
                            {
                                output.Write(buffer, 0, buffer.Length);
                            }

                            Debug.Log($"Request processed:{request.HttpMethod} {request.Url}");
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Error processing response on main thread: {ex.Message}");
                        }
                        finally
                        {
                            // close response to avoid hanging the client.
                            response.Close();
                        }

                        // Stop the listener after processing the request.
                        Stop();
                    });
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusDescription = "Bad Request: Missing data";
                    response.Close(); //Close the response here if there's an issue.
                }
            }
            catch (Exception ex)
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    Debug.LogError($"Error processing request: {ex.Message}");
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusDescription = "Internal Server Error";
                    response.Close(); //Close the response in case of an error.
                });
            }
        }
    }
}
