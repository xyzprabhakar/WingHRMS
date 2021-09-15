$(document).ready(function () {
    var role_menu_list = JSON.parse(localStorage.getItem('_menu_lst'));
    var element = document.getElementById("side-nav");
    BindMenuData(role_menu_list, element,true);
    $("#side-nav").metisMenu();
    initActiveMenu();
})

function BindMenuData(datas,parentElement,tobeadded) {
    for (var i = 0; i < datas.length; i++) {
        
        var itemLi = document.createElement("li");
        parentElement.appendChild(itemLi);
        var itemA = document.createElement("a");
        var itemI = document.createElement("i");
        var itemSpan = document.createElement("span");
        
        itemSpan.innerText = datas[i].text;
        itemI.setAttribute("class", datas[i].icon_url);

        itemLi.appendChild(itemA);
        if (tobeadded) {
            itemA.appendChild(itemI);
        }
        
        itemA.appendChild(itemSpan);
        if (datas[i].children == null || datas[i].children.length == 0) {
            itemA.href ="/"+ datas[i].urll;
        }
        else {
            itemA.href = "javascript: void(0);";            
            itemUl = document.createElement("ul");
            itemUl.setAttribute("aria-expanded", false);

            itemUl.classList.add("nav-second-level");
            var itemInnerSpan = document.createElement("span");
            var itemInnerI = document.createElement("i");
            itemInnerI.classList.add("mdi");
            itemInnerI.classList.add("mdi-chevron-right");
            itemInnerSpan.classList.add("menu-arrow");
            itemInnerSpan.appendChild(itemInnerI);
            itemA.appendChild(itemInnerSpan);
            itemLi.appendChild(itemUl);
            BindMenuData(datas[i].children, itemUl,false);            
        }        
    }
}

function initActiveMenu() {
    // === following js will activate the menu in left side bar based on url ====
    //$(".left-sidenav a").each(function () {
    //    var pageUrl = window.location.href.split(/[?#]/)[0];

    //    if (pageUrl.toLowerCase().search(this.href.toLowerCase())>-1) {
    //        $(this).addClass("active");
    //        $(this).parent().addClass("active"); // add active to li of the current link
    //        $(this).parent().parent().addClass("in");
    //        $(this).parent().parent().prev().addClass("active"); // add active class to an anchor
    //        $(this).parent().parent().parent().addClass("active");
    //        $(this).parent().parent().parent().parent().addClass("in"); // add active to li of the current link
    //        $(this).parent().parent().parent().parent().parent().addClass("active");
    //        $(this).parent().parent().parent().parent().parent().parent().addClass("active");
    //        $(this).parent().parent().parent().parent().parent().parent().addClass("in"); // add active to li of the current link
    //        $(this).parent().parent().parent().parent().parent().parent().parent().addClass("active");
    //    }
    //});

    $("#side-nav a").each(function () {
        var pageUrl = window.location.href.split(/[?#]/)[0];
        if (this.href == pageUrl) {
            $(this).addClass("active");
            $(this).parent().addClass("active"); // add active to li of the current link
            $(this).parent().parent().addClass("in");
            $(this).parent().parent().prev().addClass("active"); // add active class to an anchor
            $(this).parent().parent().parent().addClass("active");
            $(this).parent().parent().parent().parent().addClass("in"); // add active to li of the current link
            $(this).parent().parent().parent().parent().parent().addClass("active");
            $(this).parent().parent().parent().parent().parent().parent().addClass("active");
            $(this).parent().parent().parent().parent().parent().parent().addClass("in");
            $(this).parent().parent().parent().parent().parent().parent().parent().addClass("active");
        }
    });

}

//$(document).ready(function () {
//    BindData();



//    $('ul.leftMenu li a.subMenu').on('click', function (e) {
//        // debugger;
//        e.preventDefault();
//        let check = $(this).next('ul');
//        let icons = check.next('span.fa');
//        if (check.hasClass('hasSubmenu')) {

//            if (check.is(':visible')) {
//                check.slideUp();
//                icons.removeClass('fa-angle-up').addClass('fa-angle-down');
//            }
//            else {
//                check.slideDown();
//                icons.removeClass('fa-angle-down').addClass('fa-angle-up');
//            }
//        }


//    });

//    var key = CryptoJS.enc.Base64.parse("#base64Key#");
//    var iv = CryptoJS.enc.Base64.parse("#base64IV#");

//    var is_managerr_dec = CryptoJS.AES.decrypt(localStorage.getItem("is_managerr"), key, { iv: iv }).toString(CryptoJS.enc.Utf8);


//    var its_manager = is_managerr_dec;

//    if (its_manager == "yes") {
//        $("#limyteam").show();
//    }
//    else if (its_manager == "no") {
//        $("#limyteam").hide();
//    }

//});

//function initActiveMenu() {
   
//    // === following js will activate the menu in left side bar based on url ====
//    $(".list-group-href").each(function () {
        
//        var pageUrl = window.location.href.split(/[?#]/)[0];
//        alert(pageUrl);
//        if (this.href == pageUrl) {
//            $(this).addClass("active");
//            $(this).parent().addClass("active"); // add active to li of the current link
//            $(this).parent().parent().addClass("in");
//            $(this).parent().parent().prev().addClass("active"); // add active class to an anchor
//            $(this).parent().parent().parent().addClass("active");
//            $(this).parent().parent().parent().parent().addClass("in"); // add active to li of the current link
//            $(this).parent().parent().parent().parent().parent().addClass("active");
//        }
//    });
//}
//function BindData() {

//    var role_menu_list = JSON.parse(localStorage.getItem('_menu_lst'));

//    role_menu_list = role_menu_list.filter(p => p.type == 1);

//    $(function () {

//        $('#tree').bstreeview({
//            data: JSON.stringify(role_menu_list), indent: 2
// });

//        //$('#tree').bstreeview({ data: JSON.stringify(role_menu_list) });
//        initActiveMenu();
//    });

       

    

//}

//function initActiveMenu() {

//    // === following js will activate the menu in left side bar based on url ====
//    $(".left-sidenav-menu a.list-group-href").each(function () {

//        var pageUrl = window.location.href.split(/[?#]/)[0];
//        //alert(pageUrl);
//        if (this.href == pageUrl) {
//            $(this).addClass("active");
//            $(this).parent().find('.item-icon').addClass("text-primary"); // add active to li of the current link
            
//            $(this).parents().parents().addClass("show");
//           // $(this).parent().parent().not('.show').addClass("xyz");

//            //$(this).parent().parent().prev().addClass("active"); // add active class to an anchor
//            //$(this).parent().parent().parent().addClass("active");
//            //$(this).parent().parent().parent().parent().addClass("in"); // add active to li of the current link
//            //$(this).parent().parent().parent().parent().parent().addClass("active");

//           // $('.list-group-item').not('.show').toggle;
//            //$(this).find('.list-group:first').toggle();

//        }
//    });
//}



//function findValueInArray(children_menu_id, assign_menu) {

//    var result = "Doesn't exist";
//    for (var i = 0; i < assign_menu.length; i++) {
//        var assign_idd = assign_menu[i];
//        if (parseInt(assign_idd) == parseInt(children_menu_id)) {
//            result = 'Exist';;
//            break;
//        }
//    }
//    return result;
//}

