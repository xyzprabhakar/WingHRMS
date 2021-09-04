$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        

    var token = localStorage.getItem('Token');
    if (token == null) {
        window.location = '/Login';
    }
    // debugger;
    GetAllCategory(0);    


    login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    BindAllEmp_Company('ddlCompany', login_emp_id, default_company);
    BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', default_company, 0);


    BindGradeList('ddlGrade', 0)


    GetGradeLimitMasterReport(0, 0, 0, 0);

    getCurrentFinancialYear();



    // Get Quarter On Financial Year Change
    //$('#ddlfyi').bind("change", function () {
    //    BindQuarterList('ddlquarter', 0, $(this).val());
    //});
    //GetData();

    //$('#btnreset').hide();
    //$('#btnsave').hide();
    $('#btnupdate').hide();
    $('#btnsave').show();
        $('#loader').hide();

    }, 2000);// end timeout

});

$('#ddlCompany').bind("change", function () {
    $('#loader').show();
    $("#ddlEmployee option").remove();

    BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', $(this).val(), 0);

    $('#loader').hide();
});

function getCurrentFinancialYear() {

    // debugger;
    var fiscalyear = "";
    var today = new Date();
    if ((today.getMonth() + 1) <= 3) {
        fiscalyear = (today.getFullYear() - 1) + "-" + today.getFullYear()
    } else {
        fiscalyear = today.getFullYear() + "-" + (today.getFullYear() + 1)
    }
    $('#txtfiscal').val(fiscalyear);
}

//function BindCompanyList(ControlId, SelectedVal) {
//    $('#loader').show();
//    ControlId = '#' + ControlId;
//    var default_company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
//    var HaveDisplay = JSON.parse(localStorage.getItem('menu_filter_comp_')).findIndex(data => data.text == "Display Company List");
//    if (HaveDisplay == -1) {

//        $(ControlId).empty().append($("<option></option>").val(default_company_id).html("Default Company"));
//    }
//    else {

//        $('#loader').show();
//        $.ajax({
//            type: "GET",
//            url: apiurl + "apiMasters/Get_CompanyList",
//            data: {},
//            contentType: "application/json",
//            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//            dataType: "json",
//            success: function (response) {
//                var res = response;

//                $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//                $.each(res, function (data, value) {
//                    $(ControlId).append($("<option></option>").val(value.companyId).html(value.companyName));
//                })

//                //get and set selected value
//                if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                    $(ControlId).val(SelectedVal);
//                }

//                $('#loader').hide();
//            },
//            error: function (err) {
//                alert(err.responseText);
//                $('#loader').hide();
//            }
//        });
//    }
//    //ControlId = '#' + ControlId;
//    //$.ajax({
//    //    type: "GET",
//    //    url: localStorage.getItem("ApiUrl") + "apiMasters/Get_CompanyList",
//    //    data: {},
//    //    contentType: "application/json",
//    //    headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//    //    dataType: "json",
//    //    success: function (response) {
//    //        var res = response;

//    //        $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
//    //        $.each(res, function (data, value) {
//    //            $(ControlId).append($("<option></option>").val(value.companyId).html(value.companyName));
//    //        })
//    //        //get and set selected value
//    //        if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//    //            $(ControlId).val(SelectedVal);
//    //        }

//    //        $('#loader').hide();
//    //    },
//    //    error: function (err) {
//    //        $('#loader').hide();
//    //        alert(err.responseText);
//    //    }
//    //});
//}

