import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Gamer } from './models/Gamer';
import { GamerService } from './services/Gamer.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-gamer',
	templateUrl: './gamer.component.html',
	styleUrls: ['./gamer.component.scss']
})
export class GamerComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','firstName','lastName','birthYear','identityNumber', 'update','delete'];

	gamerList:Gamer[];
	gamer:Gamer=new Gamer();

	gamerAddForm: FormGroup;


	gamerId:number;

	constructor(private gamerService:GamerService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getGamerList();
    }

	ngOnInit() {

		this.createGamerAddForm();
	}


	getGamerList() {
		this.gamerService.getGamerList().subscribe(data => {
			this.gamerList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.gamerAddForm.valid) {
			this.gamer = Object.assign({}, this.gamerAddForm.value)

			if (this.gamer.id == 0)
				this.addGamer();
			else
				this.updateGamer();
		}

	}

	addGamer(){

		this.gamerService.addGamer(this.gamer).subscribe(data => {
			this.getGamerList();
			this.gamer = new Gamer();
			jQuery('#gamer').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.gamerAddForm);

		})

	}

	updateGamer(){

		this.gamerService.updateGamer(this.gamer).subscribe(data => {

			var index=this.gamerList.findIndex(x=>x.id==this.gamer.id);
			this.gamerList[index]=this.gamer;
			this.dataSource = new MatTableDataSource(this.gamerList);
            this.configDataTable();
			this.gamer = new Gamer();
			jQuery('#gamer').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.gamerAddForm);

		})

	}

	createGamerAddForm() {
		this.gamerAddForm = this.formBuilder.group({		
			id : [0],
firstName : ["", Validators.required],
lastName : ["", Validators.required],
birthYear : [0, Validators.required],
identityNumber : [0, Validators.required]
		})
	}

	deleteGamer(gamerId:number){
		this.gamerService.deleteGamer(gamerId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.gamerList=this.gamerList.filter(x=> x.id!=gamerId);
			this.dataSource = new MatTableDataSource(this.gamerList);
			this.configDataTable();
		})
	}

	getGamerById(gamerId:number){
		this.clearFormGroup(this.gamerAddForm);
		this.gamerService.getGamerById(gamerId).subscribe(data=>{
			this.gamer=data;
			this.gamerAddForm.patchValue(data);
		})
	}


	clearFormGroup(group: FormGroup) {

		group.markAsUntouched();
		group.reset();

		Object.keys(group.controls).forEach(key => {
			group.get(key).setErrors(null);
			if (key == 'id')
				group.get(key).setValue(0);
		});
	}

	checkClaim(claim:string):boolean{
		return this.authService.claimGuard(claim)
	}

	configDataTable(): void {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

	applyFilter(event: Event) {
		const filterValue = (event.target as HTMLInputElement).value;
		this.dataSource.filter = filterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
	}

  }
