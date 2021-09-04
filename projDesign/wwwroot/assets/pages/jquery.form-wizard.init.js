/**
 * Theme: Amezia - Responsive Bootstrap 4 Admin Dashboard
 * Author: Themesbrand
 * Form Wizard
 */


$(function ()
{
    $("#form-horizontal").steps({
        headerTag: "h3",
        bodyTag: "fieldset",
        transitionEffect: "slide",
        enableAllSteps: true,

        onFinished: function () {
            $("#btnSaveRepository").click();
        },
        

        labels: {
          
            finish: "Save",
            next: "Next",
            previous: "Previous"
            
        }


    });
    
});

