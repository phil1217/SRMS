$.when($.ready).then(function () {
    
    checkToken().done(function (response) {
        
        if (response.success) {
            alert(response.message);

            window.location.href = "/faculty/home";
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
               
                $.cookie("AuthFaculty", response.token, { expires: 7 });
                $.cookie("IdFaculty", response.id, { expires: 7 });

                window.location.href = "/faculty/home";
            }

        });
    });

    function authenticate(data) {
        return $.ajax({
            url: "/faculty/authenticate",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data)
        });
    }

    function checkToken() {
        return $.ajax({
            url: "/faculty/check",
            type: 'POST',
            contentType: 'application/json',
            headers: {
                'token': $.cookie("AuthFaculty")
            }
        });
    }
    
});