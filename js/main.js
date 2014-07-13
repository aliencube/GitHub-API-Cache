; "use strict";

(function ($) {
    $(document).ready(function () {
        getSha();
    });

    //  Gets the latest commit key.
    var getSha = function () {
        var url = gitHubApiCacheUrl + "/repos/aliencube/GitHub-API-Cache/git/refs/heads/master";
        $.ajax({
                type: "GET",
                url: url,
                dataType: "json",
                headers: { "Authorization": "token " + authKey }
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
                getProgressbar((count / pages.length) * 100);
            });
    };

    // Gets the contents.
    var getContents = function (data) {
        $("#main-content").html(data);
    };

    // Gets the progress bar.
    var getProgressbar = function (progress) {
        $(".progress-bar").attr("aria-valuenow", progress).css("width", progress + "%");
        if (progress < 100) {
            $("#main-content").hide();
            $("#progress-bar").show();
        } else {
            $("#progress-bar").slideUp(1000, function () {
                $("#main-content").slideDown(2000);
            });
        }
    };
})(jQuery);
