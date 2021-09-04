$('#loader').show();
var emp_role_idd;
var default_company;
var login_emp_id;

$(document).ready(function () {

    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_idd = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        BindDepartmentList('ddldepartment', default_company, -1);


        GetData();

        $("#ddlcompany").bind("change", function () {

            if ($.fn.DataTable.isDataTable('#tblReport')) {
                $('#tblReport').DataTable().clear().draw();
            }
            BindDepartmentList('ddldepartment', $(this).val(), -1);
        });


        $('#btnupdate').hide();

    }, 2000);// end timeout

});

$("#txtFile").bind("change", function () {

    EL("txtFile").addEventListener("change", readFile, false);
});

function readFile() {

    if (this.files && this.files[0]) {

        var imgsizee = this.files[0].size;
        var sizekb = imgsizee / 1024;
        sizekb = sizekb.toFixed(0);

        //  $('#HFSizeOfPhoto').val(sizekb);
        if (sizekb < 10 || sizekb > 100) {
            $("#File1").val("");
            alert('The size of the photograph should fall between 20KB to 100KB. Your Photo Size is ' + sizekb + 'kb.');
            return false;
        }
        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#File1").val("");
            alert("Photograph only allows file types of PNG, JPG, JPEG. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1);
            if (Extension == "png" || Extension == "jpeg" || Extension == "jpg") {

            }
            else {
                $("#File1").val("");
                alert("Photograph only allows file types of PNG, JPG, JPEG. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            //  EL("myImg").src = e.target.result;
            EL("HFb64").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }

function GetData() {
    //debugger;
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CurrentOpenings';

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblReport").DataTable({
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
                            targets: [4],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                    ],

                "columns": [
                    { "data": null, "title": "S.No" },
                    { "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
                    { "data": "dept_name", "name": "dept_name", "title": "Department Name", "autoWidth": true },
                    { "data": "opening_detail", "name": "opening_detail", "title": "Opening", "autoWidth": true },
                    { "data": "posted_date", "name": "posted_date", "title": "Posted On", "autoWidth": true },
                    { "data": "experience", "name": "experience", "title": "Experience", "autoWidth": true },
                    { "data": "job_description", "name": "job_description", "title": "Job Description", "autoWidth": true },
                    { "data": "role_responsibility", "name": "role_responsibility", "title": "Role and Responsibility", "autoWidth": true },
                    { "data": "current_status", "name": "current_status", "title": "Current Satus", "autoWidth": true },
                    { "data": "created_by", "name": "created_by", "title": "Created By", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,

                        "render": function (data, type, full, meta) {
                            return '<a href="" onclick="OpenDocumentt(\'' + full.doc_path + '\')"><i class="fa fa-paperclip"></i>Attachment</a>';
                        }
                    },
                    {
                        "title": "Edit", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="GetEditData(' + full.pkid_current_openings + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                        }
                    },
                    {
                        "title": "Delete", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return '<a  onclick="DeletePolicy(' + full.pkid_current_openings + ' )" > <i class="fa fa-trash"></i></a > ';
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
            messageBox("error", error.responseText);
            //alert(error);
        }
    });

}

function OpenDocumentt(path) {
    debugger;
    window.open(path);
}

$("#btnsave").bind("click", function () {
    debugger;
    $('#loader').show();
    var errormsg = "";
    var iserror = false;

    var comp_id = $("#ddlcompany").val();
    var dept_id = $("#ddldepartment").val();
    var txtOpenings = $("#txtOpenings").val();
    var txtPostedDate = $("#txtPostedDate").val();
    var txtExperience = $("#txtExperience").val();
    var txtJobDiscription = $("#txtJobDiscription").val();
    var txtRandR = $("#txtRandR").val();
    var ddlStatus = $("#ddlStatus").val();
    var txtremarks = $("#txtRemark").val();


    if (comp_id == "0" || comp_id == null || comp_id == undefined) {
        errormsg = errormsg + "Please Select Company </br>";
        iserror = true;
    }

    if (dept_id == "0" || dept_id == null || dept_id == undefined) {
        errormsg = errormsg + "Please Select Department </br>";
        iserror = true;
    }

    if (txtOpenings == "" || txtOpenings == null || txtOpenings == undefined) {
        errormsg = errormsg + "Please Enter Openings </br>";
        iserror = true;
    }

    if (txtPostedDate == "" || txtPostedDate == null || txtPostedDate == undefined) {
        errormsg = errormsg + "Please Select Posted Date</br>";
        iserror = true;
    }
    if (txtExperience == "" || txtExperience == null || txtExperience == undefined) {
        errormsg = errormsg + "Please Enter Experience</br>";
        iserror = true;
    }
    if (txtJobDiscription == "" || txtJobDiscription == null || txtJobDiscription == undefined) {
        errormsg = errormsg + "Please Enter Job Discription</br>";
        iserror = true;
    }

    if (txtRandR == "" || txtRandR == null || txtRandR == undefined) {
        errormsg = errormsg + "Please Enter Roles and Responsibilities</br>";
        iserror = true;
    }

    if (ddlStatus == "0" || ddlStatus == null || ddlStatus == undefined) {
        errormsg = errormsg + "Please Select Current Status</br>";
        iserror = true;
    }

    if (txtremarks == "0" || txtremarks == null || txtremarks == undefined) {
        errormsg = errormsg + "Please Enter Remarks</br>";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        return false;
    }



    var mydata = {

        'company_id': comp_id,
        'department_id': dept_id,
        'opening_detail': txtOpenings,
        'posted_date': txtPostedDate,
        'experience': txtExperience,
        'job_description': txtJobDiscription,
        'role_responsibility': txtRandR,
        'current_status': ddlStatus,
        'remarks': txtremarks,
        'created_by': login_emp_id,
        'is_active': 1,

    };

    var Obj = JSON.stringify(mydata);

    var file = document.getElementById("txtFile").files[0];

    var formData = new FormData();
    formData.append('AllData', Obj);
    formData.append('file', file);

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({

        url: localStorage.getItem("ApiUrl") + "/apiMasters/Save_Current_Openings/",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        headers: headerss,
        success: function (data) {
            debugger;
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();


            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == "2") {
                messageBox("error", Msg);
            }
        },
        error: function (error) {
            _GUID_New();
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });
});


function GetEditData(id) {
    debugger;

    if (id == null || id == '') {
        messageBox('info', 'There some problem please try after later !!');
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/Get_CurrentOpeningsById/' + id;
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            debugger;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }

            $('#txtOpenings').val(res.opening_detail);
            $('#txtExperience').val(res.experience);
            $('#txtJobDiscription').val(res.job_description);
            $('#txtRandR').val(res.role_responsibility);
            $('#txtRemark').val(res.remarks);
            $('#ddlStatus option[value="' + res.current_statusid + '"]').attr("selected", "selected");


            if (res.company_id != null) {
                BindAllEmp_Company('ddlcompany', login_emp_id, res.company_id);
            }

            if (res.department_id != null) {
                BindDepartmentList('ddldepartment', res.company_id, res.department_id);
            }
            if (res.department_id != null) {
                BindDepartmentList('ddldepartment', res.company_id, res.department_id);
            }

            if (res.posted_date != null) {
                let f = new Date(res.posted_date);
                f = GetDateFormatddMMyyyy(f);
                $('#txtPostedDate').val(f);
            }



            $("#hdnid").val(id);
            $('#btnupdate').show();
            $('#btnsave').hide();
            $('#loader').hide();
        },
        Error: function (err) {
            alert(err.responseText)
            $('#loader').hide();
        }
    });

}

