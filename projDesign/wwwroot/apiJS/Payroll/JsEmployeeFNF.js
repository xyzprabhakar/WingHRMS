var login_company_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {



        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        login_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        GetData();

    }, 2000);// end timeout

});

function GetData() {
    if ($.fn.DataTable.isDataTable('#tbl_emp_resigns')) {
        $('#tbl_emp_resigns').DataTable().clear().draw();
    }


    $("#loader").show();
    $('#tbl_emp_resigns').DataTable({
        "processing": true,
        "serverSide": false,
        "bDestroy": true,
        "orderMulti": false,
        "filter": true,
        "scrollX": 200,
        ajax: {
            url: localStorage.getItem("ApiUrl") + "apiEmployee/Get_EmployeeSeprationDetails/-1/" + login_emp_id,
            type: 'GET',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            dataType: "json",
            dataSrc: function (json) {
                $("#loader").hide();
                if (json.statusCode != undefined) {
                    messageBox("error", json.message);
                    return false;
                }
                return json;
            },
            error: function (err) {
                $("#loader").hide();
                messageBox("error", err.responseText);
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
            {
                targets: [5],
                render: function (data, type, row) {

                    var date = new Date(data);
                    return GetDateFormatddMMyyyy(date);
                }
            },
            {
                targets: [6],
                render: function (data, type, row) {

                    var date = new Date(data);
                    return GetDateFormatddMMyyyy(date);
                }
            },
        ],
        columns: [
            { "data": null, "title": "S.No.", "autoWidth": true },
            { "data": "emp_code", "title": "Employee Code", "autoWidth": true },
            { "data": "emp_name", "title": "Employee Name", "autoWidth": true },
            { "data": "departmenet_", "title": "Department", "autoWidth": true },
            { "data": "doj", "title": "Date of Joining", "autoWidth": true },
            { "data": "resignation_dt", "title": "Resignation Date", "autoWidth": true },
            { "data": "final_relieve_dt", "title": "Final Relieve Date", "autoWidth": true },
            {
                "title": "Action", "autoWidth": true, "render": function (data, type, full, meta) {
                    
                    // var testt = CryptoJS.AES.encrypt("'" + full.emp_id + "/" + full.sepration_id + "'", localStorage.getItem("sit_id"));
                    // return '<a href="#" onclick="FNFProcesss_(' + testt.toString() + ')" title="View" style=" float: left;"><i class="fa fa-pencil-square-o"></i>View</a>  ';
                    if (full.final_status == "Approve")
                        return '<a href="../FNFProcess/FNF_Process?View=' + CryptoJS.AES.encrypt("'" + full.emp_id + "/" + full.sepration_id + "'", localStorage.getItem("sit_id")) + '" title="View" style=" float: left;"><i class="fa fa-pencil-square-o"></i>View</a>  ';
                    else
                        return '<a href="../FNFProcess/FNF_Process?View=' + CryptoJS.AES.encrypt("'" + full.emp_id + "/" + full.sepration_id + "'", localStorage.getItem("sit_id")) + '" title="View" enabled="false" style=" float: left;"><i class="fa fa-pencil-square-o"></i>View</a>  ';
                }
            }
        ],
        "fnRowCallback": function (nRow, aData, iDisplayIndex) {
            $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
            return nRow;
        },
        "lengthMenu": [[10, 50, -1], [10, 50, "All"]]
    });



}

function FNFProcesss_(pathh) {
    alert(pathh);
}
