
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppSettings } from '../app.config';     
import { Employee } from '../Models/employee.model';


@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private _http: HttpClient) { }

   /* ----------==========     Create Employee API    ==========---------- */
   createEmployee(model: any) {
    return this._http.post<Employee>(AppSettings.baseApiUrl + "api/Employee/AddEmployee", model);
  }

   /* ----------==========     Get All employee    ==========---------- */
   getAllEmployee() {
    return this._http.get(AppSettings.baseApiUrl + "api/employee/GetAllEmployee");
  }

   /* ----------==========     Update Employee Information    ==========---------- */
   updateEmployee(id: any, model:any) {
    return this._http.put(AppSettings.baseApiUrl + `api/employee/UpdateEmployeeById?employeeId=${id}`, model);
  }

   /* ----------==========     Delete Employee   ==========---------- */
   deleteEmployee(id: any, ip: string) {
    return this._http.delete(AppSettings.baseApiUrl + `api/employee/DeleteEmployeeById?employeeId=${id}&&ipAddress=${ip}`);
  }

  

}
