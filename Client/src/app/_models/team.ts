export interface Team {
    id: number;
    rowGuiid: string;
    teamName: string;
    teamMemberDtos: TeamMember[];
}

export interface TeamAdd {
    teamName: string;
    teamMemberDtos: TeamMemberAdd[];

}

export interface TeamMember {
    id: number;
    teamId: number;
    userId: number;

    name: string; //displaying

    value: boolean; // for checkboxes in ticket-add
}

export interface TeamMemberAdd {
    teamId: number;
    userId: number;
}