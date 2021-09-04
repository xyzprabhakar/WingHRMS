;(function() {
  'use strict';
    $('.nav-tabs').scrollingTabs();

    $(".tabs-view  .btnNext").click(function (e) {
        console.log('next click');
        e.preventDefault();
        
      
        $('.nav-tabs > .active').next('li').find('a').trigger('click');
        
       

    });
    $(".tabs-view  .btnPrev").click(function (e) {
        console.log('next click');
        e.preventDefault();
      $('.nav-tabs > .active').prev('li').find('a').trigger('click');
       

    });


}());
