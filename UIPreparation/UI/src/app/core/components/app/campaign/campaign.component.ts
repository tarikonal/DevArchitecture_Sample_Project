import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Campaign } from './models/Campaign';
import { CampaignService } from './services/Campaign.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-campaign',
	templateUrl: './campaign.component.html',
	styleUrls: ['./campaign.component.scss']
})
export class CampaignComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name','startDate','endDate','discountRate','gameId', 'update','delete'];

	campaignList:Campaign[];
	campaign:Campaign=new Campaign();

	campaignAddForm: FormGroup;


	campaignId:number;

	constructor(private campaignService:CampaignService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getCampaignList();
    }

	ngOnInit() {

		this.createCampaignAddForm();
	}


	getCampaignList() {
		this.campaignService.getCampaignList().subscribe(data => {
			this.campaignList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.campaignAddForm.valid) {
			this.campaign = Object.assign({}, this.campaignAddForm.value)

			if (this.campaign.id == 0)
				this.addCampaign();
			else
				this.updateCampaign();
		}

	}

	addCampaign(){

		this.campaignService.addCampaign(this.campaign).subscribe(data => {
			this.getCampaignList();
			this.campaign = new Campaign();
			jQuery('#campaign').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.campaignAddForm);

		})

	}

	updateCampaign(){

		this.campaignService.updateCampaign(this.campaign).subscribe(data => {

			var index=this.campaignList.findIndex(x=>x.id==this.campaign.id);
			this.campaignList[index]=this.campaign;
			this.dataSource = new MatTableDataSource(this.campaignList);
            this.configDataTable();
			this.campaign = new Campaign();
			jQuery('#campaign').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.campaignAddForm);

		})

	}

	createCampaignAddForm() {
		this.campaignAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required],
startDate : [null, Validators.required],
endDate : [null, Validators.required],
discountRate : [0, Validators.required],
gameId : [0, Validators.required]
		})
	}

	deleteCampaign(campaignId:number){
		this.campaignService.deleteCampaign(campaignId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.campaignList=this.campaignList.filter(x=> x.id!=campaignId);
			this.dataSource = new MatTableDataSource(this.campaignList);
			this.configDataTable();
		})
	}

	getCampaignById(campaignId:number){
		this.clearFormGroup(this.campaignAddForm);
		this.campaignService.getCampaignById(campaignId).subscribe(data=>{
			this.campaign=data;
			this.campaignAddForm.patchValue(data);
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
