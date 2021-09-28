$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        var emp_role_id1 = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        var company_idd1 = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        console.log(emp_role_id1);

        BindEmployee_ByCompID_dir(company_idd1, emp_role_id1);

    }, 2000);// end timeout

});

function BindEmployee_ByCompID_dir(company_idd1, emp_role_id1) {
    $('#loader').show();
    // // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_Employee_from_all_Company_dir/" + company_idd1 + "/" + emp_role_id1,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $('#loader').hide();
                return false
            }

            $("#tblemplistt_dir").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,
                // dom: 'Bfrtip',
                //buttons: [
                //    {
                //        text: 'Export to Excel',
                //        title: 'Company List',
                //        extend: 'excelHtml5',
                //        exportOptions: {
                //            columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11]
                //        }
                //    },
                //],
                "columnDefs":
                    [
                        //{
                        //    targets: [9],
                        //    render: function (data, type, row) {
                        //        return data == '1' ? 'Active' : 'InActive'
                        //    }
                        //},
                        //{
                        //    targets: [10],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [11],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return new Date(row.last_modified_date) < new Date(row.created_date) ? "-" : GetDateFormatddMMyyyy(date);
                        //    }
                        //}
                    ],
                "columns": [
                    { "data": null, "title": "S.No." },
                    {
                        "title": "Profile Pic",
                        "render": function (data, type, full, meta) {
                            var empimg = "";
                            if (full.emp_img == "") {
                                empimg = "EmployeeImage/DefaultUser/defaultimage.jpg";
                            }
                            else {
                                empimg = full.emp_img;
                            }
                            return '<img src="' + localStorage.getItem("ApiUrl").replace("api/", "") + empimg + '" style="width:80;height:80" />';
                        }
                    },
                    { "data": "emp_code", "name": "emp_code", "autoWidth": true, "title": "Employee Code" },
                    { "data": "emp_name", "name": "emp_name", "autoWidth": true, "title": "Employee Name" },
                    { "data": "official_email_id", "name": "official_email_id", "autoWidth": true, "title": "Official Email" },
                    { "data": "email", "name": "email", "autoWidth": true, "title": "Email-Id" },
                    { "data": "official_contact_no", "name": "official_contact_no", "autoWidth": true, "title": "Official Number" },
                    { "data": "mobileno", "name": "mobileno", "autoWidth": true, "title": "Phone Number" },                    
                    { "data": "location_name", "name": "location_name", "autoWidth": true, "title": "Location" },
                    { "data": "dept_name", "name": "dept_name", "autoWidth": true, "title": "Department" },
                    { "data": "desig_name", "name": "desig_name", "autoWidth": true, "title": "Designation" },

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[20, 50, 100, -1], [20, 50, 100, "All"]]

            });

            $('#loader').hide();
        },
        error: function (error) {
            messageBox("error", error.responseText);
            //messageBox("error", "Server busy please try again later...!");
            $('#loader').hide();
        }
    });
}