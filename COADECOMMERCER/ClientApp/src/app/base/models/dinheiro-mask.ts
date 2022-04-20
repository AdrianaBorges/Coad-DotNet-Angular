export class DinheiroMask {

  value: number;
  formatted: string;
  startDirection: string;
  
  constructor(value: number, formatted: string, startDirection: string) {

    this.value = value;
    this.formatted = formatted;
    this.startDirection = startDirection;
  }
}

