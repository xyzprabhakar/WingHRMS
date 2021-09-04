var company_id;
var employee_id;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        employee_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindCompanyList('ddlCompany', company_id);

        // BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', company_id, employee_id);

        BindEmployeeCodee('ddlEmployeeCode', company_id, employee_id);
        Get_Details_Changes(employee_id);
        GetData(employee_id);
        BindRelationType('ddlrelation', 0);

        $('#btnUpdateFamilySection').hide();
        $('#btnSaveFamilySection').show();
        $("#divnomineepercentage").hide();

        $('#nominee_percentage').keyup(function () {
            if ($(this).val() > 100) {
                alert("No numbers above 100");
                $(this).val('100');
            }
        });

        $('#is_nominee').bind("change", function () {
            if ($(this).val() == "1") // yes
            {
                $("#divnomineepercentage").show();
            }
            else {
                $("#divnomineepercentage").hide();
            }
        });


    }, 2000);// end timeout

});

//--------bind data in jquery data table
function GetData(employee_id) {
    //// debugger;;
    $('#loader').show();

    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetFamilySection/' + employee_id,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            //// debugger;;
            $('#loader').hide();
            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                return false;
            }
            $("#tblFamily").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [6],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [4],
                            render: function (data, type, row) {


                                return data == 1 ? "Male" : data == 2 ? "Female" : data == 3 ? "Other" : "";
                            }
                        },
                        {
                            targets: [7],
                            "class": "text-center"
                        }
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name As Per Aadhar Card", "autoWidth": true },
                    { "data": "name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name", "autoWidth": true },
                    { "data": "relation", "name": "relation", "title": "Relation", "autoWidth": true },
                    { "data": "gender", "name": "gender", "title": "Gender", "autoWidth": true },
                    { "data": "occupation", "name": "occupation", "title": "Occupation", "autoWidth": true },

                    { "data": "date_of_birth", "name": "date_of_birth", "title": "Date of Birth", "autoWidth": true },
                    // { "data": "remark", "name": "passing_year", "title": "Remark", "remark": true },
                    // { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.emp_family_section_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    }
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            $('#loader').hide();

            alert(error);
        }
    });

}


function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));


}


//Save Employee Qualification Details
$('#btnSaveFamilySection').bind("click", function () {

    //validation
    //Validation();

    if (Validation() != false) {
        var employee_id = $('#ddlEmployeeCode :selected').val();
        //if (localStorage.getItem("emp_role_id") == "2") //Admin
        //{
        //    employee_id = localStorage.getItem("emp_id");
        //}

        var relation = $("#ddlrelation").val();
        var occupation = $("#occupation").val();
        var name_as_per_aadhar_card = $("#name_as_per_aadhar_card").val();
        var date_of_birth = $("#date_of_birth").val();
        var gender = $("#gender").val();
        var dependent = $("#dependent").val();
        var remark = $("#remark").val();
        var document_image = ""; //$("#HFb64document_image").val();
        var is_nominee = $("#is_nominee").val();
        var nominee_percentage = $("#nominee_percentage").val();
        var aadhar_card_no = $("#aadhar_card_number").val();

        if (nominee_percentage == '') {
            nominee_percentage = 0;
        }

        $('#loader').show();
        var myData = {
            'employee_id': employee_id,
            'relation': relation,
            'occupation': occupation,
            'name_as_per_aadhar_card': name_as_per_aadhar_card,
            'date_of_birth': date_of_birth,
            'gender': gender,
            'dependent': dependent,
            'remark': remark,
            'document_image': document_image,
            'is_nominee': is_nominee,
            'nominee_percentage': nominee_percentage,
            'aadhar_card_no': aadhar_card_no,
        };


        console.log(JSON.stringify(myData));
        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        // Save
        $.ajax({
            url: localStorage.getItem("ApiUrl") + '/apiEmployee/FamilySection',
            type: "POST",
            data: JSON.stringify(myData),
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {

                var statuscode = data.statusCode;
                var Msg = data.message;
                $('#loader').hide();
                _GUID_New();
                //if data save
                if (statuscode == "0") {
                    reset_all();
                    GetData(employee_id);
                    messageBox("success", Msg);
                    return false;
                }
                else {
                    messageBox("error", Msg);
                    return false;
                    // $('#loader').hide();
                }
            },
            error: function (request, status, error) {
                $('#loader').hide();
                _GUID_New();
                var error = "";
                var errordata = JSON.parse(request.responseText);
                try {
                    var i = 0;
                    while (Object.keys(errordata).length > i) {
                        var j = 0;
                        while (errordata[Object.keys(errordata)[i]].length > j) {
                            error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                            j = j + 1;
                        }
                        i = i + 1;
                    }

                } catch (err) { }
                messageBox("error", error);

            }

        });

    }

});

