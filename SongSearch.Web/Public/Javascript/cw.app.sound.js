$(document).ready(function () {

    soundManager.url = '/public/flash/'; // directory where SM2 .SWFs live
    //soundManager.waitForWindowLoad = true;
    // Note that SoundManager will determine and append the appropriate .SWF file to the URL,
    // eg. /path/to/sm2-flash-movies/soundmanager2.swf automatically.

    // Experimental HTML5 audio support (force-enabled for iPad), flash-free sound for Safari + Chrome. Enable if you want to try it!
    //soundManager.useHTML5Audio = true;
    soundManager.flashVersion = 9;
//    // do this to skip flash block handling for now. See the flashblock demo when you want to start getting fancy.
//    soundManager.useFlashBlock = false;
//    soundManager.allowScriptAccess = 'always';
//    // disable debug mode after development/testing..
    soundManager.debugMode = false;
    soundManager.useHighPerformance = true;
    // Option 3 (best): onready() + createSound() methods, handle load/failure together:
    
    soundManager.onready(function (oStatus) {
        // check if SM2 successfully loaded..
        if (oStatus.success) {
            // SM2 has loaded - now you can create and play sounds!
            isSoundManagerReady = true;
            //mySound = soundManager.createSound();
        } else {
            // (Optional) Hrmm, SM2 could not start. Show an error, etc.?
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
                    mySound = soundManager.createSound({ id: 'cw-sound' + mySoundId++, url: url, stream: true });
                    mySound.play();
                    lastUrlPlayed = url;

                } else {
                    soundManager._writeDebug('Warning: sound failed to load (security restrictions, 404 or bad format)', 2);
                }
            }

        } else { //first time
            mySound = soundManager.createSound({ id: 'cw-sound' + mySoundId++, url: url, stream: true });
            mySound.play();
            lastUrlPlayed = url;
        }
    }

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