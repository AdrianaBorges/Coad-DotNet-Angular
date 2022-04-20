import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MessageService } from '../services/message.service';
import { Subscription } from 'rxjs/Subscription';
import { Message } from '../models/message';

@Component({
  selector: 'app-validation-result',
  templateUrl: './validation-result.component.html',
  styleUrls: ['./validation-result.component.css']
})
export class ValidationResultComponent implements OnInit, OnDestroy {
    
  _path: string;
  lstValidationsResult: string[];
  @Input() path: string;
  valitationSb: Subscription;

  constructor(private message: MessageService) {

  }

  ngOnInit() {
    this.valitationSb = this.message.getValidation().subscribe(validation => {

      if (validation) {
        var lstValidations = this._resolvePath(validation)
        this.lstValidationsResult = lstValidations;
      }
      else {
        this.lstValidationsResult = [];
      }
      
    });
  }

  ngOnDestroy(): void {

    if (this.valitationSb)
    this.valitationSb.unsubscribe();
  }


  private _resolvePath(validation: object): string[] {

    if (this.path != null) {

      var paths = this.path.split(".");
      var currentyObject = validation;
      if (paths) {
        paths.forEach(function (path) {

          if (currentyObject && currentyObject[path]) {

            currentyObject = currentyObject[path]
          }
        });
      }
      if (currentyObject instanceof Array)
        return currentyObject as string[];      
    }
    return [] as string[];
  }

  trackMessage(index, val: string) {

    return val ? val : undefined;
  }

}
