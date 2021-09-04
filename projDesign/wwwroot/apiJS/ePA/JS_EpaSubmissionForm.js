var default_company;
var login_emp;
var empDatas;
var user_id;
var epa_runing;
var status_data = "";
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        login_emp = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        user_id = CryptoJS.AES.decrypt(localStorage.getItem("user_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

        BindEPA_FiscallYrByComp('ddlSession', default_company, 0);
        BindEPA_CycleByComp('ddlcicyle', default_company, 0);

        //BindEmployeeListForEpa('ddlempcode', login_emp);
        BindEmployeeListUnderLoginEmpFromAllComp('ddlempcode', default_company, login_emp, 0);
        setSelect('ddlempcode', login_emp);

        //var HaveDisplay = ISDisplayMenu("Display Company List");

        //if (HaveDisplay == 0) {
        //    BindEPA_FiscallYrByComp('ddlSession', default_company, 0);
        //    BindEPA_CycleByComp('ddlcicyle', default_company, 0);

        //    BindEmployeeListForEpa('ddlempcode', login_emp);
        //}
        //else {

        //}

        $('#ddlcicyle').change(function () {
            var f_year_id = $('#ddlSession').val();
            var ddlcicyle = $('#ddlcicyle').val();
            var emp_id = $('#ddlempcode').val();

            if (f_year_id == null || f_year_id == '' || f_year_id == 0) {
                alert('Please select fiscal year...!');
                BindEPA_CycleByComp('ddlcicyle', default_company, 0);
                return false;
            }
            if (emp_id == null || emp_id == '' || emp_id == 0) {
                alert('Please select employee...!');
                BindEPA_CycleByComp('ddlcicyle', default_company, 0);
                return false;
            }
            Get_EPASubmissionData(emp_id, f_year_id, ddlcicyle, default_company, 'NA');
        });


        $('#ddlempcode').change(function () {
            var f_year_id = $('#ddlSession').val();
            var ddlcicyle = $('#ddlcicyle').val();
            var emp_id = $('#ddlempcode').val();

            if (f_year_id == null || f_year_id == '' || f_year_id == 0) {
                alert('Please select fiscal year...!');
                BindEPA_CycleByComp('ddlcicyle', default_company, 0);
                return false;
            }
            if (emp_id == null || emp_id == '' || emp_id == 0) {
                alert('Please select employee...!');
                BindEPA_CycleByComp('ddlcicyle', default_company, 0);
                return false;
            }

            if (ddlcicyle != null && ddlcicyle != 0) {
                Get_EPASubmissionData(emp_id, f_year_id, ddlcicyle, default_company, 'NA');
            }
        });


        $('#btnsave').css('display', 'none');

        $('#loader').hide();




        $('#btnsave').bind("click", function () {



            if ($('#ddlStatus').val() == "" || $('#ddlStatus').val() == null || $("#ddlStatus").text() == "--Select--") {
                alert('Please select Status...');
                return false;
            }

            if ($("#ddlempcode") == "" || $("#ddlempcode") == null || $("#ddlempcode") == "0") {
                alert('Please Select Employee');
                return false;
            }

            if ($("#ddlcicyle") == "" || $("#ddlcicyle") == null || $("#ddlcicyle") == "0") {
                alert('Please select epa Cycle');
            }

            var f_year_id = $('#ddlSession').val();
            var ddlcicyle = $('#ddlcicyle').val();

            if (f_year_id == null || f_year_id == '' || f_year_id == 0) {
                alert('Please select fiscal year...!');
                BindEPA_CycleByComp('ddlcicyle', default_company, 0);
                return false;
            }
            if (empDatas == null) {
                alert('No Recoed Found...!');
                $('#loader').hide();
                return false;
            }
            else {
                var checkstr = confirm('Are you sure want to submit..?');
                if (checkstr == true) {
                    // do your code
                } else {
                    modal.style.display = "none";
                    $('#loader').hide();
                    return false;
                }
                var submission_id = $('#submission_id').val();
                var company_id = default_company;
                var emp_id = $('#emp_id').val();
                var emp_off_id = $('#emp_off_id').val();
                var fiscal_year_id = f_year_id;
                var cycle_id = ddlcicyle;
                var cycle_name = $("#ddlcicyle option:selected").text();

                var desig_id = $('#desig_id').val();
                var department_id = $('#department_id').val();
                var epa_start_date = $('#epa_start_date').val();
                var epa_end_date = $('#epa_end_date').val();
                var epa_current_status = $('#ddlStatus').val();
                var w_r_id = $('#w_r_id').val();

                var final_review = $("input[name='tab_FinalReview_tab_1']:checked").val();
                var final_remarks = $("#tab_FinalReview_tab_text").val();

                var final_review_rm1 = $("input[name='tab_FinalReview_tab_m1']:checked").val();
                var final_remarks_rm1 = $("#tab_FinalReview_tab_m1_text").val();//$("#tab_FinalReview_tab_m1_text").text();

                var final_review_rm2 = $("input[name='tab_FinalReview_tab_m2']:checked").val();
                var final_remarks_rm2 = $("#tab_FinalReview_tab_m2_text").val(); //$("#tab_FinalReview_tab_m2_text").text();

                var final_review_rm3 = $("input[name='tab_FinalReview_tab_m3']:checked").val();
                var final_remarks_rm3 = $("#tab_FinalReview_tab_m3_text").val(); //$("#tab_FinalReview_tab_m3_text").text();

                var rm_id1 = $("#rm_id1").text();
                var rm_id2 = $("#rm_id2").text();
                var rm_id3 = $("#rm_id3").text();



                var mdlEpaKPIDetails = [];



                var ind_ = 0;


                var tbl_tab_kpi = $("#tbl_tab_kpi").find(">tbody > tr").each(function () {
                    var kpi_submission_id = $(this).find("td:eq(1)").html();
                    var key_area_id = $(this).find("td:eq(2)").html();
                    var wtg = $('#txt_wtg_' + key_area_id).val();
                    var self = $(this).find("td:eq(12)").children('select').val();
                    var agreed = $(this).find("td:eq(13)").children('select').val();//$(this).find("td:eq(11)").children('select').val();
                    var score = $('#txt_score_' + key_area_id).val();
                    var comment = $(this).find("td:eq(14)").html();

                    var mdlEpaKPICriteriaDetails = [];

                    if (empDatas[0].mdlEpaKPIDetails[ind_].mdlEpaKPICriteriaDetails != null && empDatas[0].mdlEpaKPIDetails[ind_].mdlEpaKPICriteriaDetails.length > 0) {

                        for (var j = 0; j < empDatas[0].mdlEpaKPIDetails[ind_].mdlEpaKPICriteriaDetails.length; j++) {

                            mdlEpaKPICriteriaDetails.push({ "crit_id": empDatas[0].mdlEpaKPIDetails[ind_].mdlEpaKPICriteriaDetails[j].crit_id })

                        }
                    }

                    mdlEpaKPIDetails.push({ "key_area_id": key_area_id, "wtg": wtg, "self": self, "aggreed": agreed, "score": score, "comment": comment, "mdlEpaKPICriteriaDetails": mdlEpaKPICriteriaDetails });
                    ind_ += 1;
                });
                var mdlEpaKRADetails = [];

                var tbl_tab_kra = $("#tbl_tab_kra").find(">tbody > tr").each(function () {

                    var kra_id = $(this).find("td:eq(0)").html();
                    var rating_id = $(this).find("td:eq(3)").children('select').val();

                    mdlEpaKRADetails.push({ "kra_id": kra_id, "rating_id": rating_id });

                });


                var mdlEpaTabQuestions = [];
                if (empDatas[0].mdlEpaTabQuestions != null && empDatas[0].mdlEpaTabQuestions.length > 0) {

                    for (var i = 0; i < empDatas[0].mdlEpaTabQuestions.length; i++) {

                        var question_id = $('#txt_qi_' + i).val();

                        var question_answer = "";
                        var remark = "";
                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 1) {

                            question_answer = $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).val();

                        }
                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 2) {
                            question_answer = $('#ddl_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).val();
                        }
                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 3) {
                            question_answer = $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").val();
                            remark = $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).val();
                        }
                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 4) {
                            question_answer = $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").val();
                        }
                        mdlEpaTabQuestions.push({ "tab_id": empDatas[0].mdlEpaTabQuestions[i].tab_id, "question_id": question_id, "question_answer": question_answer, "remark": remark });

                    }

                }
                var myData = [{
                    'submission_id': submission_id,
                    'company_id': company_id,
                    'emp_id': emp_id,
                    'emp_off_id': emp_off_id,
                    'fiscal_year_id': fiscal_year_id,
                    'cycle_id': cycle_id,
                    'cycle_name': cycle_name,
                    'desig_id': (desig_id == "" || desig_id == null ? "0" : desig_id),
                    'department_id': department_id,
                    'epa_start_date': epa_start_date,
                    'epa_end_date': epa_end_date,
                    'epa_current_status': epa_current_status,
                    "w_r_id": w_r_id,
                    "total_score": 0,
                    "get_score": 0,
                    "user_id": user_id,
                    'final_review': final_review,
                    'final_remarks': final_remarks,
                    'final_review_rm1': final_review_rm1,
                    'final_remarks_rm1': final_remarks_rm1,
                    'final_review_rm2': final_review_rm2,
                    'final_remarks_rm2': final_remarks_rm2,
                    'final_review_rm3': final_review_rm3,
                    'final_remarks_rm3': final_remarks_rm3,
                    'rm_id1': rm_id1 == null || rm_id1 == "null" || rm_id1 == "" ? 0 : rm_id1,
                    'rm_id2': rm_id2 == null || rm_id2 == "" || rm_id2 == "null" ? 0 : rm_id2,
                    'rm_id3': rm_id3 == null || rm_id3 == "" || rm_id3 == "null" ? 0 : rm_id3,
                    'mdlEpaKPIDetails': mdlEpaKPIDetails,
                    'mdlEpaKRADetails': mdlEpaKRADetails,
                    "mdlEpaTabQuestions": mdlEpaTabQuestions
                }];
                $('#loader').show();

                var Obj = JSON.stringify(myData);

                var headerss = {};
                headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
                headerss["salt"] = $("#hdnsalt").val();


                $.ajax({
                    url: localStorage.getItem("ApiUrl") + '/apiePA/Save_EPASubmissionData',
                    type: "POST",
                    data: Obj,
                    dataType: "json",
                    contentType: "application/json",
                    headers: headerss,
                    success: function (res) {

                        _GUID_New();
                        $('#loader').hide();


                        var msg = res.message;
                        var statuscode = res.statusCode;


                        if (statuscode == 1) {
                            //messageBox("success", "EPA Submitted Successfully...!");
                            alert("EPA Submitted Successfully...!");
                            location.reload();
                        }
                        else {
                            messageBox("error", msg);
                            return false;
                        }

                    },
                    error: function (request, status, error) {
                        _GUID_New();
                        $('#loader').hide();
                        var error = "";
                        var errordata = JSON.parse(request.responseText);
                        try {
                            var i = 0;
                            while (Object.keys(errordata).length > i) {
                                var j = 0;
                                while (errordata[Object.keys(errordata)[i]].length > j) {
                                    error = error + "\r\n  * " + errordata[Object.keys(errordata)[i]][j];
                                    j = j + 1;
                                }
                                i = i + 1;
                            }

                        } catch (err) { }
                        messageBox("error", error);

                    }

                });
            };
            $('#loader').hide();
        });


    }, 2000);// end timeout

});



