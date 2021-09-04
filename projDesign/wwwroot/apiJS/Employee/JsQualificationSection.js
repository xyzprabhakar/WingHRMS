$('#loader').show();
var emp_idd;
var default_company;



$(document).ready(function () {
    setTimeout(function () {


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        emp_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem('company_id'), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        if (localStorage.getItem("new_compangy_idd") != null) {

            BindAllEmp_Company('ddlCompany', login_emp_id, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }))
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);
        }
        else {
            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
        }



        if (localStorage.getItem("new_emp_id") != null) {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
            $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');
            reset_all();
            GetData(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));

        }

        $('#ddlCompany').change(function () {
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));
            //GetData(0);
            reset_all();
        });

        $('#ddlEmployeeCode').change(function () {
            $('#ddlShift').val(0);
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            if ($(this).val() == 0) {
                reset_all();
            }
            else {
                Get_Details_Changes($(this).val());
                GetData($(this).val());
            }

        });





        $('#btnUpdateQualification').hide();
        $('#btnSaveQualification').show();

        BindEducationLevel('ddleducationlevel', 0);
        //EL("document_image").addEventListener("change", ReadFile, false);
        $('#loader').hide();



        $('#btnSaveQualification').bind("click", function () {

            if (!Validation()) { return false; }
            var ddlcompany = $("#ddlCompany").val();
            var employee_id = $('#ddlEmployeeCode :selected').val();


            var title = $("#title").val();
            var board_or_university = $("#board_or_university").val();
            var institute_or_school = $("#institute_or_school").val();
            var passing_year = $("#passing_year").val();
            var stream = $("#stream").val();
            var education_type = $("#education_type").val();
            if (education_type == '' || education_type == null) {
                education_type = 0;
            }
            var marks_division_cgpa = $("#marks_division_cgpa").val();
            var remark = $("#remark").val();

            var edu_level = $("#ddleducationlevel").val();

            var document_image = "" //$("#HFb64document_image").val();
            //Vald(ddlcompany, "ddlCompany", "Enter Company");
            //Vald(employee_id, "ddlEmployeeCode", "Enter Employee Code");
            //Vald(title, "title", "Enter title");
            //Vald(board_or_university, "board_or_university", "Enter board or university");
            //Vald(institute_or_school, "institute_or_school", "Enter institute or school");
            //Vald(passing_year, "passing_year", "Enter passing year");
            //Vald(stream, "stream", "Enter stream");
            //Vald(education_type, "education_type", "Enter education type");
            //Vald(marks_division_cgpa, "marks_division_cgpa", "Enter marks division cgpa");

            if (title == 'Title') {
                title = '';
            }

            var myData = {
                'employee_id': employee_id,
                'title': title,
                'board_or_university': board_or_university,
                'institute_or_school': institute_or_school,
                'passing_year': passing_year,
                'stream': stream,
                'education_type': education_type,
                'marks_division_cgpa': marks_division_cgpa,
                'remark': remark,
                'document_image': document_image,
                'education_level': edu_level,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id
            };

            console.log(JSON.stringify(myData));
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $('#loader').show();
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeQualification',
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
                        alert(Msg);
                        window.location.href = '/Employee/QualificationSection';
                    }
                    if (statuscode == "1") {
                        alert(Msg);
                        window.location.href = '/Employee/QualificationSection';
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

        });




        $('#btnUpdateQualification').bind("click", function () {
            $('#loader').show();
            if (!Validation()) return false;
            //// debugger;;
            var id = $("#hdnemp_qualification_section_id").val();

            var employee_id = $('#ddlEmployeeCode :selected').val();
            //if (localStorage.getItem("emp_role_id") == "2") // For Admin
            //{
            //   // ddlcompany = localStorage.getItem("company_id");
            //    employee_id = localStorage.getItem("emp_id");

            //}

            var title = $("#title").val();
            //var ddlcompany = $("#ddlCompany").val();
            var board_or_university = $("#board_or_university").val();
            var institute_or_school = $("#institute_or_school").val();
            var passing_year = $("#passing_year").val();
            var stream = $("#stream").val();
            var education_type = $("#education_type").val();
            var marks_division_cgpa = $("#marks_division_cgpa").val();
            var remark = $("#remark").val();
            var edu_level = $("#ddleducationlevel").val();

            //if (ddlcompany == "0" || ddlcompany == null || ddlcompany == "") {
            //    messageBox("error", "Enter Company ....!");
            //    $("#ddlCompany").val('');
            //    return;
            //}
            //if (employee_id == "0" || employee_id == null || employee_id == "") {
            //    messageBox("error", "Enter Employee Code ....!");
            //    $("#ddlEmployeeCode").val('');
            //    return;
            //}

            //var document_image = null;
            //if ($("#HFb64document_image").val() != '') {
            //    document_image = $("#HFb64document_image").val();
            //}
            //else {
            //    document_image = $("#hdndocument_image").val();
            //}
            var document_image = "";


            var myData = {
                'employee_id': employee_id,
                'title': title,
                'board_or_university': board_or_university,
                'institute_or_school': institute_or_school,
                'passing_year': passing_year,
                'stream': stream,
                'education_type': education_type,
                'marks_division_cgpa': marks_division_cgpa,
                'remark': remark,
                'document_image': document_image,
                'education_level': edu_level,
                'created_by': login_emp_id,
                'last_modified_by': login_emp_id
            };

            console.log(JSON.stringify(myData));
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // Save
            $.ajax({
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/UpdateEmployeeQualification/' + id,
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
                        alert(Msg);
                        window.location.href = '/Employee/QualificationSection';
                    }
                    else if (statuscode == "1") {
                        alert(Msg);
                        location.reload();
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

        });



    }, 2000);// end timeout

});

