var moviesUI = {
    _dialog: null,
    _maxId: 1,
    _isEdit: false,
    userName: null,

    setLoggedId: function () {
        if (window.moviesApiClient.token != null) {
            $('.loginControl .textBoxContainer').css('display', 'none');
            $('.loginControl .loggedInContainer #loggedInUserName').text(this.userName);
            $('.loginControl .loggedInContainer').css('display', '');
            $('.movieListRowAction div').css('display', '');
        } else {
            $('.loginControl .textBoxContainer').css('display', '');
            $('.loginControl .loggedInContainer').css('display', 'none');
            $('.movieListRowAction div').css('display', 'none');
        }
    },

    refreshListUI: function (movieList) {
        $('div.movieListBox .movieListRow').remove();
        for (var i = 0; i < movieList.length; i++) {
            var rowDiv = $('<div>').addClass('movieListRow').on('click', function () {
                moviesUI._showHideMovieList(this);
            });

            if (i % 2 == 0) {
                rowDiv.addClass('evenRow');
            }

            $('<div>').addClass('movieListRowTitle').text(movieList[i].Title).appendTo(rowDiv);
            $('<div>').addClass('movieListRowDesc').text(movieList[i].Description).appendTo(rowDiv);
            var actions = $('<div>').addClass('movieListRowAction');
            if (window.moviesApiClient.token) {
                var temp = $('<div>');
                $('<img>').attr('src', '/img/edit.png').appendTo(temp);
                temp.on('click', function (event) {
                    event.stopPropagation()
                    var movielist = $(this).parents('.movieListRow').data('movieList');
                    moviesUI.showEditMovieList(movielist.ID);
                });
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
            rowDiv.data('movieList', movieList[i]);
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

    addMovie: function (oBj) {
        return this.clearInputs(this.setIDs($(oBj).parent().parent().clone())).appendTo($('#movieTable tbody'));
    },
    deleteMovie: function (oBj) {
        if ($('#movieTable tbody tr').length != 1) {
            $(oBj).parent().parent().remove();
        } else {
            this.clearInputs($(oBj).parent().parent());
        }
    },

    initDialog: function () {
        this._dialog = $("#editControlContainer").dialog({
            autoOpen: false,
            height: 800,
            width: 750,
            modal: true,
            buttons: {
                Save: function () {
                    var movieList = {
                        Title: $('#editControlContainer #Title').val(),
                        UserID: $('#editControlContainer #UserID').val(),
                        ID: $('#editControlContainer #ID').val(),
                        Description: $('#editControlContainer #Description').val(),
                        movies: []
                    };

                    $('#movieTable tbody tr').each(function () {
                        var row = $(this);
                        var movie = {
                            Title: $("[name$='Title']", row).val(),
                            ID: $("[name$='ID']", row).val(),
                            Description: $("[name$='Description']", row).val(),
                            RealeaseYear: $("[name$='RealeaseYear']", row).val(),
                        };

                        if (movie.Title != null || movie.Description != null || movie.ID != null || movie.RealeaseYear != null) {
                            movieList.movies.push(movie);
                        }
                    });

                    if (!moviesUI._isEdit) {
                        window.moviesApiClient.createMovieList(movieList);
                    } else {
                        window.moviesApiClient.editMovieList(movieList);
                    }
                    moviesUI._dialog.dialog("close");
                },
                Cancel: function () {
                    moviesUI._dialog.dialog("close");
                }
            },
            close: function () {
                $('.editControl form')[0].reset();
                $('#movieTable tbody tr').each(function () {
                    moviesUI.deleteMovie($('.deleteButton', this));
                });
            }
        });
    },

    showCreateMovieList: function () {
        this._isEdit = false;
        this._maxId = 1;
        this._dialog.dialog("open");
    },

    showEditMovieList: function (ID) {
        this._isEdit = true;
        this._maxId = 1;
        var movieList = window.moviesApiClient.getMovieList(ID);

        $('#editControlContainer #Title').val(movieList.Title);
        $('#editControlContainer #UserID').val(movieList.UserID);
        $('#editControlContainer #ID').val(movieList.ID);
        $('#editControlContainer #Description').val(movieList.Description);

        for (var i = 0; i < movieList.movies.length; i++) {
            var row = null;
            if (i == 0) {
                row = $('#movieTable tbody tr')[0];
            } else {
                row = moviesUI.addMovie($('#movieTable tbody tr .addButton')[0])[0];
            }

            $("[name$='Title']", row).val(movieList.movies[i].Title);
            $("[name$='ID']", row).val(movieList.movies[i].ID);
            $("[name$='Description']", row).val(movieList.movies[i].Description);
            $("[name$='RealeaseYear']", row).val(movieList.movies[i].RealeaseYear);
        }

        this._dialog.dialog("open");
    },

    setIDs: function (row) {
        this._maxId++;
        var _maxId = this._maxId;
        $(':input', row).each(function () {
            if ($(this).attr('id').endsWith('Description')) {
                $(this).attr('id', 'movies_' + _maxId + '__Description');
                $(this).attr('name', 'movies.[' + _maxId + '].Description');
            } else if ($(this).attr('id').endsWith('Title')) {
                $(this).attr('id', 'movies_' + _maxId + '__Title');
                $(this).attr('name', 'movies.[' + _maxId + '].Title');
            } else if ($(this).attr('id').endsWith('RealeaseYear')) {
                $(this).attr('id', 'movies_' + _maxId + '__RealeaseYear');
                $(this).attr('name', 'movies.[' + _maxId + '].RealeaseYear');
            } else {
                $(this).attr('id', 'movies_' + _maxId + '__ID');
                $(this).attr('name', 'movies.[' + _maxId + '].ID');
            }

        });
        return row;
    },

    clearInputs: function (row) {
        $(':input', row).val('');
        return row;
    }
};