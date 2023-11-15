<?php
function Access($code, $scope)
{
    $apiUrl = 'https://id.twitch.tv/oauth2/token';
  	$scopesAllowedOnly = array("chat:read"); // Add more scopes that will be compared to what you have assigned in you Unity app.
	
    $iv = str_repeat(chr(0x0), 16); // 16 bytes of chr(0x0)
  	$key = "your_secret_key_here"; // Use the same encryption key in the ProcessRequest method of the Unity Project.
  	$client = "Client_Application_ID";
  	$client_secret = "Client_Application_Secret";
   
  	if (!empty($scope))
    {
    	$grantedScopes = explode(' ', $scope);
    
    	if (count($grantedScopes) === count($scopesAllowedOnly) && empty(array_diff($grantedScopes, $scopesAllowedOnly)))
        {
 			//You can add more check here if needed.
    	}
      	else 
        {
      		$client = $code = "null";
    	}
	} 
  	else 
    {
    	$client = $code = "null";
	}
  
    $postData = array(
        'client_id' => $client,
        'client_secret' => $client_secret,
        'code' => $code,
        'grant_type' => 'authorization_code',
        'redirect_uri' => 'http://www.example.com/callback.php',
    );
  	
  	// Make the connection to the $apiUrl
    $ch = curl_init($apiUrl);

    // Set cURL options for the POST request
    curl_setopt($ch, CURLOPT_POST, 1); // indicate that the cURL request should be a POST request.
    curl_setopt($ch, CURLOPT_POSTFIELDS, $postData); // set the data to for the post to send.
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true); // indicate that a responds sould be made.

    // Execute the cURL session and fetch the result
    $response = curl_exec($ch);

    // Check for cURL errors
    if (curl_errno($ch)) {
        echo 'Curl error: ' . curl_error($ch);
    }

    // Close cURL session
    curl_close($ch);
  
  	$Message = encryptData($response,$key,$iv);
 	$Message = str_replace(['+', '/'], ['-', '_'], $Message);
	$Message = urlencode($Message); // URL encode the entire URL
    header('Location: http://localhost:3000/?data=' . $Message);
    exit;
}

function encryptData($data, $key, $iv)
{
    $cipher = "aes-256-cbc";
    $options = OPENSSL_RAW_DATA;
    $encrypted = openssl_encrypt($data, $cipher, $key, $options, $iv);
    return base64_encode($encrypted);
}

?>