const months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

function HR_EmployeeData(companyId, stateId, locationId, deptId, eleEmployee = null, haveSelectAll = true, havePleaseSelect = true) {

    if (eleEmployee == null) {
        eleEmployee = document.getElementById("ddlemployee");
    }
    eleEmployee.length = 0

    var counter = 0, innercounter = 0;;
    if (havePleaseSelect) {
        var selectOption = document.createElement("option");
        selectOption.text = "Please Select";
        selectOption.value = 0;
        eleEmployee.add(selectOption);
        counter++;
    }
    if (haveSelectAll) {
        var selectOption = document.createElement("option");
        selectOption.text = "All";
        selectOption.value = -1;
        eleEmployee.add(selectOption);
        counter++;
    }

    var dataSrc = [];
    dataSrc = JSON.parse(localStorage.getItem("emp_under_login_emp"));
    $.each(dataSrc, function (key, value) {
        if (companyId == -1 || value.company_id == companyId) {
            if (stateId == -1 || value.state_id == stateId) {
                if (locationId == -1 || value.location_id == locationId) {
                    if (deptId == -1 || value.dept_id == deptId) {
                        var selectOption = document.createElement("option");
                        selectOption.text = value.emp_name_code;
                        selectOption.value = value._empid;
                        eleEmployee.add(selectOption);
                        counter++;
                    }
                }
            }
        }
    });

    eleEmployee.selectedIndex = 1;

}


