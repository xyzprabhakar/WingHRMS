$('#loader').show();
var company_id;
var employee_id;
var login_emp_id;

$(document).ready(function () {
    setTimeout(function () {
        


        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        company_id = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        employee_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });


        getCurrentFinancialYear();

        BindAllEmp_Company('ddlCompany', employee_id, company_id);


        $('#btnupdate').hide();

        $("#txtbilldate").datepicker({
            dateFormat: 'dd/MMM/yyyy',

            //minDate: 0,
        });

        //Start, For approve and Reject Request, Added by Supriya

        BindRequestDetail(company_id, employee_id);


        //End, For approve and Reject Request, Added by Supriya
        $('#addbtn').hide();

        $('#ddlreqtype').bind('change', function () {
            var table = $('#tblCategory').DataTable();
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                //cat.rclm_id = table.cell(rowIdx, 0).data();
                table.cell(rowIdx, 0).nodes().to$().find('select').val(0);
                table.cell(rowIdx, 1).nodes().to$().find('input').val('');

                table.cell(rowIdx, 2).nodes().to$().find('input').val(0);
                table.cell(rowIdx, 3).nodes().to$().find('input').val(0);
                table.cell(rowIdx, 4).nodes().to$().find('input').val('');
                table.cell(rowIdx, 5).nodes().to$().find('input').val('');
            });
        });

        $('#loader').hide();


        $('#addbtn').on('click', function () {
            //// debugger;
            $('#loader').show();
            var rglm_id_new = $('#hdnCategoryId').val();
            var dTable = $('#tblCategory').DataTable();
            var table_length = dTable.data().count();
            var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementCategoryLimitMaster/0/' + rglm_id_new;

            $.ajax({
                type: "GET",
                url: apiurl,
                data: {},
                dataType: "json",
                contentType: 'application/json; charset=utf-8',
                headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
                success: function (res) {
                    $('#loader').hide();
                    dTable.row.add([
                        {

                            "render": function (res, type, row) {
                                //// debugger;

                                var $select = $("<select id='ddlCategory'></select>", {});
                                $.each(data1, function (k, v) {
                                    //if (v.rclm_id == row.rclm_id) {
                                    var $option = $("<option></option>",
                                        {
                                            text: v.reimbursement_category_name,
                                            value: v.rclm_id
                                        });
                                    if (row.rclm_id === v.rclm_id) {
                                        $option.attr("selected", "selected")
                                    }
                                    $select.append($option);
                                    //}
                                    break;
                                });
                                //// debugger;
                                return $select.prop("outerHTML");
                                //return '<input type=""  readonly="readonly" autocomplete="off" id="txtbilldate">'
                            }
                        },
                        {
                            render: function (data, type, row) {

                                return '<input type="date"   autocomplete="off" id="txtbilldate">'

                                //return ' <input type="date"   autocomplete="off" id="txtMonthYear1" />  <div class="clear"></div>'

                                //return ' <input type="text" id="datetimepicker" />'

                            }
                        },
                        {
                            render: function (data, type, row) {

                                return '<input style="width:100px" class="form-control"  id="value1" maxlength="8" name="value1" type="text" onkeypress="return isNumberKey(event)" value = 0 >'

                            }
                        },
                        {
                            render: function (data, type, row) {

                                return '<input style="width:100px" class="maintCostField" id="value2" maxlength="8" onkeypress="return isNumberKey(event)" name="value2" type="text" value=0>'


                            }
                        },
                        { /*"data": "monthly_limit", "name": "monthly_limit", "autoWidth": true */
                            render: function (data, type, row) {
                                return '<input type="text" readonly   autocomplete="off" id="txtmonthyAmt">'
                            }
                        },
                        { /*"data": "yearly_limit", "name": "yearly_limit", "autoWidth": true */
                            render: function (data, type, row) {
                                return '<input type="text" readonly   autocomplete="off"  id="txtyearlyAmt">'
                            }
                        },
                    ]).draw();

                },
                error: function (error) {
                    $('#loader').hide();
                    alert(error.responseText);
                }
            });

        });


        $('#ddlCompany').bind("change", function () {
            $('#loader').show();
            $("#ddlEmployee option").remove();

            BindEmpList('ddlEmployee', $(this).val(), 0);
            $('#loader').hide();
        });



        $('#ddlEmployee').bind("change", function () {
            $('#loader').show();
            ShowCategoryList($(this).val());
            $('#loader').hide();
        });


        $("#ddlCategory").change(function () {

            var selectedCategory = $(this).children("option:selected").val();
            alert("You have selected the Category - " + selectedCategory);
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            //// debugger;;

            var ddlcomp = $("#ddlCompany").val();
            var ddlemp = $("#ddlEmployee").val();
            var ddlreqtype = $("#ddlreqtype").val();
            var ddlmonthyear = $("#txtMonthYear").val();
            var financialyr = $("#txtfiscal").val();
            var total_amt = $("#txtreqAmt").val();

            var errormsg = '';
            var iserror = false;


            //Loop through the Table rows and build a JSON array.
            var Category = new Array()
            var table = $('#tblCategory').DataTable();
            table.rows().every(function (rowIdx, tableLoop, rowLoop) {
                // var _checkcatentry = table.cell(rowIdx, 0).data();

                //// debugger;;
                var _chkbilldate = "";
                var cat = {};
                //cat.rclm_id = table.cell(rowIdx, 0).data();
                cat.rclm_id = table.cell(rowIdx, 0).nodes().to$().find('select').val();
                cat.Bill_date = table.cell(rowIdx, 1).nodes().to$().find('input').val();

                cat.Bill_amount = table.cell(rowIdx, 2).nodes().to$().find('input').val();
                cat.approved_amount = table.cell(rowIdx, 3).nodes().to$().find('input').val();
                if (ddlreqtype == 1) {
                    cat.monthly_limitt = table.cell(rowIdx, 4).nodes().to$().find('input').val();
                    cat.yearly_limitt = 0;
                }
                else if (ddlreqtype == 2) {
                    cat.yearly_limitt = table.cell(rowIdx, 5).nodes().to$().find('input').val();
                    cat.monthly_limitt = 0;
                }

                cat.created_by = employee_id;
                if (cat.approved_amount > 0) {
                    Category.push(cat);
                }

            });
            //alert(JSON.stringify(Category))




            if (ddlemp == '' || ddlemp == '0') {
                errormsg = errormsg + 'Please select employee !! <br />';
                iserror = true;
            }

            if (ddlmonthyear == '' || ddlmonthyear == "undefined") {
                errormsg = errormsg + 'Please Select Month Year !! <br />';
                iserror = true;
            }

            if (total_amt == '' || total_amt == 'NaN') {
                errormsg = errormsg + 'Please Enter Request Amount against Category Name !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }
            //supriya
            if (ddlreqtype == 1) {
                ddlmonthyear = ddlmonthyear.substring(6, 4); // give only month no
            }
            else if (ddlreqtype == 2) {
                ddlmonthyear = ddlmonthyear.substring(0, 4) //give only year
            }
            // start for checking duplicate entry
            var recipientsArray = Category.sort();

            var _duplicatecategory = [];
            var _matchresult = false;

            if (recipientsArray.length - 1 > 0) {

                for (var i = 0; i < recipientsArray.length - 1; i++) {
                    if (recipientsArray[i + 1].rclm_id == recipientsArray[i].rclm_id && recipientsArray[i + 1].Bill_date == recipientsArray[i].Bill_date) {
                        _duplicatecategory.push(recipientsArray[i]);
                    }
                    else {

                        var _plussonebilldate = recipientsArray[i + 1].Bill_date.substring(5, 7);
                        var _originaldate = recipientsArray[i].Bill_date.substring(5, 7);

                        if (recipientsArray[i + 1].rclm_id == recipientsArray[i].rclm_id && _plussonebilldate == _originaldate) {
                            if (ddlreqtype == 1) {
                                if (parseFloat(total_amt) > parseFloat(recipientsArray[i].monthly_limitt)) {
                                    _matchresult = true;
                                }
                            }
                            else if (ddlreqtype == 2) {
                                if (total_amt > recipientsArray[i].yearly_limitt) {
                                    _matchresult = true;
                                }
                            }

                        }
                    }
                }

            }
            else {
                if (ddlreqtype == 1) {
                    if (parseFloat(total_amt) > parseFloat(recipientsArray[0].monthly_limitt)) {
                        _matchresult = true;
                    }
                }
                else if (ddlreqtype == 2) {
                    if (total_amt > recipientsArray[0].yearly_limitt) {
                        _matchresult = true;
                    }
                }
            }

            if (_duplicatecategory.length > 0) {
                messageBox("error", "Same Category with Same Date will not be Saved");
                $('#loader').hide();
                return false;
            }

            if (_matchresult) {
                messageBox("error", "Total Request Amount should be less than or equal to Monthly limit");
                $('#loader').hide();
                return false;
            }
            // end for checking duplicate entry



            var myData = {

                'reimbursement_request_details': Category,
                'compId': ddlcomp,
                'emp_id': ddlemp,
                'req_type': ddlreqtype,
                'monthyear': ddlmonthyear,
                'fiscal_year_id': financialyr,
                'total_amt': total_amt,
                'created_by': employee_id
            }

            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $('#loader').show();
            // var apiurl = localStorage.getItem("ApiUrl") + '/apiPayroll/Save_ReimbursementRequest';
            // var obj = JSON.stringify(myData);

            // alert(obj);

            $.ajax({
                url: localStorage.getItem("ApiUrl") + "/apiPayroll/Save_ReimbursementRequest",
                type: "POST",
                contentType: "application/json",
                dataType: "json",
                data: JSON.stringify(myData),
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
                    else if (statuscode == "1" || statuscode == '2') {
                        messageBox("error", Msg);
                    }
                },
                error: function (response) {
                    $('#loader').hide();
                    _GUID_New();
                    messageBox("error", response.responseText);
                }

            });


        });


        $('#btnreset').bind('click', function () {
            //// debugger;;
            window.location.href = '/payroll/ReimbursementRequestMaster';
        });

    }, 2000);// end timeout

});



