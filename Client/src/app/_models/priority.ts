export interface Priority {
    id: number;
    rowGuiid: string;
    priorityName: string;
    timeType: TimeType;
    time: number;
    listIndex: number;
}

export enum TimeType {
    Minutes = 1,
    Hours = 2,
    Days = 3
}

export interface PriorityAdd {
    priorityName: string;
    timeType: TimeType;
    time: number;
    listIndex: number;
}

export const TimeTypeList = [
    {value: 1, name: "Dakika"},
    {value: 2, name: "Saat"},
    {value: 3, name: "Gün"}
  ];