function Vald(Tovald, Control, Emsg) {
    if (Tovald == "0" || Tovald == "" || Tovald == null) {
        messageBox("error", Emsg);
        var str = "#" + Control;
        $(str).val('');
        return;
    }
}



//Save Employee Qualification Details


function Validation() {
    $('#loader').show();
    var employee_id = $('#ddlEmployeeCode :selected').val();
    var ddlcompany = $("#ddlCompany").val();
    var edu_level = $("#ddleducationlevel").val();
    var remarks = $("#remark").val();
    var errormsg = '';
    var iserror = false;


    if (employee_id == '' || employee_id == '0' || employee_id == null) {
        errormsg = "Employee Code is required field...! <br/>";
        iserror = true;
        $("#ddlEmployeeCode").focus();
    }
    if (ddlcompany == '' || ddlcompany == '0' || ddlcompany == null) {
        errormsg += "Company is required field...! <br/>";
        iserror = true;
        $("#ddlCompany").focus();
    }

    if ($("#board_or_university").val() == '') {
        errormsg += "Board/Universty is required field...! <br/>";
        iserror = true;
        $("#board_or_university").focus();
    }

    if ($("#institute_or_school").val() == '') {
        errormsg += "Institute/School is required field...!<br/>";
        iserror = true;
        $("#Institute").focus();
    }

    if ($("#passing_year").val() == '') {
        errormsg += "Passing Year is required field...!<br/>";
        iserror = true;
        $("#passing_year").focus();
    }

    if ($("#stream").val() == '') {
        errormsg += "Course / Stream is required field...!<br/>";
        iserror = true;
        $("#stream").focus();
    }

    if ($("#education_type").val() == '') {
        errormsg += "Education Type is required field...!!<br/>";
        iserror = true;
        $("#education_type").focus();
    }

    //if ($("#marks_division_cgpa").val() == '') {
    //    errormsg += "Marks/Division/CGPA is required field...!!<br/>";
    //    iserror = true;
    //    $("#marks_division_cgpa").focus();
    //}

    if (edu_level == "" || edu_level == "0" || edu_level == null) {
        errormsg += "Please select education level...</br>";
        iserror = true;

        $("#ddleducationlevel").focus();
    }
    //if (remarks == "" || remarks == "0" || remarks == null) {
    //    errormsg += "Please Enter Remarks...</br>";
    //    iserror = true;

    //    $("#remark").focus();
    //}
    if (iserror) {
        messageBox("info", errormsg);
        //  messageBox("info", "eror give");
        $('#loader').hide();
        return false;
    }
    return true;
}


