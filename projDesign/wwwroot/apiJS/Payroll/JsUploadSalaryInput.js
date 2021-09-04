$("#loader").show();
var login_role_id;
var default_company;

var login_emp_id;


$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        BindAllEmp_Company('ddlCompany', login_emp_id, default_company);


        $('#txtuplodmonthyear').datepicker({
            changeMonth: true,
            changeYear: true,
            dateFormat: 'MM yy',

            onClose: function () {
                var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
                var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

            },

            beforeShow: function () {
                if ((selDate = $(this).val()).length > 0) {
                    iYear = selDate.substring(selDate.length - 4, selDate.length);
                    iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
                    $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
                    $(this).datepicker('setDate', new Date(iYear, iMonth, 1));

                }

            }
        });


        //  $("#myModal1").hide();
        $("#loader").hide();

        $("#SalaryInputFile").bind("change", function () {
            EL("SalaryInputFile").addEventListener("change", readFile, false);
        });


        $("#btnuploaddtl").bind("click", function () {
            var companyid = $("#ddlCompany").val();
            var monthyear = $("#txtuplodmonthyear").val();
            var txtmnyr = new Date($('#txtuplodmonthyear').val());
            var yr = txtmnyr.getFullYear();
            var mnth = txtmnyr.getMonth() + 1;
            if (mnth <= 9) {
                monthyear = yr.toString() + "0" + mnth.toString();
            }
            else {
                monthyear = yr.toString() + mnth.toString();
            }

            var uploadfilee = $("#SalaryInputFile").val()
            var iserror = false;
            var errormsg = "";

            if (companyid == "0" || companyid == "" || companyid == null) {
                iserror = true;
                errormsg = errormsg + "Please Select Company!</br>";
            }
            if (monthyear == "" || monthyear == null || monthyear == "0") {
                iserror = true;
                errormsg = errormsg + "Please Select Processed MonthYear!</br>";
            }
            if (uploadfilee == "" || uploadfilee == null) {
                iserror = true;
                errormsg = errormsg + "Please Upload File!</br>";
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }


            var mydata = {
                company_id: companyid,
                monthyear: monthyear,
                created_by: login_emp_id
            };

            var obj = JSON.stringify(mydata);
            var files = document.getElementById("SalaryInputFile").files;
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();;

            var formdata = new FormData();
            formdata.append('AllData', obj);

            for (var i = 0; i < files.length; i++) {
                formdata.append('Files', files[i]);
            }
            $('#loader').show();

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "apiPayroll/Upload_SalaryInput",
                type: "POST",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                dataType: "json",
                data: formdata,
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    _GUID_New();
                    var duplist = data.duplicatesalaryinputlst;
                    var missingdtllist = data.missingsalaryinputlst;
                    var deetailmsg = data.dtlMessage;

                    if (duplist != null && duplist.length > 0) {

                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Duplicate Detail in Excel',
                            backdrop: 'false'
                        });

                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });

                        $("#tbl_exceldtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": duplist,
                            "columnDefs":
                                [],

                            "columns": [
                                { "data": null, "title": "SNo.", "autoWidth": true },
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "property_details", "name": "property_details", "title": "Salary Component", "autoWidth": true },
                                { "data": "component_value", "name": "component_value", "title": "Amount", "autoWidth": true },
                            ],
                            "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },
                        });


                        messageBox("error", deetailmsg);
                    }
                    else if (missingdtllist != null && missingdtllist.length > 0) {

                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Missing / Already exist in DB Details',
                            backdrop: 'false'
                        });
                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });

                        $("#tbl_exceldtl").DataTable({
                            "processing": true, // for show progress bar
                            "serverSide": false, // for process server side
                            "bDestroy": true,
                            "filter": true, // this is for disable filter (search box)
                            "orderMulti": false, // for disable multiple column at once
                            "scrollX": 200,
                            "aaData": missingdtllist,
                            "columnDefs":
                                [],

                            "columns": [
                                { "data": null, "title": "SNo.", "autoWidth": true },
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "property_details", "name": "property_details", "title": "Salary Component", "autoWidth": true },
                                { "data": "component_value", "name": "component_value", "title": "Amount", "autoWidth": true },
                            ],
                            "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            },

                        });


                        messageBox("error", deetailmsg);

                    }
                    else {

                        var statuscode = data.statusCode;
                        var msg = data.message;
                        if (statuscode == "0") {

                            BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
                            $("#ddlMonthyear").val('');
                            $("#SalaryInputFile").val('');
                            messageBox("success", msg);
                            return false;
                        }
                        else {
                            messageBox("error", msg);
                            return false;
                        }
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


function DownloadSalaryInputFormat() {
    window.open("/UploadFormat/PayrollInput.xlsx");
}





function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#SalaryInputFile").val("");
            alert("Upload only Excel file and on define format. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#SalaryInputFile").val("");
                alert("Upload only Excel file and on define format. ");
                return;
            }
        }

        var FR = new FileReader();
        FR.onload = function (e) {
            EL("HFb64uploaddtl").value = e.target.result;

        };
        FR.readAsDataURL(this.files[0]);
    }
}

function EL(id) { return document.getElementById(id); }

