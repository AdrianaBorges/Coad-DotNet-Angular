import { trigger, state, style, transition, animate } from "@angular/animations";

export const SlideInOutAnimation = [

  trigger('slideInOut', [

    state('in', style({
      'max-height' : '400px', 'opacity': '1', 'visibility' : 'visible', 'display': 'block'
    })),
    state('out', style({
      'max-height': '0px', 'opacity': '0', 'visibility': 'hidden', 'display': 'none'
    })),
    transition('in <=> out', animate('1s'))
  ])
];
