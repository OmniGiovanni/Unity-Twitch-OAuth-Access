using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace OmniGiovanni.Web{

	//this reponce the endpoint webpage will be waiting for.
	//can check the reponce but going in the browser, inpecting the contents with (F12) and viewing the console for logs. 
[Serializable]
public struct LocalSuccessResponse
{
	public bool success;
	public string message;
}


}