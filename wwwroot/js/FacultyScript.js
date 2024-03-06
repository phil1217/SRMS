
$.when($.ready).then(function () {

    var PageIndex = $("input#PageIndex");
    var PageSize = $("select#PageSize");
    var Query = $("input#Query");
    var tbody = $("tbody#TableBody");

    var q = $("input#q");

    GetProfile({
        type: "faculty",
        id: $.cookie("IdFaculty")
    }).done(function (response) {
        $("#name").html(response.profile["name"]);
        $("#gender").html(response.profile["gender"]);
        $("#birthDate").html(response.profile["birthDate"]);
       
    });

    PageIndex.val(0);

    tbody.on('click', 'a#Update', function () {
        var record = $(this).data('record');
        
        $.each(record, function (key, value) {
            $("form#Update input[name='" + key + "']").val(value);
            $("form#Update select[name='" + key + "']").val(value);
            $("form#Update textarea[name='" + key + "']").val(value);
        });

        $("div#UpdateView").show();
    });

    $('#studentId').focusout(function () {
        var value = $(this).val();

        GetProfile({
            type: "student",
            id: value
        }).done(function (response) {

            if (response.success) {
                $("#studentName").val(response.profile["name"]);
            } else {
                $("#studentName").val('');
                alert(response.message);
            }
        });
    });

    var record = null;

    tbody.on('click', 'a#Delete', function () {
        $("div#DeleteView").show();
        record = $(this).data('record');
    });

    PageSize.on("change", function () {

        QueryRequest().done(function (response) {
            DisplayContent(response);
        });

    });

    Get({
        obj: {
            index: 0,
            size: PageSize.val()
        },
        id: $.cookie("IdFaculty")
    }).done(function (response) {
        
        if (!response.success) {
            alert(response.message);
            window.location.href = "/faculty/error";
        }

        DisplayContent(response);
    });

    $("button#cancel_delete").click(function () {
        $("div#DeleteView").hide();
    });

    $("button#delete").click(function () {

        if (record == null)
            return;

        Delete(record).done(function (response) {

            alert(response.message);

            if (response.success) {
                $("div#DeleteView").hide();
                QueryRequest().done(function (response) {
                    DisplayContent(response);
                });

            }
        });
    });

    $("a#next").click(function () {

        PageIndex.val(parseInt(PageIndex.val()) + 1);

        QueryRequest().done(function (response) {
            if (!response.success) {
                alert(response.message);
                return;
            }
            DisplayContent(response);
        });

    });

    $("a#prev").click(function () {

        PageIndex.val(parseInt(PageIndex.val()) - 1);
        QueryRequest().done(function (response) {
            if (!response.success) {
                alert(response.message);
                return;
            }
            DisplayContent(response);
        });
    });

    $("a#Refresh").click(function () {

        Query.val("");
        q.val("");

        QueryRequest().done(function (response) {
            if (!response.success) {
                alert(response.message);
                return;
            }
            DisplayContent(response);
        });

    });

    $("form#Update").submit(function (event) {

        event.preventDefault();

        $(this).find("#studentId").prop("disabled", false);
        $(this).find("#studentName").prop("disabled", false);

        var identity = $(this).find("#identity").serializeArray();
        var record = $(this).find("#record").serializeArray();

        $(this).find("#studentId").prop("disabled", true);
        $(this).find("#studentName").prop("disabled", true);

        var jsonData = {};

        identity.forEach(function (field) {

            jsonData[field.name] = field.value;

        });

        record.forEach(function (field) {

            jsonData[field.name] = field.value;

        });

        Update(jsonData).done(function (response) {

            alert(response.message);

            if (response.success) {
                $("div#UpdateView").hide();

                QueryRequest().done(function (response) {
                    if (!response.success) {
                        alert(response.message);
                        return;
                    }
                    DisplayContent(response);
                });
            }


        });

    });

    $("form#Update").find("#cancel").click(function () {
        $("div#UpdateView").hide();
    });

    $("form#Add").submit(function (event) {

        event.preventDefault();

        var form = $(this);

        $('#studentName').prop('disabled', false);
        
        var data = form.serializeArray();

        $('#studentName').prop('disabled', true);

        var jsonData = {};
        
        data.forEach(function (field) {
            jsonData[field.name] = field.value;
        });
        
        Add(jsonData).done(function (response) {

            alert(response.message);

            if (response.success) {

                //reset form
                form.find('input:not([type="submit"])').val('');
                form.find('select').val('student');

                PageIndex.val(0);
                Query.val("");

                QueryRequest().done(function (response) {
                    if (!response.success) {
                        alert(response.message);
                        return;
                    }

                    DisplayContent(response);
                });
            }

        });
        
    });

    $("a#search").click(function () {
        Query.val(q.val());

        Find({
            obj:{
                query: q.val(),
                page: {
                    index: PageIndex.val(),
                    size: PageSize.val()
                }
            },
            id: $.cookie("IdFaculty")
        }).done(function (response) {

            if (!response.success) {
                alert(response.message);
                return;
            }
            DisplayContent(response);

        });
    });

    $("button#SignOut").click(function () {
        $.removeCookie("AuthFaculty");
        window.location.href = "/user";
    });

    function ordinal(n) {
        switch (n) {
            case 1:
                return "1st";
            case 2:
                return "2nd";
            case 3:
                return "3rd";
            default:
                return "";
        }
    }

    function Update(data) {
        return $.ajax({
            url: "/faculty/data/update",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthFaculty")
            }
        });
    }

    function Add(data) {
        
        return $.ajax({
            url: "/faculty/data/add",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthFaculty")
            }
        });
    }

    function Get(data) {
        
        return $.ajax({
            url: "/faculty/data/get/account",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthFaculty")
            }
        });
    }

    function GetProfile(data) {

        return $.ajax({
            url: "/faculty/data/get/profile",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthFaculty")
            }
        });
    }

    function Find(data) {
        return $.ajax({
            url: "/faculty/data/find",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthFaculty")
            }
        });
    }

    function Delete(data) {
        return $.ajax({
            url: "/faculty/data/delete",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthFaculty")
            }
        });
    }

    function QueryRequest() {
        var data = { 
            obj:{
            index: PageIndex.val(),
            size: PageSize.val()
            },
            id: $.cookie("IdFaculty")
};
        
        if (Query.val() != "") {
            data = {
                obj: {
                    page: data.obj,
                    query: Query.val()
                },
                id:$.cookie("IdFaculty")
            };

            return Find(data);
        }

        return Get(data);
    }

    function DisplayContent(Response) {
        tbody.empty();
        
        $("form [name='FacultyId']").val($.cookie("IdFaculty"));
            
        DisplayMembers(Response.list);
        DisplayPage(Response.page);
            
    }

    function DisplayMembers(List) {
        
        List.forEach(function (record) {

            var $trash = $('<i>').append(
                $('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="white" viewBox="0 0 16 16">'
                    + '<path d="M6.5 1h3a.5.5 0 0 1 .5.5v1H6v-1a.5.5 0 0 1 .5-.5M11 2.5v-1A1.5 1.5 0 0 0 9.5 0h-3A1.5 1.5 0 0 0 5 1.5v1H1.5a.5.5 0 0 0 0 1h.538l.853 10.66A2 2 0 0 0 4.885 16h6.23a2 2 0 0 0 1.994-1.84l.853-10.66h.538a.5.5 0 0 0 0-1zm1.958 1-.846 10.58a1 1 0 0 1-.997.92h-6.23a1 1 0 0 1-.997-.92L3.042 3.5zm-7.487 1a.5.5 0 0 1 .528.47l.5 8.5a.5.5 0 0 1-.998.06L5 5.03a.5.5 0 0 1 .47-.53Zm5.058 0a.5.5 0 0 1 .47.53l-.5 8.5a.5.5 0 1 1-.998-.06l.5-8.5a.5.5 0 0 1 .528-.47M8 4.5a.5.5 0 0 1 .5.5v8.5a.5.5 0 0 1-1 0V5a.5.5 0 0 1 .5-.5" />'
                    + '</svg>')
            );

            var $pencilSquare = $('<i>').append(
                $('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="white" viewBox="0 0 16 16">'
                    + '<path d="M15.502 1.94a.5.5 0 0 1 0 .706L14.459 3.69l-2-2L13.502.646a.5.5 0 0 1 .707 0l1.293 1.293zm-1.75 2.456-2-2L4.939 9.21a.5.5 0 0 0-.121.196l-.805 2.414a.25.25 0 0 0 .316.316l2.414-.805a.5.5 0 0 0 .196-.12l6.813-6.814z"/>'
                    + '<path fill-rule="evenodd" d="M1 13.5A1.5 1.5 0 0 0 2.5 15h11a1.5 1.5 0 0 0 1.5-1.5v-6a.5.5 0 0 0-1 0v6a.5.5 0 0 1-.5.5h-11a.5.5 0 0 1-.5-.5v-11a.5.5 0 0 1 .5-.5H9a.5.5 0 0 0 0-1H2.5A1.5 1.5 0 0 0 1 2.5z"/>'
                    + '</svg>')
            );
            
            var tr = $('<tr>', {
                class: 'w-100 border-bottom m-0 p-0'
            }).append(
                $('<td>', {
                   
                }).text(record.studentId),
                $('<td>', {

                }).text(record.name),
                $('<td>', {

                }).text(record.course),
                $('<td>', {

                }).text(record.description),

                $('<td>', {

                }).text(record.grade),

                $('<td>', {

                }).text(ordinal(record.semester)),

                $('<td>', {

                }).text(record.unit),

                $('<td>').append(
                    $('<div>', {
                        class:'vh-center w-100'
                    }).append(
                    $('<a>', {
                        id: 'Update',
                        data: { record: record },
                        class: 'p-1 btn-blue',
                        html: $($pencilSquare)
                    }), $('<div>', {
                        class:'p-1'
                    }),
                    $('<a>', {
                        id: 'Delete',
                        data: { record: record },
                        class: 'p-1 btn-red',
                        html: $($trash)
                    })
                ))

            );
            

            tbody.append(tr);
            

        });

    }
  
    function DisplayPage(Page) {

        if (PageIndex.val() < 1)
            PageIndex.val(0);

        if (PageIndex.val() >= Page.count)
            PageIndex.val(Page.count-1);

        $('#page').html("page "+Page.index+" out of "+Page.count);
    }
    
});
  
   