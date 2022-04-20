import { Directive, ElementRef, HostListener } from '@angular/core';
@Directive({
  selector: '[appToogle]'
})
export class ToogleDirective {

  previewParClosedHeight = 25;

  constructor(private element: ElementRef) {


  }
  @HostListener('click', ['$event'])
  toogle(event) {

    const element = event.target;
    const parentSection = jQuery(element).parent(),
      parentWrapper = jQuery(element).parents('div.toggle'),
      isAccordion = parentWrapper.hasClass('toggle-accordion');
    let previewPar = null;
    let previewParAnimateHeight = null;

    if (isAccordion && typeof (event.originalEvent) !== 'undefined') {
      parentWrapper.find('div.toggle.active > label').trigger('click');
    }

    parentSection.toggleClass('active');

    if (parentSection.find('> p').get(0)) {

      previewPar = parentSection.find('> p');
      const previewParCurrentHeight = previewPar.css('height');
      previewParAnimateHeight = previewPar.css('height');
      previewPar.css('height', 'auto');
      previewPar.css('height', previewParCurrentHeight);

    }

    const toggleContent = parentSection.find('> div.toggle-content');

    if (parentSection.hasClass('active')) {

      jQuery(previewPar).animate({ height: previewParAnimateHeight }, 350, function () { jQuery(this).addClass('preview-active'); });
      toggleContent.slideDown(350);

    } else {

      jQuery(previewPar).animate({ height: this.previewParClosedHeight }, 350, function () { jQuery(this).removeClass('preview-active'); });
      toggleContent.slideUp(350);

    }
  }

}
