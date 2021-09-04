
var company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

        //debugger;
        $('#loader').show();
        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }


        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        $('#loader').hide();

        GetData();


    }, 2000);// end timeout

});

function GetData() {

    $('#loader').show();
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeePfEsicAccDetailsReq',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;

            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                rest_all();
                return false;
            }
            var aData = res;

            if (aData.length > 0) {
                $("#leave_action_div").css("display", "block");
            }
            else {
                $("#leave_action_div").css("display", "none");
            }
            if (res != null) {

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
                                targets: 0,
                                orderable: false,
                                "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                            },

                            {
                                targets: [4],
                                render: function (data, type, row) {
                                    return data == 1 ? "Yes" : "No";
                                }
                            },
                            {
                                targets: [8],
                                render: function (data, type, row) {
                                    return data == 1 ? "Yes" : "No";
                                }
                            },
                            {
                                targets: [11],
                                render: function (data, type, row) {
                                    return data == 1 ? "Yes" : "No";
                                }
                            },
                            {
                                targets: [12],
                                render: function (data, type, row) {
                                    return data == 1 ? "Yes" : "No";
                                }
                            },
                            {
                                targets: [6],
                                render: function (data, type, row) {
                                    return data == 1 ? "Min Slab" : data == 2 ? "12% of Basic" : "";
                                }
                            },
                            {
                                targets: [9],
                                render: function (data, type, row) {
                                    return data == 1 ? "Fixed Amount" : data == 2 ? "Basic Percentage" : "";
                                }
                            },
                            {
                                targets: [22],
                                render: function (data, type, row) {
                                    return data == 1 ? "Bank Transfer" : data == 2 ? "Cheque" : data == 3 ? "Cash" : data == 4 ? "Demand Draft" : "";
                                }
                            }

                        ],

                    "columns": [
                        {
                            "render": function (data, type, full, meta) {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.employee_id + '" value="' + full.pf_details.pf_details_id + ',' + full.esic_details.esic_details_id + ',' + full.pan_details.pan_details_id + ',' + full.adhar_details.pan_details_id + ',' + full.bank_details.bank_details_id + '" />';
                            }
                        },
                        { "data": "emp_code_", "name": "emp_code_", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "pf_details.uan_number", "name": "pf_details.uan_number", "title": "UAN No.", "autoWidth": true },
                        { "data": "pf_details.is_pf_applicable", "name": "pf_details.is_pf_applicable", "title": "Is PF Applicable", "autoWidth": true },
                        { "data": "pf_details.pf_number", "name": "pf_details.pf_number", "title": "PF No.", "autoWidth": true },
                        { "data": "pf_details.pf_group", "name": "pf_details.pf_group", "title": "PF Group", "autoWidth": true },
                        { "data": "pf_details.pf_celing", "name": "pf_details.pf_celing", "title": "PF Celing", "autoWidth": true },
                        { "data": "pf_details.is_vpf_applicable", "name": "pf_details.is_vpf_applicable", "title": "Is VPF Applicable", "autoWidth": true },
                        { "data": "pf_details.vpf_Group", "name": "pf_details.vpf_Group", "title": "VPF Group", "autoWidth": true },
                        { "data": "pf_details.vpf_amount", "name": "pf_details.vpf_amount", "title": "VPF Amount", "autoWidth": true },
                        { "data": "pf_details.is_eps_applicable", "name": "pf_details.is_eps_applicable", "title": "Is EPS Applicable", "autoWidth": true },
                        { "data": "esic_details.is_esic_applicable", "name": "esic_details.is_esic_applicable", "title": "Is ESIC Applicable", "autoWidth": true },
                        { "data": "esic_details.esic_number", "name": "esic_details.esic_number", "title": "ESIC Number", "autoWidth": true },
                        { "data": "pan_details.pan_card_name", "name": "pan_details.pan_card_name", "title": "PAN Card Name", "autoWidth": true },
                        { "data": "pan_details.pan_card_number", "name": "pan_details.pan_card_number", "title": "PAN Card Number", "autoWidth": true },
                        { "data": "adhar_details.aadha_card_name", "name": "adhar_details.aadha_card_name", "title": "Adhar Card Name", "autoWidth": true },
                        { "data": "adhar_details.aadha_card_number", "name": "adhar_details.aadha_card_number", "title": "Adhar Card Number", "autoWidth": true },
                        { "data": "bank_details.bank_id", "name": "bank_details.bank_id", "title": "Bank Id", "autoWidth": true },
                        { "data": "bank_details.bank_acc", "name": "bank_details.bank_acc", "title": "Acc Number", "autoWidth": true },
                        { "data": "bank_details.ifsc_code", "name": "bank_details.ifsc_code", "title": "IFSC Code", "autoWidth": true },
                        { "data": "bank_details.branch_name", "name": "bank_details.branch_name", "title": "Branch Name", "autoWidth": true },
                        { "data": "bank_details.payment_mode", "name": "bank_details.payment_mode", "title": "Payment Mode", "autoWidth": true },

                    ],
                    "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

                });
            }

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            $('#loader').hide();
            //messageBox("error", "Server busy please try again later...!");
        }
    });

}

