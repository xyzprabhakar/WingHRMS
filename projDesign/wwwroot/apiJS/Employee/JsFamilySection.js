$('#loader').show();

var emp_idd;
var default_company;


$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        //// debugger;;
        emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem('company_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (localStorage.getItem("new_compangy_idd") != null) {
            BindAllEmp_Company('ddlCompany', emp_idd, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);

        }
        else {
            BindAllEmp_Company('ddlCompany', emp_idd, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }


        //    var HaveDisplay = ISDisplayMenu("Display Company List");



        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            GetData(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }


        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //if ($.fn.DataTable.isDataTable('#tblFamily')) {
            //    $('#tblFamily').DataTable().clear().draw();
            //}
            $("#occupation").val('');
            $("#name_as_per_aadhar_card").val('');
            $("#date_of_birth").val('');
            $("#gender").val('0');
            $("#dependent").val('0');
            $("#remark").val('');
            $("#is_nominee").val('0');
            $("#divnomineepercentage").css("display", "none");
            $("#HFb64document_image").val('');
            $("#hdndocument_image").val('');
            $("#hdnemp_family_section_id").val('');

            if ($(this).val() == 0) {
                reset_all();
            }
            else {
                GetData($(this).val());
            }
        });

        $('#ddlEmployeeCode').change(function () {
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));

            if ($(this).val() == 0) {
                reset_all();
            }
            else {
                Get_Details_Changes($(this).val());
                GetData($(this).val());
            }

        });




        BindRelationType('ddlrelation', 0);

        $('#btnUpdateFamilySection').hide();
        $('#btnSaveFamilySection').show();
        $("#divnomineepercentage").hide();

        // EL("document_image").addEventListener("change", ReadFile, false);


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



        $('#loader').hide();

    }, 2000);// end timeout

});
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
        if (date_of_birth == '') {
            date_of_birth = '1900-01-01';
        }
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
        if (occupation == 0) {
            occupation = 'NA';
        }
        if (relation == 0) {
            relation = 'NA';
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
                    $('#loader').hide();
                    alert(Msg);
                    window.location.href = '/Employee/FamilySection';
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
    //Validation();
    //// debugger;;
    var id = $("#hdnemp_family_section_id").val();

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
        if (date_of_birth == '') {
            date_of_birth = '1900-01-01';
        }
        var gender = $("#gender").val();
        var dependent = $("#dependent").val();
        var remark = $("#remark").val();
        var is_nominee = $("#is_nominee").val();
        var nominee_percentage = "";
        var aadhar_card_no = $("#aadhar_card_number").val();

        //if (is_nominee == "1")//Yes
        //{

        //}
        //else {
        //    nominee_percentage = 0;
        //}
        if (occupation == 0) {
            occupation = 'NA';
        }
        if (relation == 0) {
            relation = 'NA';
        }
        nominee_percentage = $("#nominee_percentage").val();

        if (nominee_percentage == '') {
            nominee_percentage = 0;
        }



        var document_image = "";
        //var document_image = null;
        //if ($("#HFb64document_image").val() != '') {
        //    document_image = $("#HFb64document_image").val();
        //}
        //else {
        //    document_image = $("#hdndocument_image").val();
        //}


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
                    alert(Msg);
                    window.location.href = '/Employee/FamilySection';
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




//--------bind data in jquery data table
function GetData(employee_id) {
    //// debugger;;
    if (employee_id > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            // url: ,
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetFamilySection/' + employee_id,
            data: {},
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (res) {
                $('#loader').hide();
                //// debugger;;
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
                messageBox("error", error.responseText);

            }
        });

        $.ajax({
            type: "GET",
            // url: ,
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetNomineePercentage/' + employee_id,
            data: {},
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (res) {
                $('#hdnemp_total_percentage').val(res.total);
            },
            error: function (error) {
                messageBox("error", error.responseText);
                $('#loader').hide();
            }
        });

    }

}


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
    var total_percentage = $("#hdnemp_total_percentage").val();
    var now_percentage = $("#nominee_percentage").val();
    //var HaveDisplay = ISDisplayMenu("Display Company List");

    //if (HaveDisplay == 0) {
    //    ddlcompany = default_company;
    //   // ddlEmployeeCode = localStorage.getItem("emp_id");
    //}

    var relation = $("#ddlrelation").val();
    var name_as_per_aadhar_card = $("#name_as_per_aadhar_card").val();
    var date_of_birth = $("#date_of_birth").val();

    var result = parseInt(total_percentage) + parseInt(now_percentage);

    if (result > 100) {
        messageBox("error", "You can't distribute the nominees percentage more the 100. </br>");
        $('#loader').hide();
        isvalid = false;
    }
    else if (ddlcompany == 0 || ddlcompany == "" || ddlcompany == null) {
        messageBox("error", "Please select company...!</br>");
        $('#loader').hide();
        isvalid = false;
    }
    else if (ddlEmployeeCode == '' || ddlEmployeeCode == 0) {
        messageBox("error", "Please select employee code...!</br>");
        $('#loader').hide();
        isvalid = false;
    }
    //else if (relation == 0 || relation == null) {
    //    messageBox("error", "Relation is required field...!</br>");
    //    $('#loader').hide();
    //    isvalid = false;
    //}
    else if (name_as_per_aadhar_card == '') {
        messageBox("error", "Name As Per Aadhar Card is required field...!</br>");
        $('#loader').hide();
        isvalid = false;
    }

    //else if (date_of_birth == '') {
    //    messageBox("error", "Date of birth is required field...!</br>");
    //    $('#loader').hide();
    //    isvalid = false;
    //}
    //else if ($("#is_nominee").val() == "1" && $("#nominee_percentage").val() <= 0)
    //{   messageBox("error", "Nominee Percentange Cannot be 0 if Nominee is 'Yes'</br>");
    //        $('#loader').hide();
    //        isvalid = false;

    //}
    else if ($("#gender").val() == "0" || $("#gender").val() == null || $("#gender").val() == "") {
        messageBox("error", "Please select gender</br>");
        $('#loader').hide();
        isvalid = false;
    }
    //else if (aadhar_card_no == '' || aadhar_card_no == null || aadhar_card_no == 0) {
    //    messageBox("error", "Please enter Aadhar card no. </br>");
    //    $('#loader').hide();
    //    isvalid = false;
    //}

    else {
        isvalid = true
    }
    $('#loader').hide();
    return isvalid;
}

function ReadFile() {

    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 500) {
            $("#document_image").val("");
            alert('The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
            return false;
        }
        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#document_image").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
            if (Extension == "png" || Extension == "PNG" || Extension == "jpeg" || Extension == "JPEG" || Extension == "jpg" || Extension == "JPG") {

            }
            else {
                $("#document_image").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            // EL("HFb64pan_card_name").src = e.target.result;
            //EL("HFb64").value = e.target.result;
            EL("HFb64document_image").value = e.target.result;
        };
        FR.readAsDataURL(this.files[0]);
    }


}

function EL(id) { return document.getElementById(id); }

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