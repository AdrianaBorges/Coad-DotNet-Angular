import { Injectable } from '@angular/core';
import * as JQuery from 'jquery';

@Injectable()
export class LayoutScriptsService {

  constructor() { }

  Init(): void {

    this.InitPreloader();
    this.InitToogle();
  }

  InitPreloader(): void {

    /** Preloader **************************************************************** **/
    if (JQuery('#preloader').length > 0) {

      jQuery('#preloader').fadeOut(1000, function () {
        jQuery('#preloader').remove();
      });
    }
  }

  InitToogle(): void {
    /** Toogle **************************************************************** **/

    var $_t = this,
      previewParClosedHeight = 25;

    jQuery("div.toggle.active > p").addClass("preview-active");
    jQuery("div.toggle.active > div.toggle-content").slideDown(400);
    jQuery("div.toggle > label").click(function (e) {

      var parentSection = jQuery(this).parent(),
        parentWrapper = jQuery(this).parents("div.toggle"),
        previewPar = null,
        isAccordion = parentWrapper.hasClass("toggle-accordion");

      if (isAccordion && typeof (e.originalEvent) != "undefined") {
        parentWrapper.find("div.toggle.active > label").trigger("click");
      }

      parentSection.toggleClass("active");

      if (parentSection.find("> p").get(0)) {

        previewPar = parentSection.find("> p");
        var previewParCurrentHeight = previewPar.css("height");
        var previewParAnimateHeight = previewPar.css("height");
        previewPar.css("height", "auto");
        previewPar.css("height", previewParCurrentHeight);

      }

      var toggleContent = parentSection.find("> div.toggle-content");

      if (parentSection.hasClass("active")) {

        jQuery(previewPar).animate({ height: previewParAnimateHeight }, 350, function () { jQuery(this).addClass("preview-active"); });
        toggleContent.slideDown(350);

      } else {

        jQuery(previewPar).animate({ height: previewParClosedHeight }, 350, function () { jQuery(this).removeClass("preview-active"); });
        toggleContent.slideUp(350);

      }

    });
  }
}
