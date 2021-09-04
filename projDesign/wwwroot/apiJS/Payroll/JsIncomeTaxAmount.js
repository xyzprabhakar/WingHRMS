
$('#loader').show();
var emp_role_id;
var login_comp_id;
var login_empid;

$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        emp_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_comp_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_empid = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });;

        BindAllEmp_Company('ddlcompany', login_empid, login_comp_id)
        BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', login_comp_id, 0);
        $("#txt_tax_amount").val('');
        GetEmployeeListOfTaxAmount(login_comp_id);




        $('#ddlcompany').bind("change", function () {
            $('#loader').show();
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', $(this).val(), 0);
            $("#txt_tax_amount").val('');
            GetEmployeeListOfTaxAmount($(this).val());

            $('#loader').hide();
        });

        $('#ddlEmployeeCode').bind("change", function () {
            $('#loader').show();

            var emp_idd = $(this).val();
            if (emp_idd == '' || emp_idd == null) {
                emp_idd = 0;
            }
            GetEmployeeTaxAmount(emp_idd);

            $('#loader').hide();
        });



        $(function () {
            $("input[id*='txt_tax_amount']").keydown(function (event) {


                if (event.shiftKey == true) {
                    event.preventDefault();
                }

                if ((event.keyCode >= 48 && event.keyCode <= 57) || (event.keyCode >= 96 && event.keyCode <= 105) || event.keyCode == 8 || event.keyCode == 9 || event.keyCode == 37 || event.keyCode == 39 || event.keyCode == 46 || event.keyCode == 190) {

                } else {
                    event.preventDefault();
                }

                if ($(this).val().indexOf('.') !== -1 && event.keyCode == 190)
                    event.preventDefault();

            });
        });


        EL("EmployeeTaxDetailFile").addEventListener("change", readFile, false);

        $("#myModal").hide();

        $('#loader').hide();

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ddlEmployeeCode = $("#ddlEmployeeCode").val();
            var txt_tax_amount = $("#txt_tax_amount").val();

            var is_deleted = 0;
            var errormsg = '';
            var iserror = false;

            //validation part
            if (ddlEmployeeCode == null || ddlEmployeeCode == '') {
                errormsg = "Please Select Employee Code...! ";
                iserror = true;
            }
            if (txt_tax_amount == null || txt_tax_amount == '') {
                errormsg = "Please Enter Tax Amount ...! ";
                iserror = true;
            }
            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }

            var myData = {
                'emp_id': ddlEmployeeCode,
                'income_tax_amount': txt_tax_amount,
                'is_deleted': is_deleted,
                'created_by': login_empid,
                'last_modified_by': login_empid
            };

            var apiurl = localStorage.getItem("ApiUrl") + 'apiPayroll/Save_EmployeeIncomeTaxAmount';
            var Obj = JSON.stringify(myData);

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            // debugger;
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
                    $('#loader').hide();
                    _GUID_New();
                    if (statuscode == "0") {
                        alert(Msg);
                        location.reload();
                        //BindCompanyList('ddlcompany', 0);
                        //BindEmployeeCodeFromEmpMasterByComp('ddlEmployeeCode', 0, 0);
                        //$("#txt_tax_amount").val('');

                    }
                    else {
                        messageBox("error", Msg);
                        return false;
                        //  alert(Msg);
                    }
                },
                error: function (err) {
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", err.responseText);
                }
            });

        });

        $("#btnuploaddtl").bind("click", function () {

            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlcompany").val();

            if (login_role_id == 2)//For Admin
            {
                company_id = default_company;
            }

            if (company_id == "0" || company_id == null || company_id == "undefined") {
                errormsg = errormsg + "Please Select Company </br>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                return false;
            }



            var mydata = {

                company_id: company_id,
                created_by: login_empid
            };

            var Obj = JSON.stringify(mydata);

            var file = document.getElementById("EmployeeTaxDetailFile").files[0];
            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            var formData = new FormData();
            formData.append('AllData', Obj);
            formData.append('file', file);


            $('#loader').show();


            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_EmpTaxDetailUpload",
                type: "POST",
                data: formData,
                dataType: "json",
                processData: false,  // tell jQuery not to process the data
                contentType: false,  // tell jQuery not to set contentType
                headers: headerss,
                success: function (data) {
                    $('#loader').hide();
                    var duplist = data.duplicateemptaxdtl;
                    var missingdtllist = data.missingemptaxdtl;
                    var deetailmsg = data.missingDtlMessage;
                    _GUID_New();

                    if (duplist != null && duplist.length > 0) {

                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Duplicate Detail in Excel'
                        });

                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });


                        $("#tbl_exceldtl").dataTable({
                            "processing": true,//for show progress bar
                            "serSide": true,//for process server side
                            "bDestroy": true,
                            "filter": true,// this is for disable filter(search box)
                            "orderMulti": false,
                            "scrollX": 200,
                            "aaData": duplist,
                            "columnDefs": [],
                            "columns": [
                                { "data": null, "title": "SNo.", "autoWidth": true },
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "income_tax_amount", "name": "income_tax_amount", "title": "Income Tax Amount", "autoWidth": true }
                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            }
                        });

                        messageBox("error", deetailmsg);
                    }
                    else if (missingdtllist != null && missingdtllist.length > 0) {

                        $("#myModal").show();
                        var modal = document.getElementById("myModal");
                        modal.style.display = "block";

                        $('#myModal').modal({
                            modal: 'true',
                            title: 'Missing / Already exist in DB Details'
                        });
                        $.extend($.fn.dataTable.defaults, {
                            sDom: '<"top"i>rCt<"footer"><"bottom"flp><"clear">'
                        });


                        $("#tbl_exceldtl").dataTable({
                            "processing": false,//for show progress bar
                            "serverSide": false,//for process server side
                            "bDestroy": true,
                            "filter": true,// this is for disable filter(search box)
                            "orderMulti": false,
                            "scrollX": 200,
                            "aaData": missingdtllist,
                            "columnDefs": [],
                            "columns": [
                                { "data": null, "title": "SNo.", "autoWidth": true },
                                { "data": "emp_code", "name": "emp_code", "title": "Employee Code", "autoWidth": true },
                                { "data": "income_tax_amount", "name": "income_tax_amount", "title": "Income Tax Amount", "autoWidth": true }
                            ],
                            "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                                return nRow;
                            }
                        });


                        messageBox("error", deetailmsg);

                    }
                    else {

                        var statuscode = data.statusCode;
                        var Msg = data.message;
                        if (statuscode == "0") {
                            alert(Msg);
                            location.reload();
                        }
                        else if (statuscode == "1" || statuscode == "2") {
                            messageBox("error", Msg);
                            return false;
                        }
                    }


                },
                error: function (error) {
                    _GUID_New();
                    alert(error.responseText);
                    $('#loader').hide();
                }
            });

        });


    }, 2000);// end timeout

});






