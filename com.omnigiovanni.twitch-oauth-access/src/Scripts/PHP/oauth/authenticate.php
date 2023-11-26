<?php

session_start();
sleep(1); // let the user linger for a second;
//////////////////////////////////////////

$required = __DIR__ . '/../../../../includes/oauth_session.php';

//////////////////////////////////////////

$redirect_url = 'redirect_uri';
$client_ID = 'client_id';

/////////////////////////////////////////

if (file_exists($required)) {
include $$required;

    $state = $_GET['state'];
    $scope = $_GET['scope'];
    $endpoint = $_GET['endpoint'];



    $_SESSION['state'] = $state;
    $_SESSION['scope'] = $scope;
    $_SESSION['endpoint'] = $endpoint;


    $API = 'https://id.twitch.tv/oauth2/authorize?response_type=code&client_id=' . $client_ID. '&redirect_uri=' . $redirect_url . '&scope=' . $scope .'&state=' . $state;
    
   header('Location: ' . $API);
  


}else{

   echo 'ERROR2'; 
}







?>