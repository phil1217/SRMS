
$.when($.ready).then(function () {

    var PageIndex = $("input#PageIndex");
    var PageSize = $("select#PageSize");
    var Query = $("input#Query");
    var tbody = $("tbody#TableBody");

    var q = $("input#q");

    PageIndex.val(0);

    tbody.on('click', 'a#Update', function () {

        var account = $(this).data('member');

        $.each(account, function (key, value) {
            $("form#Update fieldset[name='account'] input[name='" + key + "']").val(value);
            $("form#Update fieldset[name='account'] select[name='" + key + "']").val(value);
        });

        GetProfile({
            type: account['UserType'] || account['userType'],
            id: account['UserId'] || account['userId']
        }).done(function (response) {
            
            if (response.success) {
                
                $.each(response.profile, function (key, value) {

                    if (key === 'gender') {

                        $("form#Update fieldset[name='profile'] input[name='" + key + "']#" + value).prop('checked', true);

                    } else {
                        $("form#Update fieldset[name='profile'] input[name='" + key + "']").val(value);

                        $("form#Update fieldset[name='profile'] select[name='" + key + "']").val(value);
                    }
                    
                });
                
                $("div#UpdateView").show();

            }

        });

    });

    var member = null;

    tbody.on('click', 'a#Delete', function () {
        $("div#DeleteView").show();
        member = $(this).data('member');
    });

    PageSize.on("change", function () {

        QueryRequest().done(function (response) {
            DisplayContent(response);
        });

    });

    GetAccount({
        index: 0,
        size: PageSize.val()
    }).done(function (response) {

        if (!response.success) {
            alert(response.message);
            window.location.href = "/admin/error";
        }

        DisplayContent(response);
    });

    $("button#cancel_delete").click(function () {
        $("div#DeleteView").hide();
    });

    $("button#delete").click(function () {

        if (member == null)
            return;

        Delete(member).done(function (response) {

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
        Refresh();
    });

    $("form#Update").submit(function (event) {

        event.preventDefault();

        var jsonData = InputToJSON($(this));
        
        Update(jsonData).done(function (response) {
            

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

        var jsonData = InputToJSON(form);
        
        Add(jsonData).done(function (response) {

            alert(response.message);

            if (response.success) {
                
                Refresh();

                //reset form
                form.find('input:not([type="submit"]):not([type="radio"])').val('');
                form.find("input[type='radio']").prop('checked', false);
                form.find('select').val('student');

            }

        });
    });

    $("a#search").click(function () {
        Query.val(q.val());

        Find({
            query: q.val(),
            page: {
                index: PageIndex.val(),
                size: PageSize.val()
            }
        }).done(function (response) {

            if (!response.success) {
                alert(response.message);
                return;
            }
            DisplayContent(response);

        });
    });

    $("button#SignOut").click(function () {
        $.removeCookie("AuthAdmin");
        window.location.href = "/user";
    });

    function Refresh() {

        Query.val("");
        q.val("");

        QueryRequest().done(function (response) {
            if (!response.success) {
                alert(response.message);
                return;
            }
            DisplayContent(response);
        });

    }
    function InputToJSON(form) {
        var account = {};
        var profile = {};

        var dataAccount = $(form).find("fieldset[name='account']").serializeArray();
        var dataProfile = $(form).find("fieldset[name='profile']").serializeArray();

        dataAccount.forEach(function (field) {
            account[field.name] = field.value;
        });
        
        dataProfile.forEach(function (field) {
            profile[field.name] = field.value;
        });

        account['Name'] = profile['Name'] || profile['name'];
        profile['AcadMemberId'] = account['UserId'] || account['userId'];
        profile['AcadMemberType'] = account['UserType'] || account['userType'];
        
        var json = {
            account: account,
            profile: profile
        };

        return json;
    }
    
    function Update(data) {
        return $.ajax({
            url: "/admin/data/update",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthAdmin")
            }
        });
    }

    function Add(data) {
        return $.ajax({
            url: "/admin/data/add",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthAdmin")
            }
        });
    }
    
    function GetAccount(data) {
        return $.ajax({
            url: "/admin/data/get/account",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthAdmin")
            }
        });
    }

    function GetProfile(data) {
        return $.ajax({
            url: "/admin/data/get/profile",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthAdmin")
            }
        });
    }

    function Find(data) {
        return $.ajax({
            url: "/admin/data/find",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthAdmin")
            }
        });
    }

    function Delete(data) {
        return $.ajax({
            url: "/admin/data/delete",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            headers: {
                'token': $.cookie("AuthAdmin")
            }
        });
    }

    function QueryRequest() {
        var data = {
            index: PageIndex.val(),
            size: PageSize.val()
        };
        
        if (Query.val() != "") {
            data = {
                page: data,
                query: Query.val()
            };

            return Find(data);
        }

        return GetAccount(data);
    }

    function DisplayContent(Response) {
       tbody.empty();
            
        DisplayMembers(Response.memberList);
        DisplayPage(Response.page);
            
    }

    function DisplayMembers(MemberList) {
        
        MemberList.forEach(function (member) {

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

            var $eye = $('<i>', {
                id: 'eye'
            }).append(
                $('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye" viewBox="0 0 16 16">'
                    + '<path d = "M16 8s-3-5.5-8-5.5S0 8 0 8s3 5.5 8 5.5S16 8 16 8M1.173 8a13 13 0 0 1 1.66-2.043C4.12 4.668 5.88 3.5 8 3.5s3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755C11.879 11.332 10.119 12.5 8 12.5s-3.879-1.168-5.168-2.457A13 13 0 0 1 1.172 8z" />'
                    + '<path d="M8 5.5a2.5 2.5 0 1 0 0 5 2.5 2.5 0 0 0 0-5M4.5 8a3.5 3.5 0 1 1 7 0 3.5 3.5 0 0 1-7 0" />'
                    + '</svg> ')
            );

            var $eyeSlash = $('<i>', {
                id: 'eyeSlash'
            }).append(
                $('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-slash" viewBox="0 0 16 16">'
                    + '<path d = "M13.359 11.238C15.06 9.72 16 8 16 8s-3-5.5-8-5.5a7 7 0 0 0-2.79.588l.77.771A6 6 0 0 1 8 3.5c2.12 0 3.879 1.168 5.168 2.457A13 13 0 0 1 14.828 8q-.086.13-.195.288c-.335.48-.83 1.12-1.465 1.755q-.247.248-.517.486z" />'
                    + '<path d="M11.297 9.176a3.5 3.5 0 0 0-4.474-4.474l.823.823a2.5 2.5 0 0 1 2.829 2.829zm-2.943 1.299.822.822a3.5 3.5 0 0 1-4.474-4.474l.823.823a2.5 2.5 0 0 0 2.829 2.829"/>'
                    + '<path d="M3.35 5.47q-.27.24-.518.487A13 13 0 0 0 1.172 8l.195.288c.335.48.83 1.12 1.465 1.755C4.121 11.332 5.881 12.5 8 12.5c.716 0 1.39-.133 2.02-.36l.77.772A7 7 0 0 1 8 13.5C3 13.5 0 8 0 8s.939-1.721 2.641-3.238l.708.709zm10.296 8.884-12-12 .708-.708 12 12z"/>'
                    + '</svg>')
            );

            var $iconContainer = $('<div>', {
                class: 'position-absolute top-0 start-0 d-flex align-items-center justify-content-end w-100 h-100',
                style:'line-height:0;'
            });

            var $passwordContainer = $('<div>', {
                class: 'text-center w-100',
                text: '*'.repeat(member.password.length)
            });

            $iconContainer.append($eye, $eyeSlash);

            $eyeSlash.hide();

            $iconContainer.find('#eyeSlash').click(function () {
                $eye.show();
                $passwordContainer.text('*'.repeat(member.password.length));
                $eyeSlash.hide();
            });

            $iconContainer.find('#eye').click(function () {
                $eyeSlash.show();
                $passwordContainer.text(member.password);
                $eye.hide();
            });

            var tr = $('<tr>', {
                class: 'w-100 border-bottom m-0 p-0'
            }).append(
                $('<td>', {
                   
                }).text(member.userType),
                $('<td>', {

                }).text(member.userId),
                $('<td>', {

                }).text(member.name),
                $('<td>', {

                }).text(member.userName),

                $('<td>', {

                }).append(
                    $('<div>', {
                        class: 'vh-center h-100 w-100'
                    }).append(
                        $('<div>', {
                            class: 'position-relative w-100'
                        }).append(
                            $passwordContainer,
                            $iconContainer
                        ))
                ),

                $('<td>').append(
                    $('<div>', {
                        class:'vh-center w-100'
                    }).append(
                    $('<a>', {
                        id: 'Update',
                        data: { member: member },
                        class: 'p-1 btn-blue',
                        html: $($pencilSquare)
                    }), $('<div>', {
                        class:'p-1'
                    }),
                    $('<a>', {
                        id: 'Delete',
                        data: { member: member },
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
  
   