function GetProcessedMonthyear() {

    $('#loader').show();
    // debugger;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apipayroll/Get_ProcessedMonthyear/' + $('#ddlCompany').val(),
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;


            // console.log(JSON.stringify(response))
            $("#txtmonthyear").val('');

            $("#txtmonthyear").val(response);

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}
function BindGradeList(ControlId, SelectedVal) {

    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_GradeMasterData/0",
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="0">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.sno).html(value.gradename));
            })

            //get and set selected value
            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
                $(ControlId).val(SelectedVal);
            }

            $('#loader').hide();
        },
        error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });
}
function GetAllCategory(category_id) {
    $('#loader').show();
    // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apipayroll/Get_ReimbursementCategoryMasterActive/" + category_id + "",
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#tblCategory').dataTable().fnClearTable();
            $('#tblCategory').dataTable().fnDestroy();

            $('#loader').hide();

            $("#tblCategory").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,
                "columnDefs":
                    [
                        
                        //},
                        //{
                        //    targets: [6],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //},
                        //{
                        //    targets: [8],
                        //    render: function (data, type, row) {

                        //        var date = new Date(data);
                        //        return GetDateFormatddMMyyyy(date);
                        //    }
                        //}
                    ],

                "columns": [
                    { "data": null },
                    { "data": "rcm_id", "name": "rcm_id", "autoWidth": true,"visible":false },
                    { "data": "reimbursement_category_name", "name": "reimbursement_category_name", "autoWidth": true },
                    {
                        render: function (data, type, row) {
                            return '<input style="width:100px" class="form-control"  id=' + row.rcm_id + '_monthyly_limit  maxlength="8" name=' + row.rcm_id + '_monthyly_limit type="text" value = "0" >';
                        }
                    },
                    {
                        render: function (data, type, row) {
                            return '<input style="width:100px" class="form-control"  id=' + row.rcm_id + '_yearly_limit maxlength="8" name=' + row.rcm_id + '_monthyly_limit type="text" value = 0 >';
                        }
                    },

                    //{
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" class="trigger_popup_fricc" onclick="ShowSalaryGroupKeyused(' + full.group_id + ')" >View</a>';
                    //        //'< a  onclick = "DeleteSalaryGroup(' + full.group_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                    //    }
                    //}
                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
            function activeFormatter(cellvalue, options, rowObject) {
                if (cellvalue == 1)
                    return "Active";
                else if (cellvalue == 0)
                    return "De-Active";
            }
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });
}
function EditCategory(category_id) {
    // debugger;
  
    if (category_id == null || category_id == '') {
        messageBox('info', 'There are some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    $("#hdnCategory").val(category_id);


    var apiurl = localStorage.getItem("ApiUrl") + "apipayroll/Get_ReimbursementGradeLimitMaster/" + category_id + "/0/0/0";
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;



            BindAllEmp_Company('ddlCompany', login_emp_id, data.company_id);
            BindEmployeeCodeFromEmpMasterByComp('ddlEmployee', data.company_id, data.emp_id);
            BindGradeList('ddlGrade', data.grade_id);
            $("#ddlActive").val(data.is_active);
            $("#txtfiscal").val(data.fiscal_year_id);
            $("#txtMonthly").val(data.monthly_limit);
            $("#txtYearly").val(data.yearly_limit);

            $('#btnupdate').show();
            $('#btnsave').hide();
          

            ShowEditCategoryList(category_id);
            openCity('click', 'tab1');

            $('#loader').hide();

        },
        Error: function (err) {
            $('#loader').hide();
            alert(err.responseText);
        }
    });

}

$('#btnreset').bind('click', function () {
    location.reload();
});

$('#btnsave').bind("click", function () {
    // debugger;
    $('#loader').show();
    var fiscal = $("#txtfiscal").val();

    var monthly = $("#txtMonthly").val();
    var yearly = $("#txtYearly").val();

    var emp_id = $('#ddlEmployee').val();

    var grade_id = $('#ddlGrade').val();
    var is_active = $('#ddlActive').val();

    var created_emp_id = login_emp_id;
    //var view = 0;
    var errormsg = '';
    var iserror = false;

    //validation part
    if (emp_id == '' && grade_id == '') {
        errormsg = "Please select Employee or Grade !! <br/>";
        iserror = true;
    }
    if (fiscal == '' ) {
        errormsg = errormsg + 'Please enter fiscal monthyear !! <br />';
        iserror = true;
    }
    if (monthly == '') {
        errormsg = errormsg + 'Please enter monthyly limit !! <br />';
        iserror = true;
    }
    if (yearly == '') {
        errormsg = errormsg + 'Please select yearly limit !! <br />';
        iserror = true;
    }
    if (is_active == '' || is_active == '2') {
        errormsg = errormsg + 'Please select Active Status !! <br />';
        iserror = true;
    }
    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }

    var categoryList = new Array()

    var table = $('#tblCategory').DataTable();
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
        var cat = {};
        cat.category_id = table.cell(rowIdx, 1).data();
        cat.monthly_limit = table.cell(rowIdx, 3).nodes().to$().find('input').val();
        cat.yearly_limit = table.cell(rowIdx, 4).nodes().to$().find('input').val();

        categoryList.push(cat);
    });

    if (emp_id == '')
    {
        emp_id = '0';
    }
    var myData = {
        'mdlrclm': categoryList,
        'grade_id': grade_id,
        'emp_id': emp_id,
        'monthly_limit': monthly,
        'yearly_limit': yearly,
        'fiscal_year_id': fiscal,
        'is_active': is_active,
        'created_by': created_emp_id,
        'modified_by': created_emp_id
        
    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Save_ReimbursementGradeLimitMaster';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (data) {

            // var resp = JSON.parse(data);
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                //$("#txtCategoryMaster").val('');
                //$('#ddlActive').val('2');
                //$('#btnupdate').hide();
                //$('#btnsave').show();
                //$("#hdgroupid").val('');
                //GetAllCategory(0);

                alert(Msg);
               // messageBox("success", Msg);
                window.location.href = '/payroll/RmbGrdLimitMaster';
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            messageBox("error",err.responseText);
        }
    });
});


