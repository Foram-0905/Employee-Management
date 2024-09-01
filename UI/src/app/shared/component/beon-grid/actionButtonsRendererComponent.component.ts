import { Component, EventEmitter, Output, ChangeDetectorRef } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'action-buttons-renderer',
  template: `
    <button type="button"  class="btn btn-outline-primary border-0" *ngIf="params?.showDownloadButton" (click)="onDownloadClick($event)"><i class="bi bi-download"></i></button>
    <button type="button"  class="btn btn-outline-secondary border-0" *ngIf="params?.showEditButton" (click)="onEditClick($event)"><i class="bi bi-pencil"></i></button>
    <button type="button" class="btn btn-outline-danger border-0"  *ngIf="params?.showDeleteButton" (click)="onDeleteClick($event)"><i class="bi bi-trash"></i></button>
    <button type="button" class="btn border-0"  *ngIf="params?.showApproveButton" (click)="onApproveClick($event)"><img src="../../../../assets/image/approve.png"></button>
    <button type="button" class="btn border-0"  *ngIf="params?.showRejectButton" (click)="onRejectClick($event)"><img src="../../../../assets/image/reject.png"></button>
    <!-- <button type="button" class="btn btn-outline-secondary border-0"  *ngIf="params?.showCheckboxButton" (click)="onCheckboxClick($event)"><i class="bi bi-plus-lg"></i></button> -->
    <!-- <input type="checkbox" id="customCheckbox" class="custom-checkbox" *ngIf="params.showCheckboxButton" [checked]="isChecked" (change)="onCheckboxClick($event)"> -->
  `,
  styleUrls: ['beon-grid.component.scss']

})
export class ActionButtonsRendererComponent implements ICellRendererAngularComp {
  constructor(private cdRef: ChangeDetectorRef) { }
  params: any;
  isChecked: boolean = false;
  // label: any;

  @Output() onEdit = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<any>();
  @Output() onDownload = new EventEmitter<any>();
  @Output() onCheckboxChange = new EventEmitter<any>();
  agInit(params: any) {
    this.params = params;
    // this.label = this.params.label || null;
  }

  showEdit(): boolean {
    return this.params.showEditButton;
  }
  showDownload(): boolean {
    return this.params.showDownloadButton;
  }
  showDelete(): boolean {
    return this.params.showDeleteButton;
  }

  showCheckbox(): boolean {
    return this.params.showCheckboxButton;
  }

  showReject():boolean{
    return this.params.showRejectButton;
  }
  showAppove():boolean{
    return this.params.showAppoveButton;
  }
  refresh(params?: any): boolean {
    this.cdRef.detectChanges();
    this.isChecked = false;
    return true;
  }
  onCheckboxClick(event: any) {
    this.isChecked = !this.isChecked;
    this.onCheckboxChange.emit({ checked: event.target.checked, data: this.params.node.data });
  }
  onRowClick(event: any) {
    this.isChecked = !this.isChecked;
    this.onCheckboxChange.emit({ checked: this.isChecked, data: this.params.node.data });
  }
  onEditClick($event: any) {
    if (this.params.onClick instanceof Function) {
      // put anything into params u want pass into parents component
      const params = {
        event: $event,
        rowData: this.params.node.data
        // ...something
      }
      this.params.onClick(this.params);
    }
  }
  onDownloadClick(event: any) {
    if (this.params.onClick instanceof Function) {
      // put anything into params u want pass into parents component
      const params = {
        event: event,
        rowData: this.params.node.data
        // ...something
      }
      this.params.onDownloadClick(this.params);
    }
  }

  onDeleteClick(event: any) {
    if (this.params.onClick instanceof Function) {
      // put anything into params u want pass into parents component
      const params = {
        event: event,
        rowData: this.params.node.data
        // ...something
      }
      this.params.onDeleteClick(this.params);
    }
  }
  onApproveClick(event: any) {

    if (this.params.onClick instanceof Function) {
      // put anything into params u want pass into parents component
      const params = {
        event: event,
        rowData: this.params.node.data
        // ...something
      }
      this.params.onApproveClick(this.params);
    }
  }
  onRejectClick(event: any) {

    if (this.params.onClick instanceof Function) {
      // put anything into params u want pass into parents component
      const params = {
        event: event,
        rowData: this.params.node.data
        // ...something
      }
      this.params.onRejectClick(this.params);
    }
  }



}
