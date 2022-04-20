import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { LoaderService } from '../../base/services/loader.service';
import { Subscription } from 'rxjs/Subscription';
import { LayoutScriptsService } from '../services/layout-scripts.service';

@Component({
  selector: 'app-loja-layout',
  templateUrl: './loja-layout.component.html',
  styleUrls: ['./loja-layout.component.css']
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

    this.ajaxLoaderUrl = "assets/images/ajax-loader-double-ring.gif";
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
