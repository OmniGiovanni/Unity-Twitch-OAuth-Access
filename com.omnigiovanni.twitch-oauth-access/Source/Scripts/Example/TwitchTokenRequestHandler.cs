using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using TMPro;
using OmniGiovanni.Web;




namespace OmniGiovanni.Example
{
	public class TwitchTokenRequestHandler : MonoBehaviour
    {
       
	    [SerializeField] private Button authenticateButton;
	    [SerializeField] private Authentication appAuthentication = new Authentication();

	    private void Start()
	    {
	    	
	    	if(authenticateButton != null)
			authenticateButton.onClick.AddListener(HandleOAuth);	
	    }     
	    
	    private void HandleOAuth()
	    {
		    authenticateButton.interactable = false;
		    appAuthentication.Request();
	    }
       
    }
}
