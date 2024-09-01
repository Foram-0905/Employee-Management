import { RequestWithFilterAndSort, filterModel } from './../models/FilterRequset';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { getNotificationByEmployeeId, getNotificationId, saveNotification, updateNotification } from '../constant/api.const';

const httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
};
@Injectable({
    providedIn: 'root'
})
export class NotificationService {
    closeNotification(notificationID: number) {
      throw new Error('Method not implemented.');
    }
    constructor(private http: HttpClient) {

    }

    sendLeaveNotification(employeeName: string, leaveDetails: any): Observable<any> {
        const notificationPayload = {
            recipient: 'lm@beon.net', // Replace with the actual HR manager's email
            subject: 'New Leave Request',
            message: `Employee ${employeeName} has requested leave. Details: ${JSON.stringify(leaveDetails)}`
        };
        return this.http.post(saveNotification, notificationPayload);
    }

    getNotificationByEmployeeId(Id: string) {
        return this.http.get<any>(`${getNotificationByEmployeeId}/${Id}`);
    }
    updateLeaveNotification(NotificationDetails: any): Observable<any> {
        return this.http.post(updateNotification, NotificationDetails);
      }
    getNotificationId(Id: string) {
        return this.http.get<any>(`${getNotificationId}/${Id}`);
    }

}