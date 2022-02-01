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

        //if (localStorage.getItem("new_compangy_idd") != null) {
        //    BindAllEmp_Company('ddlCompany', emp_idd, CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        //    // BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), 0);


        //    if (localStorage.getItem("new_emp_id") != null) {
        //        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }), CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        //        $('#ddlCompany :selected').val(CryptoJS.AES.decrypt(localStorage.getItem("new_compangy_idd"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }));
        //        $('#ddlEmployeeCode').val(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; })).trigger('chosen:updated');

        //        Get_EmployeementTypeDetail(CryptoJS.AES.decrypt(localStorage.getItem("new_emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; }))
        //    }
        //    else {
        //        BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
        //    }
        //}
        //else {
            BindAllEmp_Company('ddlCompany', emp_idd, default_company);
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', default_company, 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + default_company + "'", localStorage.getItem("sit_id")));
       // }


        // var HaveDisplay = ISDisplayMenu("Display Company List");




        BindEmployementTypeForddl('ddlemptype', 0);

        //  GetData();
        $('#loader').hide();

        $('#ddlCompany').change(function () {
            BindOnlyProbation_Confirmed_emp('ddlEmployeeCode', $(this).val(), 0);
            localStorage.setItem("new_compangy_idd", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));

            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("0", localStorage.getItem("sit_id")));

        });

        $('#ddlEmployeeCode').change(function () {
            // localStorage.setItem("new_emp_id", $(this).val());
            //localStorage.setItem("new_emp_id", CryptoJS.AES.encrypt("'" + $(this).val() + "'", localStorage.getItem("sit_id")));
            Get_EmployeementTypeDetail($(this).val());
        });


        $("#btn_save_emptype").bind("click", function () {

            var errormsg = "";
            var iserror = false;

            var company_id = $("#ddlCompany").val();
            var employee_id = $("#ddlEmployeeCode").val();
            var effective_dt = $("#effective_dt").val();

            var _empmnttype = $("#ddlemptype").val();


            if (company_id == "0" || company_id == "" || company_id == null) {
                errormsg = errormsg + "Please Select Company <br/>";
                iserror = true;
            }

            if (employee_id == "0" || employee_id == "" || employee_id == null) {
                errormsg = errormsg + "Please Select Employee <br/>";
                iserror = true;
            }

            if (_empmnttype == "" || _empmnttype == "0" || _empmnttype == null) {
                errormsg = errormsg + "Please Select Employment Status <br/>";
                iserror = true;
            }

            if (effective_dt == "" || effective_dt == null || effective_dt == undefined) {
                errormsg = errormsg + "Please Effective From Date <br/>";
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);

                return false;
            }


            $('#loader').show();
            var mydata = {

                default_company_id: company_id,
                employee_id: employee_id,
                current_employee_type: _empmnttype,
                effective_empmnt_type_dt: effective_dt,
                created_by: emp_idd
            };


            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();
            $.ajax({

                url: localStorage.getItem("ApiUrl") + "/apiEmployee/UpdateEmpEmployementType",
                type: "POST",
                data: JSON.stringify(mydata),
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

    }, 2000);// end timeout

});


$("#btnreset").bind("click", function () {
    location.reload();
});

function Get_EmployeementTypeDetail(employee_id) {

    if (employee_id > 0) {
        $('#loader').show();
        $('#tbl_emptype_dtl').DataTable({
            "processing": true,
            "serverSide": false,
            "bDestroy": true,
            "orderMulti": false,
            "filter": true,
            "scrollX": 200,
            ajax: {
                url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeement_Type_Master/' + employee_id,
                type: 'GET',
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                dataType: "json",
                dataSrc: function (json) {
                    $("#loader").hide();
                    var res = json;
                    if (res.statusCode != undefined) {
                        messageBox("info", res.message);
                        return;
                    }
                    return res;
                },
                error: function (err) {
                    $("#loader").hide();
                    $("#div_tbl").hide();
                    messageBox("info", err.responseText);
                    return false;
                }
            },
            "columnDefs": [

                {
                    targets: [4],
                    render: function (data, type, row) {

                        var date = new Date(data);
                        return GetDateFormatddMMyyyy(date);
                    }
                },
                //{
                //    targets: [5],
                //    render: function (data, type, row) {

                //        var date = new Date(data);
                //        return GetDateFormatddMMyyyy(date);
                //    }
                //},
                {
                    targets: [5],
                    render: function (data, type, row) {
                        debugger;
                        var date = new Date(data);
                        return new Date(row.last_modified_date) < new Date(row.effective_date) ? "#" : GetDateFormatddMMyyyy(date);
                    }
                }
            ],
            columns: [
                { "data": null, "title": "S.No.", "autoWidth": true },
                { "data": "emp_code", "title": "Employee Code", "autoWidth": true },
                { "data": "emp_name", "title": "Employee Name", "autoWidth": true },
                {
                    "title": "Employment Type", "autowidth": true, "render": function (data, type, full, meta) {
                        return '<a href="#" onClick=GetEmptypeDetails(' + full.employee_id + ',' + full.emptypeid + ')>' + full.emptypename + '</a>'
                    }
                },
               // { "data": "emptypename", "title": "Employeement Type", "autoWidth": true },
                { "data": "effective_date", "title": "Effective Date", "autoWidth": true },
               // { "data": "created_date", "title": "Created On", "autoWidth": true },
                { "data": "last_modified_date", "title": "Modified On", "autowidth": true },

            ],
            "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                return nRow;
            },
            "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
        });
    }
}
function GetEmptypeDetails(emp_id, type_id) {

    $('#loader').show();

    $("#myemptypeModal").show();
    var modal = document.getElementById("myemptypeModal");
    modal.style.display = "block";

    $('#myemptypeModal').dialog({
        modal: 'true',
        title: 'Employment Type Detail'
    });
   
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/GetEmployeement_Type_Details/" + emp_id + "/" + type_id,
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

            $("#tbl_emptype").DataTable({
                "processing": true,
                "serverSide": false,
                "bDestroy": true,
                "orderMulti": false,
                "filter": true,
                "scrollX": 200,              
                "aaData": res,
                //dom: 'lBfrtip',
                //buttons: [
                //    {
                //        text: 'Export to Excel',
                //        title: 'Leave Type Detail',
                //        extend: 'excelHtml5',
                //        exportOptions: {
                //            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                //        }
                //    },
                //],
                "columnDefs": [
                    //{
                    //    targets: [3],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return GetDateFormatddMMyyyy(date);
                    //    }
                    //},
                    //{
                    //    targets: [4],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return GetDateFormatddMMyyyy(date);
                    //    }
                    //},
                   
                    {
                        targets: [3],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return new Date(row.last_modified_date) < new Date(row.effective_date) ? "#" : GetDateFormatddMMyyyy(date);
                        }
                    }
                ],
                "columns": [
                    { "data": null, "title": "SNo.", "autoWidth": true },
                    { "data": "emp_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "emptypename", "title": "Employeement Type", "autoWidth": true },
                   // { "data": "effective_date", "title": "Effective Date", "autoWidth": true },
                    //{ "data": "created_date", "title": "Created On", "autoWidth": true },
                    { "data": "last_modified_date", "title": "Modified On", "autowidth": true },
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[15, 50, -1], [15, 50, "All"]],
            });

            $('#loader').hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}