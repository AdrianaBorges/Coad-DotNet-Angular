import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { LoaderService } from '../../base/services/loader.service';
import { Subscription } from 'rxjs';
import { LayoutScriptsService } from '../services/layout-scripts.service';
import { fadeAnimation } from 'src/app/base/animations/animations';

@Component({
  selector: 'app-loja-layout',
  templateUrl: './loja-layout.component.html',
  styleUrls: ['./loja-layout.component.css'],
  animations: [fadeAnimation]
})
export class LojaLayoutComponent implements OnInit, OnDestroy, AfterViewInit {

  showAjaxLoader: boolean;
  ajaxLoaderUrl: string;
  loaderSubscription: Subscription;

  constructor(
    private loader: LoaderService,
    private layoutScripts: LayoutScriptsService
  ) { }


  ngOnInit() {

    this.ajaxLoaderUrl = 'assets/images/ajax-loader-double-ring.gif';
    this.loaderSubscription = this.loader.getShowLoader().subscribe(showLoader =>

      this.showAjaxLoader = showLoader
    );
  }

  ngOnDestroy(): void {
    this.loaderSubscription.unsubscribe();
  }

  ngAfterViewInit(): void {

    this.layoutScripts.InitPreloader();
  }

}
