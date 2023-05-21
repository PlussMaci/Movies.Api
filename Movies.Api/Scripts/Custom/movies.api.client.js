var MoviesApiClient = Class.extend({
    baseUrl: null,
    token: null,
    userID: null,

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
                moviesUI.userName = payload.name;

                moviesUI.setLoggedId();
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
                moviesUI.refreshListUI(window.moviesApiClient._movieList);
            }
        });
    },
    getMovieList: function (ID) {
        return this._callApi({
            method: 'GET',
            async: false,
            url: 'api/v1.0/movieLists/' + ID
        }).responseJSON;
    },
    createMovieList: function (movieList) {
        this._callApi({
            method: 'POST',
            url: 'api/v1.0/movieLists',
            data: movieList,
            ok: function () {
                window.moviesApiClient.searchMovieList();
            }
        });
    },
    editMovieList: function (movieList) {
        this._callApi({
            method: 'PUT',
            url: 'api/v1.0/movieLists',
            data: movieList,
            ok: function () {
                window.moviesApiClient.searchMovieList();
            }
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
                    moviesUI.refreshListUI(window.moviesApiClient._movieList);
                }
            });
        }
    },
    deleteMovieList: function (ID) {
        this._callApi({
            method: 'DELETE',
            url: 'api/v1.0/movieLists/' + ID,
            ok: function () {
                window.moviesApiClient.searchMovieList();
            }
        });
    },

    _callApi: function (options) {
        options = $.extend({
            token: this.token,
            async: true,
            ok: function (result) { }
        }, options);
        var result = $.ajax({
            method: options.method,
            async: options.async,
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

        return result;
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