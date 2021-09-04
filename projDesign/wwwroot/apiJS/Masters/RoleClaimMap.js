$('#loader').show();
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');

        if (token == null) {
            window.location = '/Login';
        }

        GetData(0);
        $('#btnupdate').hide();
        $('#btnsave').show();

        BindRoleMaster('ddlrole', 0);

        $('#loader').hide();



        $('#btnreset').bind('click', function () {
            //$("#ddlrole").val('');
            location.reload();
        });

        $("#ddlrole").bind('change', function () {
            var roleid = $("#ddlrole").val();
            GetData(roleid);
        });

        $('#btnsave').bind("click", function () {
            $('#loader').show();
            var ClaimIds = [];
            var table = $("#tblclaimlist").dataTable();
            $("input:checkbox", table.fnGetNodes()).each(function () {
                if ($(this).is(":checked")) {
                    var no = $(this).val();
                    ClaimIds.push(no);
                }
            });
            //selectedIds.forEach(function (selectedId) {
            //    alert(selectedId);
            //});

            var roleid = $("#ddlrole").val();
            var errormsg = '';
            var iserror = false;

            if (roleid == '' || roleid == '0') {
                errormsg = errormsg + 'Please select role !! <br />';
                iserror = true;
            }
            if (ClaimIds.length <= 0) {
                errormsg = errormsg + 'Please select claim name !! <br />';
                iserror = true;
            }

            if (iserror) {
                messageBox("error", errormsg);
                $('#loader').hide();
                return false;
            }


            var myData = {

                'claimids': ClaimIds,
                'roleid': roleid,
            };

            $('#loader').show();
            var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/SaveRoleRoleAndClaimMap';
            var obj = JSON.stringify(myData);


            var headerss = {};
            headerss["Authorization"] = 'Bearer ' + localStorage.getItem('Token');
            headerss["salt"] = $("#hdnsalt").val();

            $.ajax({
                url: apiurl,
                type: "POST",
                data: obj,
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
                        var roleid = $("#ddlrole").val();
                        GetData(roleid);
                        // $("#ddlrole").val('');

                        messageBox("success", Msg);
                    }
                    else {
                        messageBox("error", Msg);
                    }
                    $('#loader').hide();
                },
                error: function (err) {
                    _GUID_New();
                    messageBox("error", err.responseText);
                    $('#loader').hide();
                }
            });
        });


    }, 2000);// end timeout

});


//--------bind data in jquery data table
function GetData(roleid) {
    $('#loader').show();
    var apiurl = localStorage.getItem("ApiUrl") + 'apiMasters/GetClaimByRoleId/' + roleid;
    $.ajax({
        type: "GET",
        url: apiurl,
        data: {},
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (res) {
            $('#loader').hide();
            $("#tblclaimlist").DataTable({
                "processing": true, // for show progress bar
                "serverSide": false, // for process server side
                "bDestroy": true,
                "filter": false, // this is for disable filter (search box)
                "orderMulti": false, // for disable multiple column at once
                "scrollY": 250,
                "aaData": res,
                "columnDefs":
                    [
                        {
                            targets: 0,
                            "sTitle": "<input type='checkbox' onchange='selectAll(this)' id='selectAll'></input>"
                        }
                    ],

                "columns": [
                    //{
                    //    "render": function (data, type, row, meta) {
                    //        return meta.row + meta.settings._iDisplayStart + 1;
                    //    }
                    //},

                    {
                        "render": function (data, type, full, meta) {
                            if (full.ischecked == true) {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" checked id="chk' + full.claimid + '" value="' + full.claimid + '" />';
                            }
                            else {
                                return '<input type="checkbox" onchange="selectRows(this);" class="chkRow" id="chk' + full.claimid + '" value="' + full.claimid + '" />';
                            }
                        }
                    },
                    { "data": "claimname", "name": "claimname", "autoWidth": true },

                ],
                "paging": false,
                "ordering": false,
                "info": false,
                "select": 'multi'

            });
        },
        error: function (error) {
            $('#loader').hide();
            alert(error.responseText);
        }
    });

}

function selectAll() {

    var chkAll = $('#selectAll');

    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblclaimlist").find(".chkRow");
    chkRows.each(function () {
        if (chkAll.is(':checked')) {
            $(this).prop('checked', true);
        }
        else {
            $(this).prop('checked', false);
        }
    });
}

function selectRows() {

    var chkAll = $("#selectAll");
    chkAll.prop('checked', true);
    //Fetch all row CheckBoxes in the Table.
    var chkRows = $("#tblclaimlist").find(".chkRow");

    chkRows.each(function () {
        if (!$(this).is(":checked")) {
            chkAll.prop("checked", false);
            return;
        }
    });
}