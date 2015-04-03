<?php
function __autoload($class_name) {
	$file=str_replace("/", "\\", $class_name) . '.php';
	if(file_exists($file))
	{
		require_once $file;
	}
}
class Init {
	public static function Initialize() {
		echo $_SERVER ['PHP_SELF'] . "<br>";
	}
}
Init::Initialize ();
?>