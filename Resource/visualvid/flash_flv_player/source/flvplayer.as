//--------------------------------------------------------------------------
// initial variables that might be useful to change
//--------------------------------------------------------------------------

// toggle for which file to play if none was set in html
// you can change the 'test.flv' in your filename
if(!_root.file) { file = 'video.flv' } else { file = 'Members/' + _root.file + '.flv'; }


// toggle for autostarting the video
// you can change the 'true' in 'false'
if(_root.autostart == 'true' || _root.autoStart == 'true') { autoStart = true; } 
else { autoStart = false; }

if(autoStart == true) { clicktext = "Buffering.."; } 
else if (!_root.clicktext) { clicktext = '© VisualVid. Click to play'; } 
else { clicktext = _root.clicktext; }

playText1.text = playText2.text = clicktext;


// toggle for the width and height of the video
// you can change them to the width and height you want
w = Stage.width;
h = Stage.height;

//--------------------------------------------------------------------------
// stream setup and functions
//--------------------------------------------------------------------------

// create and set netstream
nc = new NetConnection();
nc.connect(null);
ns = new NetStream(nc);
ns.setBufferTime(10);

// create and set sound object
this.createEmptyMovieClip("snd", 0);
snd.attachAudio(ns);
audio = new Sound(snd);
audio.setVolume(80);

// attach videodisplay
videoDisplay.attachVideo(ns);
videoDisplay.smoothing = true;
videoDisplay.deblocking = 5;

// Retrieve duration meta data from netstream
ns.onMetaData = function(obj) {
	this.totalTime = obj.duration;
	// these three lines were used for automatically sizing
	// it is now done by sizing the video to stage dimensions
	// if(obj.height > 0 && obj.height < Stage.height-20) {
	// setDims(obj.width, obj.height);
	// }
};

// retrieve status messages from netstream
ns.onStatus = function(object) {
	if(object.code == "NetStream.Play.Stop") {
		// rewind and pause on when movie is finished
		ns.seek(0);
		if(_root.repeat == 'true') { return; } 
		ns.pause();
		playBut._visible = true;
		pauseBut._visible = false;
		videoDisplay._visible = false;
		if (!_root.clicktext) { playText1.text = playText2.text = '© VisualVid. Click to play'; } 
		else { playText1.text = playText2.text = _root.clicktext; }
	}  else if(object.code == "NetStream.Buffer.Full") {
		playText1.text = playText2.text = "";
	} else if(object.code == "NetStream.Play.Start") {
		playText1.text = playText2.text = "Buffering..";
	}
};


//--------------------------------------------------------------------------
// controlbar functionality
//--------------------------------------------------------------------------

// play the movie and hide playbutton
function playMovie() {
	if(!isStarted) {
		ns.play(file);
		isStarted = true;
	} else {
		ns.pause();
	}
	pauseBut._visible = true;
	playBut._visible = false;
	videoDisplay._visible = true;
}

// pause the movie and hide pausebutton
function pauseMovie() {
	ns.pause();
	playBut._visible = true;
	pauseBut._visible = false;
};

// video click action
videoBg.onPress = function() {
	if(pauseBut._visible == false) {
		playMovie();
	} else {
		pauseMovie();
	}
};

// pause button action
pauseBut.onPress = function() {
	pauseMovie();
};

// play button action
playBut.onPress = function() {
	playMovie();
};

// file load progress
progressBar.onEnterFrame = function() {
	loaded = this._parent.ns.bytesLoaded;
	total = this._parent.ns.bytesTotal;
	if (loaded == total && loaded > 1000) {
		this.loa._xscale = 100;
		delete this.onEnterFrame;
	} else {
		this.loa._xscale = int(loaded/total*100);
	}
};

// play progress function
progressBar.tme.onEnterFrame = function() {
	this._xscale = ns.time/ns.totalTime*100;
};

// start playhead scrubbing
progressBar.loa.onPress = function() {
	this.onEnterFrame = function() {
		scl = (this._xmouse/this._width)*(this._xscale/100)*(this._xscale/100);
		if(scl < 0.02) { scl = 0; }
		ns.seek(scl*ns.totalTime);
	};
};

// stop playhead scrubbing
progressBar.loa.onRelease = progressBar.loa.onReleaseOutside = function () { 
	delete this.onEnterFrame;
	pauseBut._visible == false ? videoDisplay.pause() : null;
};


// fullscreen
if(_root.showfsbutton == "true") {
	FSBut.onPress = function() {
		var base = _root._url.substring(0,_root._url.lastIndexOf('/'));
		getURL(_root._url+"?file="+file+"&autostart=true&fs=true");
	};
} else if (_root.fs == "true") {
	FSBut.onPress = function() {
		getURL("javascript: history.go(-1)");
	};
}



// volume scrubbing
volumeBar.back.onPress = function() {
	this.onEnterFrame = function() {
		var xm = this._xmouse;
		if(xm>=0 && xm <= 20) {
			this._parent.mask._width = this._xmouse;
			this._parent._parent.audio.setVolume(this._xmouse*5);
		}
	};
}
volumeBar.back.onRelease = volumeBar.back.onReleaseOutside = function() {
	delete this.onEnterFrame;
}
volumeBar.icn.onPress = function() {
	if (this._parent._parent.audio.getVolume() == 0) { 
		this._parent._parent.audio.setVolume(90);
		this._parent.mask._width = 18;
	} else { 
		this._parent._parent.audio.setVolume(0);
		this._parent.mask._width = 0;
	}
}


//--------------------------------------------------------------------------
// resize and position all items
//--------------------------------------------------------------------------
function setDims(w,h) {
	// set videodisplay dimensions
	videoDisplay._width = videoBg._width = w;
	videoDisplay._height = videoBg._height = h-20;
	playText1._x = w/2-120;
	playText1._y = h/2-20;
	playText2._x = playText1._x + 1;
	playText2._y = playText1._y + 1;
		
	// resize the controlbar items .. 
	if(_root.fs == "true") {
		colorBar._y = playBut._y = pauseBut._y = progressBar._y = FSBut._y = volumeBar._y = h-30; 
		playBut._x = pauseBut._x = colorBar._x = w/2-150;
		colorBar._width = 300;
		colorBar._alpha = 25;
		progressBar._x = w/2-133;
		progressBar._width = 228;
		FSBut._x = w/2+95;
		volumeBar._x = w/2+112;
		videoDisplay._height = h;
	} else {
		colorBar._y = playBut._y = pauseBut._y = progressBar._y = FSBut._y = volumeBar._y = h-20;
		progressBar._width = w-56;
		colorBar._width = w;
		volumeBar._x = w-38;
		if(_root.showfsbutton == "true") { 
			FSBut._visible = true;
			progressBar._width -=17;
			FSBut._x = w-55;
		} else {
			FSBut._visible = false;
		}
	}
}

// here you can ovverride the dimensions of the video
setDims(w,h);



//--------------------------------------------------------------------------
// all done ? start the movie !
//--------------------------------------------------------------------------

// start playing the movie
// if no autoStart it searches for a placeholder jpg
// and hides the pauseBut

pauseBut._visible = false;
if(_root.image) { 
	imageStr = _root.image;
} else {
	imageStr = substring(file,0,file.length-3)+"jpg";
}
imageClip.loadMovie(imageStr);


if (autoStart == true) { playMovie(); }