

export interface Notification{
notificationID: string; // Guid in C# corresponds to string in TypeScript
employeeId: string; // Assuming EmployeeId is the foreign key referencing the user
notificationText: string;
notificationType: string;
isRead: boolean;
createdAt: Date;
}