$('#btnUpdateFamilySection').bind("click", function () {
    $('#loader').show();

    var id = $("#hdnemp_family_section_id").val();


    if (Validation() != false) {


        var employee_id = $('#ddlEmployeeCode :selected').val();

        var relation = $("#ddlrelation").val();
        var occupation = $("#occupation").val();
        var name_as_per_aadhar_card = $("#name_as_per_aadhar_card").val();
        var date_of_birth = $("#date_of_birth").val();
        var gender = $("#gender").val();
        var dependent = $("#dependent").val();
        var remark = $("#remark").val();
        var is_nominee = $("#is_nominee").val();
        var nominee_percentage = "";
        var aadhar_card_no = $("#aadhar_card_number").val();

        if (is_nominee == "1")//Yes
        {
            nominee_percentage = $("#nominee_percentage").val();
        }
        else {
            nominee_percentage = 0;
        }

        var document_image = "";


        var myData = {
            'employee_id': employee_id,
            'relation': relation,
            'occupation': occupation,
            'name_as_per_aadhar_card': name_as_per_aadhar_card,
            'date_of_birth': date_of_birth,
            'gender': gender,
            'dependent': dependent,
            'remark': remark,
            'document_image': document_image,
            'is_nominee': is_nominee,
            'nominee_percentage': nominee_percentage,
            'aadhar_card_no': aadhar_card_no,
        };


        console.log(JSON.stringify(myData));
        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        // Save
        $.ajax({
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/UpdateFamilySection/' + id,
            type: "POST",
            data: JSON.stringify(myData),
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {

                var res = data;

                var statuscode = data.statusCode;
                var Msg = data.message;
                $('#loader').hide();
                _GUID_New();
                //if data save
                if (statuscode == "0") {
                    reset_all();
                    GetData(employee_id);
                    messageBox("success", Msg);
                    return false;
                }
                else {
                    messageBox("error", Msg);
                    return false;
                }
            },
            error: function (request, status, error) {
                $('#loader').hide();
                _GUID_New();
                var error = "";
                var errordata = JSON.parse(request.responseText);
                try {
                    var i = 0;
                    while (Object.keys(errordata).length > i) {
                        var j = 0;
                        while (errordata[Object.keys(errordata)[i]].length > j) {
                            error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j] + "</br>";
                            j = j + 1;
                        }
                        i = i + 1;
                    }

                } catch (err) { }
                messageBox("error", error);

            }
        });


    }

    $('#loader').hide();
});

function GetEditData(emp_family_section_id) {
    $('#loader').show();
    //// debugger;;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEditFamilySection/' + emp_family_section_id,
        //data: myData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            var data = res;


            if (data.statusCode != undefined) {
                messageBox("info", data.message);
                reset_all();
                $('#loader').hide();
                return false;
            }

            $("#hdnemp_family_section_id").val(data.emp_family_section_id);

            //BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, data[0].employee_id);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $("#ddlCompany").val(), data.employee_id);

            $("#ddlEmployeeCode").val(data.employee_id);
            //// debugger;;
            BindRelationType('ddlrelation', data.relation);
            $("#ddlrelation").val(data.relation);
            // $("#relation").val(data[0].relation);
            $("#occupation").val(data.occupation);
            $("#name_as_per_aadhar_card").val(data.name_as_per_aadhar_card);
            $("#date_of_birth").val(GetDateFormatddMMyyyy(new Date(data.date_of_birth)));
            var adsfasd = GetDateFormatddMMyyyy(new Date(data.date_of_birth));
            $("#gender").val(data.gender);
            $("#dependent").val(data.dependent);
            $("#remark").val(data.remark);
            $("#is_nominee").val(data.is_nominee);
            $("#hdndocument_image").val(res.document_image);
            if (data.is_nominee == 1 && data.nominee_percentage != null) {
                $("#divnomineepercentage").show();
                $("#nominee_percentage").val(data.nominee_percentage);
            }
            else {
                $("#divnomineepercentage").hide();
            }

            $("#aadhar_card_number").val(data.aadhar_card_no);

            $('#btnUpdateFamilySection').show();
            $('#btnSaveFamilySection').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            $('#loader').hide();
            messageBox("error", err.responseText);
        }
    });
}




