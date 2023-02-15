import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppSettings } from '../app.config';
import { newOrder } from '../Models/Employee';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private _http: HttpClient) { }

   /* ----------==========     Create Employee API    ==========---------- */
   createEmployee(model: any) {
    return this._http.post<newOrder>(AppSettings.API_ENDPOINT + "api/Employee/AddEmployee", model);
  }

   /* ----------==========     Get All employee    ==========---------- */
   getAllEmployee() {
    return this._http.get(AppSettings.API_ENDPOINT + "api/employee/GetAllEmployee");
  }

   /* ----------==========     Update Employee Information    ==========---------- */
   updateEmployee(id: any, model:any) {
    return this._http.put(AppSettings.API_ENDPOINT + `api/employee/UpdateEmployeeById?employeeId=${id}`, model);
  }

   /* ----------==========     Delete Employee   ==========---------- */
   deleteEmployee(id: any) {
    return this._http.delete(AppSettings.API_ENDPOINT + `api/employee/DeleteEmployeeById?employeeId=${id}`);
  }

  

}
