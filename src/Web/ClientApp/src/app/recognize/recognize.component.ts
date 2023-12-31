import { Component, OnInit } from '@angular/core';
import { UsersService } from '../services/users.service';
import { Observable, OperatorFunction, debounceTime, distinctUntilChanged, map } from 'rxjs';
import { KudosService } from '../services/kudos.service';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ToastrService } from 'ngx-toastr';
import { User } from '../interfaces/user';
import { KudoDto } from '../interfaces/kudo';

@Component({
  selector: 'app-recognize',
  templateUrl: './recognize.component.html',
  styleUrls: ['./recognize.component.css']
})
export class RecognizeComponent implements OnInit {
  allUsers: User[] = [];
  receiver: string = '';
  title: string = '';
  message: string = '';

  //The various tags that can be given
  teamPlayer: boolean = false;
  oneOfAKind: boolean = false;
  creative: boolean = false;
  highEnergy: boolean = false;
  awesome: boolean = false;
  achiever: boolean = false;
  sweetness: boolean = false;

  senderId: string = '';
  anonymousSender: boolean = false;

  constructor(private usersService: UsersService, 
              private kudosService: KudosService,
              private router: Router,
              private jwtHelper: JwtHelperService,
              private toastr: ToastrService) 
  {
    this.jwtHelper = new JwtHelperService();
  }

  ngOnInit(): void {
    const token: any = localStorage.getItem('token');
    const decodedToken = this.jwtHelper.decodeToken(token);
    this.senderId = decodedToken.id;

    this.usersService.getAllUsers().subscribe({
      next: users => {
        this.allUsers = users;
      },
      error: error => {
        this.toastr.error("Error while retreiving users, please log out and try again.");
      }
    });
  }

  changeStatus(event: any, tag: number) {
    switch(tag) {
      case 1:
        this.teamPlayer = event.target.checked;
        break;

      case 2:
        this.oneOfAKind = event.target.checked;
        break;

      case 3:
        this.creative = event.target.checked;
        break;

      case 4:
        this.highEnergy = event.target.checked;
        break;

      case 5:
        this.awesome = event.target.checked;
        break;

      case 6:
        this.achiever = event.target.checked;
        break;

      case 7:
        this.sweetness = event.target.checked;
        break;
    }
  }

  activateButton(): boolean {
    return !!this.receiver && !!this.title && !!this.message && !! this.allUsers.map((user) => {if (user.id !== this.senderId) {return user.name} return ""}).find(x => x === this.receiver);
  }

  autofillNames: OperatorFunction<string, readonly string[]> = (text$: Observable<string>) =>
    text$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      map((term) =>
        term.length < 2 ? [] : this.allUsers.map((user) => {if (user.id !== this.senderId) {return user.name} return ""}).filter((name) => name.toLowerCase().indexOf(term.toLowerCase()) > -1).slice(0, 10),
      ),
    );

  addKudo(): void {
    let receiverId: any = this.allUsers.find(user => user.name === this.receiver)?.id;
    let newKudo: KudoDto = {
      senderId: this.anonymousSender ? "" : this.senderId,
      receiverId: receiverId,
      title: this.title,
      message: this.message,
      teamPlayer: this.teamPlayer,
      oneOfAKind: this.oneOfAKind,
      creative: this.creative,
      highEnergy: this.highEnergy,
      awesome: this.awesome,
      achiever: this.achiever,
      sweetness: this.sweetness
    };

    this.kudosService.addKudo(newKudo).subscribe({
      next: addedKudo => {
        this.router.navigate(["/home"]);
      },
      error: error => {
        this.toastr.error(error.error, "Error while adding kudo");
      }
    })
  }

  setAnonymousSender(): void {
    this.anonymousSender = !this.anonymousSender;
  }
}
