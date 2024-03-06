$.when($.ready).then(function () {
    
    checkToken().done(function (response) {
        
        if (response.success) {
            alert(response.message);

            window.location.href = "/student/home";
        }
    });

    $('form#SignIn').submit(function (event) {

        event.preventDefault();

        var data = $(this).serializeArray();

        var jsonData = {};

        data.forEach(function (field) {
            jsonData[field.name] = field.value;
        });

        authenticate(jsonData).done(function (response) {

            alert(response.message);
            
            if (response.success) {
               
                $.cookie("AuthStudent", response.token, { expires: 7 });
                $.cookie("IdStudent", response.id, { expires: 7 });

                window.location.href = "/student/home";
            }

        });
    });

    function authenticate(data) {
        return $.ajax({
            url: "/student/authenticate",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        });
    }

    function checkToken() {
        return $.ajax({
            url: "/student/check",
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'token': $.cookie("AuthStudent")
            }
        });
    }
    
});