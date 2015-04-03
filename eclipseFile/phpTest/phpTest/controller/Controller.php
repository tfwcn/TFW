<?php

namespace phpTest\controller {

	class Controller {
		protected function View() {
			$backtrace = debug_backtrace ();
			array_shift ( $backtrace );
			$className = $backtrace [0] ["class"];
			$functionName = $backtrace [0] ["function"];
			echo $className . " - " . $functionName . "<br>";
		}
	}
}