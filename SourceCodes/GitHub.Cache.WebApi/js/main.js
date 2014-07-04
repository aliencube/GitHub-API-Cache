; "use strict";

(function ($) {
    $(document).ready(function () {
        getSha();
    });

    //  Gets the latest commit key.
    var getSha = function () {
        var url = "/api/ref/aliencube/GitHub-API-Cache/master";
        $.ajax({
                type: "GET",
                url: url,
                dataType: "json",
                headers: { "Authorization": "token 2650cba9ca98e349ee9aedec383329a39477950c" }
            })
            .done(function(data) {
                var sha = data.object.sha;

                $.each(pages, function (i, page) {
                    getMarkdown(page, sha);
                });
            });
    };

    var count = 0;
    // Gets the given markdown page.
    var getMarkdown = function (page, sha) {
        var url = gitcdn + "/" + sha + "/" + page.doc;
        $.ajax({
                type: "GET",
                url: url,
                dataType: "text"
            })
            .done(function(data) {
                data = marked(data);
                getContents(data);

                count++;
            });
    };

    // Gets the contents.
    var getContents = function (data) {
        $("#main-content").html(data);
    };
})(jQuery);
