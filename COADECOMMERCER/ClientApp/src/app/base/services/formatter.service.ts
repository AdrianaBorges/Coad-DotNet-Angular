import { Injectable } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { DinheiroMask } from '../models/dinheiro-mask';

@Injectable()
export class FormatterService {

  constructor(private decimalPipe: DecimalPipe) { }

  formatFromModel(value: string): DinheiroMask {

    if (value) {

      value = value.replace(/\D/g, "");
      var decimalNumber = Number(value);
      decimalNumber = (decimalNumber / 100);

      var formatedValue = this.decimalPipe.transform(decimalNumber, '1.2-2', 'pt');
      var result = new DinheiroMask(decimalNumber, formatedValue, 'ngModel');

      return result;
    }
  }

  formatFromValue(value: number): DinheiroMask {

    if (value) {

      var formatedValue = this.decimalPipe.transform(value, '1.2-2', 'pt');
      var result = new DinheiroMask(value, formatedValue, 'maskDinheiro');
      return result;
    }
  }

}