//-------update city data
$("#btnupdate").bind("click", function () {
    $('#loader').show();
    // debugger;
    var fiscal = $("#txtfiscal").val();

    var monthly = $("#txtMonthly").val();
    var yearly = $("#txtYearly").val();

    var emp_id = $('#ddlEmployee').val();

    var grade_id = $('#ddlGrade').val();
    var is_active = $('#ddlActive').val();
    var created_emp_id = login_emp_id;
    //var view = 0;
    var errormsg = '';
    var iserror = false;
    var rglm_id = $("#hdnCategory").val();
    
  
    //validation part
    if (emp_id == '' && grade_id == '') {
        errormsg = "Please select Employee or Grade !! <br/>";
        iserror = true;
    }
    if (fiscal == '') {
        errormsg = errormsg + 'Please enter fiscal monthyear !! <br />';
        iserror = true;
    }
    if (monthly == '') {
        errormsg = errormsg + 'Please enter monthyly limit !! <br />';
        iserror = true;
    }
    if (yearly == '') {
        errormsg = errormsg + 'Please select yearly limit !! <br />';
        iserror = true;
    }
    if (is_active == '' || is_active == '2') {
        errormsg = errormsg + 'Please select Active Status !! <br />';
        iserror = true;
    }
    if (iserror) {
        messageBox("error", errormsg);
        $('#loader').hide();
        //  messageBox("info", "eror give");
        return false;
    }
   
    var categoryList = new Array()

    var table = $('#tblCategory').DataTable();
    table.rows().every(function (rowIdx, tableLoop, rowLoop) {
      
        var cat = {};
        cat.rglm_id = table.cell(rowIdx, 1).data();
        cat.category_id = table.cell(rowIdx, 5).data();
        cat.monthly_limit = table.cell(rowIdx, 3).nodes().to$().find('input').val();
        cat.yearly_limit = table.cell(rowIdx, 4).nodes().to$().find('input').val();

        categoryList.push(cat);
    });
   
    if (emp_id == '') {
        emp_id = '0';
    }
    var myData = {
        'mdlrclm': categoryList,
        'rglm_id': rglm_id,
        'grade_id': grade_id,
        'emp_id': emp_id,
        'monthly_limit': monthly,
        'yearly_limit': yearly,
        'fiscal_year_id': fiscal,
        'is_active': is_active,
        'created_by': created_emp_id,
        'modified_by': created_emp_id

    };
    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Update_ReimbursementGradeLimitMaster';
    var Obj = JSON.stringify(myData);
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: apiurl,
        type: "POST",
        data: Obj,
        dataType: "json",
        contentType: "application/json",
        headers: headerss,
        success: function (data) {

            // var resp = JSON.parse(data);
            var statuscode = data.statusCode;
            var Msg = data.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                //$("#txtCategoryMaster").val('');
                //$('#ddlActive').val('2');
                //$('#btnupdate').hide();
                //$('#btnsave').show();
                //$("#hdgroupid").val('');
                //GetAllCategory(0);
                alert(Msg);
                location.reload();
               // messageBox("success", Msg);
               // window.location.href = '/payroll/RmbGrdLimitMaster';
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            messageBox("error",err.responseText);
        }
    });
});


