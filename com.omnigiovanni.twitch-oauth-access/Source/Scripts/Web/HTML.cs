using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OmniGiovanni.Web
{
	public static class HTML
	{
		public static string Page = @"
			<!DOCTYPE html>
			<html lang='en'>
			<head>
			<meta charset='UTF-8'>
			<title>Authentication Complete</title>
			<meta name='viewport' content='width=device-width, initial-scale=1.0'>
		
			</head>
			<body>
			<h3>Authentication complete. You may now close this window.</h3>
			<button onclick='closePage()'>Close Page</button>
			<script>
			
			function closePage() {
			// Close the current browser window
			window.close();
			}
			</script>

			</body>
			</html>
			";   	
    }
}
