$('#loader').show();
var login_role_id;
var default_company;
var login_emp_id;
var user_Dep_name;
var HaveDisplay;
var arr = new Array("1", "101", "105");
$(document).ready(function () {
    setTimeout(function () {

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }

        default_company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        login_emp_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        user_Dep_name = localStorage.getItem("user_Dep_name");
        login_role_id = CryptoJS.AES.decrypt(localStorage.getItem("emp_role_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
        $('#loader').hide();

        GetData();

    }, 2000);// end timeout

});


function GetData() {

    $('#loader').show();

    var apiurl = localStorage.getItem("ApiUrl") + 'apiNoDues/Get_NoDuesClearenceRequestOfEmps';
    $.ajax({
        type: "POST",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {


            if (res.statusCode != undefined) {
                messageBox("info", res.message);
                return false;
            }

            $("#tbl_EmpParticulars").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": true, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                //"scrollY": 200,
                "aaData": res,
                "columnDefs":
                    [
                        //{
                        //    targets: [5],
                        //    render: function (data, type, row) {
                        //        return data == '1' ? 'Active' : 'InActive'
                        //    }
                        //},
                        {
                            targets: [6, 7, 8],
                            render: function (data, type, row) {
                                var date = new Date(data);
                                return GetDateFormatddMMyyyy(date);
                            }
                        },
                    ],

                "columns": [
                    { "data": null, "title": "S.No.", "autoWidth": true },
                    {
                        "title": "Employee Code", "autowidth": true, "render": function (data, type, full, meta) {
                            if (full.is_outstanding == "Yes")
                                return '<a href="#" onClick="GetNoDuesParticularsofEmployee(' + full.emp_id + ',' + full.sepration_id + ',' + full.company_id + ')" >' + full.emp_code + '</a>';
                            else
                                return full.emp_code;
                        }
                    },
                    { "data": "emp_name", "name": "emp_name", "title": "Employee Name", "autoWidth": true },
                    { "data": "department_Name", "name": "department_Name", "title": "Department", "autoWidth": true },
                    { "data": "department_pending", "name": "department_pending", "title": "Pending Department", "autoWidth": true },
                    { "data": "is_outstanding", "name": "is_outstanding", "title": "Is Outstanding", "autoWidth": true },
                    //{ "data": "designation", "name": "designation", "title": "Designation", "autoWidth": true },
                    //{ "data": "location", "name": "location", "title": "Location", "autoWidth": true },
                    { "data": "_date_of_joining", "name": "_date_of_joining", "title": "Date of Joining", "autoWidth": true },
                    { "data": "date_of_Resign", "name": "date_of_Resign", "title": "Date of Resigning", "autoWidth": true },
                    { "data": "date_of_Reliving", "name": "date_of_Reliving", "title": "Date of Reliving", "autoWidth": true },

                    //{
                    //    "title": "Action", "autoWidth": true,
                    //    "render": function (data, type, full, meta) {
                    //        return '<a href="#" onclick="GetEditData(' + full.machine_id + ')" ><i class="fa fa-pencil-square-o"></i></a>';
                    //    }
                    //}
                ],
                "lengthMenu": [[5, 10, 50, -1], [5, 10, 50, "All"]],
                "fnRowCallback": function (nRow, aData, iDisplayIndex) {
                    $("td:nth-child(1)", nRow).html(iDisplayIndex + 1);
                    return nRow;
                },

            });

            $('#loader').hide();
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}

function GetNoDuesParticularsofEmployee(emp_id, sep_id, comp_id) {

    $('#loader').show();

    $("#modal_Popup").show();
    var modal = document.getElementById("modal_Popup");
    modal.style.display = "block";

    $('#modal_Popup').dialog({
        modal: 'true',
        title: 'No Dues Clearence Form'
    });

    var myData = {
        'fkid_CompanyId': comp_id,
        'fkid_EmpId': emp_id,
        'fkid_EmpSaperationId': sep_id
    };


    $.ajax({
        url: localStorage.getItem("ApiUrl") + "apiNoDues/GetNoDuesParticularsofEmployee",
        type: "POST",
        contentType: "application/json",
        dataType: "json",
        data: JSON.stringify(myData),
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (response) {
            var res = response;
            _GUID_New();

            AddParticularControls(res);


            $("#loader").hide();
        },
        error: function (err) {
            $("#loader").hide();
            messageBox("error", err.responseText);
        }
    });
}

var itemCount = 0;
var itemCountPerDept = { departmentName: [], itemcount: [] };

function AddParticularControls(res) {
    console.log(res);
    if (res == null || res.length < 1) {
        alert('No Particular Available....!');
        return false;
    }
    var dept = new Array();

    loop1: for (var i = 0; i < res.length; i++) {
        var d = res[i].dept_name;
        if (d != undefined || d != null) {
            for (var j = 0; j < dept.length; j++) {
                if (dept[j] == d) {
                    continue loop1;
                }
            }
            dept.push(d);
        }
    }

    var divPartic = document.getElementById("divParticulars");
    divPartic.innerHTML = "";
    /// Creating Different Department Particular Tab
    if (dept.length > 1) {
        var divTabBtnMain = document.createElement("div");
        divTabBtnMain.className = "col-md-12 mt-4";

        var divTabBtnInner = document.createElement("div");
        divTabBtnInner.className = "tab btn-group btn-block";

        for (var k = 0; k < dept.length; k++) {

            var _currDept = dept[k].toString().replace(" ", "_").toLowerCase();
            var _btn = document.createElement("button");
            if (k == 0) _btn.className = "tablinks  btn btn-primary text-light  border-info col-6 rounded-pill";
            else _btn.className = "tablinks  btn border-info col-6 rounded-pill";
            _btn.id = "btntab" + _currDept;
            _btn.innerText = dept[k];
            _btn.setAttribute("onclick", "openCity(event, 'tab" + _currDept + "')");
            if (!arr.includes(login_role_id)) {

                if (user_Dep_name.toString().replace(" ", "_").toLowerCase() != dept[k].toString().replace(" ", "_").toLowerCase()) {
                    _btn.disabled = true;
                }

            }
            divTabBtnInner.appendChild(_btn);
        }

    }

    divTabBtnMain.append(divTabBtnInner);
    divPartic.appendChild(divTabBtnMain);
    /// Creating Different Department Particular Tab
    if (dept.length > 1) {

        var divTabsOuter = document.createElement("div");
        divTabsOuter.className = "col-md-12 mt-4";

        for (var k = 0; k < dept.length; k++) {
            var _currDept = dept[k].toString().replace(" ", "_").toLowerCase();
            var divtabMain = document.createElement("div");
            divtabMain.className = "tabcontent";
            divtabMain.id = "tab" + _currDept;
            if (k == 0) divtabMain.style.display = "block";
            else divtabMain.style.display = "none";
            var divslidewrapper = document.createElement("div");
            divslidewrapper.className = "slider-wrap";
            var divclrscr = document.createElement("div");
            divclrscr.className = "clear";
            var divPart = document.createElement("div");
            var a = 0;
            divPart.innerHTML += '<h1><span id="lbl_mdlDepartment">' + dept[k].toString() + '</span> </h1> <hr />';
            for (var i = 0; i < res.length; i++) {

                if (res[i].dept_name.toString().replace(" ", "_").toLowerCase() == _currDept) {
                    if (itemCountPerDept.departmentName.includes(_currDept)) {
                        a += 1;
                    }
                    else {
                        a += 1;
                    }

                    divPart.innerHTML += '<div> <h2> <label id="lblPartName' + _currDept + a + '">' + res[i].particular_name + '</label> </h2><input type="hidden" id="hdn_pc_id' + _currDept + a + '" value="' + res[i].id + '"  /><input type="hidden" id="hdn_d_id' + _currDept + a + '" value="' + res[i].dept_id + '"  /><input type="hidden" id="hdn_es_id' + _currDept + a + '" value="' + res[i].es_id + '"  /> ';

                    if (res[i].is_Outstanding == 0) {

                        divPart.innerHTML += '<div class="col-md-6"> <div class="form-group" id="divOutstanding' + _currDept + a + '"> Outstanding : <input  type="radio" onchange="HideShow(divFRemarks' + _currDept + a + ',1)"  name="rd_out' + _currDept + a + '" value="1" class="radio" />&nbsp;&nbsp; Yes &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; <input  type="radio" onchange="HideShow(divFRemarks' + _currDept + a + ',0)" name="rd_out' + _currDept + a + '" value="0" checked class="radio" />&nbsp;&nbsp; No </div > </div > ';
                    }
                    else if (res[i].is_Outstanding == 1) {
                        divPart.innerHTML += '<div class="col-md-6"> <div class="form-group" id="divOutstanding' + _currDept + a + '"> Outstanding :<input  type="radio" onchange="HideShow(divFRemarks' + _currDept + a + ',1)" checked name="rd_out' + _currDept + a + '" value="1" class="radio" />&nbsp;&nbsp; Yes &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; <input  type="radio" onchange="HideShow(divFRemarks' + _currDept + a + ',0)" name="rd_out' + _currDept + a + '" value="0" class="radio" />&nbsp;&nbsp; No </div > </div > ';
                    }
                    else {
                        divPart.innerHTML += '<div class="col-md-6"> <div class="form-group" id="divOutstanding' + _currDept + a + '"> Outstanding :<input  type="radio" onchange="HideShow(divFRemarks' + _currDept + a + ',1)" name="rd_out' + _currDept + a + '" value="1" class="radio" />&nbsp;&nbsp; Yes &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;&nbsp; <input  type="radio" onchange="HideShow(divFRemarks' + _currDept + a + ',0)" name="rd_out' + _currDept + a + '" value="0" class="radio" />&nbsp;&nbsp; No </div > </div > ';
                    }

                    if (res[i].remarks != null && res[i].remarks != undefined && res[i].remarks != "") {
                        if (res[i].is_Outstanding == 0) {
                            divPart.innerHTML += '<div class="col-md-6"> <div class="form-group" style="visibility: hidden;" id="divFRemarks' + _currDept + a + '"> Remarks : <input id="txtfRemarks' + _currDept + a + '" type="text" placeholder="Enter Remarks" value="' + res[i].remarks + '" class="mdi-textbox" /> </div> </div> </div > ';
                        }
                        else {
                            divPart.innerHTML += '<div class="col-md-6"> <div class="form-group" id="divFRemarks' + _currDept + a + '"> Remarks : <input id="txtfRemarks' + _currDept + a + '" type="text" placeholder="Enter Remarks" value="' + res[i].remarks + '" class="mdi-textbox" /> </div> </div> </div > ';
                        }

                    }
                    else {
                        if (res[i].is_Outstanding == 0) {
                            divPart.innerHTML += '<div class="col-md-6"> <div class="form-group" style="visibility: hidden;" id="divFRemarks' + _currDept + a + '"> Remarks : <input id="txtfRemarks' + _currDept + a + '" type="text" placeholder="Enter Remarks" class="mdi-textbox" /> </div> </div> </div > ';
                        }
                        else {
                            divPart.innerHTML += '<div class="col-md-6"> <div class="form-group" id="divFRemarks' + _currDept + a + '"> Remarks : <input id="txtfRemarks' + _currDept + a + '" type="text" placeholder="Enter Remarks" class="mdi-textbox" /> </div> </div> </div > ';
                        }

                    }
                }


            }


            itemCountPerDept.departmentName.push(_currDept);
            itemCountPerDept.itemcount.push(a);

            if (itemCountPerDept.departmentName.includes(_currDept)) {
                debugger;
                if (user_Dep_name.toString().replace(" ", "_").toLowerCase() != _currDept) {
                    if (!arr.includes(login_role_id)) {
                        divPart.innerHTML += "<div class='col-md-12'> <div class='form-group text-center' > <button type='button' class='btn btn-primary btn-lg'  disabled='true' id='btnSave" + _currDept + "' onclick='SaveClearenceData(\"" + _currDept + "\")' >Save</button> </div> </div>";
                    }
                    else {

                        divPart.innerHTML += "<div class='col-md-12'> <div class='form-group text-center' > <button type='button' class='btn btn-primary btn-lg' id='btnSave" + _currDept + "' onclick='SaveClearenceData(\"" + _currDept + "\")' >Save</button> </div> </div>";
                    }
                } else {

                    divPart.innerHTML += "<div class='col-md-12'> <div class='form-group text-center' > <button type='button' class='btn btn-primary btn-lg' id='btnSave" + _currDept + "' onclick='SaveClearenceData(\"" + _currDept + "\")' >Save</button> </div> </div>";
                }
            }

            divslidewrapper.appendChild(divclrscr);
            divslidewrapper.appendChild(divPart);
            divtabMain.appendChild(divslidewrapper);
            divTabsOuter.appendChild(divtabMain);
        }
    }
    divPartic.appendChild(divTabsOuter);

    $('#loader').hide();
    return false;
}

function HideShow(id, visibility) {
    
    if (visibility == 1) {
        $(id).css('visibility', '');
    }
    else if (visibility == 0) {
        $(id).css('visibility', 'hidden');
    }
}

function SaveClearenceData(dep) {

    var errormsg = '';

    $('#loader').show();
    itemCount = 0;
    if (itemCountPerDept.departmentName.includes(dep)) {
        var g = itemCountPerDept.departmentName.indexOf(dep);
        if (g < 0) itemCount = 0;
        else itemCount = itemCountPerDept.itemcount[g];
    }
    var lst_part_ids = new Array();
    var lst_dept_ids = new Array();
    var lst_empSep_ids = new Array();
    var lst_outstandings = new Array();
    var lst_fremarks = new Array();

    if (itemCount > 0) {
        for (var i = 0; i < itemCount; i++) {

            var pid = $("#hdn_pc_id" + dep + (i + 1)).val();
            var pname = $("#lblPartName" + dep + (i + 1)).text();
            var outs = $('input[name="rd_out' + dep + (i + 1) + '"]:checked').val();
            var did = $("#hdn_d_id" + dep + (i + 1)).val();
            var esid = $("#hdn_es_id" + dep + (i + 1)).val();
            var fremk = $("#txtfRemarks" + dep + (i + 1)).val();

            if (pid == null || pid == '' || pid == undefined) {
                errormsg = errormsg + "Invalid Particular id.. <br/>";
                break;
            }
            if (outs == null || outs == '' || outs == undefined) {
                errormsg = errormsg + "Please select outStanding of" + pname + ".. <br/>";
                break;
            }
            if (did == null || did == '' || did == undefined) {
                errormsg = errormsg + "Invalid Department id.. <br/>";
                break;
            }
            if ((fremk == null || fremk == '' || fremk == undefined) && outs == "1") {
                errormsg = errormsg + "Please enter valid Remarks of " + pname + " <br/>";
                break;
            }
            lst_part_ids.push(pid);
            lst_dept_ids.push(did);
            lst_empSep_ids.push(esid);
            lst_outstandings.push(outs);
            lst_fremarks.push(fremk);
        }
    }
    else { errormsg = errormsg + "Please enter valid Particular value <br/>"; }

    if (errormsg != "") {
        messageBox("error", errormsg);
        $('#loader').hide();
        return;
    }


    var myData = {
        'lst_part_id': lst_part_ids,
        'lst_dept_id': lst_dept_ids,
        'lst_empSep_ids': lst_empSep_ids,
        'lst_final_Remarks': lst_fremarks,
        'lst_outstandings': lst_outstandings
    };

    if (lst_part_ids.length == 1 && lst_fremarks[0] == "") {
        messageBox("error", "Invalid Remarks...");
        $('#loader').hide();
        return;
    }

    var apiurl = localStorage.getItem("ApiUrl") + 'apiNoDues/Save_NoDueClearenceForm';
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
            var statuscode = data.statusCode;
            var Msg = data.message;

            _GUID_New();
            if (statuscode == "0") {
                $('#loader').hide();
                messageBox("success", Msg);
                setTimeout(function () {
                    location.reload(true);
                }, 2000);
                // location.reload();
            }
            else if (statuscode == "1" || statuscode == '2') {
                $('#loader').hide();
                messageBox("error", Msg);
                return false;
            }
        },
        error: function (err) {
            $('#loader').hide();
            _GUID_New();
            alert(err.responseText);
        }
    });

}














