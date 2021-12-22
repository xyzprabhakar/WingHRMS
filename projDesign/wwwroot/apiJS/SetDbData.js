
$(document).ready(function () {
    fncGetRefreshData()
})

function fncGetRefreshData() {
    var refreshData = localStorage.getItem("refreshData");    
    if (refreshData == 1) {
        fncSetApplication();        
    }
    
    localStorage.setItem("refreshData", 0);    
}
function fncCreateAllDb(e) {
    var thisDB = e.target.result;
    if (!thisDB.objectStoreNames.contains("tblApplicationMaster")) {
        thisDB.createObjectStore("tblApplicationMaster", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblModuleMaster")) {
        thisDB.createObjectStore("tblModuleMaster", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblSubModuleMaster")) {
        thisDB.createObjectStore("tblSubModuleMaster", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblDocumentMaster")) {
        thisDB.createObjectStore("tblDocumentMaster", { keyPath: "id" });
    }
}

function fncSetApplication() {
    var baseUrl = localStorage.getItem("baseUrl");
    var dBVersion= localStorage.getItem("dBVersion");
    var apiurl = baseUrl + ("User/GetUserDocuments/true/true/true/true");
    var headerss = {};
    headerss["Authorization"] = 'Bearer ' + localStorage.getItem('token');
    $.ajax({
        url: apiurl,
        type: "POST",
        dataType: 'json',
        headers: headerss,
        contentType: 'application/json; charset=utf-8',
        success: function (data) {
            if (data.messageType == 1) {
                var openRequest = indexedDB.open("dpbs", dBVersion);
                openRequest.onupgradeneeded = function (e) {                    
                    fncCreateAllDb(e);
                }
                openRequest.onsuccess = function (e) {
                    var db = e.target.result;
                    let ObjectStoreApplication = db.transaction("tblApplicationMaster", "readwrite").objectStore("tblApplicationMaster");
                    let ApplicationReqSuccess = ObjectStoreApplication.clear();
                    ApplicationReqSuccess.onsuccess = function (event) {
                        for (var i in data.returnId.application) {
                            ObjectStoreApplication.add(data.returnId.application[i]);
                        }                        
                    };
                    let ObjectStoreModule = db.transaction("tblModuleMaster", "readwrite").objectStore("tblModuleMaster");
                    let ModuleReqSuccess = ObjectStoreModule.clear();
                    ModuleReqSuccess.onsuccess = function (event) {
                        for (var i in data.returnId.module) {
                            ObjectStoreModule.add(data.returnId.module[i]);
                        }
                    };
                    let ObjectStoreSubModule = db.transaction("tblSubModuleMaster", "readwrite").objectStore("tblSubModuleMaster");
                    let SubModuleReqSuccess = ObjectStoreSubModule .clear();
                    SubModuleReqSuccess .onsuccess = function (event) {
                        for (var i in data.returnId.module) {
                            ObjectStoreSubModule .add(data.returnId.module[i]);
                        }
                    };
                    let ObjectStoreDocument = db.transaction("tblDocumentMaster", "readwrite").objectStore("tblDocumentMaster");
                    let DocumentReqSuccess = ObjectStoreDocument.clear();
                    DocumentReqSuccess.onsuccess = function (event) {
                        for (var i in data.returnId.module) {
                            ObjectStoreDocument.add(data.returnId.module[i]);
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

