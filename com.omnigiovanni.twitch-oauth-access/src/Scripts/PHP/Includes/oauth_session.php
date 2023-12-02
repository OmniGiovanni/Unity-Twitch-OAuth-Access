<?php

session_start();

//Note: If you want to keep errors simple and to prevent users getting too acquainted with them ,then you can get change the echo errors to a genaric message like "Something went wrong, please try again later."

function Access($code)
{

$scopesAllowedOnly = array("chat:read"); // Add more scopes that will be compared to what you have assigned in you Unity app.

$apiUrl = 'https://id.twitch.tv/oauth2/token';
$client = "client_id";
$client_secret = "client_secret";
$redirect = "redirect_uri";


$state = isset($_SESSION['state']) ? $_SESSION['state'] : '';
$scope = isset($_SESSION['scope']) ? $_SESSION['scope'] : '';


if (!empty($scope) && !empty($state))
    {


        $grantedScopes = explode(' ', $scope);
        
        if (count($grantedScopes) === count($scopesAllowedOnly) && empty(array_diff($grantedScopes, $scopesAllowedOnly)))
        {

        $postData = array(
        'client_id' => $client,
        'client_secret' => $client_secret,
        'code' => $code,
        'grant_type' => 'authorization_code', // twitch expects as is: 'authorization_code'
        'redirect_uri' => $redirect,
        );

        
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

            //You can add more checks here if needed.
         return $response;
        }
        else 
        {
            echo 'Error: Scopes Are Mismatch';
        }

    } 
    else 
    {
         echo 'Error: Scope & State Sessions Are Missing!';
    }

}

?>