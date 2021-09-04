var emp_role_idd;
var login_company_id;
var login_emp_id;
var is_manager;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        var _manager = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);
        if (_manager == "yes") {
            is_manager = 1;
        }
        else {
            is_manager = 0;
        }

        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
        BindTabMaster('ddltabmstr', login_company_id, 0);
        BindDepartmentList_except_all('ddldept', login_company_id, 0);

        // HaveDisplay = ISDisplayMenu("Display Company List");


        GetData(login_company_id);

        $("#dsdropdwon").hide();

        $("#btnupdate").hide();




        $("#ddlcompany").bind("change", function () {

            if ($(this).val() > 0) {
                GetData($(this).val());
                if ($.fn.DataTable.isDataTable('#tblquesmstr')) {
                    $('#tblquesmstr').DataTable().clear().draw();
                }
            }
            BindWorkingRoleByDeptID('ddlwrkrole', $(this).val(), 0, 0);
            BindTabMaster('ddltabmstr', $(this).val(), 0);
            BindDepartmentList_except_all('ddldept', $(this).val(), 0);


            $("#ddlanstype").val('0');
            $("#txtddlds").val('');
            $("#dsdropdwon").hide();
            $("#txtquestion").val('');
            $("#txtdesc").val('');

        });

        $("#ddldept").bind("change", function () {

            if ($("#ddlcompany").val() == "0") {
                messageBox("error", "Please select company");
                return false;
            }
            BindWorkingRoleByDeptID('ddlwrkrole', $("#ddlcompany").val(), $(this).val(), 0);




        });

        $("#ddlanstype").bind("change", function () {

            if ($(this).val() == "2") {
                $("#dsdropdwon").show();
            }
            else {
                $("#dsdropdwon").hide();
            }

        });

        $("#btnsave").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var wrk_roleid = $("#ddlwrkrole").val();
            var tab_id = $("#ddltabmstr").val();
            var ans_type = $("#ddlanstype").val();
            var question = $("#txtquestion").val();
            var description = $("#txtdesc").val();
            var ddldsval = $("#txtddlds").val();
            var dept_id = $("#ddldept").val();

            var is_error = false;
            var error_msg = "";

            if (company_id == "" || company_id == "0" || company_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select company</br>";
            }
            if (dept_id == "" || dept_id == "0" || dept_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select department</br>";
            }

            if (wrk_roleid == "0" || wrk_roleid == "" || wrk_roleid == null) {
                is_error = true;
                error_msg = error_msg + "Please select working role</br>";
            }
            if (tab_id == "" || tab_id == "0" || tab_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select tab name</br>";
            }

            if (question == "" || question == null) {
                is_error = true;
                error_msg = error_msg + "Please enter question</br>";
            }

            if (ans_type == "" || ans_type == "0") {
                is_error = true;
                error_msg = error_msg + "Please select answer type</br>";
            }

            if (ans_type == "2") {
                if (ddldsval == "" || ddldsval == "" || ddldsval == null) {
                    is_error = true;
                    error_msg = error_msg + "Please enter Data Source";
                }
            }
            else {
                ddldsval = "";
            }
            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }





            var mydata = {
                company_id: company_id,
                wrk_role_id: wrk_roleid,
                tab_id: tab_id,
                ques: question,
                ans_type: ans_type,
                ans_type_ddl: ddldsval,
                description: description,
                created_by: login_emp_id,
                dept_id: dept_id,
            }
            $("#loader").show();

            if (confirm("Do you want to save this ?")) {
                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + "apiePA/Save_QuestionMaster",
                    type: "POST",
                    contentType: "application/json",
                    dataType: "json",
                    data: JSON.stringify(mydata),
                    headers: headerss,
                    success: function (response) {
                        _GUID_New();
                        var statuscode = response.statusCode;
                        var msg = response.message;
                        if (statuscode == "0") {
                            BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                            BindTabMaster('ddltabmstr', login_company_id, 0);
                            BindDepartmentList_except_all('ddldept', login_company_id, 0);
                            GetData(login_company_id);
                            BindWorkingRoleByDeptID('ddlwrkrole', login_company_id, 0, 0);
                            $("#ddlanstype").val('0');
                            $("#txtddlds").val('');
                            $("#dsdropdwon").hide();
                            $("#txtquestion").val('');
                            $("#txtdesc").val('');
                            $("#loader").hide();
                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            $("#loader").hide();
                            messageBox("error", msg);
                        }
                    },
                    error: function (request, status, error) {
                        _GUID_New();
                        $('#loader').hide();
                        var error = "";
                        var errordata = JSON.parse(request.responseText);
                        try {
                            var i = 0;
                            while (Object.keys(errordata).length > i) {
                                var j = 0;
                                while (errordata[Object.keys(errordata)[i]].length > j) {
                                    error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                    j = j + 1;
                                }
                                i = i + 1;
                            }

                        } catch (err) { }
                        messageBox("error", error);

                    }

                });

            }
            else {
                $("#loader").hide();
                return false;
            }

        });


        $("#btnreset").bind("click", function () {
            $("#loader").show();
            location.reload();
            $("#loader").hide();
        });

        $("#btnupdate").bind("click", function () {

            var company_id = $("#ddlcompany").val();
            var wrk_roleid = $("#ddlwrkrole").val();
            var tab_id = $("#ddltabmstr").val();
            var ans_type = $("#ddlanstype").val();
            var question = $("#txtquestion").val();
            var description = $("#txtdesc").val();
            var ddldsval = $("#txtddlds").val();
            var dept_id = $("#ddldept").val();
            var is_error = false;
            var error_msg = "";

            if (company_id == "" || company_id == "0" || company_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select company</br>";
            }
            if (dept_id == "" || dept_id == "0" || dept_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select department</br>";
            }

            if (wrk_roleid == "0" || wrk_roleid == "" || wrk_roleid == null) {
                is_error = true;
                error_msg = error_msg + "Please select working role</br>";
            }
            if (tab_id == "" || tab_id == "0" || tab_id == null) {
                is_error = true;
                error_msg = error_msg + "Please select tab name</br>";
            }

            if (question == "" || question == null) {
                is_error = true;
                error_msg = error_msg + "Please enter question</br>";
            }

            if (ans_type == "" || ans_type == "0") {
                is_error = true;
                error_msg = error_msg + "Please select answer type</br>";
            }

            if (ans_type == "2") {
                if (ddldsval == "" || ddldsval == "" || ddldsval == null) {
                    is_error = true;
                    error_msg = error_msg + "Please enter Data Source";
                }
            }
            else {
                ddldsval = "";
            }

            if (is_error) {
                messageBox("error", error_msg);
                return false;
            }

            var mydata = {
                question_id: $("#hdnquesmstrid").val(),
                company_id: company_id,
                wrk_role_id: wrk_roleid,
                tab_id: tab_id,
                ques: question,
                ans_type: ans_type,
                ans_type_ddl: ddldsval,
                description: description,
                created_by: login_emp_id,
                dept_id: dept_id,
            }
            $("#loader").show();

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();


            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiePA/Update_QuestionMaster",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(mydata),
                headers: headerss,
                success: function (response) {
                    _GUID_New();
                    var statuscode = response.statusCode;
                    var msg = response.message;
                    if (statuscode == "0") {

                        BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                        BindTabMaster('ddltabmstr', login_company_id, 0);
                        BindDepartmentList_except_all('ddldept', login_company_id, 0);
                        GetData(0);

                        BindWorkingRoleByDeptID('ddlwrkrole', login_company_id, 0, 0);
                        $("#ddlanstype").val('0');
                        $("#txtddlds").val('');
                        $("#dsdropdwon").hide();
                        $("#txtquestion").val('');
                        $("#txtdesc").val('');
                        $("#hdnquesmstrid").val('');
                        //$("#loader").hide();
                        //messageBox("success", msg);
                        $("#btnupdate").hide();
                        $("#btnsave").show();
                        $("#loader").hide();
                        messageBox("success", msg);

                    }
                    else {
                        $("#loader").hide();
                        messageBox("error", msg);
                    }
                },
                error: function (request, status, error) {
                    _GUID_New();
                    $('#loader').hide();
                    var error = "";
                    var errordata = JSON.parse(request.responseText);
                    try {
                        var i = 0;
                        while (Object.keys(errordata).length > i) {
                            var j = 0;
                            while (errordata[Object.keys(errordata)[i]].length > j) {
                                error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                j = j + 1;
                            }
                            i = i + 1;
                        }

                    } catch (err) { }
                    messageBox("error", error);

                }

            });


        });

    }, 2000);// end timeout

});


