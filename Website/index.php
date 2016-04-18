<?php

error_reporting(E_ALL ^ E_NOTICE);//Turns some warnings off
define('INCLUDE_CHECK',true);
require 'connect.php';
// connect is only included if INCLUDE_CHECK is Definded


session_name('lnoeLogin');
// Starting the session

session_set_cookie_params(2*7*24*60*60);
// Making the cookie live for 2 weeks

session_start();
if(isset($_SESSION['id']))
{
    if($_SESSION['id'] && !isset($_COOKIE['lnoeRemember']) && !$_SESSION['rememberMe'])
    {
        //If you reset browser without cookie or aren't logged in
        $_SESSION = array();
        session_destroy();
    }
}

if(isset($_GET['logoff']))
{
	$_SESSION = array();
	session_destroy();
	
	header("Location: index.php");
	exit;
}

//Checking login form
if(isset($_POST['submit']))
if($_POST['submit']=='Login')
{
	
	$err = array();
	
	if(!$_POST['username'] || !$_POST['password'])
		$err[] = 'All the fields must be filled in!';
	//If there are no errors
	if(!count($err))
	{
		$_POST['username'] = mysqli_real_escape_string($link,$_POST['username']);
		$_POST['password'] = mysqli_real_escape_string($link,$_POST['password']);
		$_POST['rememberMe'] = (int)$_POST['rememberMe'];
		// Escaping all input data
		$row = mysqli_fetch_assoc(mysqli_query($link,"SELECT id,usr FROM lnoe_members WHERE usr='{$_POST['username']}' AND pass='".$_POST['password']."'"));

		if($row['usr'])
		{
			// If everything is OK login		
            //Need to add check for if user is already logged in elsewhere
			$_SESSION['usr']=$row['usr'];
			$_SESSION['id'] = $row['id'];
			$_SESSION['rememberMe'] = $_POST['rememberMe'];		
			// Store some data in the session	
			setcookie('lnoeRemember',$_POST['rememberMe']);
		}
		else $err[]='Wrong username and/or password!';
	}
	
	header("Location: index.php");
	exit;
}
else if($_POST['submit']=='Register')
{
	// If the Register form has been submitted
	
	$err = array();
	
	if(strlen($_POST['username'])<4 || strlen($_POST['username'])>32)
	{
		$err[]='Your username must be between 3 and 32 characters!';
	}
	
	if(preg_match('/[^a-z0-9\-\_\.]+/i',$_POST['username']))
	{
		$err[]='Your username contains invalid characters!';
	}
		
	if(!count($err))
	{

        		
		$_POST['pass'] = mysqli_real_escape_string($link,$_POST['pass']);
        $pass = ($_POST['pass']);
		$_POST['username'] = mysqli_real_escape_string($link,$_POST['username']);
		// Escape the input data
		
		
		mysqli_query($link,"	INSERT INTO lnoe_members(usr,pass,email,regIP,dt)
						VALUES(
						
							'".$_POST['username']."',
							'".$pass."',
							'".$_POST['pass']."',
							'".$_SERVER['REMOTE_ADDR']."',
							NOW()
						)");
		
		if(mysqli_affected_rows($link)==1)
		{
			$_SESSION['msg']['reg-success']='Registration Successful: Please Log In';
		}
		else $err[]='This username is already taken!';
	}

	if(count($err))
	{
		$_SESSION['msg']['reg-err'] = implode('<br />',$err);
	}	
	
	header("Location: index.php");
	exit;
}

$script = '';
if(isset($_SESSION['msg']))
if($_SESSION['msg'])
{
	// The script below shows the sliding panel on page load
    //Sliding panel taken from morphos.is
	
	$script = '
	<script type="text/javascript">
	
		$(function(){
		
			$("div#panel").show();
			$("#toggle a").toggle();
		});
	
	</script>';
	
}
?>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Last Night</title>
    
    <link rel="stylesheet" type="text/css" href="index.css" media="screen" />
    <link rel="stylesheet" type="text/css" href="login_panel/css/slide.css" media="screen" />
    
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js"></script>
    <script src="login_panel/js/slide.js" type="text/javascript"></script>
    
    <?php echo $script; ?>
</head>

<body>

<!-- Panel -->
<div id="toppanel">
	<div id="panel">
		<div class="content clearfix">
			<div class="left">
				<h1>Welcome To Last Night On Earth Online</h1>
				<p class="grey">Please support the creators of last night on earth at <a href="http://www.flyingfrog.net/" title="Go to site">www.flyingfrog.net</a>.</p>
			</div>
            
            
            <?php
			
            
			if(!$_SESSION['id']):
			
			?>
            
			<div class="left">
				<!-- Login Form -->
				<form class="clearfix" action="" method="post">
					<h1>Member Login</h1>
                    
                    <?php
						
						if($_SESSION['msg']['login-err'])
						{
							echo '<div class="err">'.$_SESSION['msg']['login-err'].'</div>';
							unset($_SESSION['msg']['login-err']);
						}
					?>
					
					<label class="grey" for="username">Username:</label>
					<input class="field" type="text" name="username" id="username" value="" size="23" />
					<label class="grey" for="password">Password:</label>
					<input class="field" type="password" name="password" id="password" size="23" />
	            	<label><input name="rememberMe" id="rememberMe" type="checkbox" checked="checked" value="1" /> &nbsp;Remember me</label>
        			<div class="clear"></div>
					<input type="submit" name="submit" value="Login" class="bt_login" />
				</form>
			</div>
			<div class="left right">			
				<!-- Register Form -->
				<form action="" method="post">
					<h1>Not a member yet? Sign Up!</h1>		
                    
                    <?php
						
						if($_SESSION['msg']['reg-err'])
						{
							echo '<div class="err">'.$_SESSION['msg']['reg-err'].'</div>';
							unset($_SESSION['msg']['reg-err']);
						}
						
						if($_SESSION['msg']['reg-success'])
						{
							echo '<div class="success">'.$_SESSION['msg']['reg-success'].'</div>';
							unset($_SESSION['msg']['reg-success']);
						}
					?>
                    		
					<label class="grey" for="username">Username:</label>
					<input class="field" type="text" name="username" id="username" value="" size="23" />
					<label class="grey" for="email">Password:</label>
					<input class="field" type="text" name="email" id="email" size="23" />
					<label></label>
					<input type="submit" name="submit" value="Register" class="bt_register" />
				</form>
			</div>
            
            <?php
			
			else:
			
			?>
            
            <div class="left">
            
            <h1>Game 1</h1>
            
            <a href="Lobby1.php">Lobby 1</a><br>
            <p>- or -</p>
            <a href="?logoff">Log off</a>
            
            </div>
            
            <div class="left right">
               <h1>Game 2</h1>
               <a href="Lobby2.php">Lobby 2</a><br>
            </div>
            
            <?php
			endif;
			?>
		</div>
	</div> <!-- /login -->	

    <!-- The tab on top -->	
	<div class="tab">
		<ul class="login">
	    	<li class="left">&nbsp;</li>
	        <li>Hello <?php 
            if(isset($_SESSION['usr']))
            echo $_SESSION['usr'] ? $_SESSION['usr'] : 'Guest';?>! </li>
           
			<li class="sep">|</li>
			<li id="toggle">
				<a id="open" class="open" href="#"><?php 
                if(isset($_SESSION['id']))
                echo $_SESSION['id']?'Open Panel':'Log In | Register';?></a>
				<a id="close" style="display: none;" class="close" href="#">Close Panel</a>			
			</li>
	    	<li class="right">&nbsp;</li>
		</ul> 
	</div> <!-- / top -->
	
</div> <!--panel -->

<div class="pageContent">
    <div id="main">
      <div class="container">
        <h1>Last Night on Earth Online</h1>
        </div>
          
        </div>
</div>

</body>
</html>
