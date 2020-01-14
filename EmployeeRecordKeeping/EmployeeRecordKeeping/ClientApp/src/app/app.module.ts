import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { DataTablesModule } from 'angular-datatables';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { EmployeeComponent } from './employee/employee.component';
import { EmployeeService } from './services/employee.service';
import { HttpService } from './services/http.service';
import { HttpModule } from '@angular/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { NgxSpinnerModule } from "ngx-spinner";
import {
  MatButtonModule, MatToolbarModule, MatMenuModule, MatSidenavModule, MatCardModule, MatIconModule, MatDialogModule, MatInputModule, MatRadioModule, MatTableModule,
  MatCheckboxModule, MatTabsModule, MatSelectModule, MatOptionModule, MatProgressSpinnerModule, MatExpansionModule, MatProgressBarModule, MatNativeDateModule,
  MatAutocompleteModule, MatTooltipModule, MatDatepickerModule, MatSnackBarModule, MatSliderModule } from '@angular/material';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    EmployeeComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    HttpModule,
    NgxSpinnerModule,
    DataTablesModule,
    ToastrModule.forRoot(),
    BrowserAnimationsModule,
    MatButtonModule, MatToolbarModule, MatMenuModule, MatSidenavModule, MatCardModule, MatIconModule, MatDialogModule, MatInputModule, MatRadioModule, MatTableModule,
    MatCheckboxModule, MatTabsModule, MatSelectModule, MatOptionModule, MatProgressSpinnerModule, MatExpansionModule, MatProgressBarModule, MatNativeDateModule, MatSnackBarModule, MatSliderModule,
    MatAutocompleteModule, MatTooltipModule, MatDatepickerModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'employee', component: EmployeeComponent },
    ])
  ],
  providers: [EmployeeService, HttpService],
  bootstrap: [AppComponent]
})
export class AppModule { }
