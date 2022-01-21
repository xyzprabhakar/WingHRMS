


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
    if (!thisDB.objectStoreNames.contains("tblOrganisation")) {
        thisDB.createObjectStore("tblOrganisation", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblCompany")) {
        thisDB.createObjectStore("tblCompany", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblZone")) {
        thisDB.createObjectStore("tblZone", { keyPath: "id" });
    }
    if (!thisDB.objectStoreNames.contains("tblLocation")) {
        thisDB.createObjectStore("tblLocation", { keyPath: "id" });
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
    let tblState_store = tx.objectStore("tblState");
    if (!tblState_store.indexNames.contains("countryId")) {
        tblState_store.createIndex("countryId", "countryId", { unique: false });
    }

    let tblCompany_store = tx.objectStore("tblCompany");
    if (!tblCompany_store.indexNames.contains("parentId")) {
        tblCompany_store.createIndex("parentId", "parentId", { unique: false });
    }
    let tblZone_store = tx.objectStore("tblZone");
    if (!tblZone_store .indexNames.contains("parentId")) {
        tblZone_store .createIndex("parentId", "parentId", { unique: false });
    }
    let tblLocation_store = tx.objectStore("tblLocation");
    if (!tblLocation_store.indexNames.contains("parentId")) {
        tblLocation_store.createIndex("parentId", "parentId", { unique: false });
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
                    location.reload();
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


function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');        
        vars.push(hash[0].toLowerCase());
        vars[hash[0].toLowerCase()] = hash[1];        
    }
    return vars;
}

function GetDateFormatddMMyyyy(date) {
    var month = (date.getMonth() + 1).toString();    
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    return day + '-' + months[month - 1] + '-' + date.getFullYear();
}

function GetDateFormatddMMyyyyHHtt(date) {
    var month = (date.getMonth() + 1).toString();    
    var day = date.getDate().toString();
    day = day.length > 1 ? day : '0' + day;
    const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

    let hours = date.getHours();
    let minutes = date.getMinutes();
    let ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes.toString().padStart(2, '0');    
    return day + '-' + months[month - 1] + '-' + date.getFullYear() + " " + hours + ':' + minutes + ' ' + ampm;
}

function BindEnums(ControlInputName, result,selectedValue) {
    $('#' + ControlInputName).empty();
    let isselected = false;
    $('#' + ControlInputName).append(`<option value="" >--Please select --</option>`);
    for (var i in result) {
        if (result[i].id == selectedValue) {
            isselected = true;
            $('#' + ControlInputName).append(`<option value="${result[i].id}"  selected>${result[i].displayText}</option>`);            
        }
        else {
            $('#' + ControlInputName).append(`<option value="${result[i].id}" >${result[i].displayText}</option>`);            
        }
    }
    if (result.length == 1) {
        isselected = true;
        $('#' + ControlInputName).val(result[0].id);
    }
    if (isselected) {
        $('#' + ControlInputName).trigger('change');
    }
}

function SortByName(a, b) {
    var aName = a.name.toLowerCase();
    var bName = b.name.toLowerCase();
    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
}