function GetData(companyid) {
    if ($.fn.DataTable.isDataTable('#tblquesmstr')) {
        $('#tblquesmstr').DataTable().clear().draw();
    }
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_QuestionMaster/0/" + companyid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tblquesmstr").DataTable({
                "processing": true,//for show process bar
                "serverSide": false,//for process server side
                "orderMulti": false,//for disable multiple column at once
                "bDestroy": true,// for remove previous data
                "filter": true,//for enable search box
                "scrollX": 200,
                "aaData": res,
                "columnDefs": [],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department", "autoWidth": true },
                    { "data": "working_role_name", "name": "working_role_name", "title": "Working Role", "autoWidth": true },
                    { "data": "tab_name", "name": "tab_name", "title": "Tab Name", "autoWidth": true },
                    { "data": "ques", "name": "ques", "title": "Question", "autoWidth": true },
                    { "data": "anss_type", "name": "anss_type", "title": "Answer Type", "autoWidth": true },
                    {
                        "title": "Data Source", "autoWidth": true, "render": function (data, type, full, meta) {
                            return (full.ans_type == "2" && full.ans_type_ddl != "" && full.ans_type_ddl != null) ? full.ans_type_ddl : "-";
                        }
                    },
                    { "data": "description", "name": "description", "title": "Description", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true },
                    { "data": "modified_date", "name": "modified_date", "title": "Modified On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                            return '<a href="#" onclick=GetEditData(' + full.ques_mstr_id + ',' + full.company_id + ') style=" float: left;"><i class="fa fa-pencil-square-o"></i></a><a  onclick="DeleteQuestionMaster(' + full.ques_mstr_id + ',' + full.company_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                },// for S.No.
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
            });
        }
    });
}

