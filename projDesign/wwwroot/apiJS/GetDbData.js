function fncGetRefreshData() {
    var refreshData = localStorage.getItem("refreshData");
    $('#loader').show();
    if (refreshData == 1) {
        fncGetApplicationApi();        
    }
    else {
       // fncGetApplicationDb();        
    }
    $('#loader').hide();
}


function fncCreateAllDb(e) {

}

function fncGetApplication() {    
    var openRequest = indexedDB.open("dpbs", 1 );
    openRequest.onupgradeneeded = function (e) {
        var thisDB = e.target.result;
        if (!thisDB.objectStoreNames.contains("tblApplicationMaster")) {
            thisDB.createObjectStore("tblApplicationMaster", { keyPath: "id" });
        }
        console.log("hi1");
    }
    openRequest.onsuccess = function (e) {
        var db = e.target.result;
        let ObjectStore = db.transaction("tblApplicationMaster", "readwrite")
            .objectStore("tblApplicationMaster");
        return ObjectStore.getAll();
        //obstore.openCursor().onsuccess = function (event) {
        //    var cursor = event.target.result;
        //    if (cursor) {
        //        var option = document.createElement("option");
        //        option.text = cursor.value.countryCode + " - " + cursor.value.countryName;
        //        option.value = cursor.value.countryId;
        //        countrydropdown.add(option);
        //        cursor.continue();
        //    }
        //};
    }
    openRequest.onerror = function (e) {
        console.log(e);
    }
    console.log("hi");
}

function fncSetApplication() {
    var baseUrl = localStorage.getItem("baseUrl");
    var apiurl = baseUrl + ("User/GetUserApplication");
    $.ajax({
        url: apiurl,
        type: "POST",        
        dataType: 'json',
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.messageType == 1) {
                var openRequest = indexedDB.open("dpbs", 1);
                openRequest.onupgradeneeded = function (e) {                    
                    var thisDB = e.target.result;
                    if (!thisDB.objectStoreNames.contains("tblApplicationMaster")) {
                        thisDB.createObjectStore("tblApplicationMaster", { keyPath: "id" });
                    }
                }
                openRequest.onsuccess = function (e) {
                    var db = e.target.result;
                    let ObjectStore = db.transaction("tblApplicationMaster", "readwrite")
                        .objectStore("tblApplicationMaster");
                    let ReqSuccess = ObjectStore.clear();
                    ReqSuccess.onsuccess = function (event) {
                        for (var i in data.returnId) {
                            ObjectStore.add(data.returnId[i]);
                        }                        
                    };
                }
                openRequest.onerror = function (e) {                    
                    console.log(e);
                }                
            }
            else {
                alert(data.message);
            }
        },
        error: function (err) {
            console.log(err)
        }
    });
}