using UnityEngine;
using UnityEngine.UI;
using System;
using OmniGiovanni.Web;

namespace OmniGiovanni.Example
{
	public class TwitchTokenRequestHandler : MonoBehaviour
    {
       
	    [SerializeField] private Button authenticateButton;
	    [SerializeField] private Authentication appAuthentication = new Authentication();
	    private void Start()
	    {
	    	
	    	appAuthentication.requestFinalCallBackEvent += OnAuthenticationCallback; 
	    	
	    	if(authenticateButton != null){
		    	authenticateButton.onClick.AddListener(HandleOAuth);
	    	}
	    }     
	   
	    
	    private void HandleOAuth()
	    {
		    authenticateButton.interactable = false;
		    appAuthentication.Request();
	    }
	    
	    
	    private void OnAuthenticationCallback()
	    {
	    
	    	//TODO: prevent this callback getting called twice for some reason.

	    }
	    
	    protected void OnDestroy()
	    {
	    	appAuthentication.requestFinalCallBackEvent -= OnAuthenticationCallback; 
	    }
       
    }
}
