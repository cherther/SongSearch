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
            isSoundManagerReady = true;
            //mySound = soundManager.createSound();
        } else {
            flash('error', 'There was an error loading our Flash sound player on your system. Please turn off any Flash blocking software while using this site.');
        }
    });

});


//----------------------------------------------------------------------------------------------------------------------------------------
var isSoundManagerReady = false;
var mySoundId = 0 ;
var mySound;
var lastUrlPlayed;

//-----------------------------------------------------------------------------------
//Sound manager controllers
//-----------------------------------------------------------------------------------
//***********************************************
//  mediaPlay: starts, stops, toggles
//***********************************************
function mediaPlay(url) {

    if (isSoundManagerReady) {

        if (mySound){
            if (url == lastUrlPlayed) {
                if (mySound.readyState != 2) {
                    if (mySound.playState != 1) {
                        // not yet playing
                        mySound.play();
                    } else {
                        mySound.togglePause();
                    }
                } else {
                    soundManager._writeDebug('Warning: sound failed to load (security restrictions, 404 or bad format)', 2);
                }
            } else {

                if (mySound.readyState != 2) {
                    if (mySound.playState == 1) {
                        // playing
                        mySound.stop();
                    }
                    //                    mySound.play(url);
                    mySound = _getSound(url);
    
                    mySound.play();
                    lastUrlPlayed = url;

                } else {
                    soundManager._writeDebug('Warning: sound failed to load (security restrictions, 404 or bad format)', 2);
                }
            }

        } else { //first time
            mySound = _getSound(url);
            mySound.play();
            lastUrlPlayed = url;
        }

      
    }

}

function _getSound(url) {
    return soundManager.createSound(
                {
                    id: 'cw-sound' + mySoundId++,
                    url: url,
                    stream: true,
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

    if (isSoundManagerReady) {
        if (mySound && mySound.readyState != 2 && mySound.playState == 1) {
            // playing
            mySound.stop();
            lastUrlPlayed = null;
        }
    }
}

