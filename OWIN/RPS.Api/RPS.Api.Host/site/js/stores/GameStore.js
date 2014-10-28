﻿var AppDispatcher = require('../dispatcher/AppDispatcher');
var EventEmitter = require('events').EventEmitter;
var merge = require('react/lib/merge');

var CHANGE_EVENT = 'change';

var _games = {};

var GameStore = merge(EventEmitter.prototype, {

    emitChange: function () {
        this.emit(CHANGE_EVENT);
    },

    /**
     * @param {function} callback
     */
    addChangeListener: function (callback) {
        this.on(CHANGE_EVENT, callback);
    },

    get: function (id) {
        return _games[id];
    },

    getAll: function () {
        return _games;
    },

});

GameStore.dispatchToken = AppDispatcher.register(function (payload) {
    var action = payload.action;

    switch (action.type) {

        case "RECEIVE_AVAILABLE_GAMES":
            _games = action.games;
            GameStore.emitChange();
            break;

        default:
            // do nothing
    }

});

module.exports = GameStore;