function dateCheck(from, to, check) {

    if (new Date(from) <= new Date(check) && new Date(check) <= new Date(to)) {
        return true;
    }

    //var d1 = from.split("/");
    //var d2 = to.split("/");
    //var c = check.split("/");

    //var from = new Date(d1[2], parseInt(d1[1]) - 1, d1[0]);  // -1 because months are from 0 to 11
    //var to = new Date(d2[2], parseInt(d2[1]) - 1, d2[0]);
    //var check = new Date(c[2], parseInt(c[1]) - 1, c[0]);



    //if (check >= from && check <= to) {
    //    return true;
    //}
    return false;
}

function Get_EPASubmissionData(emp_id, f_year_id, ddlcicyle, default_company, cyclename) {
    $('#loader').show();

    if (emp_id == null || emp_id == '') {
        alert('There some problem please try after later !!');
        return false;
    }

    //// STATUS LIST ////////////////////////////////////////////////////////////////////////////


    //var apiurl_for_status = localStorage.getItem("ApiUrl") + 'apiePA/Get_StatusMaster/' + default_company + '/0';

    //$.ajax({
    //    type: "GET",
    //    url: apiurl_for_status,
    //    data: {},
    //    dataType: "json",
    //    contentType: 'application/json; charset=utf-8',
    //    headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
    //    success: function (res) {
    //        status_data = res;
    //        //alert(JSON.stringify(res));
    //    }
    //});




    //// STATUS LIST  ////////////////////////////////////////////////////////////////////////////

    //// START EPA FORM  ///////////////////////////////////////////////////////////////////////
    $('#ul_tab').empty();
    $('#tab_div_data').empty();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiePA/Get_EPASubmissionData/' + emp_id + '/' + f_year_id + '/' + ddlcicyle + '/' + default_company + '/' + cyclename;

    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {


            empDatas = res;
            if (empDatas.statusCode != undefined) {
                messageBox("error", empDatas.message);
                return false;
            }

            $('#rm_id1').text(res[0].rm_id1);
            $('#rm_id3').text(res[0].rm_id3);
            $('#rm_id2').text(res[0].rm_id2);

            var kpiDatas = [];
            var kpiColumn = [];

            if (empDatas == null || empDatas.length == 0) {

            }
            else {
                if (empDatas.statusCode != undefined && empDatas.statusCode == 1) {
                    alert(empDatas.message);
                    return;
                }
                var haveCriteria = false;
                if (empDatas[0].mdlEpaKPIDetails != null) {
                    for (var i = 0; i < empDatas[0].mdlEpaKPIDetails.length; i++) {

                        var criteriaDatas = {};
                        criteriaDatas["kpi_submission_id"] = empDatas[0].mdlEpaKPIDetails[i].kpi_submission_id;
                        criteriaDatas["key_area_id"] = empDatas[0].mdlEpaKPIDetails[i].key_area_id;
                        criteriaDatas["otype_id"] = empDatas[0].mdlEpaKPIDetails[i].otype_id;
                        criteriaDatas["objective_name"] = empDatas[0].mdlEpaKPIDetails[i].objective_name;
                        criteriaDatas["key_area"] = empDatas[0].mdlEpaKPIDetails[i].key_area;
                        criteriaDatas["expected_result"] = empDatas[0].mdlEpaKPIDetails[i].expected_result;
                        criteriaDatas["wtg"] = empDatas[0].mdlEpaKPIDetails[i].wtg;
                        criteriaDatas["self"] = empDatas[0].mdlEpaKPIDetails[i].self;
                        criteriaDatas["aggreed"] = empDatas[0].mdlEpaKPIDetails[i].aggreed;
                        criteriaDatas["score"] = empDatas[0].mdlEpaKPIDetails[i].score;
                        criteriaDatas["comment"] = empDatas[0].mdlEpaKPIDetails[i].comment;
                        criteriaDatas["is_deleted"] = empDatas[0].mdlEpaKPIDetails[i].is_deleted;

                        for (var j = 0; j < empDatas[0].mdlEpaKPIDetails[i].mdlEpaKPICriteriaDetails.length; j++) {
                            var val = j + 1;
                            var index = empDatas[0].mdlEpaKPIDetails[i].mdlEpaKPICriteriaDetails.findIndex(function (item, i) {
                                return item.crit_number === val
                            });
                            if (index >= 0) {
                                criteriaDatas["criteria" + (j + 1) + "id"] = empDatas[0].mdlEpaKPIDetails[i].mdlEpaKPICriteriaDetails[index].crit_id;
                                criteriaDatas["criteria" + (j + 1) + "name"] = empDatas[0].mdlEpaKPIDetails[i].mdlEpaKPICriteriaDetails[index].criteria_name;
                                haveCriteria = true;
                            }
                        }

                        kpiDatas.push(criteriaDatas);
                    }


                }

                ///////////////////////epa_start_date////////////////////


                var epa_start_date = (empDatas[0].epa_start_date != null && empDatas[0].epa_start_date != "") ? new Date(empDatas[0].epa_start_date) : new Date("2000-01-01");
                var epa_end_date = (empDatas[0].epa_end_date != null && empDatas[0].epa_end_date != "") ? new Date(empDatas[0].epa_end_date) : new Date("2000-01-01");
                var dt = new Date();
                var current_date = new Date(dt);

                if (dateCheck(GetDateFormatddMMyyyy(epa_start_date), GetDateFormatddMMyyyy(epa_end_date), GetDateFormatddMMyyyy(current_date))) {

                    if (empDatas[0].epa_close_status != "1") {
                        $('#btnsave').css('display', 'block');
                        epa_runing = true;
                    }
                    else {
                        epa_runing = false;
                        $('#btnsave').css('display', 'none');

                    }

                }
                else {
                    epa_runing = false;
                    $('#btnsave').css('display', 'none');
                }

                ////hide epa button if not initiate
                //for (var i = 0; i < status_data.length; i++) {
                //    if (status_data[i].display_for == 1) {
                //        if (status_data[i].epa_status_id > empDatas[0].epa_current_status) {
                //            //$('#ddlStatus').append($("<option></option>").val(status_data[i].epa_status_id).html(status_data[i].status_name));

                //            $('#btnsave').css('display', 'block');
                //            break;
                //        }
                //    }
                //}
                ////hide epa button if not initiate


                /////////////////////////////////////////////////////////////

                //append columns
                $('#submission_id').val(empDatas[0].submission_id);
                $('#emp_off_id').val(empDatas[0].emp_off_id);
                $('#desig_id').val(empDatas[0].desig_id);
                $('#department_id').val(empDatas[0].department_id);
                $('#epa_start_date').val(empDatas[0].epa_start_date);
                $('#epa_end_date').val(empDatas[0].epa_end_date);

                var epastartdate = new Date(res[0].epa_start_date);
                $('#lblw_epa_start_date').text(GetDateFormatddMMyyyy(epastartdate));

                var epaenddate = new Date(res[0].epa_end_date);
                $('#lblw_epa_end_date').text(GetDateFormatddMMyyyy(epaenddate));


                $('#lbl_current_status').text(empDatas[0].epa_current_status_name);
                $('#lblclocestatus').text(empDatas[0].epa_close_status_name);

                $('#w_r_id').val(empDatas[0].w_r_id);
                $('#emp_id').val(empDatas[0].emp_id);
                kpiColumn.push({
                    "render": function (data, type, full, meta) {
                        return '<input type="checkbox" checked="checked" class="chkRow" id="chk' + full.key_area_id + '" value="' + full.key_area_id + '" />';
                    }
                });
                kpiColumn.push({ "data": "kpi_submission_id", "name": "kpi_submission_id", "title": "kpi_submission_id" });

                kpiColumn.push({ "data": "key_area_id", "name": "key_area_id", "title": "key_area_id" });

                kpiColumn.push({

                    "title": "Objective Type",
                    "render": function (data, type, full, meta) {
                        BindKPIObjectiveType("dll_objective_type_" + full.key_area_id + "", default_company, full.otype_id);
                        return '<select  disabled="disabled"  id="dll_objective_type_' + full.key_area_id + '" class="form-control"> <option value="0">--Select--</option></select>';
                    }
                });

                kpiColumn.push({ "data": "key_area", "name": "key_area", "title": "Key Aera", });
                kpiColumn.push({ "data": "expected_result", "name": "expected_result", "title": "Expected Result" });
                if (haveCriteria) {
                    for (var j = 0; j < empDatas[0].mdlEpaKPIDetails[0].mdlEpaKPICriteriaDetails.length; j++) {
                        kpiColumn.push({ "data": "criteria" + (j + 1) + "name", "name": "criteria" + (j + 1) + "name", "title": "criteria " + (j + 1), });

                    }
                }
                kpiColumn.push({
                    "title": "Wtg (%)",
                    "render": function (data, type, full, meta) {
                        return '<input  typr="text" style="width:60px" disabled="disabled" value="' + full.wtg + '"  id="txt_wtg_' + full.key_area_id + '" class="form-control" />';
                    }
                });

                kpiColumn.push({
                    "title": "Self",
                    "render": function (data, type, full, meta) {
                        $('#dll_self_' + full.key_area_id).val(full.self);
                        return '<select disabled="disabled" style="width:60px" id="dll_self_' + full.key_area_id + '" class="form-control"> <option value="0">--Select--</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option></select>';
                    }
                });

                kpiColumn.push({
                    "title": "Agreed",
                    "render": function (data, type, full, meta) {

                        $('#dll_agreed_' + full.key_area_id).val(full.aggreed);
                        return '<select disabled="disabled" style="width:60px" id="dll_agreed_' + full.key_area_id + '" class="form-control">  <option value="0">--Select--</option><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option></select>';
                    }
                });

                kpiColumn.push({
                    "title": "Score",
                    "render": function (data, type, full, meta) {
                        return '<input typr="text" style="width:60px" disabled="disabled" value="' + full.score + '"  id="txt_score_' + full.key_area_id + '" class="form-control" />';
                    }
                });
                kpiColumn.push({ "data": "comment", "name": "comment", "title": "Comment", });







                //Employee Details
                if (res[0].rm1_name != null) {
                    $('#lblmanager11').text(res[0].rm1_name);
                }
                if (res[0].rm2_name != null) {
                    $('#lblmanager22').text(res[0].rm2_name);
                }
                if (res[0].rm3_name != null) {
                    $('#lblmanager33').text(res[0].rm3_name);
                }

                $('#lblDesignation').text(res[0].desig_name);

                var date_of_joining = new Date(res[0].joining_dt);
                $('#lblJoining').text(GetDateFormatddMMyyyy(date_of_joining));

                $('#lblw_r_name').text(res[0].w_r_name);
                $('#lbldepartment_name').text(res[0].department_name);

                //////////////////// End Employee Details ////////////////////////////////////////////////


                /////////////// Tab ///////////////////////////////////////////////

                $('#ul_tab').append("<li class='nav-item'><a class='nav-link active' data-toggle='tab' href='#tab_KPI' role='tab'>KPI - Objective Setting</a></li>");
                $('#ul_tab').append("<li class='nav-item'><a class='nav-link' data-toggle='tab' href='#tab_KRA' role='tab'>KRA - Accountabilities</a></li>");

                $('#tab_div_data').append('<div class="tab-pane active p-3" id="tab_KPI" role="tabpanel"><table id="tbl_tab_kpi" class="table table-bordered mb-0"></table></div>');
                $('#tab_div_data').append('<div class="tab-pane p-3" id="tab_KRA" role="tabpanel"><table id="tbl_tab_kra" class="table table-bordered mb-0"></table></div>');


                tab_array = [];
                if (res[0].mdlEpaTabQuestions != null) {
                    for (var i = 0; i < res[0].mdlEpaTabQuestions.length; i++) {

                        //If tab name already exists
                        if (jQuery.inArray(res[0].mdlEpaTabQuestions[i].tab_name, tab_array) != -1) {

                            ///if (res[0].mdlEpaTabQuestions[i].ans_type == 2) {



                            var _appendtable;
                            _appendtable = "<tr><td> Q. " + res[0].mdlEpaTabQuestions[i].ques + "</td></tr>";
                            _appendtable += "<tr><td style='display: none;'><input type='text' id='txt_qi_" + i + "' value='" + res[0].mdlEpaTabQuestions[i].question_id + "' /></td><td>";
                            _appendtable += "" + res[0].mdlEpaTabQuestions[i].ans_type == 1 ? ('<div class="col-md-12"><div class="col-md-6"><textarea  class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + (res[0].mdlEpaTabQuestions[i].question_answer != null && res[0].mdlEpaTabQuestions[i].question_answer != "" ? res[0].mdlEpaTabQuestions[i].question_answer : "") + '</textarea></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 2 ? ('<div class="col-md-12"><div class="col-md-3"><select disabled="disabled" class="form-control" id="ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '">  </select></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 3 ? ('<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div><div class="col-md-6"><textarea disabled="disabled" class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + res[0].mdlEpaTabQuestions[i].remarks + '</textarea></div></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 4 ? ('<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div></div>') : '' + "";

                            $('#tbl_' + res[0].mdlEpaTabQuestions[i].tab_id).append(_appendtable);

                            //$('#tbl_' + res[0].mdlEpaTabQuestions[i].tab_id).append("<tr><td> Q. " + res[0].mdlEpaTabQuestions[i].ques + "</tr><tr><td style='display: none;'><input type='text' id='txt_qi_" + i + "' value='" + res[0].mdlEpaTabQuestions[i].question_id + "' /></td><td>" + res[0].mdlEpaTabQuestions[i].ans_type == 1 ? ('<div class="col-md-12"><div class="col-md-6"><textarea class="form-control" disabled="disabled" id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '">' + res[0].mdlEpaTabQuestions[i].question_answer != "" && res[0].mdlEpaTabQuestions[i] != null ? res[0].mdlEpaTabQuestions[i].question_answer : '' + '</textarea></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 2 ? ('<div class="col-md-12"><div class="col-md-3"><select disabled="disabled" class="form-control" id="ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '"><option value="">Please Select</option>  </select></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 3 ? '<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for= "rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for= "rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div><div class="col-md-6"><textarea disabled="disabled" class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + res[0].mdlEpaTabQuestions[i].remarks + '</textarea></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 4 ? ('<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div></div>') : '' + "</td></tr>");
                            // $('#tbl_' + res[0].mdlEpaTabQuestions[i].tab_id).append("<tr><td> Q. " + res[0].mdlEpaTabQuestions[i].ques + "</td></tr><tr><td style='display: none;'><input type='text' id='txt_qi_" + i + "' value='" + res[0].mdlEpaTabQuestions[i].question_id + "' /></td><td>" + res[0].mdlEpaTabQuestions[i].ans_type == 1 ? ('<div class="col-md-12"><div class="col-md-6"><textarea  class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + (res[0].mdlEpaTabQuestions[i].question_answer != null && res[0].mdlEpaTabQuestions[i].question_answer != "" ? res[0].mdlEpaTabQuestions[i].question_answer : "") + '</textarea></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 2 ? ('<div class="col-md-12"><div class="col-md-3"><select disabled="disabled" class="form-control" id="ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '">  </select></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 3 ? ('<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div><div class="col-md-6"><textarea disabled="disabled" class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + res[0].mdlEpaTabQuestions[i].remarks + '</textarea></div></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 4 ? ('<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div></div>') : '' + "</td></tr>");


                            $.each(res[0].mdlEpaTabQuestions[i].ans_type_ddl.split(','), function (g, e) {
                                $('#ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i).append('<option value="' + e.trim() + '">' + e.trim() + '</option>');
                            });

                            if (res[0].mdlEpaTabQuestions[i].ans_type == 3 || res[0].mdlEpaTabQuestions[i].ans_type == 4) {
                                if (res[0].mdlEpaTabQuestions[i].question_answer != "" && res[0].mdlEpaTabQuestions[i].question_answer != null) {
                                    $("input[name=" + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "][value=" + res[0].mdlEpaTabQuestions[i].question_answer + "]").prop('checked', true);
                                }
                            }

                            if (res[0].mdlEpaTabQuestions[i].question_answer != "" && res[0].mdlEpaTabQuestions[i].question_answer != null) {
                                $('#ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i).val(res[0].mdlEpaTabQuestions[i].question_answer.trim());
                            }

                            // };



                        } else {

                            tab_array[i] = res[0].mdlEpaTabQuestions[i].tab_name;

                            $('#ul_tab').append("<li class='nav-item'><a class='nav-link' data-toggle='tab' href='#div_" + res[0].mdlEpaTabQuestions[i].tab_id + "' role='tab'>" + tab_array[i] + "</a></li>");

                            var _createdivtable = "<div class='tab-pane p-3' id='div_" + res[0].mdlEpaTabQuestions[i].tab_id + "' role='tabpanel'><table id='tbl_" + res[0].mdlEpaTabQuestions[i].tab_id + "' class='table table-bordered mb-0' style='width: 100 %'>";
                            _createdivtable += "<tr><td> Q. " + res[0].mdlEpaTabQuestions[i].ques + "</td></tr>";
                            _createdivtable += "<tr><td style='display: none;'><input type='text' id='txt_qi_" + i + "' value='" + res[0].mdlEpaTabQuestions[i].question_id + "' /></td><td>";
                            _createdivtable += "" + res[0].mdlEpaTabQuestions[i].ans_type == 1 ? ('<div class="col-md-12"><div class="col-md-6"><textarea  class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + (res[0].mdlEpaTabQuestions[i].question_answer != null && res[0].mdlEpaTabQuestions[i].question_answer != "" ? res[0].mdlEpaTabQuestions[i].question_answer : "") + '</textarea></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 2 ? ('<div class="col-md-12"><div class="col-md-3"><select disabled="disabled" class="form-control" id="ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '">  </select></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 3 ? ('<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div><div class="col-md-6"><textarea disabled="disabled" class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + res[0].mdlEpaTabQuestions[i].remarks + '</textarea></div></div></div>') : res[0].mdlEpaTabQuestions[i].ans_type == 4 ? ('<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div></div>') : '' + "";
                            //testtab += "" + res[0].mdlEpaTabQuestions[i].ans_type == 1 ? ('<div class="col-md-12"><div class="col-md-6"><textarea  class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + (res[0].mdlEpaTabQuestions[i].question_answer != null && res[0].mdlEpaTabQuestions[i].question_answer != "") ? res[0].mdlEpaTabQuestions[i].question_answer : 'supriii' + '</textarea></div></div>') : 'supriya' + "";
                            _createdivtable += "</td></tr>";
                            _createdivtable += "</table></div>";

                            $('#tab_div_data').append(_createdivtable);

                            //$('#tab_div_data').append(  ? '' : '') + "</td></tr></table></div>");

                            //$('#tab_div_data').append("<div class='tab-pane p-3' id='div_" + res[0].mdlEpaTabQuestions[i].tab_id + "' role='tabpanel'><table id='tbl_" + res[0].mdlEpaTabQuestions[i].tab_id + "' class='table table-bordered mb-0' style='width: 100 %'><tr><td> Q. " + res[0].mdlEpaTabQuestions[i].ques + "</td></tr><tr><td style='display: none;'><input type='text' id='txt_qi_" + i + "' value='" + res[0].mdlEpaTabQuestions[i].question_id + "' /></td><td>" + (res[0].mdlEpaTabQuestions[i].ans_type == 1 ? '<div class="col-md-12"><div class="col-md-6"><textarea disabled="disabled" class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + (res[0].mdlEpaTabQuestions[i].question_answer != null && res[0].mdlEpaTabQuestions[i].question_answer != "") ? res[0].mdlEpaTabQuestions[i].question_answer :"" + '</textarea></div></div>' : res[0].mdlEpaTabQuestions[i].ans_type == 2 ? '<div class="col-md-12"><div class="col-md-3"><select disabled="disabled" class="form-control" id="ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '">  </select></div></div>' : res[0].mdlEpaTabQuestions[i].ans_type == 3 ? '<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div><div class="col-md-6"><textarea disabled="disabled" class="form-control"  id="txt_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >' + res[0].mdlEpaTabQuestions[i].remarks + '</textarea></div></div>' : res[0].mdlEpaTabQuestions[i].ans_type == 4 ? '<div class="col-md-12"><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="Yes" class="custom-control-input"><label class="custom-control-label" for="rad_y' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" > Yes </label></div></div><div class="col-md-1"><div class="custom-control custom-radio"><input type="radio" id="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" name="' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" value="No" class="custom-control-input"><label class="custom-control-label" for="rad_n' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + '" >  No </label></div></div></div>' : '') + "</td></tr></table></div>");

                            //  if (res[0].mdlEpaTabQuestions[i].ans_type == 2) {


                            $.each(res[0].mdlEpaTabQuestions[i].ans_type_ddl.split(','), function (g, e) {
                                $('#ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i).append('<option value="' + e.trim() + '">' + e.trim() + '</option>');
                            });

                            if (res[0].mdlEpaTabQuestions[i].ans_type == 3 || res[0].mdlEpaTabQuestions[i].ans_type == 4) {
                                if (res[0].mdlEpaTabQuestions[i].question_answer != "" && res[0].mdlEpaTabQuestions[i].question_answer != null) {
                                    $("input[name=" + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "][value=" + res[0].mdlEpaTabQuestions[i].question_answer + "]").prop('checked', true);
                                }
                            }

                            if (res[0].mdlEpaTabQuestions[i].question_answer != "" && res[0].mdlEpaTabQuestions[i].question_answer != null) {
                                $('#ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i).val(res[0].mdlEpaTabQuestions[i].question_answer.trim());
                            }
                            //else {
                            //    $('#ddl_' + res[0].mdlEpaTabQuestions[i].tab_id + '_' + i).val(res[0].mdlEpaTabQuestions[i].question_answer);
                            //}


                            // };

                        }
                    }
                }
                $('#ul_tab').append("<li class='nav-item'><a class='nav-link' data-toggle='tab' href='#tab_FinalReview' role='tab'>Final Review</a></li>");
                $('#tab_div_data').append('<div class="tab-pane p-3" id="tab_FinalReview" role="tabpanel"> <div class="col-md-12 col-xl-12" id="tab_FinalReview_tab1"><div class="card m-b-30 text-center"><div class="card-header"><ul class="nav nav-tabs card-header-tabs"><li class="nav-item"><a class="nav-link active" href="#">Final Review</a></li></ul></div><div class="card-body"><div class="form-group mb-0 row"><div class="col-md-12"><div class="col-md-2"><input type="radio" id="tab_FinalReview_tab_0" checked name="tab_FinalReview_tab_1" value="0" class="custom-control-input"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_1" name="tab_FinalReview_tab_1" value="1" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_1">Outstanding</label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_2" name="tab_FinalReview_tab_1" value="2" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_2">Good </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_3" name="tab_FinalReview_tab_1" value="3" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_3">Standard </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_4" name="tab_FinalReview_tab_1" value="4" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_4">Need Improvement </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_5" name="tab_FinalReview_tab_1" value="5" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_5">Poor </label></div></div></div><div class="col-md-12"><br /></div><div class="col-md-12"><div class="custom-control custom-radio"><label class="custom-control" style="float: left;">  Final Remark for Employee</label><textarea id="tab_FinalReview_tab_text" disabled="disabled" style="width: 79%;float: right;" class="form-control"></textarea></div></div></div></div></div></div><div class="col-md-12"><hr /></div><div class="col-md-12 col-xl-12" id="tab_FinalReview_tab_m1"><div class="card m-b-30 text-center"><div class="card-header"><ul class="nav nav-tabs card-header-tabs"><li class="nav-item"><a class="nav-link active" href="#">Reporting Manager 1</a></li></ul></div><div class="card-body"><div class="form-group mb-0 row"><div class="col-md-12"><div class="col-md-2"><input type="radio" id="tab_FinalReview_tab_m1_0" checked name="tab_FinalReview_tab_m1" style="dispaly:none" value="0" class="custom-control-input"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m1_1" name="tab_FinalReview_tab_m1" value="1" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m1_1">Outstanding</label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m1_2" name="tab_FinalReview_tab_m1" value="2" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m1_2">Good </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m1_3" name="tab_FinalReview_tab_m1" value="3" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m1_3">Standard </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m1_4" name="tab_FinalReview_tab_m1" value="4" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m1_4">Need Improvement </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m5" name="tab_FinalReview_tab_m1" value="5" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m5">Poor </label></div></div><div class="col-md-12"><br /></div><div class="col-md-12"> <div class="custom-control custom-radio"><label class="custom-control" style="float: left;">  Final Remark for Employee</label><textarea id="tab_FinalReview_tab_m1_text" disabled="disabled" style="width: 79%;float: right;" class="form-control"></textarea></div></div></div></div></div></div></div><div class="col-md-12"><hr /></div><div class="col-md-12 col-xl-12" id="tab_FinalReview_tab_m2"><div class="card m-b-30 text-center"><div class="card-header"><ul class="nav nav-tabs card-header-tabs"><li class="nav-item"><a class="nav-link active" href="#">Reporting Manager 2</a></li></ul></div><div class="card-body"><div class="form-group mb-0 row"><div class="col-md-12"><div class="col-md-2"><input type="radio" id="tab_FinalReview_tab_m2_0" checked name="tab_FinalReview_tab_m2" value="0" class="custom-control-input"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m2_1" name="tab_FinalReview_tab_m2" value="1" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m2_1">Outstanding</label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m2_2" name="tab_FinalReview_tab_m2" value="2" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m2_2">Good </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m2_3" name="tab_FinalReview_tab_m2" value="3" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m2_3">Standard </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m2_4" name="tab_FinalReview_tab_m2" value="4" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m2_4">Need Improvement </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m2_5" name="tab_FinalReview_tab_m2" value="5" class="custom-control-input"><label class="custom-control-label" disabled="disabled" for="tab_FinalReview_tab_m2_5">Poor </label></div></div><div class="col-md-12"><br /></div><div class="col-md-12"><div class="custom-control custom-radio"><label class="custom-control" style="float: left;">  Remark for Employee</label><textarea id="tab_FinalReview_tab_m2_text" disabled="disabled" style="width: 79%;float: right;" class="form-control"></textarea></div></div></div></div></div></div></div><div class="col-md-12"><hr /></div><div class="col-md-12 col-xl-12" id="tab_FinalReview_tab_m3"><div class="card m-b-30 text-center"><div class="card-header"><ul class="nav nav-tabs card-header-tabs"><li class="nav-item"><a class="nav-link active" href="#">Reporting Manager 3</a></li></ul></div><div class="card-body"><div class="form-group mb-0 row"><div class="col-md-12"><div class="col-md-2"><input type="radio" id="tab_FinalReview_tab_m3_0" checked name="tab_FinalReview_tab_m3" value="0" class="custom-control-input"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m3_1" name="tab_FinalReview_tab_m3" value="1" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m3_1">Outstanding</label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m3_2" name="tab_FinalReview_tab_m3" value="2" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m3_2">Good </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m3_3" name="tab_FinalReview_tab_m3" value="3" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m3_3">Standard </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m3_4" name="tab_FinalReview_tab_m3" value="4" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m3_4">Need Improvement </label></div></div><div class="col-md-2"><div class="custom-control custom-radio"><input type="radio" id="tab_FinalReview_tab_m3_5" name="tab_FinalReview_tab_m3" value="5" class="custom-control-input"><label class="custom-control-label" for="tab_FinalReview_tab_m3_5">Poor </label></div></div><div class="col-md-12"><br /></div><div class="col-md-12"><div class="custom-control custom-radio"><label class="custom-control" style="float: left;">  Remark for Employee</label><textarea id="tab_FinalReview_tab_m3_text" disabled="disabled" style="width: 79%;float: right;" class="form-control"></textarea></div></div></div></div></div></div></div></div>');

                $("#tab_FinalReview_tab_1").prop("disabled", true);
                $("#tab_FinalReview_tab_2").prop("disabled", true);
                $("#tab_FinalReview_tab_3").prop("disabled", true);
                $("#tab_FinalReview_tab_4").prop("disabled", true);
                $("#tab_FinalReview_tab_5").prop("disabled", true);


                $("#tab_FinalReview_tab_m1_1").prop("disabled", true);
                $("#tab_FinalReview_tab_m1_2").prop("disabled", true);
                $("#tab_FinalReview_tab_m1_3").prop("disabled", true);
                $("#tab_FinalReview_tab_m1_4").prop("disabled", true);
                $("#tab_FinalReview_tab_m5").prop("disabled", true);

                $("#tab_FinalReview_tab_m2_1").prop("disabled", true);
                $("#tab_FinalReview_tab_m2_2").prop("disabled", true);
                $("#tab_FinalReview_tab_m2_3").prop("disabled", true);
                $("#tab_FinalReview_tab_m2_4").prop("disabled", true);
                $("#tab_FinalReview_tab_m2_5").prop("disabled", true);

                $("#tab_FinalReview_tab_m3_1").prop("disabled", true);
                $("#tab_FinalReview_tab_m3_2").prop("disabled", true);
                $("#tab_FinalReview_tab_m3_3").prop("disabled", true);
                $("#tab_FinalReview_tab_m3_4").prop("disabled", true);
                $("#tab_FinalReview_tab_m3_5").prop("disabled", true);





                /////////////// Tab End //////////////////////////////////////////


                /////////////////////////// Start Tab KPI - Objective Setting ////////////////////////////////////////

                $("#tbl_tab_kpi").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": false,
                    "filter": false, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once
                    "scrollX": 150,
                    "paging": false,
                    "ordering": false,
                    //"info": false,
                    "data": kpiDatas,
                    "columnDefs":
                        [
                            {
                                "width": "160px",
                                "targets": [3]
                            },
                            {
                                targets: [0, 1, 2,],
                                className: "hide_column"
                            }],
                    "columns":
                        kpiColumn,

                });


                /////////////////////////// End Tab KPI - Objective Setting ////////////////////////////////////////



                /////////////////////////// Start KRA - Accountabilities ////////////////////////////////////////


                var mdlEpaKRADetails = res[0].mdlEpaKRADetails;

                $("#tbl_tab_kra").DataTable({
                    "processing": true, // for show progress bar
                    "serverSide": false, // for process server side
                    "bDestroy": false,
                    "filter": false, // this is for disable filter (search box)
                    "orderMulti": false, // for disable multiple column at once
                    //"scrollX": 150,
                    "paging": false,
                    "ordering": false,
                    //"info": false,
                    "aaData": mdlEpaKRADetails,
                    "columnDefs":
                        [{
                            targets: [0],
                            className: "hide_column"
                        }],
                    "columns": [
                        { "data": "kra_id", "name": "kra_id", "title": "kra_id", },
                        { "data": "description", "name": "description", "title": "Description", },
                        { "data": "factor_result", "name": "factor_result", "title": "Factor/ Expected Result", },
                        {
                            "title": "Rating",
                            "render": function (data, type, full, meta) {
                                BindkpiRatingList("dll_rating_" + full.kra_id + "", default_company, full.rating_id);
                                return '<select disabled="disabled"  id="dll_rating_' + full.kra_id + '" class="form-control"> <option value="0">--Select--</option></select>';
                            }

                        }
                    ],

                });


                /////////////////////////// End KRA - Accountabilities ////////////////////////////////////////


                /////////////////////////////  Enable and disable //////////////////////////////////////////

                var count_of_emp = $('#ddlempcode option').length;
                $('#ddlStatus').empty();
                if (count_of_emp <= 2) {
                    if (epa_runing) {

                        if (empDatas[0].epa_current_status == "") {

                        }

                        $('#ddlStatus').append('<option selected="selected" value="0">--Select--</option>');

                        //for (var i = 0; i < status_data.length; i++) {
                        //    if (status_data[i].display_for == 1) {
                        //        if (status_data[i].epa_status_id > empDatas[0].epa_current_status) {
                        //            $('#ddlStatus').append($("<option></option>").val(status_data[i].epa_status_id).html(status_data[i].status_name));
                        //        }
                        //    }
                        //}

                        if (empDatas[0].mdlEPANextStatusDetails != null && empDatas[0].mdlEPANextStatusDetails.length > 0) {
                            for (var j = 0; j < empDatas[0].mdlEPANextStatusDetails.length; j++) {
                                $('#ddlStatus').append($("<option></option>").val(empDatas[0].mdlEPANextStatusDetails[j].next_status_id).html(empDatas[0].mdlEPANextStatusDetails[j].next_Status_name));
                            }
                        }

                        if (empDatas[0].epa_current_status != "6" && empDatas[0].epa_current_status != "7" && empDatas[0].epa_current_status != "4" && empDatas[0].epa_close_status != "1") {

                            if (empDatas[0].mdlEpaKPIDetails != null && empDatas[0].mdlEpaKPIDetails.length > 0) {
                                for (var j = 0; j < empDatas[0].mdlEpaKPIDetails.length; j++) {
                                    $('#dll_self_' + empDatas[0].mdlEpaKPIDetails[j].key_area_id).prop("disabled", false);
                                }
                            }

                            if (empDatas[0].mdlEpaKRADetails != null && empDatas[0].mdlEpaKRADetails.length > 0) {
                                for (var j = 0; j < empDatas[0].mdlEpaKRADetails.length; j++) {
                                    $('#dll_rating_' + empDatas[0].mdlEpaKRADetails[j].kra_id).prop("disabled", false);
                                }

                            }

                            if (empDatas[0].mdlEpaTabQuestions != null && empDatas[0].mdlEpaTabQuestions.length > 0) {

                                for (var i = 0; i < empDatas[0].mdlEpaTabQuestions.length; i++) {
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 1 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);

                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 2 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $('#ddl_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);
                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 3 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", false);
                                        $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);
                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 4 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", false);
                                    }
                                }
                            }

                        }

                        if (empDatas[0].epa_current_status == "6" || empDatas[0].epa_current_status == "7" || empDatas[0].epa_current_status == "4" || empDatas[0].epa_close_status == "1") {

                            if (empDatas[0].mdlEpaTabQuestions != null && empDatas[0].mdlEpaTabQuestions.length > 0) {

                                for (var i = 0; i < empDatas[0].mdlEpaTabQuestions.length; i++) {
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 1 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", true);

                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 2 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $('#ddl_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", true);
                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 3 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", false);
                                        $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", true);
                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 4 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                        $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", true);
                                    }
                                }
                            }


                        }








                    }
                }
                if (count_of_emp > 2) {

                    var data_emp_id = $('#emp_id').val();
                    if (data_emp_id == login_emp) {
                        if (epa_runing) {


                            $('#ddlStatus').append('<option selected="selected" value="0">--Select--</option>');

                            //for (var i = 0; i < status_data.length; i++) {
                            //    if (status_data[i].display_for == 1) {
                            //        if (status_data[i].epa_status_id > empDatas[0].epa_current_status) {
                            //            $('#ddlStatus').append($("<option></option>").val(status_data[i].epa_status_id).html(status_data[i].status_name));
                            //        }
                            //    }
                            //}

                            if (empDatas[0].mdlEPANextStatusDetails != null && empDatas[0].mdlEPANextStatusDetails.length > 0) {
                                for (var j = 0; j < empDatas[0].mdlEPANextStatusDetails.length; j++) {
                                    $('#ddlStatus').append($("<option></option>").val(empDatas[0].mdlEPANextStatusDetails[j].next_status_id).html(empDatas[0].mdlEPANextStatusDetails[j].next_Status_name));
                                }
                            }

                            if (empDatas[0].epa_current_status != "6" && empDatas[0].epa_current_status != "7" && empDatas[0].epa_current_status != "4" && empDatas[0].epa_close_status != "1") {

                                if (empDatas[0].mdlEpaKPIDetails != null && empDatas[0].mdlEpaKPIDetails.length > 0) {

                                    for (var j = 0; j < empDatas[0].mdlEpaKPIDetails.length; j++) {
                                        $('#dll_self_' + empDatas[0].mdlEpaKPIDetails[j].key_area_id).prop("disabled", false);
                                    }
                                }


                                if (empDatas[0].mdlEpaKRADetails != null && empDatas[0].mdlEpaKRADetails.length > 0) {

                                    for (var j = 0; j < empDatas[0].mdlEpaKRADetails.length; j++) {
                                        $('#dll_rating_' + empDatas[0].mdlEpaKRADetails[j].kra_id).prop("disabled", false);
                                    }

                                }



                                if (empDatas[0].mdlEpaTabQuestions != null && empDatas[0].mdlEpaTabQuestions.length > 0) {
                                    for (var i = 0; i < empDatas[0].mdlEpaTabQuestions.length; i++) {
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 1 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);

                                        }
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 2 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $('#ddl_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);
                                        }
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 3 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", false);
                                            $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);
                                        }
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 4 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", false);
                                        }
                                    }
                                }

                            }



                            if (empDatas[0].epa_current_status == "6" || empDatas[0].epa_current_status == "7" || empDatas[0].epa_current_status == "4" || empDatas[0].epa_close_status == "1") {

                                if (empDatas[0].mdlEpaTabQuestions != null && empDatas[0].mdlEpaTabQuestions.length > 0) {
                                    for (var i = 0; i < empDatas[0].mdlEpaTabQuestions.length; i++) {
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 1 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", true);

                                        }
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 2 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $('#ddl_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", true);
                                        }
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 3 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", true);
                                            $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", true);
                                        }
                                        if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 4 && empDatas[0].mdlEpaTabQuestions[i].for_all_emp == 1) {
                                            $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", true);
                                        }
                                    }
                                }

                            }







                        }
                    }
                    else {
                        if (epa_runing) {

                            if (empDatas[0].mdlEpaKPIDetails != null && empDatas[0].mdlEpaKPIDetails.length > 0) {

                                for (var j = 0; j < empDatas[0].mdlEpaKPIDetails.length; j++) {

                                    $('#dll_agreed_' + empDatas[0].mdlEpaKPIDetails[j].key_area_id).prop("disabled", false);


                                }
                            }





                            //start added by supriya
                            if (empDatas[0].mdlEpaTabQuestions != null && empDatas[0].mdlEpaTabQuestions.length > 0) {
                                for (var i = 0; i < empDatas[0].mdlEpaTabQuestions.length; i++) {
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 1 && empDatas[0].mdlEpaTabQuestions[i].is_manager == 1) {
                                        $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);

                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 2 && empDatas[0].mdlEpaTabQuestions[i].is_manager == 1) {
                                        $('#ddl_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);
                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 3 && empDatas[0].mdlEpaTabQuestions[i].is_manager == 1) {
                                        $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", false);
                                        $('#txt_' + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i).prop("disabled", false);
                                    }
                                    if (empDatas[0].mdlEpaTabQuestions[i].ans_type == 4 && empDatas[0].mdlEpaTabQuestions[i].is_manager == 1) {
                                        $("input[name='" + empDatas[0].mdlEpaTabQuestions[i].tab_id + '_' + i + "']:checked").prop("disabled", false);
                                    }
                                }
                            }





                            //end by supriya

                            var check_manager_number = "0";
                            if (empDatas[0].rm_id1 == login_emp) {
                                check_manager_number = 1;
                            }
                            else if (empDatas[0].rm_id2 == login_emp) {
                                check_manager_number = 2
                            }
                            else if (empDatas[0].rm_id3 == login_emp) {
                                check_manager_number = 3
                            }

                            $('#ddlStatus').append('<option selected="selected" value="0">--Select--</option>');

                            if (empDatas[0].mdlEPANextStatusDetails != null && empDatas[0].mdlEPANextStatusDetails.length > 0) {

                                for (var j = 0; j < empDatas[0].mdlEPANextStatusDetails.length; j++) {
                                    $('#ddlStatus').append($("<option></option>").val(empDatas[0].mdlEPANextStatusDetails[j].next_status_id).html(empDatas[0].mdlEPANextStatusDetails[j].next_Status_name));
                                }
                            }


                            //for (var i = 0; i < status_data.length; i++) {
                            //    if (check_manager_number == 1) {
                            //        if (status_data[i].display_for_rm1 == 1) {
                            //            if (status_data[i].epa_status_id > empDatas[0].epa_current_status) {
                            //                $('#ddlStatus').append($("<option></option>").val(status_data[i].epa_status_id).html(status_data[i].status_name));
                            //            }
                            //        }
                            //    }
                            //   else if (check_manager_number == 2) {
                            //        if (status_data[i].display_for_rm2 == 1) {
                            //            $('#ddlStatus').append($("<option></option>").val(status_data[i].epa_status_id).html(status_data[i].status_name));
                            //        }
                            //    }
                            //   else if (check_manager_number == 3) {
                            //        if (status_data[i].display_for_rm3 == 1) {
                            //            $('#ddlStatus').append($("<option></option>").val(status_data[i].epa_status_id).html(status_data[i].status_name));
                            //        }
                            //    }
                            //}


                            if (res[0].rm_id1 == login_emp) {

                                $("#tab_FinalReview_tab_m1_1").prop("disabled", false);
                                $("#tab_FinalReview_tab_m1_2").prop("disabled", false);
                                $("#tab_FinalReview_tab_m1_3").prop("disabled", false);
                                $("#tab_FinalReview_tab_m1_4").prop("disabled", false);
                                $("#tab_FinalReview_tab_m5").prop("disabled", false);

                                $("#tab_FinalReview_tab_m1_text").prop("disabled", false);
                            }
                            else if (res[0].rm_id2 == login_emp) {
                                $("#tab_FinalReview_tab_m2_1").prop("disabled", false);
                                $("#tab_FinalReview_tab_m2_2").prop("disabled", false);
                                $("#tab_FinalReview_tab_m2_3").prop("disabled", false);
                                $("#tab_FinalReview_tab_m2_4").prop("disabled", false);
                                $("#tab_FinalReview_tab_m2_5").prop("disabled", false);

                                $("#tab_FinalReview_tab_m2_text").prop("disabled", false);
                            }
                            else if (res[0].rm_id3 == login_emp) {
                                $("#tab_FinalReview_tab_m3_1").prop("disabled", false);
                                $("#tab_FinalReview_tab_m3_2").prop("disabled", false);
                                $("#tab_FinalReview_tab_m3_3").prop("disabled", false);
                                $("#tab_FinalReview_tab_m3_4").prop("disabled", false);
                                $("#tab_FinalReview_tab_m3_5").prop("disabled", false);

                                $("#tab_FinalReview_tab_m3_text").prop("disabled", false);
                            }
                        }
                    }

                    /////////////////////////////  Enable and disable  /////////////////////////////////////////



                    /////////////////////////// Start Final Review ////////////////////////////////////////

                    //$("input[name=tab_FinalReview_tab_1][value=" + res[0].final_review + "]").prop('checked', true);
                    //$('#tab_FinalReview_tab_text').text(res[0].final_remarks);


                    //$("input[name=tab_FinalReview_tab_m1][value=" + res[0].final_review_rm1 + "]").prop('checked', true);
                    //$('#tab_FinalReview_tab_m1_text').text(res[0].final_remarks_rm1);
                    ////$('#rm_id1').text(res[0].rm_id1);

                    //$("input[name=tab_FinalReview_tab_m2][value=" + res[0].final_review_rm2 + "]").prop('checked', true);
                    //$('#tab_FinalReview_tab_m2_text').text(res[0].final_remarks_rm2);
                    ////$('#rm_id2').text(res[0].rm_id2);

                    //$("input[name=tab_FinalReview_tab_m3][value=" + res[0].final_review_rm3 + "]").prop('checked', true);
                    //$('#tab_FinalReview_tab_m3_text').text(res[0].final_remarks_rm3);
                    // $('#rm_id3').text(res[0].rm_id3);



                    /////////////////////////// End Final Review ////////////////////////////////////////
                }




                $("input[name=tab_FinalReview_tab_1][value=" + res[0].final_review + "]").prop('checked', true);
                $('#tab_FinalReview_tab_text').text(res[0].final_remarks);


                $("input[name=tab_FinalReview_tab_m1][value=" + res[0].final_review_rm1 + "]").prop('checked', true);
                $('#tab_FinalReview_tab_m1_text').text(res[0].final_remarks_rm1);
                //$('#rm_id1').text(res[0].rm_id1);

                $("input[name=tab_FinalReview_tab_m2][value=" + res[0].final_review_rm2 + "]").prop('checked', true);
                $('#tab_FinalReview_tab_m2_text').text(res[0].final_remarks_rm2);
                //$('#rm_id2').text(res[0].rm_id2);

                $("input[name=tab_FinalReview_tab_m3][value=" + res[0].final_review_rm3 + "]").prop('checked', true);
                $('#tab_FinalReview_tab_m3_text').text(res[0].final_remarks_rm3);
            }
        },
        Error: function (err) {

        }
    });
    //// END EPA FORM ////////////////////////////////////////////////////////////////////
    $('#loader').hide();
}



