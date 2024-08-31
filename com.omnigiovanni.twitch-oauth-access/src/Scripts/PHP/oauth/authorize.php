<?php
session_start();

$code = isset($_GET['code']) ? $_GET['code'] : '';

// Assuming the function Access() is defined in the oauth_session.php file
$required = include('../../includes/oauth_session.php');
$rep = Access($code);
$endpoint = isset($_SESSION['endpoint']) ? $_SESSION['endpoint'] : '';
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Authentication Endpoint</title>
    <style>
        body {
            display: flex;
            align-items: center;
            justify-content: center;
            height: 100vh;
            margin: 0;
        }

        .message-box {
            text-align: center;
            padding: 20px;
            border: 2px solid #ccc;
            border-radius: 10px;
        }
    </style>
</head>
<body>

    
    <div id="loading-message" class="message-box">
        <h1>Waiting for Response From Application...</h1>
    </div>

    <script>
        document.addEventListener('DOMContentLoaded', function() {

            fetch('http://localhost:<?php echo $endpoint; ?>/?data=' + encodeURIComponent(JSON.stringify(<?php echo $rep; ?>)), {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
            })
            .then(response => response.json())
            .then(data => {
                // Remove the loading message
                document.getElementById('loading-message').style.display = 'none';

                if (data != null) {
                    if (data.success) {
                        var messageBox = document.createElement('div');
                        messageBox.className = 'message-box';

                        var h1 = document.createElement('h1');
                        h1.textContent = 'Authentication Successful';

                        var p1 = document.createElement('p');
                        p1.textContent = 'You can now close this window and';

                        var p2 = document.createElement('p');
                        p2.textContent = 'continue using APPLICATION_NAME.';

                        messageBox.appendChild(h1);
                        messageBox.appendChild(p1);
                        messageBox.appendChild(p2);

                        // Remove existing body content
                        document.body.innerHTML = '';

                        // new content
                        document.body.appendChild(messageBox);
                    }
                } else {
                    console.error('Error:', 'data was empty');
                }
                //
            })
            .catch(error => {
                console.error('Error:', error);
            });
        });
    </script>

</body>
</html>
