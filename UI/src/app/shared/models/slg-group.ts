// slg-group.model.ts

export interface SLGGroup {
    id: string;
    initialStatus: string;
    statusName: string;
    statusSequence: string;
    relevantExperience: string;
    action:number;
  }


export interface SaveSLGGroup {
  id: string;
  initialStatus: string;
  statusName: string;
  statusSequence: string;
  relevantExperience: number;
  action:number;
}
