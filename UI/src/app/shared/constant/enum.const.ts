export enum ActionEnum {
    "Insert"=1,
    "Update"=2
  }

  export enum filterConditionAndOrEnum {
    "AndCondition"=1,
    "OrCondition"=2
  }
  export enum StatusEnum {
    Available = "b0b9c254-3760-4089-8a6f-c6353c808aa8",
    InUse="15a21acf-a265-453c-8c3f-e13fcc65da4b",
    InRepair="d1f24060-4667-4512-b752-c273f0fc6ec4",
    Retired="f32e061c-5ca0-4449-95e4-6a5e7809b1ba"
    // Add other status IDs here if needed
  }
  export enum CreditDebit{
    "Credit" = '8c4c6742-833c-4f10-ad2a-17f682cc6d9b',
    "Debit" = '8c4c6742-833c-5f10-ad2a-17f682cc6d9b'
  }
  export const DefaultEmployee = {
    employee: localStorage.getItem('SelectedEmployeeForEdit')?.toLowerCase().replace(/"/g, "") || '',
  };

  export enum LeaveTypeEmployee{
    Quotabased="3fa85f64-5717-4562-b3fc-2c963f66af29",
    Unlimited="3fa85f64-5717-4562-b3fc-2c963f66af30"
  }
  export enum EmployementType{
    Consultant = "2fa85f64-5717-4562-b3fc-2c963f66afa1",
    Corporate = "2fa85f64-5717-4562-b3fc-2c963f66afa2"
  }