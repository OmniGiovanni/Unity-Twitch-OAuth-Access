<?php
$required = include('../../../../includes/access.php'); //file located outside the public_html
$code = isset($_GET['code']) ? $_GET['code'] : '';
$scope = isset($_GET['scope']) ? $_GET['scope'] : '';
Access($code,$scope);
?>
