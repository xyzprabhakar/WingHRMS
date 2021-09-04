var company_id;
var login_emp_id;

$(document).ready(function () {

    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindCompanyList('ddlcompany', company_id);

        BindEmployeeCodee('ddlemployee', company_id, login_emp_id);

        GetData(login_emp_id);

    }, 2000);// end timeout

});


function BindEmployeeCodee(ControlId, CompanyId, SelectedVal) {
    ControlId = '#' + ControlId;
    var data = JSON.parse(localStorage.getItem("emp_under_login_emp")).filter(p => p._empid == SelectedVal);
    $(ControlId).append($("<option></option>").val(data[0]._empid).html(data[0].emp_name_code));

}


function GetData(login_emp_id) {
    $('#loader').show();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiEmployee/Get_health_card_masterByEmpId/" + login_emp_id,
        type: "GET",
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                $("#loader").hide();
                return false;
            }

            $("#tblhealthcard").DataTable({
                "processing": false,//for show progress bar
                "serverSide": false,// for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at one
                "scrollX": 200,
                "aaData": response,
                "columnDefs": [
                    {
                        targets: [5],
                        render: function (data, type, row) {

                            var date = new Date(data);
                            return GetDateFormatddMMyyyy(date);
                        }
                    }
                    //{
                    //    targets: [6],
                    //    render: function (data, type, row) {

                    //        var date = new Date(data);
                    //        return GetDateFormatddMMyyyy(date);

                    //    }
                    //}

                ],
                "columns": [

                    { "data": null, "title": "S.No", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "title": "Company", "autoWidth": true },
                    { "data": "emp_name_code", "name": "emp_name_code", "title": "Employee", "autoWidth": true },
                    {
                        "title": "Action", "autoWidth": true,

                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="OpenDocumentt(' + full.employee_id + ')"><i class="fa fa-download"></i></a>';
                        }
                    },
                    { "data": "remarks", "name": "remarks", "title": "Remarks", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "title": "Created On", "autoWidth": true }
                    //{ "data": "modified_dt", "name": "modified_dt", "title": "Modified On", "autoWidth": true }

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]
            });

            $('#loader').hide();


        },
        error: function (error) {
            $('#loader').hide();

            alert(error.responseText);
        }
    });
}


function OpenDocumentt(emp_id) {
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_health_card_master/1/" + emp_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        data: {},
        success: function (response) {
            window.open(response.healthcard_path);
        },
        error: function (exception, error) {
            alert(error);
        }
    });
}

