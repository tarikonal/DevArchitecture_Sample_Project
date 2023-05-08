import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Game } from './models/Game';
import { GameService } from './services/game.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-game',
	templateUrl: './game.component.html',
	styleUrls: ['./game.component.scss']
})
export class GameComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name','description','price', 'update','delete'];

	gameList:Game[];
	game:Game=new Game();

	gameAddForm: FormGroup;


	gameId:number;

	constructor(private gameService:GameService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getGameList();
		this.getGameListDto();
    }

	ngOnInit() {

		this.createGameAddForm();
	}


	getGameListDto() {
		this.gameService.getGameListDto().subscribe(data => {
			this.gameList = data;
			this.dataSource = new MatTableDataSource(data);
			this.configDataTable();
		});
	}

	getGameList() {
		this.gameService.getGameList().subscribe(data => {
			this.gameList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.gameAddForm.valid) {
			this.game = Object.assign({}, this.gameAddForm.value)

			if (this.game.id == 0)
				this.addGame();
			else
				this.updateGame();
		}

	}

	addGame(){

		this.gameService.addGame(this.game).subscribe(data => {
			this.getGameList();
			this.game = new Game();
			jQuery('#game').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.gameAddForm);

		})

	}

	updateGame(){

		this.gameService.updateGame(this.game).subscribe(data => {

			var index=this.gameList.findIndex(x=>x.id==this.game.id);
			this.gameList[index]=this.game;
			this.dataSource = new MatTableDataSource(this.gameList);
            this.configDataTable();
			this.game = new Game();
			jQuery('#game').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.gameAddForm);

		})

	}

	createGameAddForm() {
		this.gameAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required],
description : ["", Validators.required],
price : [0, Validators.required]
		})
	}

	deleteGame(gameId:number){
		this.gameService.deleteGame(gameId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.gameList=this.gameList.filter(x=> x.id!=gameId);
			this.dataSource = new MatTableDataSource(this.gameList);
			this.configDataTable();
		})
	}

	getGameById(gameId:number){
		this.clearFormGroup(this.gameAddForm);
		this.gameService.getGameById(gameId).subscribe(data=>{
			this.game=data;
			this.gameAddForm.patchValue(data);
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
