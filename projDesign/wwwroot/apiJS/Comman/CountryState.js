async function BindCountryState(CountryInputName, countryId, stateInputName, stateId) {
    if (CountryInputName == "" || CountryInputName === undefined || CountryInputName == null) {
        return;
    }
    GetDataAll_IndexDb("tblCountry").then((result) => {        
        $('#' + CountryInputName).empty();
        $('#' + CountryInputName).append(`<option value=""> -- Please Select --</option>`);
        for (let i in result) {
            if (result.countryId == countryId) {
                if (result[i].isActive) {
                    $('#' + CountryInputName).append(`<option value="${result[i].countryId}" selected> ${result[i].code} - ${result[i].name}</option>`);
                }
                else {
                    $('#' + CountryInputName).append(`<option value="${result[i].countryId}" disabled selected> ${result[i].code} - ${result[i].name}</option>`);
                }
            }
            else {
                if (result[i].isActive) {
                    $('#' + CountryInputName).append(`<option value="${result[i].countryId}" > ${result[i].code} - ${result[i].name}</option>`);
                }
                else {
                    $('#' + CountryInputName).append(`<option value="${result[i].countryId}" disabled> ${result[i].code} - ${result[i].name}</option>`);
                }
            }
        }

        if (!(stateInputName == "" || stateInputName === undefined || stateInputName == null)) {
            $('#' + CountryInputName).on('change paste', function (event,_stateId) {
                BindState($('#' + CountryInputName).val(), stateInputName, _stateId)
            });
        }
    });
    

}
function BindState( countryId, stateInputName, stateId) {
    
    if (!(stateInputName == "" || stateInputName === undefined || stateInputName == null)) {
        if (countryId == 0 || countryId == "" || countryId == null || countryId == undefined) {
            return;
        }
        console.log(stateId);
        let _countryId = parseInt(countryId);
        var keyRangeValue = IDBKeyRange.only(_countryId);
        GetDataFromIndex_IndexDb(keyRangeValue, "tblState", "countryId").then((result) => {            
            $('#' + stateInputName).empty();
            $('#' + stateInputName).append(`<option value=""> -- Please Select --</option>`);
            for (var i in result) {
                if (result[i].stateId == stateId) {
                    if (result[i].isActive) {
                        $('#' + stateInputName).append(`<option value="${result[i].stateId}" selected> ${result[i].code} - ${result[i].name}</option>`);
                    }
                    else {
                        $('#' + stateInputName).append(`<option value="${result[i].stateId}" disabled selected> ${result[i].code} - ${result[i].name}</option>`);
                    }
                }
                else {
                    if (result[i].isActive) {
                        $('#' + stateInputName).append(`<option value="${result[i].stateId}" > ${result[i].code} - ${result[i].name}</option>`);
                    }
                    else {
                        $('#' + stateInputName).append(`<option value="${result[i].stateId}" disabled> ${result[i].code} - ${result[i].name}</option>`);
                    }
                }
            }
        });
        
    }
}