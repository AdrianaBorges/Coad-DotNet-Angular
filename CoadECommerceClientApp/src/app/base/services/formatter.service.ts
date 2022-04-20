import { Injectable } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { DinheiroMask } from '../models/dinheiro-mask';

@Injectable()
export class FormatterService {

  constructor(private decimalPipe: DecimalPipe) { }

  formatFromModel(value: string): DinheiroMask {

    if (value) {

      value = value.replace(/\D/g, '');
      let decimalNumber = Number(value);
      decimalNumber = (decimalNumber / 100);

      const formatedValue = this.decimalPipe.transform(decimalNumber, '1.2-2', 'pt');
      const result = new DinheiroMask(decimalNumber, formatedValue, 'ngModel');

      return result;
    }
  }

  formatFromValue(value: number): DinheiroMask {

    if (value) {

      const formatedValue = this.decimalPipe.transform(value, '1.2-2', 'pt');
      const result = new DinheiroMask(value, formatedValue, 'maskDinheiro');
      return result;
    }
  }

}
