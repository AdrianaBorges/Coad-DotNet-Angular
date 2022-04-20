import { Directive, Output, EventEmitter, Input, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { NgControl } from '@angular/forms';
import { FormatterService } from './services/formatter.service';
import { DinheiroMask } from './models/dinheiro-mask';

@Directive({
  selector: '[appMaskDinheiro]'
})
export class MaskDinheiroDirective implements OnInit, OnDestroy{
    
    
  modelValue: number;
  subscription: Subscription;
  lastMask: DinheiroMask;
  @Output() appMaskDinheiroChange: EventEmitter<number> = new EventEmitter();
  @Output() ngModelChange: EventEmitter<any> = new EventEmitter();


  @Input()
  get appMaskDinheiro() {
    return this.modelValue;
  }
  set appMaskDinheiro(value) {

    this.modelValue = value;
    var mask = this.formatterService.formatFromValue(this.modelValue);
    this.format(mask);
    this.appMaskDinheiroChange.emit(this.appMaskDinheiro);
  }

  constructor(
    private ngControl: NgControl,
    private formatterService: FormatterService) { }


  ngOnInit(): void {

    var initValue = this.appMaskDinheiro;
    if (initValue) {

      var mask = this.formatterService.formatFromValue(initValue);
      this.format(mask);

    }
    const ctrl = this.ngControl.control;
    this.subscription = ctrl.valueChanges.subscribe(value => { // Observavel que notifica mudanças no ngModel

      var mask = this.formatterService.formatFromModel(value);
      this.format(mask);
    });
  }

  format(mask: DinheiroMask): void {
    if (mask &&
      (mask.formatted != this.ngControl.control.value ||
      mask.value != this.appMaskDinheiro)) {

      if (mask.formatted != this.ngControl.control.value) {

        this.ngControl.control.setValue(mask.formatted);
        this.ngModelChange.emit(mask.formatted);
      }

      if (mask.value != this.appMaskDinheiro &&
        mask.startDirection == 'ngModel') {

        this.appMaskDinheiro = mask.value;
      }
    }
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
