

var MyPlugin = {
    IsMobile: function () {
        var ua = window.navigator.userAgent.toLowerCase();
        var mobilePattern = /android|iphone|ipad|ipod/i;

        return ua.search(mobilePattern) !== -1 || (ua.indexOf("macintosh") !== -1 && "ontouchend" in document);
    },
    GiveMePlayerData: function () {
		myGameInstance.SendMessage('YandexManager', 'SetName', player.getName());
        var photo = player.getPhoto("medium");
        if (photo) {
		    myGameInstance.SendMessage('YandexManager', 'SetPhoto', );
        }
    },
    RateGame : function (){
	    ysdk.feedback.canReview()
            .then(({ value, reason }) => {
                if (value) {
                    ysdk.feedback.requestReview()
                        .then(({ feedbackSent }) => {
                            console.log(feedbackSent);
                        })
                } else {
                    console.log(reason)
            }
        })
    },
    SetToLeaderBoard: function(time, score){
        function SetScoreToLeaderBoard(name, value) {
            ysdk.getLeaderboards()
                .then(lb => lb.getLeaderboardPlayerEntry(name))
                .then(res => {
                    if(res.score < value) {
                        ysdk.getLeaderboards()
                        .then(lb => {
                            lb.setLeaderboardScore(name, value);
                        });
                    }
                })
                .catch(err => {
                    if (err.code === 'LEADERBOARD_PLAYER_NOT_PRESENT') {
                        ysdk.getLeaderboards()
                            .then(lb => {
                                lb.setLeaderboardScore(name, value);
                            });
                    }
                });
        }

        SetScoreToLeaderBoard('MaxScore', score);

        setTimeout(function() {
            SetScoreToLeaderBoard('MaxTime', time);
        }, 1500) 
    },
    GetLang : function (){
	    var lang = ysdk.environment.i18n.lang;
	    var bufferSize = lengthBytesUTF8(lang) + 1;
	    var buffer = _malloc(bufferSize);
	    stringToUTF8(lang, buffer, bufferSize);
	    return buffer;
    },
};  
mergeInto(LibraryManager.library, MyPlugin);