function GetGradeLimitMasterReport(category_id, grade_id, emp_id, fiscal_year_id) {
    $('#loader').show();
    // debugger;
    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apipayroll/Get_ReimbursementGradeLimitMaster/" + category_id + "/" + grade_id + "/" + emp_id + "/" + fiscal_year_id,
        type: 'GET',
        dataType: 'json',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblGradeLimit").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 150,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: [6],
                            render: function (data, type, row) {
                                return data == '1' ? 'Active' : 'De-Active'
                            }
                        }
                       
                    ],

                "columns": [
                    { "data": null },
                    { "data": "grade_name", "name": "grade_name", "autoWidth": true, },
                    { "data": "emp_code", "name": "emp_code", "autoWidth": true },
                    { "data": "fiscal_year_id", "name": "fiscal_year_id", "autoWidth": true },
                    { "data": "monthly_limit", "name": "monthly_limit", "autoWidth": true },
                    { "data": "yearly_limit", "name": "yearly_limit", "autoWidth": true },
                    { "data": "is_active", "name": "is_active", "autoWidth": true },

                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#"  onclick="ShowCategoryList(' + full.rglm_id + ')" >View</a>';
                            //'< a  onclick = "DeleteSalaryGroup(' + full.group_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    },
                    {
                        "render": function (data, type, full, meta) {

                            return '<a href="#" onclick="EditCategory(' + full.rglm_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                            //'< a  onclick = "DeleteSalaryGroup(' + full.group_id + ')" title = "Delete" > <i class="fa fa-trash"></i></a > ';
                        }
                    }

                ],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });
            function activeFormatter(cellvalue, options, rowObject) {
                if (cellvalue == 1)
                    return "Active";
                else if (cellvalue == 0)
                    return "De-Active";
            }
        },
        error: function (error) {
            $('#loader').hide();
            messageBox("error", error.responseText);
        }
    });

};
 function ShowCategoryList(category_id) {
        // debugger;
     $('#loader').show();
        if (category_id == null || category_id == '') {
            messageBox('info', 'There are some problem please try after later !!');
            $('#loader').hide();
            return false;
        }
        // Get the modal
        var modal = document.getElementById("myModal");

        // Get the <span> element that closes the modal
        // var span = document.getElementsByClassName("close")[0];

        // When the user clicks the button, open the modal
       
            modal.style.display = "block";
        

        var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementCategoryLimitMaster/0/' + category_id  ;
        
        $.ajax({
            type: "GET",
            url: apiurl,
            data: {},
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
            success: function (res) {
                $('#loader').hide();
                $("#tblcategoryList").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": true,
                    "filter": true, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once
                    "scrollX": 150,
                    "aaData": res,
                    "columns": [
                        //{
                        //    "render": function (data, type, row, meta) {
                        //        return meta.row + meta.settings._iDisplayStart + 1;
                        //    }
                        //},

                        { "data": null },
                        { "data": "rcm_id", "name": "rcm_id", "autoWidth": true, "visible": false },
                        { "data": "reimbursement_category_name", "name": "reimbursement_category_name", "autoWidth": true },
                        { "data": "monthly_limit", "name": "monthly_limit", "autoWidth": true, },
                        { "data": "yearly_limit", "name": "yearly_limit", "autoWidth": true, }

                    ],
                    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                        $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                        return nRow;
                    },
                    "lengthMenu": [[15, 50, -1], [15, 50, "All"]]


                });
            },
            error: function (error) {
                $('#loader').hide();
                alert(error.responseText);
            }
        });
    


}

function ShowEditCategoryList(category_id) {
    $('#loader').show();
    $("#tblCategory").DataTable().clear().draw();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + "/apiPayroll/Get_ReimbursementCategoryLimitMaster/0/" + category_id,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            if (res.length > 0) {
                $('#loader').hide();
                $("#tblCategory").DataTable({
                    "processing": true,// for show progress bar
                    "serverSide": false,//// for process server side
                    "bDestroy": true,
                    "filter": true,// this is for disable filter (search box)
                    "orderMulti": false,// for disable multiple column at once
                    "scrollX": 200,
                    "aaData": res,
                    "columnDefs": [],
                    "columns": [
                        { "data": null },
                        { "data": "rclm_id", "name": "rclm_id", "autoWidth": true, "visible": false },
                        { "data": "reimbursement_category_name", "name": "reimbursement_category_name", "autoWidth": true },
                        {
                            "render": function (data, type, row) {
                                return '<input style="width:100px" class="form-control"  id=' + row.rclm_id + '_monthyly_limit  maxlength="8" name=' + row.rclm_id + '_monthyly_limit type="text" value =' + row.monthly_limit + ">";
                            }
                        },
                        {
                          
                            "render": function (data, type, row) {
                                return '<input style="width:100px" class="form-control"  id=' + row.rclm_id + '_monthyly_limit  maxlength="8" name=' + row.rclm_id + '_monthyly_limit type="text" value =' + row.yearly_limit + ">";
                            }
                        },
                        { "data": "rcm_id", "name": "rcm_id", "autoWidth": true, "visible": false }
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
            $('#loader').hide();
            alert(error.responseText);
        }
    });
}



