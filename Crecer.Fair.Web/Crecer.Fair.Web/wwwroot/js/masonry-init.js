/**
 * Sell – Bootstrap 5 e-commerce template v. 2.0.0
 * Homepage: https://themes.getbootstrap.com/product/sell-bootstrap-4-e-commerce-template/
 * Copyright 2021, Bootstrapious - https://bootstrapious.com
 */

'use strict';

(function ($) {
    var $grid = $('.masonry-wrapper').masonry({
        itemSelector: '.item',
        columnWidth: '.item',
        percentPosition: true,
        transitionDuration: 300,
    });

    $grid.imagesLoaded().progress( function() {
        $grid.masonry();
    });
}(jQuery));