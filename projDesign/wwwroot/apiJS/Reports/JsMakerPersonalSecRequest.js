
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
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeePersonalDetailsReq',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;

            if (res.statusCode != undefined) {
                messageBox("error", res.message);
                $("#loader").hide();
                return false;
            }
            //debugger;
            if (res != null) {

                $("#tblReport").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once
                    //"scrollY": 200,
                    "aaData": res,
                    dom: 'lBfrtip',
                    buttons: [
                        {
                            text: 'Export to Excel',
                            title: 'Maker Personal Section Report',
                            extend: 'excelHtml5',
                            exportOptions: {
                                columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18]
                            }
                        },
                    ],
                    "columnDefs":
                        [
                            {
                                targets: [3],
                                render: function (data, type, row) {
                                    return data == 1 ? "A+" : data == 2 ? "O+" : data == 3 ? "B+" : data == 4 ? "AB+" : data == 5 ? "A-" : data == 6 ? "O-" : data == 7 ? "B-" : data == 8 ? "AB-" : "";
                                }
                            },
                        ],

                    "columns": [
                        { "data": null, "title": "S.No.", "autoWidth": true },
                        { "data": "emp_code_", "name": "emp_code_", "title": "Employee Code", "autoWidth": true },
                        { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                        { "data": "employeePersonalDetailData.blood_group", "name": "blood_group", "title": "Blood Group", "autoWidth": true },
                        { "data": "employeePersonalDetailData.primary_contact_number", "name": "primary_contact_number", "title": "Contact Number", "autoWidth": true },
                        { "data": "employeePersonalDetailData.primary_email_id", "name": "primary_email_id", "title": "Email", "autoWidth": true },
                        { "data": "employeePersonalDetailData.permanent_address_line_one", "name": "permanent_address_line_one", "title": "Address", "autoWidth": true },
                        { "data": "permanentCity", "name": "permanent_city", "title": "City", "autoWidth": true },
                        { "data": "employeePersonalDetailData.permanent_pin_code", "name": "permanent_pin_code", "title": " Pin Code", "autoWidth": true },
                        { "data": "permanentState", "name": "permanent_state", "title": "State", "autoWidth": true },
                        { "data": "permanentCountry", "name": "permanent_country", "title": "Country", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_name", "name": "emergency_contact_name", "title": "Emergency Contact Person", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_relation", "name": "emergency_contact_relation", "title": "Emergency Contact Relation", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_mobile_number", "name": "emergency_contact_mobile_number", "title": "Emergency Contact Number", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_line_one", "name": "emergency_contact_line_one", "title": "Emergency Contact Address", "autoWidth": true },
                        { "data": "emergencyCity", "name": "emergency_contact_city", "title": "Emergency Contact City", "autoWidth": true },
                        { "data": "employeePersonalDetailData.emergency_contact_pin_code", "name": "emergency_contact_pin_code", "title": "Emergency Contact Pin Code", "autoWidth": true },
                        { "data": "emergencyState", "name": "emergency_contact_state", "title": "Emergency Contact State", "autoWidth": true },
                        { "data": "emergencyCountry", "name": "emergency_contact_country", "title": "Emergency Contact Country", "autoWidth": true }

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

        }
    });

}
