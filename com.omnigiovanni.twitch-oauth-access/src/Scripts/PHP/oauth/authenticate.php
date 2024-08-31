<?php
session_start();
sleep(1); // let the user linger for a second;

//////////////////////////////////////////

$required = __DIR__ . '/../../includes/oauth_session.php';

//////////////////////////////////////////

$redirect_url = 'redirect_uri'; // OAuth Redirect URL
$client_ID = 'client_id';

/////////////////////////////////////////

$missing_items = [];

if (file_exists($required)) {
    // The file exists, include it
    include $required;

    // Check if 'state', 'scope', and 'endpoint' are set in the GET request
    $state = isset($_GET['state']) ? $_GET['state'] : null;
    $scope = isset($_GET['scope']) ? $_GET['scope'] : null;
    $endpoint = isset($_GET['endpoint']) ? $_GET['endpoint'] : null;

    // Collect missing parameters
    if ($state === null) {
        $missing_items[] = 'state';
    }
    if ($scope === null) {
        $missing_items[] = 'scope';
    }
    if ($endpoint === null) {
        $missing_items[] = 'endpoint';
    }

    // Check if there are any missing items
    if (empty($missing_items)) {
       
        $_SESSION['state'] = $state;
        $_SESSION['scope'] = $scope;
        $_SESSION['endpoint'] = $endpoint;

        //Construct the API URL
        $API = 'https://id.twitch.tv/oauth2/authorize?response_type=code&client_id=' . $client_ID . '&redirect_uri=' . $redirect_url . '&scope=' . $scope . '&state=' . $state;

        // Redirect to the API URL
        header('Location: ' . $API);
        exit();
    } else {
      
        echo 'Missing required parameters: ' . implode(', ', $missing_items) . '.';
    }
} else {
    
    echo 'Required file not found: ' . htmlspecialchars($required);
}
?>
