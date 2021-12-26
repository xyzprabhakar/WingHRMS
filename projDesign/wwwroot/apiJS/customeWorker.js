//Load all data in index DB
var w;
if (localStorage.getItem("refreshData") == 1) {
    startWorker();
    localStorage.setItem("refreshData", 0);
}

function startstopWorker() {    
    if (document.getElementById("lblDataStatus").innerHTML.trim() == "Reload") {
        stopWorker();
        document.getElementById("lblDataStatus").innerHTML = "Reloading..";
        startWorker();
    }
    else {
        stopWorker();
        document.getElementById("lblDataStatus").innerHTML = "Reload";
    }
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
    if (!thisDB.objectStoreNames.contains("tblMenuMaster")) {
        thisDB.createObjectStore("tblMenuMaster", { keyPath: "applicationId" });
    }

    if (!thisDB.objectStoreNames.contains("tblCountry")) {
        thisDB.createObjectStore("tblCountry", { keyPath: "countryId" });
    }
    if (!thisDB.objectStoreNames.contains("tblState")) {
        thisDB.createObjectStore("tblState", { keyPath: "stateId" });
    }

    //create index coresponding to tables
    var tx = e.target.transaction;
    let tblModuleMaster_store = tx.objectStore("tblModuleMaster");
    if (!tblModuleMaster_store.indexNames.contains("enmApplication")) {
        tblModuleMaster_store.createIndex("enmApplication", "enmApplication", { unique: false });
    }
    let tblSubModuleMaster_store = tx.objectStore("tblSubModuleMaster");
    if (!tblSubModuleMaster_store.indexNames.contains("enmModule")) {
        tblSubModuleMaster_store.createIndex("enmModule", "enmModule", { unique: false });
    }
    let tblDocumentMaster_store = tx.objectStore("tblDocumentMaster");
    if (!tblDocumentMaster_store.indexNames.contains("enmModule")) {
        tblDocumentMaster_store.createIndex("enmModule", "enmModule", { unique: false });
    }
    if (!tblDocumentMaster_store.indexNames.contains("enmApplication")) {
        tblDocumentMaster_store.createIndex("enmApplication", "enmApplication", { unique: false });
    }
    let tblSubModuleMaster_store = tx.objectStore("tblState");
    if (!tblSubModuleMaster_store.indexNames.contains("countryId")) {
        tblSubModuleMaster_store.createIndex("countryId ", "countryId ", { unique: false });
    }
}

function startWorker() {
    var baseUrl = localStorage.getItem("baseUrl");
    var dBVersion = localStorage.getItem("dBVersion");
    var token = localStorage.getItem('token');
    
    var workerData = { baseUrl_: baseUrl, dBVersion_: dBVersion, token_: token, startDownload_:1 };
    if (typeof (Worker) !== "undefined") {
        if (typeof (w) == "undefined") {
            w = new Worker("/apiJS/SetDbData.js");
            w.postMessage(workerData);
            }
            w.onmessage = function (event) {
                if (event.data == "Done") {
                    document.getElementById("lblDataStatus").innerHTML = "Reload";
                    //stopWorker();
                }
                else {
                    document.getElementById("lblDataStatus").innerHTML = event.data;
                }
                
            };
        
    } else {
        document.getElementById("lblDataStatus").innerHTML = "Sorry, your browser does not support Web Workers...";
    }
}

function stopWorker() {
    if (w != null) {
        if (w !== "undefined") {
            w.terminate();
            w = undefined;
        }
    }
}
//document.onreadystatechange = function () {
//    startWorker();
//    if (document.readyState !== "complete") {
//        document.querySelector(
//            "body").style.visibility = "hidden";
//        document.querySelector(
//            "#loader").style.visibility = "visible";
//    } else {
//        document.querySelector(
//            "#loader").style.display = "none";
//        document.querySelector(
//            "body").style.visibility = "visible";
//    }
//};

///BindCountryState
function BindCountryState(CountryInputName, countryId, stateInputName, stateId) {
    if (CountryInputName == "" || CountryInputName === undefined || CountryInputName == null) {
        return;
    }
    let dBVersion = localStorage.getItem('dBVersion');
    var openRequest = indexedDB.open("dpbs", dBVersion);
    openRequest.onsuccess = function (e) {
        var db = e.target.result;
        let ObjectStoreCountry = db.transaction("tblCountry", "read")
            .objectStore("tblCountry");
        
        $('#'+CountryInputName).empty();
        ObjectStoreCountry.openCursor().onsuccess = function (event) {
            var cursor = event.target.result;
            if (cursor) {
                if (cursor.value.countryId == countryId) {
                    if (cursor.value.isActive) {
                        $('#' +CountryInputName).append(`<option value"${cursor.value.countryId}" selected> ${cursor.value.code}-${cursor.value.name}</option>`);
                    }
                    else {
                        $('#' +CountryInputName).append(`<option value"${cursor.value.countryId}" disabled selected> ${cursor.value.code}-${cursor.value.name}</option>`);
                    }
                }
                else {
                    if (cursor.value.isActive) {
                        $('#' +CountryInputName).append(`<option value"${cursor.value.countryId}" > ${cursor.value.code}-${cursor.value.name}</option>`);
                    }
                    else {
                        $('#' +CountryInputName).append(`<option value"${cursor.value.countryId}" disabled> ${cursor.value.code}-${cursor.value.name}</option>`);
                    }
                }
                cursor.continue();
            }
        };
        if (!(stateInputName == "" || stateInputName === undefined || stateInputName == null)) {
            
            $('#'+CountryInputName).on('change', function () {
                BindState(CountryInputName, countryId, stateInputName, stateId)
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
        openRequest.onsuccess = function (e) {
            var db = e.target.result;
            let ObjectStoreState = db.transaction("tblState", "read")
                .objectStore("tblState");
            ObjectStoreState.index(parseInt(countryId)).openCursor().onsuccess == function (eventInner) {
                var cursorInner = eventInner.target.result;
                if (cursorInner) {
                    if (cursorInner.value.stateId == stateId) {
                        if (cursor.value.isActive) {
                            $('#'+stateInputName).append(`<option value"${cursorInner.value.stateId}" selected> ${cursorInner.value.code}-${cursorInner.value.name}</option>`);
                        }
                        else {
                            $('#' +stateInputName).append(`<option value"${cursorInner.value.stateId}" disabled selected> ${cursorInner.value.code}-${cursorInner.value.name}</option>`);
                        }
                    }
                    else {
                        if (cursor.value.isActive) {
                            $('#' +stateInputName).append(`<option value"${cursorInner.value.stateId}" > ${cursorInner.value.code}-${cursorInner.value.name}</option>`);
                        }
                        else {
                            $('#' +stateInputName).append(`<option value"${cursorInner.value.stateId}" disabled> ${cursorInner.value.code}-${cursorInner.value.name}</option>`);
                        }
                    }
                    cursorInner.continue();
                }
            }
        }
        

    }
}