function selectAll() {
    //debugger;
    var chkAll = $('#selectAll');

    var allPages = $('#tblReport').DataTable().cells().nodes();
    var currentPages = $('#tblReport').DataTable().rows({ page: 'current' }).nodes();

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblReport").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(allPages).find('.chkRow').prop('checked', false);
            $(currentPages).find('.chkRow').prop('checked', true);
        }
        else {
            $(allPages).find('.chkRow').prop('checked', false);
        }
    });
}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblReport").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}

$('#BtnSave').bind('click', function () {
    //debugger;
    $('#loader').show();
    var pfIds = [];
    var esicIds = [];
    var panIds = [];
    var adharIds = [];
    var bankIds = [];
    var table = $("#tblReport").dataTable();
    $("input:checkbox", table.fnGetNodes()).each(function () {
        if ($(this).is(":checked")) {
            var no = $(this).val();
            pfIds.push(no.split(",")[0]);
            esicIds.push(no.split(",")[1]);
            panIds.push(no.split(",")[2]);
            adharIds.push(no.split(",")[3]);
            bankIds.push(no.split(",")[4]);
        }
    });

    //cehck validation part


    var action_type = $('#ddlaction_type').val();
    if (action_type == null || action_type == '' || action_type == 2) {
        alert("Please select action type ...!");
        $('#loader').hide();
        return false;
    }


    if (confirm("Total " + pfIds.length + " application selected to Process. \nDo you want to process this?")) {

        if (pfIds == null || pfIds == '' || pfIds.length <= 0) {
            alert('There some problem please try after later...!');
            $('#loader').hide();
            return false;
        }


        var myData = {
            'pf_Ids': pfIds,
            'esic_Ids': esicIds,
            'pan_Ids': panIds,
            'adhar_Ids': adharIds,
            'bank_Ids': bankIds,
            'is_deleted': action_type
        };

        var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/ApproveEmpStatutoryRequests/';
        var Obj = JSON.stringify(myData);

        var headerss = {};
        headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
        headerss["salt"] = $("#hdnsalt").val();

        $('#loader').show();
        $.ajax({
            type: "POST",
            url: apiurl,
            data: Obj,
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: headerss,
            success: function (res) {
                //debugger;
                data = res;
                var statuscode = data.statusCode;
                var Msg = data.message;
                $('#loader').hide();
                _GUID_New();
                if (statuscode == "0") {
                    messageBox("success", Msg);
                    GetData();
                    $("#ddlaction_type").val('2');
                }
                else {
                    messageBox("error", Msg);
                }

            },
            error: function (request, status, error) {
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
                $('#loader').hide();
            }

        });
    }
    else {
        $('#loader').hide();
        return false;
    }
});
