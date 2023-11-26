using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace OmniGiovanni.Web{

[Serializable]
public struct AccessTokenResponse
{
	public string access_token;
	public int expires_in;
	public string refresh_token;
	public string[] scope;
	public string token_type;
}


}