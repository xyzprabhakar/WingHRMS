$('#loader').show();
var company_id;
var login_emp_id;
var HaveDisplay;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $('#loader').hide();


        $('#btnreset').bind('click', function () {
            location.reload();
        });


        $('#btnsave').bind("click", function () {
            GetData();
        });


    }, 2000);// end timeout

});


function GetData() {
    $('#loader').show();
    var emp_id = $("#ddlemployee").val();

    var dtpFromDt = $("#dtpFromDt").val();
    var dtpToDt = $("#dtpToDt").val();

    var companyid = $("#ddlcompany").val() != undefined ? $("#ddlcompany").val() : company_id;

    if (companyid == null || companyid == '' || companyid == 0) {
        messageBox('info', 'Please select company...!');
        return false;
    }
    if (emp_id == null || emp_id == '') {
        messageBox('info', 'Please select to employee!!');
        return false;
    }


    var BulkEmpID = [];
    var for_all_emp = 0;

    if (emp_id == -1) {
        for_all_emp = 1;
        var options_ = $("select#ddlemployee option").filter('[value!=\"' + 0 + '\"]').map(function () { return $(this).val(); }).get();

        BulkEmpID = options_;

    }
    else {
        BulkEmpID.push(emp_id);
    }


    var mydata = {
        'empdtl': BulkEmpID,
        'all_emp': for_all_emp,
        'from_date': dtpFromDt,
        'to_date': dtpToDt,
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();


    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployee_Attendance_InOut_Report';
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            debugger;
            var appform = "AttendanceInOutReport.xlsx";
            var exlurl = localStorage.getItem("ApiUrl").replace("api/", "");
            var ahref = document.createElement("a");
            ahref.href = exlurl + "excelexport/" + appform;
            ahref.download = appform;
            //  ahref.target = "_blank";
            ahref.click();

            //_GUID_New();
            //var aData = res;
            //if (res.statusCode != undefined) {
            //    messageBox("info", res.message);
            //    return false;
            //}
            //$("#tblattendance_inout").DataTable({
            //    "processing": true, // for show progress bar
            //    "serverSide": false, // for process server side
            //    "bDestroy": true,
            //    "filter": true, // this is for disable filter (search box)
            //    "orderMulti": false, // for disable multiple column at once              
            //    "scrollX": 800,
            //    dom: 'lBfrtip',
            //    buttons: [
            //        {
            //            text: 'Export to Excel',
            //            title: 'Attendance Monthly Report',
            //            extend: 'excelHtml5'
            //        }
            //    ],
            //    "aaData": aData,
            //    "columnDefs":
            //        [

            //        ],

            //    "columns": [
            //        { "data": null, "title": "S.No.", "autoWidth": true },
            //        { "data": "employee_code", "name": "employee_code", "title": "Employee Code", "autoWidth": true },
            //        { "data": "employee_name", "name": "employee_name", "title": "Employee Name", "autoWidth": true },
            //        { "data": "company_name", "name": "company_name", "title": "Company Name", "autoWidth": true },
            //        { "data": "branch_name", "name": "branch_name", "title": "Branch Name", "autoWidth": true },
            //        { "data": "department_name", "name": "department_name", "title": "Department Name", "autoWidth": true },
            //        { "data": "doj", "name": "doj", "title": "Date of Joining", "autoWidth": true },
            //        { "data": "shift_type", "name": "shift_type", "title": "Shift Type", "autoWidth": true },
            //        { "data": "shift_timing", "name": "shift_timing", "title": "Shift Timing", "autoWidth": true },
            //        { "data": "shift_hrs", "name": "shift_hrs", "title": "Shift Hrs", "autoWidth": true },
            //        {
            //            "data": "empdata",
            //            "render": function (data, type, full, meta) {
            //                var datewisedata = "";

            //                var datewise = data.split(",");
            //                if (meta.row == 0) {
            //                    datewisedata = "<table><tr>";

            //                    for (var i = 0; i < datewise.length; i++) {
            //                        var daymnth = datewise[i].split(";")[0];
            //                        // var d = new Date(daymnth);
            //                        // var daymonthyear = daymnth.getDay() + " " + daymnth.getMonth + " " + daymnth.getFullYear;
            //                        datewisedata += "<td colspan='4'>" + daymnth + "</td>";  
            //                    };
            //                    datewisedata += "</tr>";
            //                    datewisedata += "<tr>";
            //                    for (var i = 0; i < datewise.length; i++) {
            //                        datewisedata += "<td>In Time</td><td>Out Time</td><td>Total Hrs</td><td>Status</td>";
            //                    };
            //                    datewisedata += "</tr>";
            //                    datewisedata += "</table>";
            //                }

            //                datewisedata += "<table><tr>";

            //                // datewisedata += "<tr>";
            //                for (var i = 0; i < datewise.length; i++) {
            //                    var inout = datewise[i].split(";")[1];
            //                    var inoutcol = inout.split("|");
            //                    var outtime = inoutcol[2];
            //                    var intime = inoutcol[3];
            //                    var totalhrs = "";
            //                    if (outtime != "" && intime != "") {
            //                        totalhrs = (parseFloat(outtime) - parseFloat(intime)).toFixed(2).toString();
            //                    }
            //                    if (totalhrs != "") {
            //                        totalhrs = totalhrs.replace(".", ":");
            //                    }
            //                    // for (var j = 0; j < inoutcol.length; j++) {
            //                    datewisedata += "<td>" + inoutcol[0] + "</td><td>" + inoutcol[1] + "</td><td>" + totalhrs + "</td><td>" + inoutcol[4] + "</td>";
            //                    //}
            //                }
            //                datewisedata += "</tr>";
            //                datewisedata += "</table>";
            //                debugger;
            //                return datewisedata;
            //            }
            //        },
            //        //{
            //        //    // "title": "Donor Name",
            //        //    "render": function (data, type, full, meta) {
            //        //        var dname = full.empdata

            //        //    }

            //        //},
            //        //{ "data": "empdata", "name": "empdata", "title": "empdata", "autoWidth": true },

            //    ],
            //    "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
            //    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            //        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
            //        return nRow;
            //    },

            //});
            $('#loader').hide();
        },
        error: function (error) {
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });
    //$('#loader').hide();
}
