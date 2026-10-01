(function ($) {
    "use strict";

    window.initSidebar = function () {

        var $nav = $('.page-sidebar');
        var $header = $('.page-main-header');
        var $toggle = $('#sidebar-toggle');
        var $menu = $('.sidebar-menu');

        // اگر sidebar هنوز در DOM نیست، کاری نکن
        if (!$menu.length) {
            return;
        }

        /*
         * Sidebar menu
         * ابتدا handler قبلی را حذف می‌کنیم
         * تا در Blazor چند بار ثبت نشود.
         */
        $menu.off('click.sidebarMenu');

        $menu.on('click.sidebarMenu', 'li a', function (e) {

            var $this = $(this);
            var $submenu = $this.next('.sidebar-submenu');

            // اگر این لینک زیرمنو ندارد،
            // اجازه بده Blazor navigation خودش انجام شود.
            if (!$submenu.length) {
                return;
            }

            // این لینک فقط برای باز و بسته کردن منو است
            e.preventDefault();

            var animationSpeed = 300;

            if ($submenu.is(':visible')) {

                // بستن submenu
                $submenu.slideUp(animationSpeed, function () {
                    $submenu.removeClass('menu-open');
                });

                $this.parent('li').removeClass('active');

            } else {

                // بستن submenuهای دیگر
                var $parent = $this.parents('ul').first();

                $parent
                    .find('.sidebar-submenu:visible')
                    .not($submenu)
                    .slideUp(animationSpeed)
                    .removeClass('menu-open');

                $parent
                    .find('li.active')
                    .removeClass('active');

                // باز کردن submenu
                $submenu.slideDown(animationSpeed, function () {
                    $submenu.addClass('menu-open');
                });

                $this.parent('li').addClass('active');
            }
        });


        /*
         * Sidebar toggle
         */
        $toggle.off('click.sidebarToggle');

        $toggle.on('click.sidebarToggle', function () {

            $nav.toggleClass('open');
            $header.toggleClass('open');

        });


        /*
         * Body click
         */
        $('.body-part')
            .off('click.sidebarBody')
            .on('click.sidebarBody', function () {

                $nav.addClass('open');
                $header.addClass('open');

            });


        /*
         * Responsive
         */
        function responsiveSidebar() {

            var width = $(window).width();

            if (width + 17 <= 991) {

                $toggle.addClass('open');
                $nav.addClass('open');

            } else {

                $toggle.removeClass('open');
                $nav.removeClass('open');

            }
        }

        // resize قبلی را حذف کن
        $(window).off('resize.sidebarResponsive');

        // فقط یک resize handler
        $(window).on('resize.sidebarResponsive', function () {
            responsiveSidebar();
        });

        responsiveSidebar();


        /*
         * Active menu
         */
        $menu.find('a').removeClass('active');
        $menu.find('li').removeClass('active');

        var current = window.location.pathname;

        $menu.find('a').each(function () {

            var link = $(this).attr('href');

            if (link && link !== '#') {

                if (current.indexOf(link) !== -1) {

                    $(this).parents('li').addClass('active');
                    $(this).addClass('active');

                }
            }
        });


        /*
         * Feather icons
         */
        if (typeof feather !== 'undefined') {
            feather.replace();
        }
    };

})(jQuery);