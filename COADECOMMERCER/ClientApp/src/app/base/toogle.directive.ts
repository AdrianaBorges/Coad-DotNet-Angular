import { Directive, ElementRef } from '@angular/core';

@Directive({
  selector: '[appToogle]',
  host: {
    '(click)': 'toogle($event)'
  } 
})
export class ToogleDirective {

  previewParClosedHeight: number = 25;

  constructor(private element: ElementRef) {


  }

  toogle(event) {

    var element = event.target;
    var parentSection = jQuery(element).parent(),
      parentWrapper = jQuery(element).parents("div.toggle"),
      previewPar = null,
      isAccordion = parentWrapper.hasClass("toggle-accordion");

    if (isAccordion && typeof (event.originalEvent) != "undefined") {
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

      jQuery(previewPar).animate({ height: this.previewParClosedHeight }, 350, function () { jQuery(this).removeClass("preview-active"); });
      toggleContent.slideUp(350);

    }
  }

}
