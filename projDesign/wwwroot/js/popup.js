$(function(){

    $('.message-pop .header i').on('click', function () {
        $(this).parents('.message-pop').hide('slow');
    });

    $('#offDays').on('change', function () {
        let values=$(this).val();
        if(values=="Dynamic"){$('table#dynamics').show();}
        else{$('table#dynamics').hide();}
    });





    // tabs function
    var get=2;
    
    $('#nexttabsBTN').on('click', function () {
        if(get<=4){ $('.bag'+get).addClass('active');
            var minus=get-1;
            $('#c'+minus).css('display','none');
            $('#c'+get).css('display','block');
        }
        else{get=4;}
        get++;
    });

    // prev button

    $('#prevBtntabs').on('click', function () {
        $('.tabs-container').hide();
        get--;
        let turn=get+1;
        if(get>=1){$('#c'+get).css('display','block');
            $('.bag'+turn).removeClass('active');
        }
        else{get=1;$('#c1').css('display','block');}
    });
});

function messageBox(messageType , text)
{
    //$('.message-pop').show();
    //$('.message-pop p.text').html(text);
    //if (messageType == "success") {
    //    $('.message-pop').removeClass('bg-red').removeClass('bg-yeallow').addClass('bg-green');
    //    $('.message-pop span').removeClass('fa-times').removeClass('fa-exclamation-triangle').addClass('fa-check').addClass('color-green');

    //}
    //if (messageType == "error") {
    //    $('.message-pop').removeClass('bg-yeallow').removeClass('bg-green').addClass('bg-red');
    //    $('.message-pop span').removeClass('fa-check').removeClass('fa-exclamation-triangle').addClass('fa-times').addClass('color-red');
    //}
    //if (messageType == "info") {
    //    $('.message-pop').removeClass('bg-red').removeClass('bg-green').addClass('bg-yeallow');
    //    $('.message-pop span').removeClass('fa-check').removeClass('fa-times').addClass('fa-exclamation-triangle').addClass('color-yeallow');
    //}
    msgBoxImagePath = "../js/msgbox/images/"; 
    if (messageType == 'error') {
        $.msgBox({
            title: "Please Correct Following Information !!",
            content: text,
            type: "error"
        });
    }
    else if (messageType == 'alert') {
        $.msgBox({
            title: "Please Correct Following Information !!",
            content: text,
            type: "alert"
        });
    }
    else if (messageType == 'info') {
        $.msgBox({
            title: "Information !!",
            content: text,
            type: "info"
        });
    }
    else if (messageType == 'success') {
        $.msgBox({
            title: "Information !!",
            content: text,
            type: "success"
        });
    }
}

function ConfirmMsgBox(text) {
    $.msgBox({
        title: "Are You Sure",
        content: text,
        type: "confirm",
        buttons: [{ value: "Yes" }, { value: "No" }, { value: "Cancel" }],
        success: function (result) {
            if (result == "Yes") {
                return true;
            }
            return false;
        }
    });
}
function MsgBoxAndRedirect(text,url) {
    $.msgBox({
        title: "Information !!",
        content: text,
        type: "confirm",
        buttons: [{ value: "OK" }],
        success: function (result) {
            if (result == "OK") {
                window.location.href = url;
            }
            return false;
        }
    });
}

