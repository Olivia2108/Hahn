
import {Component, ViewChild, OnInit, TemplateRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog} from '@angular/material/dialog';
import {MatPaginator} from '@angular/material/paginator';
import {MatTable, MatTableDataSource} from '@angular/material/table' 
import { Employee } from 'src/app/Models/Employee';
import { ApiService } from 'src/app/services/api.service';
import { NotificationService } from 'src/app/services/notification.service';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
    title = 'Employees';
    selectedEmployee: any;
    Id: number;
    OrderDate: string;

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
              private apiService: ApiService) {

                this.employeeForm = this._formBuilder.group({
      Name: ["", [Validators.required]],
      Email: ["", Validators.required],
      Phone: ["", Validators.required],
      Salary: ["", Validators.required],
      Department: ["", Validators.required], 
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
         
      }
      this.apiService.createEmployee(model).subscribe((data:any) =>{
        this.notificationService.success("Employee created successfuly");
        this.employeeForm.reset();
        this.closeModal();
        window.location.reload();
      })
    }
    
    /* ----------==========     Get All Employees    ==========---------- */
    getAllEmployees(){
      this.apiService.getAllEmployee().subscribe((data:any) =>{
        this.employee = data.item1;
        console.log(this.employee);
        this.dataSource = new MatTableDataSource<Employee>(this.employee)
        this.dataSource.paginator = this.paginator;
 
     })
  }

    
    /* ----------==========     Update Employee Order    ==========---------- */
    updateShippinOrders(){
      this.Id = this.selectedEmployee.Id;
      let model ={
        name : this.selectedEmployee.name,
        email: this.selectedEmployee.email,
        salary: this.selectedEmployee.salary,
        department: this.selectedEmployee.department,
        phone: this.selectedEmployee.phone

      }
      this.apiService.updateEmployee(this.Id, model).subscribe((data:any) =>{
        this.notificationService.success("Employee Information updated successfuly");
        this.closeModal();
     })
  }

    /* ----------==========     Delete Employee    ==========---------- */
    deleteEmployee(){
      this.Id = this.selectedEmployee.Id;
      console.log(this.Id);
      this.apiService.deleteEmployee(this.Id).subscribe((data:any) =>{
        this.notificationService.success("Employee deleted successfuly");
        this.closeModal();
        window.location.reload();
     }
    )
  }

    openUpdateModal(){
      this.dialog.open(this.updateModal,{
        minWidth:'500px',
        minHeight:'300px'

      })
    }

    openNewModal(){
      this.dialog.open(this.newModal,{
        minWidth:'800px',
        minHeight:'350px'

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
