import { Component, OnInit, AfterViewInit } from '@angular/core';
import { SlideInOutAnimation } from 'src/app/base/animations/animations';
import { LayoutScriptsService } from 'src/app/layouts/services/layout-scripts.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
  animations : [SlideInOutAnimation]
})
export class CheckoutComponent implements OnInit, AfterViewInit {

  constructor(private scripts: LayoutScriptsService) { }

  ngOnInit() {
  }

  ngAfterViewInit(): void {
    this.scripts.InitToogle();
  }

}