function DeletePolicy(request_id) {
    debugger;
    $("#loader").show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/DeleteCurrentOpeningsById/' + request_id;

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    if (confirm("Do you want to delete this?")) {

        $.ajax({
            url: apiurl,
            type: "POST",
            data: {},
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {
                debugger;
                _GUID_New();
                var statuscode = data.statusCode;
                var msg = data.message;
                GetData();

                $("#loader").hide();
                if (statuscode == "0") {
                    messageBox("success", msg);
                    return false;
                }
                else {
                    messageBox("info", msg);
                    return false;
                }
            },
            error: function (err) {
                _GUID_New();
                $("#loader").hide();
                messageBox("error", err.responseText);
            }

        });

    }
    else {
        $("#loader").hide();
        return false;
    }
}

$("#btnupdate").bind("click", function () {
    debugger;
    $('#loader').show();
    var errormsg = "";
    var iserror = false;

    var comp_id = $("#ddlcompany").val();
    var dept_id = $("#ddldepartment").val();
    var txtOpenings = $("#txtOpenings").val();
    var txtPostedDate = $("#txtPostedDate").val();
    var txtExperience = $("#txtExperience").val();
    var txtJobDiscription = $("#txtJobDiscription").val();
    var txtRandR = $("#txtRandR").val();
    var ddlStatus = $("#ddlStatus").val();
    var txtremarks = $("#txtRemark").val();
    var id = $("#hdnid").val();


    if (comp_id == "0" || comp_id == null || comp_id == undefined) {
        errormsg = errormsg + "Please Select Company </br>";
        iserror = true;
    }

    if (dept_id == "0" || dept_id == null || dept_id == undefined) {
        errormsg = errormsg + "Please Select Department </br>";
        iserror = true;
    }

    if (txtOpenings == "" || txtOpenings == null || txtOpenings == undefined) {
        errormsg = errormsg + "Please Enter Openings </br>";
        iserror = true;
    }

    if (txtPostedDate == "" || txtPostedDate == null || txtPostedDate == undefined) {
        errormsg = errormsg + "Please Select Posted Date</br>";
        iserror = true;
    }
    if (txtExperience == "" || txtExperience == null || txtExperience == undefined) {
        errormsg = errormsg + "Please Enter Experience</br>";
        iserror = true;
    }
    if (txtJobDiscription == "" || txtJobDiscription == null || txtJobDiscription == undefined) {
        errormsg = errormsg + "Please Enter Job Discription</br>";
        iserror = true;
    }

    if (txtRandR == "" || txtRandR == null || txtRandR == undefined) {
        errormsg = errormsg + "Please Enter Roles and Responsibilities</br>";
        iserror = true;
    }

    if (ddlStatus == "0" || ddlStatus == null || ddlStatus == undefined) {
        errormsg = errormsg + "Please Select Current Status</br>";
        iserror = true;
    }

    if (txtremarks == "0" || txtremarks == null || txtremarks == undefined) {
        errormsg = errormsg + "Please Enter Remarks</br>";
        iserror = true;
    }

    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        return false;
    }



    var mydata = {
        'pkid_current_openings': id,
        'company_id': comp_id,
        'department_id': dept_id,
        'opening_detail': txtOpenings,
        'posted_date': txtPostedDate,
        'experience': txtExperience,
        'job_description': txtJobDiscription,
        'role_responsibility': txtRandR,
        'current_status': ddlStatus,
        'remarks': txtremarks,
        'modified_by': login_emp_id,
        'is_active': 1,

    };

    var Obj = JSON.stringify(mydata);

    var file = document.getElementById("txtFile").files[0];

    var formData = new FormData();
    formData.append('AllData', Obj);
    formData.append('file', file);

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();
    $.ajax({

        url: localStorage.getItem("ApiUrl") + "/apiMasters/Update_Current_Openings/",
        type: "POST",
        data: formData,
        dataType: "json",
        processData: false,  // tell jQuery not to process the data
        contentType: false,  // tell jQuery not to set contentType
        headers: headerss,
        success: function (data) {
            debugger;
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();


            if (statuscode == "0") {
                alert(Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == "2") {
                messageBox("error", Msg);
            }
        },
        error: function (error) {
            _GUID_New();
            messageBox("error", error.responseText);
            $('#loader').hide();
        }
    });
});