(function ($) {

    var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });

    $.fn.HR_HeaderData = function (options) {

        // This is the easiest way to have default options.
        var settings = $.extend({
            // These are the defaults.
            haveFromDate: true,
            haveToDate: true,
            haveCompanyData: true,
            haveStateData: true,
            haveLocationData: true,
            haveDepartmentData: true,
            haveEmpData: true,
            haveSelectAll: true,
            havePleaseSelect: true,
        }, options);


        var dataSrc = [];

        dataSrc = JSON.parse(localStorage.getItem("emp_under_login_emp"));

        if (dataSrc == null) {
            alert("Data Source Could Not Be Null");
            return;
        }

        var allElement = document.createElement("div");
        var eleCompany = document.createElement("select");
        var eleState = document.createElement("select");
        var eleLocation = document.createElement("select");
        var eleDept = document.createElement("select");
        var eleEmployee = document.createElement("select");
        var eleFromDt = document.createElement("input");
        var eleToDt = document.createElement("input");
        var currentDt = new Date();
        var previousDt = new Date();
        currentDt = new Date(currentDt.getFullYear(), currentDt.getMonth(), 25);
        previousDt.setDate(previousDt.getDate() - 30);
        previousDt = new Date(previousDt.getFullYear(), previousDt.getMonth(), 26);



        {
            var year = previousDt.getFullYear().toString();
            var month = (previousDt.getMonth() + 1).toString();
            var days = previousDt.getDate().toString();
            if (month.length == 1) {
                month = "0" + month;
            }
            if (days.length == 1) {
                days = "0" + days;
            }


            var divFromDt = document.createElement("div");
            var divFromDtinner1 = document.createElement("div")
            var lblFromDt = document.createElement("label")
            var divFromDtinner2 = document.createElement("div")

            eleFromDt.id = "dtpFromDt";
            eleFromDt.type = "date";
            divFromDt.classList.add("col-md-6");
            divFromDt.classList.add("form-group");
            divFromDtinner1.classList.add("col-md-4");
            lblFromDt.classList.add("col-form-label");
            divFromDtinner2.classList.add("col-md-6");
            eleFromDt.classList.add("form-control");
            eleFromDt.classList.add("ip-ap");
            eleFromDt.flatpickr({
                dateFormat: "d-M-Y",
            });

            eleFromDt.value = days + "-" + months[month-1] + "-" + year;
            lblFromDt.innerText = "From Dt";
            divFromDtinner2.appendChild(eleFromDt);
            divFromDtinner1.appendChild(lblFromDt);
            divFromDt.appendChild(divFromDtinner1);
            divFromDt.appendChild(divFromDtinner2);
            if (!options.haveFromDate) {
                divFromDt.style.display = "none";
            }
            allElement.appendChild(divFromDt);

        }
        {
            var year = currentDt.getFullYear().toString();
            var month = (currentDt.getMonth() + 1).toString();
            var days = currentDt.getDate().toString();
            if (month.length == 1) {
                month = "0" + month;
            }
            if (days.length == 1) {
                days = "0" + days;
            }

            var divToDt = document.createElement("div");
            var divToDtinner1 = document.createElement("div")
            var lblToDt = document.createElement("label")
            var divToDtinner2 = document.createElement("div")
            eleToDt.id = "dtpToDt";
            eleToDt.type = "date";

            divToDt.classList.add("col-md-6");
            divToDt.classList.add("form-group");
            divToDtinner1.classList.add("col-md-4");
            lblToDt.classList.add("col-form-label");
            divToDtinner2.classList.add("col-md-6");
            eleToDt.classList.add("form-control");
            eleToDt.classList.add("ip-ap");
            eleToDt.flatpickr({
                dateFormat: "d-M-Y",
            });

            eleToDt.value = days + "-" + months[month - 1] + "-" + year;
            lblToDt.innerText = "To Dt";
            divToDtinner2.appendChild(eleToDt);
            divToDtinner1.appendChild(lblToDt);
            divToDt.appendChild(divToDtinner1);
            divToDt.appendChild(divToDtinner2);
            if (!options.haveFromDate) {
                divToDt.style.display = "none";
            }
            allElement.appendChild(divToDt);

        }

        {

            var divCompany = document.createElement("div");
            var divCompanyinner1 = document.createElement("div")
            var lblCompany = document.createElement("label")
            var divCompanyinner2 = document.createElement("div")


            divCompany.classList.add("col-md-6");
            divCompany.classList.add("form-group");
            divCompanyinner1.classList.add("col-md-4");
            lblCompany.classList.add("col-form-label");
            divCompanyinner2.classList.add("col-md-6");
            eleCompany.classList.add("form-control");
            eleCompany.classList.add("bg-ligth");

            lblCompany.innerText = "Company";




            var companyDataSrc = Array.from(new Set(dataSrc.map(s => s.company_id))).
                map(company_id => {
                    return {
                        company_id: company_id,
                        company_name: dataSrc.find(s => s.company_id == company_id).company_name
                    };
                });

            eleCompany.id = "ddlcompany";
            if (options.havePleaseSelect) {
                var selectOption = document.createElement("option");
                selectOption.text = "Please Select";
                selectOption.value = 0;
                eleCompany.add(selectOption);
            }
            if (options.haveSelectAll) {
                var selectOption = document.createElement("option");
                selectOption.text = "All";
                selectOption.value = -1;
                eleCompany.add(selectOption);
            }

            $.each(companyDataSrc, function (key, value) {
                var selectOption = document.createElement("option");
                selectOption.text = value.company_name;
                selectOption.value = value.company_id;
                eleCompany.add(selectOption);

            });

            if (companyDataSrc.length == 1) {
                eleCompany.value = companyDataSrc[0].company_id
            }
            else {

                //var default_Company = CryptoJS.AES.decrypt(localStorage.getItem("company_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
                eleCompany.value = parseInt(default_Company);

            }
            eleCompany.addEventListener("change", function () {
                HR_EmployeeData(document.getElementById("ddlcompany").value, document.getElementById("ddlstate").value,
                    document.getElementById("ddllocation").value, document.getElementById("ddldepartment").value);
            });

            if ((!options.haveCompanyData) || (companyDataSrc.length == 1)) {
                divCompany.style.display = "none";
            }

            divCompanyinner2.appendChild(eleCompany);
            divCompanyinner1.appendChild(lblCompany);
            divCompany.appendChild(divCompanyinner1);
            divCompany.appendChild(divCompanyinner2);
            allElement.appendChild(divCompany);
        }


        {

            var divState = document.createElement("div");
            var divStateinner1 = document.createElement("div")
            var lblState = document.createElement("label")
            var divStateinner2 = document.createElement("div")


            divState.classList.add("col-md-6");
            divState.classList.add("form-group");
            divStateinner1.classList.add("col-md-4");
            lblState.classList.add("col-form-label");
            divStateinner2.classList.add("col-md-6");
            eleState.classList.add("form-control");
            eleState.classList.add("bg-ligth");

            lblState.innerText = "State";




            var companyDataSrc = Array.from(new Set(dataSrc.map(s => s.state_id))).
                map(state_id => {
                    return {
                        state_id: state_id,
                        state_name: dataSrc.find(s => s.state_id == state_id).state_name
                    };
                });

            eleState.id = "ddlstate";
            if (options.havePleaseSelect) {
                var selectOption = document.createElement("option");
                selectOption.text = "Please Select";
                selectOption.value = 0;
                eleState.add(selectOption);
            }
            if (options.haveSelectAll) {
                var selectOption = document.createElement("option");
                selectOption.text = "All";
                selectOption.value = -1;
                selectOption.selected = true;
                eleState.add(selectOption);
            }

            $.each(companyDataSrc, function (key, value) {
                var selectOption = document.createElement("option");
                selectOption.text = value.state_name;
                selectOption.value = value.state_id;
                eleState.add(selectOption);

            });

            if (companyDataSrc.length > 0) {
                if (companyDataSrc.length == 1) {
                    eleState.value = companyDataSrc[0].state_id
                }
                //else {
                //    eleState.value = companyDataSrc[companyDataSrc.length - 1].state_id
                //}
            }

            eleCompany.addEventListener("change", function () {
                HR_EmployeeData(document.getElementById("ddlcompany").value, document.getElementById("ddlstate").value,
                    document.getElementById("ddllocation").value, document.getElementById("ddldepartment").value);
            });

            if ((!options.haveStateData) || (companyDataSrc.length == 1)) {
                divState.style.display = "none";
            }


            divStateinner2.appendChild(eleState);
            divStateinner1.appendChild(lblState);
            divState.appendChild(divStateinner1);
            divState.appendChild(divStateinner2);
            allElement.appendChild(divState);
        }

        {

            var divLocation = document.createElement("div");
            var divLocationinner1 = document.createElement("div")
            var lblLocation = document.createElement("label")
            var divLocationinner2 = document.createElement("div")


            divLocation.classList.add("col-md-6");
            divLocation.classList.add("form-group");
            divLocationinner1.classList.add("col-md-4");
            lblLocation.classList.add("col-form-label");
            divLocationinner2.classList.add("col-md-6");
            eleLocation.classList.add("form-control");
            eleLocation.classList.add("bg-ligth");

            lblLocation.innerText = "Location";




            var companyDataSrc = Array.from(new Set(dataSrc.map(s => s.location_id))).
                map(location_id => {
                    return {
                        location_id: location_id,
                        location_name: dataSrc.find(s => s.location_id == location_id).location_name
                    };
                });

            eleLocation.id = "ddllocation";
            if (options.havePleaseSelect) {
                var selectOption = document.createElement("option");
                selectOption.text = "Please Select";
                selectOption.value = 0;
                eleLocation.add(selectOption);
            }
            if (options.haveSelectAll) {
                var selectOption = document.createElement("option");
                selectOption.text = "All";
                selectOption.value = -1;
                selectOption.selected = true;
                eleLocation.add(selectOption);
            }

            $.each(companyDataSrc, function (key, value) {
                var selectOption = document.createElement("option");
                selectOption.text = value.location_name;
                selectOption.value = value.location_id;
                eleLocation.add(selectOption);

            });

            if (companyDataSrc.length > 0) {
                if (companyDataSrc.length == 1) {
                    eleLocation.value = companyDataSrc[0].location_id
                }
                //else {

                //    eleLocation.value = companyDataSrc[companyDataSrc.length - 1].location_id
                //}
            }

            eleLocation.addEventListener("change", function () {
                HR_EmployeeData(document.getElementById("ddlcompany").value, document.getElementById("ddlstate").value,
                    document.getElementById("ddllocation").value, document.getElementById("ddldepartment").value);
            });

            if ((!options.haveLocationData) || (companyDataSrc.length == 1)) {
                divLocation.style.display = "none";
            }


            divLocationinner2.appendChild(eleLocation);
            divLocationinner1.appendChild(lblLocation);
            divLocation.appendChild(divLocationinner1);
            divLocation.appendChild(divLocationinner2);
            allElement.appendChild(divLocation);
        }

        {

            var divDept = document.createElement("div");
            var divDeptinner1 = document.createElement("div")
            var lblDept = document.createElement("label")
            var divDeptinner2 = document.createElement("div")
            var eleDept = document.createElement("select");

            divDept.classList.add("col-md-6");
            divDept.classList.add("form-group");
            divDeptinner1.classList.add("col-md-4");
            lblDept.classList.add("col-form-label");
            divDeptinner2.classList.add("col-md-6");
            eleDept.classList.add("form-control");
            eleDept.classList.add("bg-ligth");

            lblDept.innerText = "Department";


            var companyDataSrc = Array.from(new Set(dataSrc.map(s => s.dept_id))).
                map(dept_id => {
                    return {
                        dept_id: dept_id,
                        dept_name: dataSrc.find(s => s.dept_id == dept_id).dept_name
                    };
                });

            eleDept.id = "ddldepartment";
            if (options.havePleaseSelect) {
                var selectOption = document.createElement("option");
                selectOption.text = "Please Select";
                selectOption.value = 0;
                eleDept.add(selectOption);
            }
            if (options.haveSelectAll) {
                var selectOption = document.createElement("option");
                selectOption.text = "All";
                selectOption.value = -1;
                selectOption.selected = true;
                eleDept.add(selectOption);
            }

            $.each(companyDataSrc, function (key, value) {
                var selectOption = document.createElement("option");
                selectOption.text = value.dept_name;
                selectOption.value = value.dept_id;
                eleDept.add(selectOption);

            });

            if (companyDataSrc.length > 0) {
                if (companyDataSrc.length == 1) {
                    eleDept.value = companyDataSrc[0].dept_id
                }
                //else {
                //    eleDept.value = companyDataSrc[companyDataSrc.length - 1].dept_id
                //}
            }

            eleDept.addEventListener("change", function () {
                HR_EmployeeData(document.getElementById("ddlcompany").value, document.getElementById("ddlstate").value,
                    document.getElementById("ddllocation").value, document.getElementById("ddldepartment").value);
            });

            if ((!options.haveDepartmentData) || (companyDataSrc.length == 1)) {
                divDept.style.display = "none";
            }
            divDeptinner2.appendChild(eleDept);
            divDeptinner1.appendChild(lblDept);
            divDept.appendChild(divDeptinner1);
            divDept.appendChild(divDeptinner2);
            allElement.appendChild(divDept);
        }

        {
            var divEmp = document.createElement("div");
            var divEmpinner1 = document.createElement("div")
            var lblEmp = document.createElement("label")
            var divEmpinner2 = document.createElement("div")
            var eleEmployee = document.createElement("select");

            divEmp.classList.add("col-md-6");
            divEmp.classList.add("form-group");
            divEmpinner1.classList.add("col-md-4");
            lblEmp.classList.add("col-form-label");
            divEmpinner2.classList.add("col-md-6");
            eleEmployee.classList.add("form-control");
            eleEmployee.classList.add("bg-ligth");

            lblEmp.innerText = "Employee";


            eleEmployee.id = "ddlemployee";




            //HR_EmployeeData(eleCompany.value, eleState.value, eleLocation, eleDept.value, eleEmployee, options.haveSelectAll, options.havePleaseSelect);
            HR_EmployeeData(eleCompany.value, eleState.value, eleLocation.value, eleDept.value, eleEmployee, options.haveSelectAll, options.havePleaseSelect);
            var default_empID = CryptoJS.AES.decrypt(localStorage.getItem("emp_id"), localStorage.getItem("sit_id")).toString(CryptoJS.enc.Utf8).replace(/[\'\"]/g, function (m) { return m === "'" ? '' : ''; });
            // default All option selected -1==All , 0=Please select
            eleEmployee.value = parseInt(-1);


            if ((!options.haveEmpData)) {
                divEmp.style.display = "none";
            }

            divEmpinner2.appendChild(eleEmployee);
            divEmpinner1.appendChild(lblEmp);
            divEmp.appendChild(divEmpinner1);
            divEmp.appendChild(divEmpinner2);
            allElement.appendChild(divEmp);
        }



        //eleCompany.editableSelect();
        //eleState.editableSelect();
        //eleDept.editableSelect();
        //eleLocation.editableSelect();
        //eleEmployee.editableSelect();


        // Greenify the collection based on the settings variable.
        return this.append(allElement);

    };


}(jQuery));