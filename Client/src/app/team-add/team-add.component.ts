import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Team, TeamAdd, TeamMember } from '../_models/team';
import { TeamService } from '../_services/team.service';
import { UserService } from '../_services/user.service';
import { User } from '../_models/user';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-team-add',
  templateUrl: './team-add.component.html',
  styleUrl: './team-add.component.css'
})
export class TeamAddComponent implements OnInit{
  @Output() addItemEvent = new EventEmitter<Team>();

  //////

  teamAdd: TeamAdd = {} as TeamAdd;
  teamAddFormInitialised: boolean = false;

  teamAddForm: FormGroup = new FormGroup({});
  teamAddFormErrors: string[] = [];

  //////

  teamName: string = "";
  teamId: number = 0;

  existingTeamMembers: TeamMember[] = [];
  existingUsers: User[] = [];
  existingUsersLookup: User[] = [];
  usersLookup: User[] = [];

  usersToAdd: User[] = [];
  IdsToDel: number[] = []; //del

  constructor(private userService: UserService, private teamService: TeamService, private toastrService: ToastrService,
    private activatedRoute: ActivatedRoute, private router: Router) {   
  }

  ngOnInit(): void {
    this.activatedRoute.queryParams.subscribe(params => {
      //parse from string to number
      let queryId = +params['id'] || 0;
      this.teamId = queryId; //if not 0, edit mode
      if (queryId > 0) {
        this.advancedMode = true;
        this.teamService.getTeam(queryId).subscribe({
          next: teamResponse => {
            this.teamName = teamResponse.teamName;
            this.teamAdd.teamName = teamResponse.teamName;
            if (teamResponse.teamMemberDtos) {
              this.existingTeamMembers = teamResponse.teamMemberDtos;
            }

            this.userService.getUsers().subscribe({
              next: response => {
                // those who are in the team, go to existingUsers, others go to usersLookup
                response.forEach(user => {
                  if (this.existingTeamMembers.find(x => x.userId == user.id)) {
                    this.existingUsers.push(user);
                  } else {
                    this.usersLookup.push(user);
                  }
                });

                this.InitTeamAddForm();
              },
              error: err => {
                console.log(err);
                this.toastrService.error("Takım getirilirken hata oluştu.");
              }
            });
          },
          error: err => {
            console.log(err);
            this.toastrService.error("Takım getirilirken hata oluştu.");
          }
        });
      } else {
        this.userService.getUsers().subscribe({
          next: response => {
            this.usersLookup = response;
            this.InitTeamAddForm();
          },
          error: err => {
            console.log(err);
            this.toastrService.error("Kullanıcılar getirilirken hata oluştu.");
          }
        });
      }
    })
  }

  onSubmit() {
    // this.trySubmitTeamAddForm();
  }

  resetTeamAddForm() {
    if (this.teamId > 0) {
      this.teamAddForm.reset(this.teamAdd);
    } else {
      this.teamAddForm.reset();
    }
  }

  trySubmitTeamAddForm() {

    this.teamAddFormErrors = [];
    if (!this.teamAddForm.valid) {
      this.teamAddFormErrors.push("Zorunlu alanları doldurunuz.");
      return;
    }

    const values = {...this.teamAddForm.value};

    this.teamAdd.teamName = values.teamName;

    this.teamAdd.teamMemberDtos = [];

    this.usersToAdd.forEach(user => {
      this.teamAdd.teamMemberDtos.push({teamId: this.teamId, userId: user.id});
    });

    if (this.teamId == 0) {
      this.teamService.addTeam(this.teamAdd).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Takım başarıyla eklendi.");
            this.addItemEvent.emit(response);
            this.router.navigate(['/team/edit'], { queryParams: { id: response.id } });
          } else {
            this.toastrService.error("Takım eklenirken hata oluştu.");
          }
          this.resetTeamAddForm();
        },
        error: err => {
          console.log(err);
          this.toastrService.error("Takım eklenirken hata oluştu.");
        }
      });
    } else {
      var teamMemberIdsToDel: number[] = [];

      //if, userid of a member of TeamMember, is in IdsToDel, add the member id to TeamMemberIdsToDel
      this.existingTeamMembers.forEach(teamMember => {
        if (this.IdsToDel.find(x => x == teamMember.userId)) {
          teamMemberIdsToDel.push(teamMember.id);
        }
      });

      this.teamService.updateTeam(this.teamAdd, this.teamId, teamMemberIdsToDel).subscribe({
        next: response => {
          if (response) {
            this.toastrService.success("Takım başarıyla güncellendi.");
            this.router.navigate(['/team/edit'], { queryParams: { id: response.id } });
          } else {
            this.toastrService.error("Takım güncellenirken hata oluştu.");
          }
          this.resetTeamAddForm();
        },
        error: err => {
          console.log(err);
        }
      });
    }
  }

  InitTeamAddForm() {
    this.teamAddForm = new FormGroup({
      teamName: new FormControl(this.teamAdd.teamName, [Validators.required]),
    });

    this.teamAddForm.valueChanges.subscribe(() => {
      this.teamAddFormErrors = [];
    });

    this.teamAddFormInitialised = true;
  }

  removeTeamMemberFromExisting(user: User) {
    this.existingUsers = this.existingUsers.filter(x => x.id != user.id);
    this.existingUsersLookup.push(user);
    this.IdsToDel.push(user.id);
  }

  addTeamMemberFromExistingLookup(user: User) {
    this.IdsToDel = this.IdsToDel.filter(x => x != user.id);
    this.existingUsersLookup = this.existingUsersLookup.filter(x => x.id != user.id);
    this.existingUsers.push(user);
  }

  removeTeamMemberFromAdd(user: User) {
    this.usersToAdd = this.usersToAdd.filter(x => x.id != user.id);
    this.usersLookup.push(user);
  }

  addTeamMemberFromLookup(user: User) {
    // if id is in IdsToDel, remove it from IdsToDel
    this.IdsToDel = this.IdsToDel.filter(x => x != user.id);
    this.usersToAdd.push(user);
    this.usersLookup = this.usersLookup.filter(x => x.id != user.id);
  }
  advancedMode: boolean = false;
  legacy: boolean = false;
}
