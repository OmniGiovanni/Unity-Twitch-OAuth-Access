using System.Collections;
using System.Collections.Generic;


namespace OmniGiovanni.Web
{
	internal sealed class LocalListener
    {
     
     
     
     
	    private void StartListener(int localport)
	    {
		
		    //	httpListener = new HttpListener();
		    //httpListener.Prefixes.Add($"http://localhost:{localport}/");
		    //httpListener.Start();

		    //Create & start a new thread so the main thread remains responsive and doesn't freeze the application.
		    //listenerThread = new Thread(Listen);
		    //	listenerThread.Start();
		
	    }
	    
	    
	    private void Listen()
	    {
			
		    //    try
		    //   {
			    //	httpListener.Start();	
		    //    while (!stopListening)
		    //	    {	
				    //	    // Wait for a request and process it
		    //	    HttpListenerContext context = httpListener.GetContext();
		    //	    ProcessRequest(context);
		    //    }
		    //	    }
		    //    catch (HttpListenerException e)
		    //    {
		    //	    Debug.LogError("HttpListenerException: " + e.Message);
		    //    }
	    }
		
	    
	    private void StopListener()
	    {
		    //stopListening = true;
		    //listenerThread.Join(); // Wait for the listener thread to finish
		    //	if (httpListener.IsListening)
		    //	{
		    //		httpListener.Stop();
		    //	}

		    //	httpListener.Close();
			
			
	    }
	    
	    
    }
}
