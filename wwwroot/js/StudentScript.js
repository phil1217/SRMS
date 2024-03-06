
$.when($.ready).then(function () {

    var PageIndex = $("input#PageIndex");
    var PageSize = $("select#PageSize");
    var Query = $("input#Query");
    var tbody = $("tbody#TableBody");

    var q = $("input#q");

    GetProfile({
        type:"student",
        id: $.cookie("IdStudent")
    }).done(function (response) {
        $("#name").html(response.profile["name"]);
        $("#gender").html(response.profile["gender"]);
        $("#birthDate").html(response.profile["birthDate"]);
    });
    
    PageIndex.val(0);
    
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
        id: $.cookie("IdStudent")
    }).done(function (response) {
        
        if (!response.success) {
            alert(response.message);
            window.location.href = "/student/error";
        }

        DisplayContent(response);
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
            id: $.cookie("IdStudent")
        }).done(function (response) {

            if (!response.success) {
                alert(response.message);
                return;
            }
            DisplayContent(response);

        });
    });

    $("button#SignOut").click(function () {
        $.removeCookie("AuthStudent");
        $.removeCookie("IdStudent");
        window.location.href = "/user";
    });
    
    function Get(data) {
        
        return $.ajax({
            url: "/student/data/get/account",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthStudent")
            }
        });
    }

    function Find(data) {
        return $.ajax({
            url: "/student/data/find",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthStudent")
            }
        });
    }

    function GetProfile(data) {
        return $.ajax({
            url: "/student/data/get/profile",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthStudent")
            }
        });
    }

    function QueryRequest() {
        var data = { 
            obj:{
            index: PageIndex.val(),
            size: PageSize.val()
            },
            id: $.cookie("IdStudent")
};
        
        if (Query.val() != "") {
            data = {
                obj: {
                    page: data.obj,
                    query: Query.val()
                },
                id:$.cookie("IdStudent")
            };

            return Find(data);
        }

        return Get(data);
    }

    function DisplayContent(Response) {
        tbody.empty();
        
        $("form [name='StudentId']").val($.cookie("IdStudent"));
            
        DisplayMembers(Response.list);
        DisplayPage(Response.page);
            
    }

    function DisplayMembers(List) {
        
        List.forEach(function (record) {
            
            var tr = $('<tr>', {
                class: 'w-100 border-bottom m-0 p-0'
            }).append(
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
                $('<td>', {
                    class:'text-uppercase'
                }).text(record.remark)
            );
            

            tbody.append(tr);
            

        });

    }

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
  
    function DisplayPage(Page) {

        if (PageIndex.val() < 1)
            PageIndex.val(0);

        if (PageIndex.val() >= Page.count)
            PageIndex.val(Page.count-1);

        $('#page').html("page "+Page.index+" out of "+Page.count);
    }
    
});
  
   