function BindCountryState(CountryInputName, countryId, stateInputName, stateId) {
    if (CountryInputName == "" || CountryInputName === undefined || CountryInputName == null) {
        return;
    }
    let dBVersion = localStorage.getItem('dBVersion');
    var openRequest = indexedDB.open("dpbs", dBVersion);
    openRequest.onsuccess = function (e) {
        var db = e.target.result;
        let ObjectStoreCountry = db.transaction("tblCountry", "readwrite")
            .objectStore("tblCountry");

        $('#' + CountryInputName).empty();
        $('#' + CountryInputName).append(`<option value=""> -- Please Select --</option>`);
        ObjectStoreCountry.openCursor().onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                if (cursor.value.countryId == countryId) {
                    if (cursor.value.isActive) {
                        $('#' + CountryInputName).append(`<option value="${cursor.value.countryId}" selected> ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                    else {
                        $('#' + CountryInputName).append(`<option value="${cursor.value.countryId}" disabled selected> ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                }
                else {
                    if (cursor.value.isActive) {
                        $('#' + CountryInputName).append(`<option value="${cursor.value.countryId}" > ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                    else {
                        $('#' + CountryInputName).append(`<option value="${cursor.value.countryId}" disabled> ${cursor.value.code} - ${cursor.value.name}</option>`);
                    }
                }
                cursor.continue();
            }
            db.close();
        };

        if (!(stateInputName == "" || stateInputName === undefined || stateInputName == null)) {
            $('#' + CountryInputName).on('change', function () {
                BindState(CountryInputName, $('#' + CountryInputName).val(), stateInputName, stateId)
            });
        }
    }

}
function BindState(CountryInputName, countryId, stateInputName, stateId) {

    if (!(stateInputName == "" || stateInputName === undefined || stateInputName == null)) {
        if (!(countryId > 0)) {
            countryId = $('#' + CountryInputName).val();
        }
        let dBVersion = localStorage.getItem('dBVersion');
        var openRequest = indexedDB.open("dpbs", dBVersion);
        $('#' + stateInputName).empty();
        $('#' + stateInputName).append(`<option value=""> -- Please Select --</option>`);
        openRequest.onsuccess = function (e) {
            var db = e.target.result;
            let ObjectStoreState = db.transaction("tblState", "readwrite")
                .objectStore("tblState");
            var keyRangeValue = IDBKeyRange.only(parseInt(countryId));            
            var getAllRequest = ObjectStoreState.index("countryId").getAll(parseInt(countryId));
            getAllRequest.onsuccess = function () {
                for (var i in getAllRequest.result) {

                    if (getAllRequest.result[i].stateId == stateId) {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + stateInputName).append(`<option value="${getAllRequest.result[i].stateId}" selected> ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + stateInputName).append(`<option value="${getAllRequest.result[i].stateId}" disabled selected> ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                    else {
                        if (getAllRequest.result[i].isActive) {
                            $('#' + stateInputName).append(`<option value="${getAllRequest.result[i].stateId}" > ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                        else {
                            $('#' + stateInputName).append(`<option value="${getAllRequest.result[i].stateId}" disabled> ${getAllRequest.result[i].code} - ${getAllRequest.result[i].name}</option>`);
                        }
                    }
                }
                db.close();
            }
        }


    }
}