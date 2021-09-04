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

        GetData();
        //$('#btnsave').bind("click", function () {
        //    GetData();
        //});


    }, 2000);// end timeout

});


function GetData() {
    $('#loader').show();
    var emp_id = $("#ddlemployee").val();

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
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();


    var apiurl = localStorage.getItem("ApiUrl") + 'apiEmployee/GetEmployeeDumpData';
    $('#loader').show();
    $.ajax({
        type: "POST",
        url: apiurl,
        data: JSON.stringify(mydata),
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            _GUID_New();
            var aData = res;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }


            $("#tblleaveapp").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once              
                "scrollX": 800,
                dom: 'lBfrtip',
                buttons: [
                    {
                        text: 'Export to Excel',
                        title: 'Employee Dump Data',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75]
                        }
                    },
                ],
                "aaData": aData,
                "columnDefs":
                    [

                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "company_name", "name": "leave_type_name", "title": "Company Name", "autoWidth": true },
                    { "data": "employee_code", "name": "employee_code", "title": "Employee Code", "autoWidth": true },
                    { "data": "status", "name": "Status", "title": "Status", "autoWidth": true },
                    { "data": "salutation", "name": "Salutation", "title": "Salutation", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "card_number", "name": "card_number", "title": "Card Number", "autoWidth": true },
                    { "data": "gender", "name": "gender", "title": "Gender", "autoWidth": true },
                    { "data": "group_joining_date", "name": "group_joining_date", "title": "Group Joining Date", "autoWidth": true },
                    { "data": "date_of_joining", "name": "date_of_joining", "title": "Date of joining", "autoWidth": true },
                    { "data": "date_of_birth", "name": "date_of_birth", "title": "Date of Birth", "autoWidth": true },
                    { "data": "manager_code", "name": "manager_code", "title": "Manager Code", "autoWidth": true },
                    { "data": "manager_name", "name": "manager_name", "title": "Manager Name", "autoWidth": true },
                    { "data": "department_joining_date", "name": "department_joining_date", "title": "Department Joining Date", "autoWidth": true },
                    { "data": "religion", "name": "religion", "title": "Religion", "autoWidth": true },
                    { "data": "marital_status", "name": "marital_status", "title": "Marital Status", "autoWidth": true },
                    { "data": "official_email_id", "name": "official_email_id", "title": "Official Email Id", "autoWidth": true },
                    { "data": "location_name", "name": "location_name", "title": "Location Name", "autoWidth": true },
                    { "data": "sub_location", "name": "sub_location", "title": "Sub Location", "autoWidth": true },
                    { "data": "department_name", "name": "department_name", "title": "Department Name", "autoWidth": true },
                    { "data": "sub_department", "name": "sub_department", "title": "Sub Department", "autoWidth": true },
                    { "data": "nationality", "name": "nationality", "title": "Nationality", "autoWidth": true },
                    { "data": "ot_allowed", "name": "ot_allowed", "title": "OT Allowed", "autoWidth": true },
                    { "data": "compoff_allwoed", "name": "compoff_allwoed", "title": "Compoff Allwoed", "autoWidth": true },
                    { "data": "punch_type", "name": "punch_type", "title": "Punch Type", "autoWidth": true },
                    { "data": "role_name", "name": "role_name", "title": "Role Name", "autoWidth": true },
                    { "data": "weekoff", "name": "weekoff", "title": "Week off", "autoWidth": true },
                    { "data": "current_employment_type", "name": "current_employment_type", "title": "Current Employment Type", "autoWidth": true },
                    { "data": "last_working_date", "name": "last_working_date", "title": "Last working Date", "autoWidth": true },
                    { "data": "blood_group", "name": "blood_group", "title": "Blood Group", "autoWidth": true },
                    { "data": "primary_contact_no", "name": "primary_contact_no", "title": "Primary Contact No.", "autoWidth": true },
                    { "data": "secondary_contact_no", "name": "secondary_contact_no", "title": "Secondary Contact No.", "autoWidth": true },
                    { "data": "primary_email_id", "name": "primary_email_id", "title": "Primary Email ID", "autoWidth": true },
                    { "data": "secondary_email_id", "name": "secondary_email_id", "title": "Secondary Email ID", "autoWidth": true },
                    { "data": "permanent_address_line1", "name": "permanent_address_line1", "title": "Permanent Address Line 1", "autoWidth": true },
                    { "data": "permanent_address_line2", "name": "permanent_address_line2", "title": "Permanent Address Line 2", "autoWidth": true },
                    { "data": "permanent_pin_code", "name": "permanent_pin_code", "title": "Permanent PIN Code", "autoWidth": true },
                    { "data": "permanent_country", "name": "permanent_country", "title": "Permanent Country", "autoWidth": true },
                    { "data": "permanent_state", "name": "permanent_state", "title": "Permanent State", "autoWidth": true },
                    { "data": "permanent_city", "name": "permanent_city", "title": "Permanent City", "autoWidth": true },
                    { "data": "permanent_document_type", "name": "permanent_document_type", "title": "Permanent Document Type", "autoWidth": true },
                    { "data": "corresponding_address_line1", "name": "corresponding_address_line1", "title": "Corresponding Address Line 1", "autoWidth": true },
                    { "data": "corresponding_address_line2", "name": "corresponding_address_line2", "title": "Corresponding Address Line 2", "autoWidth": true },
                    { "data": "corresponding_pin_code", "name": "corresponding_pin_code", "title": "Corresponding PIN Code", "autoWidth": true },
                    { "data": "corresponding_country", "name": "corresponding_country", "title": "Corresponding Country", "autoWidth": true },
                    { "data": "corresponding_state", "name": "corresponding_state", "title": "Corresponding State", "autoWidth": true },
                    { "data": "corresponding_city", "name": "corresponding_city", "title": "Corresponding City", "autoWidth": true },
                    { "data": "corresponding_document_type", "name": "corresponding_document_type", "title": "Corresponding Document Type", "autoWidth": true },
                    { "data": "emergency_contact_name", "name": "emergency_contact_name", "title": "Emergency Contact Name", "autoWidth": true },
                    { "data": "emergency_contact_relation", "name": "emergency_contact_relation", "title": "Emergency Contact Relation", "autoWidth": true },
                    { "data": "emergency_contact_mobile_no", "name": "emergency_contact_mobile_no", "title": "Emergency Contact Mobile No.", "autoWidth": true },
                    { "data": "emergency_address_line1", "name": "emergency_address_line1", "title": "Emergency Address Line 1", "autoWidth": true },
                    { "data": "emergency_address_line2", "name": "emergency_address_line2", "title": "Emergency Address Line 2", "autoWidth": true },
                    { "data": "emergency_pin_code", "name": "emergency_pin_code", "title": "Emergency PIN Code", "autoWidth": true },
                    { "data": "emergency_country", "name": "emergency_country", "title": "Emergency Country", "autoWidth": true },
                    { "data": "emergency_state", "name": "emergency_state", "title": "Emergency State", "autoWidth": true },
                    { "data": "emergency_city", "name": "emergency_city", "title": "Emergency City", "autoWidth": true },
                    { "data": "emergency_document_type", "name": "emergency_document_type", "title": "Emergency Document Type", "autoWidth": true },
                    { "data": "uan_number", "name": "uan_number", "title": "UAN Number", "autoWidth": true },
                    { "data": "pf_applicable", "name": "pf_applicable", "title": "PF Applicable", "autoWidth": true },
                    { "data": "pf_number", "name": "pf_number", "title": "PF Number", "autoWidth": true },
                    { "data": "pf_group", "name": "pf_group", "title": "PF Group", "autoWidth": true },
                    { "data": "pf_ceiling", "name": "pf_ceiling", "title": "PF Ceiling", "autoWidth": true },
                    { "data": "is_vpf_applicable", "name": "is_vpf_applicableVPF Group", "title": "IS VPF Applicable", "autoWidth": true },
                    { "data": "vpf_group", "name": "vpf_group", "title": "VPF Group", "autoWidth": true },
                    { "data": "vpf_amount", "name": "vpf_amount", "title": "VPF Amount", "autoWidth": true },
                    { "data": "eps_applicable", "name": "eps_applicable", "title": "EPS Applicable", "autoWidth": true },
                    { "data": "is_esic_applicable", "name": "is_esic_applicable", "title": "IS ESIC Applicable", "autoWidth": true },
                    { "data": "esic_no", "name": "esic_no", "title": "ESIC No.", "autoWidth": true },
                    { "data": "pan_card_name", "name": "pan_card_name", "title": "PAN Card Name", "autoWidth": true },
                    { "data": "pan_card_no", "name": "pan_card_no", "title": "PAN Card No.", "autoWidth": true },
                    { "data": "aadhar_card_name", "name": "aadhar_card_name", "title": "Aadhar Card Name", "autoWidth": true },
                    { "data": "aadhar_card_no", "name": "aadhar_card_no", "title": "Aadhar Card No.", "autoWidth": true },
                    { "data": "bank_name", "name": "bank_name", "title": "Bank Name", "autoWidth": true },
                    { "data": "account_no", "name": "account_no", "title": "Account No.", "autoWidth": true },
                    { "data": "ifsc_code", "name": "ifsc_code", "title": "IFSC Code", "autoWidth": true },
                    { "data": "branch_name", "name": "branch_name", "title": "Branch Name", "autoWidth": true },
                    { "data": "payment_mode", "name": "payment_mode", "title": "Payment Mode", "autoWidth": true },
                    { "data": "confirmation_date", "name": "confirmation_date", "title": "Confirmation Date", "autoWidth": true },
                    { "data": "grade_name", "name": "grade_name", "title": "Grade", "autoWidth": true },
                    { "data": "designation_name", "name": "designation_name", "title": "Designation", "autoWidth": true },
                    { "data": "notice_period", "name": "notice_period", "title": "Notice Period", "autoWidth": true },
                    { "data": "emp_type", "name": "emp_type", "title": "Employement Type", "autoWidth": true },
                ],
                "lengthMenu": [[10, 50, -1], [10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });
            $('#loader').hide();
        },
        error: function (error) {
            _GUID_New();
            alert(error.responseText);
            $('#loader').hide();
        }
    });
    $('#loader').hide();
}