function Validation() {
    $('#loader').show();
    var isvalid = false;

    var ddlcompany = $("#ddlCompany").val();
    var ddlEmployeeCode = $('#ddlEmployeeCode :selected').val();
    var aadhar_card_no = $("#aadhar_card_number").val();
    //var HaveDisplay = ISDisplayMenu("Display Company List");

    //if (HaveDisplay == 0) {
    //    ddlcompany = default_company;
    //   // ddlEmployeeCode = localStorage.getItem("emp_id");
    //}

    var relation = $("#ddlrelation").val();
    var name_as_per_aadhar_card = $("#name_as_per_aadhar_card").val();
    var date_of_birth = $("#date_of_birth").val();

    if (ddlcompany == 0 || ddlcompany == "" || ddlcompany == null) {
        messageBox("error", "Please select company...!</br>");
        $('#loader').hide();
        isvalid = false;
    }
    else if (ddlEmployeeCode == '' || ddlEmployeeCode == 0) {
        messageBox("error", "Please select employee code...!</br>");
        $('#loader').hide();
        isvalid = false;
    }
    else if (relation == 0 || relation == null) {
        messageBox("error", "Relation is required field...!</br>");
        $('#loader').hide();
        isvalid = false;
    }
    else if (name_as_per_aadhar_card == '') {
        messageBox("error", "Name As Per Aadhar Card is required field...!</br>");
        $('#loader').hide();
        isvalid = false;
    }

    else if (date_of_birth == '') {
        messageBox("error", "Date of birth is required field...!</br>");
        $('#loader').hide();
        isvalid = false;
    }
    else if ($("#is_nominee").val() == "1" && $("#nominee_percentage").val() == 0) {
        messageBox("error", "Nominee Percentange Cannot be 0 if Nominee is 'Yes'</br>");
        $('#loader').hide();
        isvalid = false;

    }
    else if ($("#gender").val() == "0" || $("#gender").val() == null || $("#gender").val() == "") {
        messageBox("error", "Please select gender'</br>");
        $('#loader').hide();
        isvalid = false;
    }
    else if (aadhar_card_no == '' || aadhar_card_no == null || aadhar_card_no == 0) {
        messageBox("error", "Please enter Aadhar card no. </br>");
        $('#loader').hide();
        isvalid = false;
    }
    else {
        isvalid = true
    }
    $('#loader').hide();
    return isvalid;
}

function reset_all() {
    $("#occupation").val('');
    $("#name_as_per_aadhar_card").val('');
    $("#aadhar_card_number").val('');
    $("#date_of_birth").val('');
    $("#gender").val('0');
    $("#dependent").val('0');
    $("#remark").val('');
    $("#is_nominee").val('0');
    $("#nominee_percentage").val('');
    if ($.fn.DataTable.isDataTable('#tblFamily')) {
        $('#tblFamily').DataTable().clear().draw();
    }
    BindRelationType('ddlrelation', 0);

}

function Get_Details_Changes(employee_id) {
    //debugger;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/Employee_family_changes/' + employee_id,
        //data: myData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            var data = res;
            debugger;
            //if (data.statusCode != undefined) {
            //    messageBox("info", data.message);
            //    $('#loader').hide();
            //    return false;
            //}
            if (data != null && data.relation != null && data.relation != "") {
                $("#lbl_relation").css("color", "red");
            }
            else {
                $("#lbl_relation").css("color", "#6c757d");
            }

            if (data != null && data.occupation != null && data.occupation != "") {
                $("#lbl_occupation").css("color", "red");
            }
            else {
                $("#lbl_occupation").css("color", "#6c757d");
            }
            if (data != null && data.name_as_per_aadhar_card != null && data.name_as_per_aadhar_card != "") {
                $("#lbl_name").css("color", "red");
            }
            else {
                $("#lbl_name").css("color", "#6c757d");
            }

            if (data != null && data.aadhar_card_no != null && data.aadhar_card_no != "") {
                $("#lbl_aadhar").css("color", "red");
            }
            else {
                $("#lbl_aadhar").css("color", "#6c757d");
            }

            if (data != null && data.date_of_birth != null && data.date_of_birth != "") {
                $("#lbldob").css("color", "red");
            }
            else {
                $("#lbldob").css("color", "#6c757d");
            }

            if (data != null && data.gender != null && data.gender != "") {
                $("#lbl_gender").css("color", "red");
            }
            else {
                $("#lbl_gender").css("color", "#6c757d");
            }

            if (data != null && data.dependent != null && data.dependent != "") {
                $("#lbl_dependent").css("color", "red");
            }
            else {
                $("#lbl_dependent").css("color", "#6c757d");
            }

            if (data != null && data.is_nominee != null && data.is_nominee != "") {
                $("#lbl_nominee").css("color", "red");
            }
            else {
                $("#lbl_nominee").css("color", "#6c757d");
            }

            $('#loader').hide();
        },
        Error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}