var GameServerActionCreators = require('../actions/GameServerActionCreators');
var $ = require('jquery');

var url = "/api/Games/";

var poll = function (url, time, cb) {
    $.ajax({
        url: url,
        type: "GET",
        dataType: 'json',
        success: function (data) {
            cb(data);  // process results here
        },
        error: function (xhr, status, err) {
            console.error(url, status, err.toString());
            setTimeout(poll(url,time,cb), time);
        }
    });
};

module.exports = {
    getAvailableGames: function() {
        $.ajax({
            url: url + "available/",
            dataType: 'json',
            success: function(data) {
                GameServerActionCreators.receiveAvailable(data);
            },
            error: function(xhr, status, err) {
                console.error(url, status, err.toString());
            }
        });
    },
    createGame: function(command, cb) {
        $.ajax({
            url: url,
            dataType: 'json',
            type: 'POST',
            data: JSON.stringify(command),
            contentType: 'application/json; charset=utf-8',
            success: function(data, status, xhr) {
                poll(xhr.getResponseHeader('Location'), 2000, cb);
            },
            error: function(xhr, status, err) {
                console.error(url, status, err.toString());
            }
        });
    },
    makeMove: function (command, cb) {
        $.ajax({
            url: url + "available/" + command.gameId + "/",
            dataType: 'json',
            type: 'PUT',
            data: JSON.stringify(command),
            contentType: 'application/json; charset=utf-8',
            success: function (data, status, xhr) {
                poll(xhr.getResponseHeader('Location'), 2000, cb);
            },
            error: function (xhr, status, err) {
                console.error(url, status, err.toString());
            }
        });
    }

};
