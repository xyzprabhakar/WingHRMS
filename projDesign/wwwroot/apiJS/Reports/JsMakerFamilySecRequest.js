
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
    //debugger;

    $('#loader').show();
    $.ajax({
        type: "GET",
        // url: ,
        url: localStorage.getItem("ApiUrl") + 'apiEmployee/GetFamilySectionReq',
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            //// debugger;;
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
                        title: 'Employee Maker Section Report',
                        extend: 'excelHtml5',
                        exportOptions: {
                            columns: [1, 2, 3, 4, 5, 6, 7, 8]
                        }
                    },
                ],
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [6],
                            render: function (data, type, row) {


                                return data == 1 ? "Male" : data == 2 ? "Female" : data == 3 ? "Other" : "";
                            }
                        },
                        //{
                        //    targets: [7],
                        //    "class": "text-center"
                        //}
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    { "data": "emp_code_", "name": "emp_code_", "title": "Employee Code", "autoWidth": true },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "em.name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name As Per Aadhar Card", "autoWidth": true },
                    { "data": "em.name_as_per_aadhar_card", "name": "name_as_per_aadhar_card", "title": "Name", "autoWidth": true },
                    { "data": "em.relation", "name": "relation", "title": "Relation", "autoWidth": true },
                    { "data": "em.gender", "name": "gender", "title": "Gender", "autoWidth": true },
                    { "data": "em.occupation", "name": "occupation", "title": "Occupation", "autoWidth": true },
                    { "data": "em.date_of_birth", "name": "date_of_birth", "title": "Date of Birth", "autoWidth": true },
                    // { "data": "remark", "name": "passing_year", "title": "Remark", "remark": true },
                    // { "data": "created_date", "name": "created_date", "title": "Created On", "autoWidth": true },
                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.emp_family_section_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //}
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
        },
        error: function (error) {
            messageBox("error", error.responseText);

        }
    });

}