function getCurrentFinancialYear() {

    //// debugger;;
    var fiscalyear = "";
    var today = new Date();
    if ((today.getMonth() + 1) <= 3) {
        fiscalyear = (today.getFullYear() - 1) + "-" + today.getFullYear()
    } else {
        fiscalyear = today.getFullYear() + "-" + (today.getFullYear() + 1)
    }
    $('#txtfiscal').val(fiscalyear);
}

function BindCategoryName(ControlId, rglm_id, SelectedVal) {
    $('#loader').show();
    // alert('Start')
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementCategoryLimitMaster/0/' + rglm_id,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.rclm_id).html(value.reimbursement_category_name));
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

function BindEmpList(ControlId, CompanyId, SelectedVal) {
    $('#loader').show();
    ControlId = '#' + ControlId;
    $.ajax({
        type: "GET",
        url: apiurl + "apiMasters/Get_EmployeeCodeFromEmpMasterByComp/" + CompanyId,
        data: {},
        contentType: "application/json",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        dataType: "json",
        success: function (response) {
            var res = response;

            $(ControlId).empty().append('<option selected="selected" value="">--Please select--</option>');
            $.each(res, function (data, value) {
                $(ControlId).append($("<option></option>").val(value.employee_id).html(value.emp_code));
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
//$(document).ready(function () {
//    $('#txtmonthyear').datepicker({
//        changeMonth: true,
//        changeYear: true,
//        //dateFormat: 'MM yy',
//        dateFormat: 'yymm',

//        onClose: function () {
//            var iMonth = $("#ui-datepicker-div .ui-datepicker-month :selected").val();
//            var iYear = $("#ui-datepicker-div .ui-datepicker-year :selected").val();
//            $(this).datepicker('setDate', new Date(iYear, iMonth, 1));
//        },

//        beforeShow: function () {
//            if ((selDate = $(this).val()).length > 0) {
//                iYear = selDate.substring(selDate.length - 4, selDate.length);
//                iMonth = jQuery.inArray(selDate.substring(0, selDate.length - 5), $(this).datepicker('option', 'monthNames'));
//                $(this).datepicker('option', 'defaultDate', new Date(iYear, iMonth, 1));
//                $(this).datepicker('setDate', new Date(iYear, iMonth, 1));
//            }
//        }
//    });
//});

function ShowCategoryList(emp_id) {
    //// debugger;;
    $('#loader').show();
    if (emp_id == null || emp_id == '') {
        messageBox('info', 'There are some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementGradeLimitMaster/0/0/' + emp_id + '/' + 201905;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            data = res;

            $('#hdnCategoryId').val(data[0].rglm_id);
            $('#hdnmonthlylimit').val(data[0].monthly_limit);
            $('#hdnyearlylimit').val(data[0].yearly_limit);

            var rglm_id = $('#hdnCategoryId').val();
            //// debugger;;
            if (rglm_id != "" || rglm_id != null || rglm_id != undefined) {
                $('#addbtn').show();
            }
            ShowCategoryBind(rglm_id);

            $('#loader').hide();

            //BindCategoryName('ddlCategory', rglm_id, 0)
            //alert('end')
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}
function GetCategotylimit(obj) {


    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementCategoryLimitMaster/' + rclm_id + '/0';
    $('#loader').show();
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            $('#loader').hide();
        },
        error: function (error) {
            alert(error.responseText);
            $('#loader').hide();
        }
    });

}
function ShowCategoryBind(rglm_id) {
    //// debugger;;
    $('#loader').show();
    if (rglm_id == null || rglm_id == '') {
        messageBox('info', 'There are some problem please try after later !!');
        $('#loader').hide();
        return false;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementCategoryLimitMaster/0/' + rglm_id;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {

            var data1 = res;
            $('#loader').hide();
            $("#tblCategory").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollX": 150,
                "aaData": res,
                "columns": [

                    //{ "data": null },
                    //commented here 
                    //{ "data": "rclm_id", "name": "rclm_id", "autoWidth": true, "visible": false },
                    //{ "data": "reimbursement_category_name", "name": "reimbursement_category_name", "autoWidth": true },
                    {

                        "render": function (res, type, row) {
                            //// debugger;;
                            //var $select = $("<select></select>", {
                            //});
                            //$.each(ShiftList, function (k, v) {

                            //    var $option = $("<option></option>", {
                            //        "text": v.shift_name + ' (' + v.shift_short_name + ')',
                            //        "value": v.shift_id
                            //    });
                            //    if (full.shft_id1 === v.shift_id) {
                            //        $option.attr("selected", "selected")
                            //    }
                            //    $select.append($option);
                            //});
                            //return $select.prop("outerHTML");
                            var $select = $("<select id=ddlcategorylst name=ddlcategorylst ><option value=0>Please Select</option></select>", {});
                            $.each(data1, function (k, v) {
                                //if (v.rclm_id == row.rclm_id) {

                                var $option = $("<option></option>",
                                    {
                                        text: v.reimbursement_category_name,
                                        value: v.rclm_id
                                    });
                                //if (row.rclm_id === v.rclm_id) {
                                //    $option.attr("selected", "selected")
                                //}
                                $select.append($option);
                                //}
                            });

                            return $select.prop("outerHTML");

                            //return '<input type=""  readonly="readonly" autocomplete="off" id="txtbilldate">'
                        }
                    },
                    {
                        render: function (data, type, row) {

                            return '<input type="date"   autocomplete="off" id="txtbilldate">'

                            //return ' <input type="date"   autocomplete="off" id="txtMonthYear1" />  <div class="clear"></div>'

                            //return ' <input type="text" id="datetimepicker" />'

                        }
                    },
                    {
                        render: function (data, type, row) {

                            return '<input style="width:100px" class="form-control"  id="value1" maxlength="8" name="value1" type="text" onkeypress="return isNumberKey(event)" value = 0 >'

                        }
                    },
                    {
                        render: function (data, type, row) {

                            return '<input style="width:100px" class="maintCostField" id="reqamt"  maxlength="8" onkeypress="return isNumberKey(event)" name="reqamt" type="text" value = 0 >'


                        }
                    },
                    { /*"data": "monthly_limit", "name": "monthly_limit", "autoWidth": true */
                        render: function (data, type, row) {
                            return '<input type="text" readonly   autocomplete="off" id="txtmonthyAmt">'
                        }
                    },
                    { /*"data": "yearly_limit", "name": "yearly_limit", "autoWidth": true */
                        render: function (data, type, row) {
                            return '<input type="text" readonly   autocomplete="off"  id="txtyearlyAmt">'
                        }
                    },
                    //{ "data": "monthly_limit", "name": "monthly_limit", "autoWidth": true },
                    //{ "data": "yearly_limit", "name": "yearly_limit", "autoWidth": true },
                ],

            });


            //supriya Start get detail of reimbursment category List 

            //start get total request amt and show in requeste amt textbox 
            $(document).on('change', '.maintCostField', function () {

                var _requestamt = [];

                $('input:text[name=reqamt]').each(function () {
                    _requestamt.push($(this).val());
                });
                var _totalrequestedamt = 0;
                $.each(_requestamt, function () {

                    _totalrequestedamt += parseFloat(this);

                });
                $("#txtreqAmt").val(_totalrequestedamt);
            });
            //end
            //star Bind Monthly and yearly amt

            var table = $('#tblCategory').DataTable();


            $(document).on('change', '#ddlcategorylst', function () {
                //alert($(this).val());
                // alert($(this).closest('tr').index());
                var _requesttype = $("#ddlreqtype").val();
                var _categoryidd = $(this).val();
                var _rowindexx = $(this).closest('tr').index();
                if (_requesttype == 0) {

                    $("#ddlcategorylst").val(0);
                    messageBox("error", "Please Select Request Type");
                    return false;
                }
                if (_categoryidd == 0) {
                    messageBox("error", "Please Select Category");
                    return false;
                }

                var urll = localStorage.getItem("ApiUrl") + 'apipayroll/Get_ReimbursementCategoryLimitMaster/' + _categoryidd + '/0';
                $.get(urll, {}, function (data) {
                    var res = data;
                    if (_requesttype == 1) {
                        table.cell(_rowindexx, 4).nodes().to$().find('input').val(res[0].monthly_limit);
                        table.cell(_rowindexx, 5).nodes().to$().find('input').val('');

                    }
                    else if (_requesttype == 2) {
                        table.cell(_rowindexx, 5).nodes().to$().find('input').val(res[0].yearly_limit);
                        table.cell(_rowindexx, 4).nodes().to$().find('input').val('');

                    }
                });
            });

            //end
            //supriya end get detail of reimbursment category List 

        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });
}

//window.location.href = '/payroll/RmbGrdLimitMaster';

//Start, For approve and Reject Request, Added by Supriya

function BindRequestDetail(companyId, employee_id) {
    //// debugger;;
    $('#loader').show();
    $.ajax({
        //url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_ReimRquestDetail/0/' + companyId,
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Get_ReimRquestDetail/' + employee_id + "/" + companyId,
        type: "GET",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            $('#loader').hide();
            $("#tblreimbrequestapproval").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs":
                    [
                        //{
                        //    targets: [2],
                        //    render: function (data, type, row) {
                        //        return data == '1' ? 'Temporary' : data == '2' ? 'Probation' : data == '3' ? 'Confirmed' : data == '4' ? 'Contract' : data == '10' ? 'Notice' : data=='99'?'FNF': data == '100' ? 'Terminate' : ''
                        //    }
                        //},
                        {
                            targets: [3],
                            render: function (data, type, row) {
                                return data == '1' ? 'Yearly' : 'Monthly'
                            }
                        },
                        {
                            targets: [7],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                        {
                            targets: [8],
                            render: function (data, type, row) {

                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "company_name", "name": "company_name", "autoWidth": true },
                    { "data": "employee_name", "name": "employee_name", "autoWidth": true },
                    { "data": "request_type", "name": "request_type", "autoWidth": true },
                    { "data": "fiscal_year_id", "name": "fiscal_year_id", "autoWidth": true },
                    { "data": "request_month_year", "name": "request_month_year", "autoWidth": true },
                    { "data": "total_request_amount", "name": "total_request_amount", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "autoWidth": true },
                    {

                        render: function (data, type, full, row) {
                            if (full.remarks == null || full.remarks == '') {
                                return '<input style="width:120px" class="form-control"  id="txtRemarks" type="text" placeholder="Enter Remarks">';
                            }
                            else {
                                return '<lable>' + full.remarks + '</lable>'
                            }

                        }
                    },
                    {
                        "render": function (data, type, full, meta) {
                            return '<a href="#" onclick="ViewRequestDetail(' + full.rrm_id + ')" >View</a>';
                        }
                    },
                    {
                        "render": function (data, type, full, meta) {
                            if (full.remarks == '' || full.remarks == null) {

                                return '<input type="hidden" id="rr_id" value="' + full.rrm_id + '" /> <a href="#" class="btnSaveE" style="float: left;" title="Approve"><i class="far fa-thumbs-up"></i></a>&nbsp;<a href="#" id="btnSave"  class="btnSaveD"  style="float: right;" title="Reject"><i class="fa fa-thumbs-down"></i></a>';
                            }
                            else if ((full.remarks != '' || full.remarks != null) && full.is_approvred == "2") {
                                return '<label>' + "Rejected" + '</label>';
                            }
                            else if ((full.remarks != '' || full.remarks != null) && full.is_approvred == "1") {
                                return '<label>' + "Accepted" + '</label>';
                            }

                        }
                    }
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });


            $(document).on("click", ".btnSaveE", function () {

                var rr_id = $(this).parents('tr').children('td').children('input[type="hidden"]').val();
                var txtRemarks = $(this).parents('tr').children('td').children('input[type="text"]').val();

                if (txtRemarks == '') {
                    messageBox("error", "Please enter Remarks");
                    return false;
                }

                BindAcceptRejectRequest(rr_id, txtRemarks, 1);

            });
            $(document).on("click", ".btnSaveD", function () {

                var rr_id = $(this).parents('tr').children('td').children('input[type="hidden"]').val();
                var txtRemarks = $(this).parents('tr').children('td').children('input[type="text"]').val();
                if (txtRemarks == '') {
                    messageBox("error", "Please enter Remarks");
                    return false;
                }
                BindAcceptRejectRequest(rr_id, txtRemarks, 2);

            });

        },
        error: function (err) {
            alert(err.responseText);
        }
    });
}


function ViewRequestDetail(reim_request_master_id) {
    //// debugger;;
    $('#loader').show();
    if (reim_request_master_id == null || reim_request_master_id == '') {
        messageBox('info', 'There are some problem please try again later');
        $('#loader').hide();
        return false;
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    var modal = document.getElementById("myModal");
    modal.style.display = "block";

    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/View_ReimbursementRequestDetail/' + reim_request_master_id,
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: "{}",
        headers: headerss,
        success: function (response) {
            var ress = response;
            $('#loader').hide();
            _GUID_New();
            $("#tblreimb_req_dtl").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollX": 200,
                "aaData": response,
                "columnDefs":
                    [
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
                        }
                    ],

                "columns": [
                    { "data": "sno", "name": "sno", "autoWidth": true },
                    { "data": "reimbursement_category_name", "name": "reimbursement_category_name", "autoWidth": true },
                    { "data": "bill_amount", "name": "bill_amount", "autoWidth": true },
                    { "data": "request_amount", "name": "request_amount", "autoWidth": true },
                    { "data": "bill_date", "name": "bill_date", "autoWidth": true },
                    { "data": "created_dt", "name": "created_dt", "autoWidth": true },
                    { "data": "modified_dt", "name": "modified_dt", "autoWidth": true },
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]]

            });

        },
        error: function (err) {
            _GUID_New();
            alert(err.responseText);
        }
    });
}


function BindAcceptRejectRequest(rrm_id, _remarkss, isapproved) {
    $('#loader').show();
    var mydata = {
        rrm_id: rrm_id,
        remarks: _remarkss,
        is_approvred: isapproved,
        modified_by: employee_id
    }

    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
    headerss["salt"] = $("#hdnsalt").val();

    $.ajax({
        url: localStorage.getItem("ApiUrl") + 'apiPayroll/Edit_ReimbursementRequest',
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(mydata),
        headers: headerss,
        success: function (response) {
            var statuscode = response.statusCode;
            var Msg = response.message;
            $('#loader').hide();
            _GUID_New();
            if (statuscode == "0") {
                messageBox("success", Msg);
                location.reload();
            }
            else if (statuscode == "1" || statuscode == '2') {
                messageBox("error", Msg);
            }
        },
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            alert(err.responseText);
        }
    });
}





    //End, For approve and Reject Request, Added by Supriya

