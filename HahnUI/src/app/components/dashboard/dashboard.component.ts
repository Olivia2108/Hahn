import { IpService } from './../../services/ip.service';

import {Component, ViewChild, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatTable, MatTableDataSource} from '@angular/material/table';  
import { ApiService } from 'src/app/services/employee.service';
import { NotificationService } from 'src/app/services/notification.service';
import { Employee } from 'src/app/Models/employee.model';
import { HttpErrorResponse } from '@angular/common/http';
import { catchError, of } from 'rxjs';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    title = 'Employees';
    selectedEmployee: Employee;
    Id: string;
    DateCreated: string;
    ipAddress: string;
    showValidationError: boolean;
    validationError: any;

  result: any[] = [];
  employeeForm: FormGroup;
  updateForm: FormGroup;
  employee: Employee[] = [];
  @ViewChild("updateModal", { static: true }) updateModal: TemplateRef<any>;
  @ViewChild("newModal", { static: true }) newModal: TemplateRef<any>;
  @ViewChild("deleteModal", { static: true }) deleteModal: TemplateRef<any>;
  
  displayedColumns: string[] = ['S/N', 'Name', 'Email', 'Department', 'Salary', 'Phone', 'DateCreated', 'Actions'];
  
  @ViewChild(MatPaginator, { static: true}) paginator!: MatPaginator;
  
  dataSource = new MatTableDataSource<any>([]);
  
  
  constructor(public dialog: MatDialog, private _formBuilder: FormBuilder,
              private notificationService: NotificationService, 
              private apiService: ApiService,
              private ipService: IpService) {

                this.employeeForm = this._formBuilder.group({
      name: ["", [Validators.required]],
      email: ["", [Validators.required, Validators.email]],
      phone: ["", [Validators.required, Validators.minLength(11), Validators.maxLength(11)]],
      salary: ["", [Validators.required]],
      department: ["", [Validators.required]], 
    });

    this.updateForm = this._formBuilder.group({
      Id:[null],
      Name: [null],
      Email: [null],
      Phone: [null],
      Salary: [null],
      Department: [null],
    });
  }
  
  ngOnInit(): void {
    this.getAllEmployees();
    this.getIP();
  }
  
  
  getIP(){
    this.ipService.getIPAddress()
    .subscribe((res:any)=>{  
      this.ipAddress=res.ip;  
    });
  }
  
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
   
  /* ----------==========     OnSelected User    ==========---------- */
  
  onSelected(row: any) {
    this.selectedEmployee = row; 
    console.log(this.selectedEmployee);
  }

  /* ----------==========     Get All Employees    ==========---------- */
  getAllEmployees(){
    this.apiService.getAllEmployee()
    .subscribe({
      next: (data: any) => {
        this.employee = data.data;
        console.log(this.employee);
        this.dataSource = new MatTableDataSource<Employee>(this.employee)
        this.dataSource.paginator = this.paginator;

      }, 
      error : (response) =>{ 
        console.log(response);
        this.notificationService.error(response.error.message); 
      }
    });
  //   .subscribe((data:any) =>{
  //     this.employee = data.data;
  //     console.log(this.employee);
  //     this.dataSource = new MatTableDataSource<Employee>(this.employee)
  //     this.dataSource.paginator = this.paginator;

  //  })
  }

  
  /* ----------==========     Add new Employee    ==========---------- */
  createEmployee(){
    if (this.employeeForm.invalid) {
      return this.notificationService.error("Kindly fill all fields");
    }
    let model = {
 
        "id": this.employeeForm.value.id,
        "name": this.employeeForm.value.name,
        "email": this.employeeForm.value.email,
        "salary": this.employeeForm.value.salary,
        "phone": this.employeeForm.value.phone,
        "department": this.employeeForm.value.department, 
        "ipAddress": this.ipAddress
         
      }
       
      this.apiService.createEmployee(model) 
      .subscribe({
        next: (data: any) => {
          console.log(data);
          this.employeeForm.reset();
          this.closeModal();
          window.location.reload();
          this.notificationService.success("Employee created successfuly");

        }, 
        error : (response) =>{ 
          console.log(response);
          this.notificationService.error(response.error.message); 
        }
      });  
      
    }
    
    
    
    /* ----------==========     Update Employee    ==========---------- */
    updateEmployee(){
      this.Id = this.selectedEmployee.id;
      let model ={
        name : this.selectedEmployee.name,
        email: this.selectedEmployee.email,
        salary: this.selectedEmployee.salary,
        department: this.selectedEmployee.department,
        phone: this.selectedEmployee.phone,
        ipAddress: this.ipAddress

      }
      this.apiService.updateEmployee(this.Id, model)
      .subscribe({
        next: (data: any) => {
          this.closeModal();
          window.location.reload();
          this.notificationService.success("Employee Information updated successfuly");


        }, 
        error : (response) =>{ 
          console.log(response);
          this.notificationService.error(response.error.message); 
        }
      }); 
  }

    /* ----------==========     Delete Employee    ==========---------- */
    deleteEmployee(){
      this.Id = this.selectedEmployee.id;
      console.log(this.Id);
      this.apiService.deleteEmployee(this.Id, this.ipAddress)
      .subscribe({
        next: (data: any) => {
          this.notificationService.success("Employee deleted successfuly");
          this.closeModal();
          window.location.reload();
        }, 
        error : (response) =>{ 
          console.log(response);
          this.notificationService.error(response.error.message);  
          //window.location.reload();
        }
      }); 
  }

    openUpdateModal(){
      this.dialog.open(this.updateModal,{
        minWidth:'500px',
        minHeight:'300px'

      })
    }

    openNewModal(){
      this.dialog.open(this.newModal,{
        minWidth:'500px',
        minHeight:'350px', 


      })
    }

    openDeleteModal(){
      this.dialog.open(this.deleteModal,{
        minWidth:'200px',
        minHeight:'150px'

      })
    }

    closeModal(){
      this.dialog.closeAll()
    }

}