//function BindEmployeeListForEpa(ControlId, SelectedVal) {


//    var listapi = localStorage.getItem("ApiUrl");
//    var key = CryptoJS.enc.Base64.parse("#base64Key#");
//    var iv = CryptoJS.enc.Base64.parse("#base64IV#");
//    var user_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("user_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

//    var login_emp_name_dec = CryptoJS.AES.decrypt(localStorage.getItem("login_emp_name"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);

//    var login_name_code = login_emp_name_dec + "(" + user_name_dec + ")";

//    ControlId = '#' + ControlId;
//    $.ajax({
//        type: "GET",
//        //url: listapi + "apiMasters/Get_EmployeeHeadList",
//        url: listapi + "apiMasters/Get_Employee_Under_LoginEmp/" + SelectedVal,
//        data: {},
//        contentType: "application/json; charset=utf-8",
//        dataType: "json",
//        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
//        success: function (response) {
//            var res = response;


//            $(ControlId).append('<option selected="selected" value="0">--Select--</option>');

//            $(ControlId).append('<option selected="selected" value=' + SelectedVal + '>' + login_name_code + '</option>');

//            $.each(res, function (data, value) {
//                $(ControlId).append($("<option></option>").val(value.empid).html(value.empname + "(" + value.empcode + ")"));
//            })
//            if (SelectedVal != '' && SelectedVal != '0' && SelectedVal != 'undefined') {
//                $(ControlId).val(SelectedVal);
//            }

//            $('#loader').hide();

//        },
//        error: function (err) {
//            alert(err.responseText);
//            $('#loader').hide();
//        }
//    });
//}