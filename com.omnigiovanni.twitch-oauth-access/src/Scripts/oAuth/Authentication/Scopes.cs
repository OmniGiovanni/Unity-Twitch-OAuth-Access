using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace OmniGiovanni.Web
{
	internal sealed class Scopes
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
		
		
		public string ConstuctToString(TwitchOAuthScope scopes)
		{
			if(scopes == TwitchOAuthScope.None)
				return null;
				
			string result = string.Join("+", ScopeMappings
				.Where(kv => scopes.HasFlag(kv.Key))
				.Select(kv => kv.Value));
		
		
			return result;
		}
     
    }
}
