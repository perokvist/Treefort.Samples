var GameServerActionCreators = require('../actions/GameServerActionCreators');
var $ = require('jquery');

module.exports = {

    getAvailableGames: function (url) {
        $.ajax({
            url: url,
            dataType: 'json',
            success: function (data) {
                GameServerActionCreators.receiveAvailable(data);
            },
            error: function (xhr, status, err) {
                console.error(url, status, err.toString());
            }
        });
    }
};