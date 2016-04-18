<?php
error_reporting(E_ALL ^ E_NOTICE);
session_name('tzLogin');
session_set_cookie_params(2*7*24*60*60);
session_start();
?>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Lobby 1</title>
    
    <link rel="stylesheet" type="text/css" href="demo.css" media="screen" />
    
</head>

<body>

<div id="main">
  <div class="container">
    <h1>Registered Users Only!</h1>
    </div>
    
    <div class="container">
    
    <?php
    
	if($_SESSION['id'])
	echo '<h1>Hello, '.$_SESSION['usr'].'! You are registered and logged in!</h1>';
	else echo '<h1>Please, <a href="index.php">login</a></h1>';
    ?>
    </div>
    
    <body class="template">
    <div class="template-wrap clear">
      <canvas class="emscripten" id="canvas" oncontextmenu="event.preventDefault()" height="600px" width="960px"></canvas>
      <br>
      <div class="logo"></div>
      <div class="fullscreen"><img src="TemplateData/fullscreen.png" width="38" height="38" alt="Fullscreen" title="Fullscreen" onclick="SetFullscreen(1);" /></div>
    </div>
    <script type='text/javascript'>
  var Module = {
    TOTAL_MEMORY: 268435456,
    errorhandler: null,			// arguments: err, url, line. This function must return 'true' if the error is handled, otherwise 'false'
    compatibilitycheck: null,
    dataUrl: "Release/Desktop.data",
    codeUrl: "Release/Desktop.js",
    memUrl: "Release/Desktop.mem",
  };
</script>
 
    <?php
    
	if($_SESSION['id'])
    echo "<script language='JavaScript' src='Release/UnityLoader.js' type='text/javascript'></script>"; 
    ?>
      


</body>
</html>