function GetEditData(mstrid, compid) {
    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Get_QuestionMaster/" + mstrid + "/" + compid,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }

            BindAllEmp_Company('ddlcompany', login_emp_id, res.company_id);
            $("#ddlcompany").attr("disbaled", "disabled");
            BindDepartmentList_except_all('ddldept', res.company_id, res.dept_id);
            BindWorkingRoleByDeptID('ddlwrkrole', res.company_id, res.dept_id, res.wrk_role_id);
            BindTabMaster('ddltabmstr', res.company_id, res.tab_id);
            $("#ddlanstype").val(res.ans_type);
            if (res.ans_type == "2" && res.ans_type_ddl != "" && res.ans_type_ddl != null) {
                $("#dsdropdwon").show();
                $("#txtddlds").val(res.ans_type_ddl);
            }
            else {
                $("#dsdropdwon").hide();
            }
            $("#txtquestion").val(res.ques);
            $("#txtdesc").val(res.description);
            $("#hdnquesmstrid").val(res.ques_mstr_id);

            $("#btnsave").hide();
            $("#btnupdate").show();
            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}


function DeleteQuestionMaster(quesid, companyid) {

    $("#loader").show();
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiePA/Delete_QuestionMaster/" + quesid + "/" + companyid + "/" + login_emp_id,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            _GUID_New();
            if (response.statusCode == "0") {
                BindAllEmp_Company('ddlcompany', login_emp_id, login_company_id);
                BindTabMaster('ddltabmstr', login_company_id, 0);
                BindDepartmentList_except_all('ddldept', login_company_id, 0);
                GetData(login_company_id);
                BindWorkingRoleByDeptID('ddlwrkrole', login_company_id, 0, 0);
                $("#ddlanstype").val('0');
                $("#txtddlds").val('');
                $("#dsdropdwon").hide();
                $("#txtquestion").val('');
                $("#txtdesc").val('');
                $("#loader").hide();
                messageBox("success", response.message);
                return false;
            }
            else {
                $("#loader").hide();
                messageBox("error", response.message);
                return false;
            }

        },
        error: function (request, status, error) {
            _GUID_New();
            $('#loader').hide();
            var error = "";
            var errordata = JSON.parse(request.responseText);
            try {
                var i = 0;
                while (Object.keys(errordata).length > i) {
                    var j = 0;
                    while (errordata[Object.keys(errordata)[i]].length > j) {
                        error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                        j = j + 1;
                    }
                    i = i + 1;
                }

            } catch (err) { }
            messageBox("error", error);

        }


    });
}