//--------bind data in jquery data table
function GetData(employee_id) {
    // debugger;;

    if ($.fn.DataTable.isDataTable('#tblQualification')) {
        $('#tblQualification').DataTable().clear().draw();
    }

    if (employee_id > 0) {
        $('#loader').show();
        $.ajax({
            type: "GET",
            url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeQualification/' + employee_id,
            data: {},
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (res) {

                $('#loader').hide();

                if (res.statusCode != undefined) {
                    messageBox("error", res.message);
                    return false;
                }
                //// debugger;;
                $("#tblQualification").DataTable({
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
                                targets: [7],
                                render: function (data, type, row) {

                                    var date = new Date(data);
                                    return GetDateFormatddMMyyyy(date);
                                }
                            }
                            //{
                            //    targets: [5],
                            //    "class": "text-center"

                            //}
                        ],

                    "columns": [
                        { "data": null, "title": "SNo.", "autoWdith": true },
                        { "data": "board_or_university", "name": "board_or_university", "title": "Board/Universty", "autoWidth": true },
                        { "data": "institute_or_school", "name": "institute_or_school", "title": "Institue/School", "autoWidth": true },
                        { "data": "passing_year", "name": "passing_year", "title": "Passign Year", "autoWidth": true },
                        { "data": "stream", "name": "stream", "title": "Course", "autoWidth": true },
                        { "data": "education_level_", "name": "education_level_", "title": "Education Level", "autoWidth": true },
                        { "data": "education_type_", "name": "education_type_", "title": "Education Type", "autoWdith": true },
                        { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },

                        {
                            "title": "Action", "autoWidth": true,
                            "render": function (data, type, full, meta) {
                                return '<a href="#" onclick="GetEditData(' + full.emp_qualification_section_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
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
                $('#loader').hide();
            }
        });

    }

}

function GetEditData(qualification_id) {
    //// debugger;;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEditEmployeeQualification/' + qualification_id,
        //data: myData,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            var data = res;

            if (data.statusCode != undefined) {
                messageBox("info", data.message);
                $('#loader').hide();
                return false;
            }

            $("#hdnemp_qualification_section_id").val(data.emp_qualification_section_id);

            $("#title").val(data.title);
            $("#board_or_university").val(data.board_or_university);
            $("#institute_or_school").val(data.institute_or_school);
            $("#passing_year").val(data.passing_year);
            $("#stream").val(data.stream);
            $("#education_type").val(data.education_type);
            $("#marks_division_cgpa").val(data.marks_division_cgpa);
            $("#remark").val(data.remark);
            $("#hdndocument_image").val(data.document_image);

            BindEducationLevel('ddleducationlevel', data.education_level);

            $('#btnUpdateQualification').show();
            $('#btnSaveQualification').hide();

            $('#loader').hide();
        },
        Error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}

function ReadFile() {
    $('#loader').show();
    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 500) {
            $("#document_image").val("");
            messageBox("info", 'The size of the photograph should fall between 10KB to 500KB. Your Photo Size is ' + sizekb + 'kb.');
            $('#loader').hide();
            return false;
        }
        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#document_image").val("");
            messageBox("info", 'Photograph only allows file types of PNG, JPG, JPEG.');
            $('#loader').hide();
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg" || Extension == "PNG" || Extension == "JPEG" || Extension == "JPG") {

            }
            else {
                $("#document_image").val("");
                messageBox("info", 'Photograph only allows file types of PNG, JPG, JPEG.');
                $('#loader').hide();
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
    $("#title").val('');
    $("#board_or_university").val('');
    $("#institute_or_school").val('');
    $("#passing_year").val('');
    $("#stream").val('');
    $("#education_type").val('');
    BindEducationLevel('ddleducationlevel', 0);
    $("#marks_division_cgpa").val('');
    $("#remark").val('');
    $("#btnSaveQualification").show();
    $("#btnUpdateQualification").hide();
}


function Get_Details_Changes(employee_id) {
    //debugger;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/EmployeeQualification_changes/' + employee_id,
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
            
            if (data != null && data.board_or_university != null && data.board_or_university != "") {
                $("#lbl_board").css("color", "red");
            }
            else {
                $("#lbl_board").css("color", "#6c757d");
            }

            if (data != null && data.institute_or_school != null && data.institute_or_school != "") {
                $("#lbl_school").css("color", "red");
            }
            else {
                $("#lbl_school").css("color", "#6c757d");
            }

            if (data != null && data.passing_year != null && data.passing_year != "") {
                $("#lbl_year").css("color", "red");
            }
            else {
                $("#lbl_year").css("color", "#6c757d");
            }

            if (data != null && data.stream != null && data.stream != "") {
                $("#lbl_course").css("color", "red");
            }
            else {
                $("#lbl_course").css("color", "#6c757d");
            }

            if (data != null && data.education_type != null && data.education_type != "") {
                $("#lbl_edu_type").css("color", "red");
            }
            else {
                $("#lbl_edu_type").css("color", "#6c757d");
            }

            if (data != null && data.education_level != null && data.education_level != "") {
                $("#lbl_edu_level").css("color", "red");
            }
            else {
                $("#lbl_edu_level").css("color", "#6c757d");
            }
            //if (data.marks_division_cgpa != null && data.marks_division_cgpa != "") {
            //    $("#lbl_marks").css("color", "red");
            //}

            $('#loader').hide();
        },
        Error: function (err) {
            messageBox("error", err.responseText);
            $('#loader').hide();
        }
    });
}


