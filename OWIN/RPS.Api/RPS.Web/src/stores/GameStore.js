var AppDispatcher = require('../dispatcher/AppDispatcher');
var EventEmitter = require('events').EventEmitter;
var merge = require('react/lib/merge');
var _ = require('underscore');

var CHANGE_EVENT = 'change';

var _games = {};
var _score = [];

var GameStore = merge(EventEmitter.prototype, {
    emitChange: function() {
        this.emit(CHANGE_EVENT);
    },

    /**
     * @param {function} callback
     */
    addChangeListener: function(callback) {
        this.on(CHANGE_EVENT, callback);
    },

    get: function(id) {
        return _games[id];
    },

    getAll: function() {
        return _games;
    },
    getScore: function() {
        return _score;
    }
});

GameStore.dispatchToken = AppDispatcher.register(function (payload) {
    var action = payload.action;

    switch (action.type) {

        case "RECEIVE_AVAILABLE_GAMES":
            _games = action.games;
            GameStore.emitChange();
            break;
        case "GAME_CREATED":
            _games.push(action.game);
            GameStore.emitChange();
            break;
        case "GAME_ENDED":
            _score.push(action.game);
            _games = _.filter(_games, function(x) { return x.gameId != action.game.gameId; });
            GameStore.emitChange();
            break;
        default:
            // do nothing
    }

});

module.exports = GameStore;