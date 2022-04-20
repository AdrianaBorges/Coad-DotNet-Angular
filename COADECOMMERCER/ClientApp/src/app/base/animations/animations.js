"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var animations_1 = require("@angular/animations");
exports.SlideInOutAnimation = [
    animations_1.trigger('slideInOut', [
        animations_1.state('in', animations_1.style({
            'max-height': '400px', 'opacity': '1', 'visibility': 'visible', 'display': 'block'
        })),
        animations_1.state('out', animations_1.style({
            'max-height': '0px', 'opacity': '0', 'visibility': 'hidden', 'display': 'none'
        })),
        animations_1.transition('in <=> out', animations_1.animate('1s'))
    ])
];
//# sourceMappingURL=animations.js.map