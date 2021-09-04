
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
                return false;
            }
            if (res != null) {

                $("#tblReport").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once
                    //"scrollY": 200,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Employee Maker Statuory Secation Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22]
                            }
                        },
                    ],
                    "aaData": res,
                    "columnDefs":
                        [
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
                            },
                        ],

                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
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
                        { "data": "bank_name", "name": "bank_name", "title": "Bank", "autoWidth": true },
                        { "data": "bank_details.bank_acc", "name": "bank_details.bank_acc", "title": "Acc Number", "autoWidth": true },
                        { "data": "bank_details.ifsc_code", "name": "bank_details.ifsc_code", "title": "IFSC Code", "autoWidth": true },
                        { "data": "bank_details.branch_name", "name": "bank_details.branch_name", "title": "Branch Name", "autoWidth": true },
                        { "data": "bank_details.payment_mode", "name": "bank_details.payment_mode", "title": "Payment Mode", "autoWidth": true },

                    ],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },
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
