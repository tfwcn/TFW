<?php
use phpTest\controller\IndexController;
include 'Init.php';
$a = new IndexController();
$a->AView();
echo $_SERVER["QUERY_STRING"]."</br>";
?>