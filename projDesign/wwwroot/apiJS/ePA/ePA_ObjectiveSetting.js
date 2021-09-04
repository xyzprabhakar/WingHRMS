$('#loader').show();
$(document).ready(function () {
    setTimeout(function () {
        

        var token = localStorage.getItem('Token');
        if (token == null) {
            window.location = '/Login';
        }
        // debugger;
        BindFinancialList('ddlfyi', 0);
        // Get Quarter On Financial Year Change
        $('#ddlfyi').bind("change", function () {
            BindQuarterList('ddlquarter', 0, $(this).val());
        });
        GetData();

        $('#btnacceptance').hide();
        $('#btnsave').hide();

        $("#txtfromdate").datepicker({
            dateFormat: 'mm/dd/yy',
            minDate: 0,
            onSelect: function (fromselected, evnt) {
                $("#txttodate").datepicker('setDate', null);
                $("#txttodate").datepicker({
                    dateFormat: 'mm/dd/yy',
                    minDate: fromselected,
                    onSelect: function (toselected, evnt) {

                        if (Date.parse(fromselected) > Date.parse(toselected)) {
                            messageBox('error', 'To Date must be greater than from date !!');
                            //$('#txtfromdate').val('');
                            $('#txttodate').val('');
                        }
                    }
                });
            }

        });

        $('#loader').hide();

    }, 2000);// end timeout

});


//Get Last Company Id
function GetData() {
    // debugger;
    $.ajax({
        type: "GET",
        url: localStorage.getItem("ApiUrl") + 'apiePA/Get_TabMasterData/0',
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        headers: { 'Authorization': 'Bearer ' + localStorage.getItem('Token') },
        success: function (data) {
            var res = data;
            console.log(data);
            $.each(res, function (data, value) {
                $("#ul_tab").append($("<li style='padding-right:1%;'><a rel='external' href='#'>" + value.tab_id + "-" + value.tab_name + "</li>"));

                // $("#tblePAObjective").append($("<thead><tr><th style='padding-right:1%;'>"+ value.tab_name + "</th></tr></thead>"));
            })

        },
        error: function (error) {
            messageBox("error", "Server busy please try again later...!");
        }
    });

}