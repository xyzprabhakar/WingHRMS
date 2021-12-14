$(document).ready(function () {
    fncGetRefreshData()
})

function fncGetRefreshData() {
    var refreshData = localStorage.getItem("refreshData");
    $('#loader').show();
    if (refreshData == 1) {
        fncSetApplication();
    }
    localStorage.setItem("refreshData", 0);
    $('#loader').hide();
}
function fncCreateAllDb(e) {
    var thisDB = e.target.result;
    if (!thisDB.objectStoreNames.contains("tblApplicationMaster")) {
        thisDB.createObjectStore("tblApplicationMaster", { keyPath: "id" });
    }
}
function fncGetApplication() {    
    
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
                    fncCreateAllDb(e);
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

