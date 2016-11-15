$(document).ready(function() {

    console.log($(".like_button_input"));

    $(".like_button_input").one("click", function() {

        var name = $(this).closest(".imageContainer").find("img").attr("src");

        var image = { 'name': name, 'value': 1 };

        var likeNumberContainer = $(this).closest(".imageContainer").find("p[id^=like_number]");

        console.log(image.name);

        $.ajax({
            url: "/api/LikeSystem/Post",
            type: "POST",
            dataType: "json",
            data: image,
            success: function(data) {
                likeNumberContainer.text(data);
                console.log(data);

                console.log(likeNumberContainer.attr("class"));

                if (data >= 0) {
                    likeNumberContainer.removeClass("counter_number_red");
                    likeNumberContainer.addClass("counter_number_green");
                } else {
                    likeNumberContainer.removeClass("counter_number_green");
                    likeNumberContainer.addClass("counter_number_red");
                }
            }
        });
    });

    $(".dislike_button_input").one("click", function() {

        var name = $(this).closest(".imageContainer").find("img").attr("src");

        var image = { 'name': name, 'value': -1 };
        var likeNumberContainer = $(this).closest(".imageContainer").find("p[id^=like_number]"); //.attr('class');

        //console.log(image.name);

        $.ajax({
            url: "/api/LikeSystem/Post",
            type: "POST",
            dataType: "json",
            data: image,
            success: function(data) {
                likeNumberContainer.text(data);
                console.log(data);

                // console.log(likeNumberContainer.attr("class"));

                if (data > -1) {

                    likeNumberContainer.removeClass("counter_number_red");
                    likeNumberContainer.addClass("counter_number_green");
                } else {
                    likeNumberContainer.removeClass("counter_number_green");
                    likeNumberContainer.addClass("counter_number_red");
                }
            }
        });
    });
});