function GetEmployeeTaxAmount(employee_id) {
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_EmployeeIncomeTaxAmount/' + employee_id,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            // debugger;
            var res = data;
            var statuscode = res.statusCode;
            var Msg = res.message;
            $('#loader').hide();

            if (statuscode != undefined && statuscode == 0) {
                //messageBox("error", Msg);
                $("#txt_tax_amount").val('0');
            }
            else if (statuscode != undefined && (statuscode == '1' || statuscode == '2')) {
                $("#txt_tax_amount").val('0');
                messageBox("error", Msg);
                return false;
            }
            else {
                $("#txt_tax_amount").val(res);
            }
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });

}


function GetEmployeeListOfTaxAmount(company_id) {
    //// debugger;;
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/GetEmployeeListOfTaxAmount/' + company_id,
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
            $("#tbl_get_emp_tax_amt").DataTable({
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
                            targets: [3],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": null },
                    {
                        "data": "employee_name", "name": "employee_name",
                        //"data": function (data, type, dataToSet) {
                        //    return data.employee_first_name + " " + data.employee_middle_name + " " + data.employee_last_name;
                        //}
                    },
                    { "data": "income_tax_amount", "name": "income_tax_amount", "autoWidth": true },
                    { "data": "created_date", "name": "created_date", "autoWidth": true }
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
        }
    });

}



function readFile() {

    if (this.files && this.files[0]) {

        var ftype = this;
        var fileupload = ftype.value;
        if (fileupload == '') {
            $("#EmployeeTaxDetailFile").val("");
            alert("Upload only Excel File and on define format. ");
            return;
        }
        else {
            var Extension = fileupload.substring(fileupload.indexOf('.') + 1).toLowerCase();
            if (Extension == "xlsx") {

            }
            else {
                $("#EmployeeTaxDetailFile").val("");
                alert("Upload only Excel File and on define format. ");
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




function DownloadEmployeeTaxExcelFile() {
    window.open("/UploadFormat/Employee Income Tax.xlsx");
}
