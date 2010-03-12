// Client timer. Used for slideshow.
Type.registerNamespace('Gsp');
Gsp.Timer = function(userCallback)
{
	this._interval = 10000; 
	this._raiseTickDelegate = Function.createDelegate(this, this._tick);
	this._userCallback = userCallback; 
	this._timer = null; 
} 

function Gsp$Timer$get_interval()
{
	return this._interval; 
} 

function Gsp$Timer$set_interval(value)
{
	this._interval = value; 
} 

function Gsp$Timer$stop()
{
	this._stopTimer(); 
} 

function Gsp$Timer$start()
{
	this._startTimer();
} 

function Gsp$Timer$_tick()
{
	if (this._userCallback !== null)
		this._userCallback();
	this._startTimer(); 
} 

function Gsp$Timer$_startTimer()
{
	this._timer = window.setTimeout( 
		this._raiseTickDelegate, this.get_interval()); 
}

function Gsp$Timer$_stopTimer()
{
	if (this._timer !== null)
	{
		window.clearTimeout(this._timer);
		this._timer = null; 
	}
}

function Gsp$Timer$_isRunning()
{
	return (this._timer !== null)
}

Gsp.Timer.prototype =
{ 
	get_interval: Gsp$Timer$get_interval,
	set_interval: Gsp$Timer$set_interval, 
	stop: Gsp$Timer$stop, 
	start: Gsp$Timer$start, 
	_tick: Gsp$Timer$_tick, 
	_startTimer: Gsp$Timer$_startTimer, 
	_stopTimer: Gsp$Timer$_stopTimer,
	_isRunning: Gsp$Timer$_isRunning
}

Gsp.Timer.registerClass('Gsp.Timer'); 
Sys.Application.notifyScriptLoaded();
