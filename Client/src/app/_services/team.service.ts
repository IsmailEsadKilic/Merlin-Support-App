import { Injectable } from '@angular/core';
import { environment } from '../environment';
import { HttpClient } from '@angular/common/http';
import { Team, TeamAdd } from '../_models/team';

@Injectable({
  providedIn: 'root'
})
export class TeamService {
  baseUrl = environment.apiUrl + 'team';

  constructor(private http: HttpClient) { }

  getTeams() {
    return this.http.get<Team[]>(this.baseUrl);
  }

  getTeam(id: number) {
    return this.http.get<Team>(this.baseUrl + '/' + id);
  }

  addTeam(teamAdd: TeamAdd) {
    return this.http.put<Team>(this.baseUrl + '/add', teamAdd);
  }

  updateTeam(teamAdd: TeamAdd, teamId: number, teamMemberIdsToDel: number[]) {
    return this.http.post<Team>(this.baseUrl + '/update/' + teamId, {"teamDto": teamAdd, "teamMemberIdsToDel": teamMemberIdsToDel});
  }

  deleteTeam(teamId: number) {
    return this.http.delete(this.baseUrl + '/' + teamId);
  }
}
