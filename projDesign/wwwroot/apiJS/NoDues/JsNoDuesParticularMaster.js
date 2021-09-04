$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;
var HaveDisplay;
$(document).ready(function () {
    setTimeout(function () {


        $("#btnupdate").hide();
        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }


        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        //BindAllEmp_Company('ddlcompany', login_emp_id, default_company);
        BindDepartmentListAll('ddldepartment', - 1);
        //GetEmployeeListbyDepartment('ddlEmployee', default_company, 0, -1);

        //$("#ddlcompany").bind("change", function () {

        //    if ($.fn.DataTable.isDataTable('#tblReport')) {
        //        $('#tblReport').DataTable().clear().draw();
        //    }

        //    BindDepartmentList('ddldepartment', $(this).val(), -1);
        //    GetData();
        //});

        GetData();

        $('#loader').hide();

    }, 2000);// end timeout

});


var itemCount = 0;
$("#btnAddParticular").bind("click", function () {
    itemCount += 1;
    var divPartic = document.getElementById("divParticulars");
    if (divPartic != null) {
        var div = document.createElement("div");
        div.className = "col-md-6";
        div.innerHTML += '<div class= "form-group" id="divPar' + itemCount + '"><input id="txtParticulars' + itemCount + '" type="text" placeholder="Particular' + itemCount + '" class="form-control" /> </div > ';
        divPartic.append(div);
    }
});
$("#btnRemoveParticular").bind("click", function () {

    var divPartic = document.getElementById('divPar' + itemCount);
    if (divPartic != null) {
        divPartic.remove();
        itemCount -= 1;
    }
});

$("#btnsave").bind("click", function () {

    var errormsg = '';

    $('#loader').show();
    //var ddlcompany = $("#ddlcompany").val();
    var ddldepartment = $("#ddldepartment").val();
    var txtRemarks = $("#txtRemarks").val();

    var txtParticulars = new Array();
    if (itemCount > 0) {
        for (var i = 0; i < itemCount; i++) {
            var parvalue = $("#txtParticulars" + (i + 1)).val().toUpperCase();
            if (parvalue == null || parvalue == '' || parvalue == undefined) {
                errormsg = errormsg + "Please enter valid Particular value <br/>";
                break;
            }
            txtParticulars.push(parvalue);
        }
    }
    else { errormsg = errormsg + "Please enter valid Particular value <br/>"; }

    if (errormsg != "") {
        messageBox("error", errormsg);
        $('#loader').hide();
        return;
    }

    //if (ddlcompany == '' || ddlcompany == 0 || ddlcompany == null) {
    //    messageBox("error", "Please Select Company...!");
    //    $('#loader').hide();
    //    return;
    //}

    if (ddldepartment == '' || ddldepartment == 0) {
        messageBox("error", "Please Select Department...!");
        $('#loader').hide();
        return;
    }

    //if (txtRemarks == null || txtRemarks == '') {
    //    messageBox("error", "Please Enter Remarks...!");
    //    $('#loader').hide();
    //    return;
    //}

    var myData = {
        //'company_id': ddlcompany,
        'department_id': ddldepartment,
        'remarks': txtRemarks,
        'particularList': txtParticulars,
        'created_by': login_emp_id,
        'is_deleted': 0,
    };

    if (confirm("Are you sure you want to add this ?")) {
        var apiurl = localStorage.getItem("ApiUrl") + 'apiNoDues/Save_NoDueParticularMaster';
        var Obj = JSON.stringify(myData);
        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();
        $.ajax({
            url: apiurl,
            type: "POST",
            data: Obj,
            dataType: "json",
            contentType: "application/json",
            headers: headerss,
            success: function (data) {
                var statuscode = data.statusCode;
                var Msg = data.message;

                _GUID_New();
                if (statuscode == "0") {
                    $('#loader').hide();
                    messageBox("success", Msg);
                    setTimeout(function () {
                        location.reload(true);
                    }, 2000);
                    // location.reload();
                }
                else if (statuscode == "1" || statuscode == '2') {
                    $('#loader').hide();
                    messageBox("error", Msg);
                    return false;
                }
            },
            error: function (err) {
                $('#loader').hide();
                _GUID_New();
                alert(err.responseText);
            }
        });
    }
});

function GetData() {
    //debugger;
    $('#loader').show();



    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiNoDues/GetNoDuesParticularMasterData",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: {},
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            _GUID_New();

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
                                var d = GetDateFormatddMMyyyy(date);
                                if (d == '01/Jan/1') {
                                    return "";
                                }
                                else
                                    return d;
                            }
                        },
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                  //  { "data": "comp_name", "name": "comp_name", "title": "Company", "autoWidth": true },
                    { "data": "dept_Name", "name": "dept_Name", "title": "Department", "autoWidth": true },
                    { "data": "particular_name", "name": "particular_name", "title": "Particular Name", "autoWidth": true },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "title": "Created Date", "autoWidth": true },
                    { "data": "created_by", "name": "created_by", "title": "Created By", "autoWidth": true },
                    {
                        "title": "Delete", "autoWidth": true,
                        "render": function (data, type, full, meta) {
                            return (full.is_deleted == 1) ? '' : '<a  onclick="DeleteParticular(' + full.particular_id + ' )" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });



            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });

}

function DeleteParticular(request_id) {
    $("#loader").show();

    debugger;

    var myData = {
        'pkid_ParticularMaster': request_id,
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apiNoDues/DeleteNoDuesParticularMaster/' + request_id;

    if (confirm("Do you want to delete this?")) {

        $.ajax({
            url: apiurl,
            type: "POST",
            data: {},
            dataType: "json",
            contentType: "application/json",
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (data) {
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


$("#btnreset").bind("click", function () {
    location.reload();
});

