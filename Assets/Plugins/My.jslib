mergeInto(LibraryManager.library, {
	
	SaveDataExtern: function(data, score){
		var dataString = UTF8ToString(data);
		var myobj = JSON.parse(dataString);
		player.setData(myobj);

		ysdk.getLeaderboards()
		.then(lb => {
   			 // С extraData
			lb.setLeaderboardScore('Records', score);
		});
	},

	LoadDataExtern: function(){
		player.getData().then(_data => {
			const myJSON = JSON.stringify(_data);
			myGameInstance.SendMessage('DataController', 'LoadData', myJSON);
		});
	},

	ShowFullScreenAdvExtern: function(){
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function(wasShown) {
					myGameInstance.SendMessage('Managers', 'ResumeGame');
          // some action after close
				},
				onError: function(error) {
					myGameInstance.SendMessage('Managers', 'ResumeGame');
          // some action on error
				}
			}
		})
	},

	ShowRewardedAdvExtern: function(){
		ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
					console.log('Video ad open.');
				},
				onRewarded: () => {
					console.log('Rewarded!');
					myGameInstance.SendMessage('Managers', 'SpawnVilleger');
				},
				onClose: () => {
					console.log('Video ad closed.');
					myGameInstance.SendMessage('Managers', 'ResumeGame');
					myGameInstance.SendMessage('GUI', 'OnSawRewardedAdv');
				}, 
				onError: (e) => {
					console.log('Error while open video ad:', e);
				}
			}
		})
	},
});