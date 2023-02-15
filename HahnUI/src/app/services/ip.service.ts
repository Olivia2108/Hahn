import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IpService {

  constructor(private http: HttpClient) { }
 
  getIPAddress(): Observable<string> 
  { 
    return this.http.get<string>("http://api.ipify.org/?format=json"); 

  }
}
