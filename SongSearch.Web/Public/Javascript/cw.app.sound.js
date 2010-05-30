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
            flash('error', 'There was an error loading our Flash sound player on your system. Please turn off any Flash blocking software while using this site.');
        }
    });

});


//----------------------------------------------------------------------------------------------------------------------------------------
var _isSoundManagerReady = false;
var _mySoundId = 0;
var _mySound;
var _lastUrlPlayed;
var _currentVolume = 60;

//-----------------------------------------------------------------------------------
//Sound manager controllers
//-----------------------------------------------------------------------------------
//***********************************************
//  mediaPlay: starts, stops, toggles
//***********************************************
function mediaPlay(url) {

    if (_isSoundManagerReady) {

        if (_mySound) {
            if (url == _lastUrlPlayed) {
                if (_mySound.readyState != 2) {
                    if (_mySound.playState != 1) {
                        // not yet playing
                        _mySound.play();
                    } else {
                        _mySound.togglePause();
                    }
                } else {
                    soundManager._writeDebug('Warning: sound failed to load (security restrictions, 404 or bad format)', 2);
                }
            } else {

                if (_mySound.readyState != 2) {
                    if (_mySound.playState == 1) {
                        // playing
                        _mySound.stop();
                    }
                    //                    mySound.play(url);
                    _mySound = getSound(url);

                    _mySound.play();
                    _lastUrlPlayed = url;

                } else {
                    soundManager._writeDebug('Warning: sound failed to load (security restrictions, 404 or bad format)', 2);
                }
            }

        } else { //first time
            _mySound = getSound(url);
            _mySound.play();
            _lastUrlPlayed = url;
        }

      
    }

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
        if (_mySound && _mySound.readyState != 2 && _mySound.playState == 1) {
            // playing
            _mySound.stop();
            _lastUrlPlayed = null;
        }
    }
}

function changeVolume(value) {
    if (_isSoundManagerReady) {
        if (_mySound && _mySound.readyState != 2 && _mySound.playState == 1) {
            _mySound.setVolume(value);
            _currentVolume = value;
        }
    }
}

