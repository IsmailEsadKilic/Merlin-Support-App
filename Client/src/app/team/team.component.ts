import { Component, OnInit, ViewChild } from '@angular/core';
import { Team } from '../_models/team';
import { TeamService } from '../_services/team.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-team',
  templateUrl: './team.component.html',
  styleUrl: './team.component.css'
})
export class TeamComponent implements OnInit{
  @ViewChild('addModal') addModal: any;
  teams: Team[] = [];

  constructor(private teamService: TeamService, private toastrService: ToastrService) { }

  ngOnInit() {
    this.teamService.getTeams().subscribe(teams => {
      this.teams = teams;
    });
  }

  showAddModal() {
    this.addModal.show();
  }

  hideAddModal() {
    this.addModal.hide();
  }

  remove(team: Team) {
    if (!confirm('Bu takımı silmek istediğinize emin misiniz?')) {
      return;
    }
    this.teamService.deleteTeam(team.id).subscribe({
      next: () => {
        this.teams = this.teams.filter(c => c.id !== team.id);
      },
      error: (error) => {
        console.log(error);
        this.toastrService.error('Takım silinirken hata oluştu');
      }
    });
  }
  legacy: boolean = false;
}
