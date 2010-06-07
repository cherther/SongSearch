$(document).ready(function () {

    soundManager.url = '/public/flash/';
    //soundManager.useHTML5Audio = true;
    soundManager.flashVersion = 9;
    //soundManager.useFlashBlock = true;
    soundManager.debugMode = false;
    soundManager.useHighPerformance = true;

    soundManager.onready(function (oStatus) {
        // check if SM2 successfully loaded..
        if (oStatus.success) {
            // SM2 has loaded - now you can create and play sounds!
            _isSoundManagerReady = true;
            //mySound = soundManager.createSound();
        } else {
            feedback('error', 'There was an error loading our Flash sound player on your system. Please turn off any Flash blocking software while using this site.');
        }
    });

});


//----------------------------------------------------------------------------------------------------------------------------------------
var _isSoundManagerReady = false;
var _mySoundId = 0;
var _mySound;
var _lastUrlPlayed;
var _currentVolume = 60;

var sm_rs_uninitialised = 0;
var sm_rs_loading = 1;
var sm_rs_failed_error = 2;
var sm_rs_loaded_success = 3;

var sm_ps_stopped = 0;
var sm_ps_playing = 1;

//-----------------------------------------------------------------------------------
//Sound manager controllers
//-----------------------------------------------------------------------------------
//***********************************************
//  mediaPlay: starts, stops, toggles
//***********************************************
function soundPlay(url) {

    var begin_playState;
    var begin_readyState;

    if (_isSoundManagerReady) {

        if (_mySound) {
            begin_readyState = _mySound.readyState;
            begin_playState = _mySound.playState;
            if (url == _lastUrlPlayed) { //second click on same link
                if (begin_readyState != sm_rs_failed_error) {
                    if (begin_playState != sm_ps_playing) {
                        // not yet playing
                        _mySound.play();
                    } else {
                        _mySound.togglePause();
                    }
                } else {
                    soundManager._writeDebug('Warning: sound failed to load (security restrictions, 404 or bad format)', 2);
                }
            } else {

                if (begin_readyState != sm_rs_failed_error) {
                    _mySound.unload();

                    if (begin_playState == sm_ps_playing) {
                        // still playing? huh?
                        _mySound.stop();
                    }
                    _mySound = getSound(url);

                    _mySound.play();
                    _lastUrlPlayed = url;

                } else {
                    soundManager._writeDebug('Warning: sound failed to load (security restrictions, 404 or bad format)', 2);
                }
            }

        } else { //first time
            begin_playState = sm_ps_stopped;
            begin_readyState = sm_rs_uninitialised;
            _mySound = getSound(url);
            _mySound.play();
            _lastUrlPlayed = url;
        }
      
    }

    return begin_playState;
}

function soundPlayRepeat() {
    var url = _lastUrlPlayed;
    _lastUrlPlayed = null;
    return soundPlay(url);
}

function getSound(url) {
    _currentVolume = _currentVolume != null && _currentVolume >= 0 ? _currentVolume : 60;
    return soundManager.createSound(
                {
                    id: 'cw-sound' + _mySoundId++,
                    url: url,
                    stream: true,
                    volume: _currentVolume,
                    onload: function () { setTotalMediaLength(this.durationEstimate); },
                    onfinish: function () {
                        toggleAllPlayButtons();
                        setCurrentMediaTime(0);
                        setTotalMediaLength(0);
                        setCurrentPosition(0, 1);
                        setCurrentLoadPercentage(0, this.bytesTotal)
                    },
                    onstop: function () {
                        setCurrentMediaTime(0);
                        setTotalMediaLength(0);
                        setCurrentPosition(0, 1);
                        setCurrentLoadPercentage(0, this.bytesTotal)
                    },
                    whileplaying: function () {
                        setCurrentMediaTime(this.position);
                        setCurrentPosition(this.position, this.durationEstimate);
                    },
                    whileloading: function () {
                        setCurrentLoadPercentage(this.bytesLoaded, this.bytesTotal);
                    }
                }
                );
}

//***********************************************
//  mediaStop: hard stop
//***********************************************
function mediaStop() {

    if (_isSoundManagerReady) {
        if (_mySound && _mySound.readyState != sm_rs_failed_error && _mySound.playState == sm_ps_playing) {
            // playing
            _mySound.unload();
            _mySound.stop();
            _lastUrlPlayed = null;
        }
    }
}

function changeVolume(value) {
    if (_isSoundManagerReady) {
        if (_mySound && _mySound.readyState != sm_rs_failed_error && _mySound.playState == sm_ps_playing) {
            _mySound.setVolume(value);
            _currentVolume = value;
        }
    }
}

