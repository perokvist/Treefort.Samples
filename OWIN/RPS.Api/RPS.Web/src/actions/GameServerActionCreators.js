var AppDispatcher = require('../dispatcher/AppDispatcher');

module.exports = {
    receiveAvailable: function(games) {
        AppDispatcher.handleServerAction({
            type: "RECEIVE_AVAILABLE_GAMES",
            games: games
        });
    }
  };