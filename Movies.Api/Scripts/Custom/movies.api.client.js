var MoviesApiClient = Class.extend({
    baseUrl: null,
    token: null,
    userID: null,
    userName: null,

    _movieList: [],

    login: function () {
        this._callApi({
            method: 'POST',
            url: 'api/v1.0/account',
            data: {
                UserName: $('.loginControl #UserName').val(),
                Password: $('.loginControl #Password').val()
            },
            ok: function (response) {
                window.moviesApiClient.token = response.token;
                var payload = window.moviesApiClient._parseJwt(response.token);
                window.moviesApiClient.userID = payload.userID;
                window.moviesApiClient.userName = payload.name;

                window.moviesApiClient.setLoggedId();
                window.moviesApiClient.getAllMovieList();
            }
        });
    },
    getAllMovieList: function () {
        this._callApi({
            method: 'GET',
            url: 'api/v1.0/movieLists',
            ok: function (response) {
                window.moviesApiClient._movieList = response;
                window.moviesApiClient._refreshListUI();
            }
        });
    },
    getMovieList: function () {
        var ID = null;
        this._callApi({
            method: 'GET',
            url: 'api/v1.0/movieLists/' + ID
        });
    },
    createMovieList: function () {
        var movieList = {};
        this._callApi({
            method: 'POST',
            url: 'api/v1.0/movieLists',
            data: movieList
        });
    },
    editMovieList: function () {
        var movieList = {};
        this._callApi({
            method: 'PUT',
            url: 'api/v1.0/movieLists',
            data: movieList
        });
    },
    searchMovieList: function () {
        var name = $('#ListName').val();

        if (name == '') {
            this.getAllMovieList();
        } else {
            this._callApi({
                method: 'GET',
                url: 'api/v1.0/movieLists/movie/' + name,
                ok: function (response) {
                    window.moviesApiClient._movieList = response;
                    window.moviesApiClient._refreshListUI();
                }
            });
        }
    },
    deleteMovieList: function (ID) {
        this._callApi({
            method: 'DELETE',
            url: 'api/v1.0/movieLists/' + ID
        });
    },
    setLoggedId: function () {
        if (this.token != null) {
            $('.loginControl .textBoxContainer').css('display', 'none');
            $('.loginControl .loggedInContainer #loggedInUserName').text(this.userName);
            $('.loginControl .loggedInContainer').css('display', '');
        } else {
            $('.loginControl .textBoxContainer').css('display', '');
            $('.loginControl .loggedInContainer').css('display', 'none');
        }
    },

    showCreateMovieList: function () {
        dialog = $("#editControl").dialog({
            autoOpen: false,
            height: 400,
            width: 350,
            modal: true,
            buttons: {
                Cancel: function () {
                    dialog.dialog("close");
                }
            },
            close: function () {
                form[0].reset();
                allFields.removeClass("ui-state-error");
            }
        });
    },

    _callApi: function (options) {
        options = $.extend({
            token: this.token,
            ok: function (result) {
                console.log(result);
            }
        }, options);
        $.ajax({
            method: options.method,
            url: this.baseUrl + options.url,
            beforeSend: function (xhr) {
                if (options.token != null) {
                    xhr.setRequestHeader('Authorization', 'Bearer ' + options.token);
                }
            },
            data: options.data,
        }).done(function (result) {
            options.ok(result);
        }).fail(function (jqXHR, textStatus, errorThrown) {
            if (!jqXHR.responseJSON.ErrorCode) {
                alert(jqXHR.responseJSON.Message);
            } else {
                alert(jqXHR.responseJSON.Message + ' (' + jqXHR.responseJSON.ErrorCode + ')');
            }
        });
    },

    _refreshListUI: function () {
        $('div.movieListBox .movieListRow').remove();
        for (var i = 0; i < this._movieList.length; i++) {
            var rowDiv = $('<div>').addClass('movieListRow').on('click', function () {
                window.moviesApiClient._showHideMovieList(this);
            });

            if (i % 2 == 0) {
                rowDiv.addClass('evenRow');
            }

            $('<div>').addClass('movieListRowTitle').text(this._movieList[i].Title).appendTo(rowDiv);
            $('<div>').addClass('movieListRowDesc').text(this._movieList[i].Description).appendTo(rowDiv);
            var actions = $('<div>').addClass('movieListRowAction');
            if (this.token) {
                var temp = $('<div>');
                $('<img>').attr('src', '/img/edit.png').appendTo(temp);
                temp.appendTo(actions);

                temp = $('<div>');
                temp.on('click', function (event) {
                    event.stopPropagation()
                    var movielist = $(this).parents('.movieListRow').data('movieList');
                    window.moviesApiClient.deleteMovieList(movielist.ID);
                });
                $('<img>').attr('src', '/img/del.png').appendTo(temp);
                temp.appendTo(actions);
            }

            actions.appendTo(rowDiv);
            rowDiv.data('movieList', this._movieList[i]);
            rowDiv.appendTo($('div.movieListBox'));
        }
    },
    _showHideMovieList: function (oBj) {
        oBj = $(oBj);
        if (oBj.hasClass('opened')) {
            oBj.removeClass('opened');
            oBj.css('height', '');
            $('.movieContainer', oBj).remove();
        } else {
            var _current = $(oBj).data('movieList');

            var moviesDiv = $('<div>').addClass('movieContainer');
            var moviesDivRow = $('<div>').addClass('movieContainerRowHeader');
            $('<div>').addClass('movieListRowTitle').text('Title').appendTo(moviesDivRow);
            $('<div>').addClass('movieListRowDesc').text('Description').appendTo(moviesDivRow);
            $('<div>').addClass('movieListRowAction').text('Release').appendTo(moviesDivRow);

            moviesDivRow.appendTo(moviesDiv);

            for (var i = 0; i < _current.movies.length; i++) {
                var moviesDivRow = $('<div>').addClass('movieContainerRow');
                $('<div>').addClass('movieListRowTitle').text(_current.movies[i].Title).appendTo(moviesDivRow);
                $('<div>').addClass('movieListRowDesc').text(_current.movies[i].Description).appendTo(moviesDivRow);
                $('<div>').addClass('movieListRowAction').text(_current.movies[i].RealeaseYear).appendTo(moviesDivRow);
                moviesDivRow.appendTo(moviesDiv);
            }
            moviesDiv.appendTo(oBj);
            oBj.addClass('opened');
            oBj.css('height', (25 * (_current.movies.length + 2)) + 'px');
        }
    },

    _parseJwt: function (token) {
        var base64Url = token.split('.')[1];
        var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload);
    }

});
window.moviesApiClient = new MoviesApiClient();