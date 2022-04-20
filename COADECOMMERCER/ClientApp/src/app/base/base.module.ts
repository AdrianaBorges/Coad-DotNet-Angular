import { NgModule } from '@angular/core';
import { CommonModule, DecimalPipe } from '@angular/common';
import { MaskDinheiroDirective } from './mask-dinheiro.directive';
import { FormatterService } from './services/formatter.service';
import { PaginatorComponent } from './paginator/paginator.component';
import { MessageComponent } from './message/message.component';
import { MessageService } from './services/message.service';
import { LoaderService } from './services/loader.service';
import { ValidationResultComponent } from './validation-result/validation-result.component';
import { ToogleDirective } from './toogle.directive';


@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [
    MaskDinheiroDirective,
    PaginatorComponent,
    MessageComponent,
    ValidationResultComponent,
    ToogleDirective],
  exports: [
    MaskDinheiroDirective,
    PaginatorComponent,
    MessageComponent,
    ValidationResultComponent,
    ToogleDirective
  ],
  providers: [
    DecimalPipe,
    FormatterService,
    MessageService,
    LoaderService
  ]
})
export class BaseModule { }
