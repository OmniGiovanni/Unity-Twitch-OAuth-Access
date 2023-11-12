<?php
function Access($code, $scope)
{
    $apiUrl = 'https://id.twitch.tv/oauth2/token';
  	
  	$iv = str_repeat(chr(0x0), 16); // 16 bytes of chr(0x0)
  	$key = "your_secret_key_here"; // Same key in the ProcessRequest method of the Unity Project, for encrypting the localhost response.

    $postData = array(
        'client_id' => "Client_Application_ID", //Client ID obtained in the dev.twitch.tv.
        'client_secret' => "Client_Application_Secret", //Client secret obtained in the dev.twitch.tv.
        'code' => $code,
        'grant_type' => 'authorization_code',
        'redirect_uri' => 'http://www.example.com/callback.php' // OAuth Redirect URL
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