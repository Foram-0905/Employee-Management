import { employee } from "./employee";
import { SaveAssets } from "../constant/api.const";
import { Asset_Status } from "./assets_status";
import { Asset_type } from "./assets_type";

export interface ManageAssets {
    id: string;
    serialNumber: string;
    status: string;
    statusName:string|null;
    manufacturer: string;
    assetType: string;
    assetTypeName:string|null;
    model: string;
    moreDetails: string;
    purchaseDate: Date;
    warrantyDueDate: Date;
    warranty: string|null; 
    currentOwner: string|null;
    currentOwnerFullName:string|null;
    previousOwner: string|null;
    previousOwnerFullName:string|null;
    note: string;
    // currentOwnerDetails:employee|null;
    // previousOwnerDetails:employee|null;
    statusDetails:Asset_Status|null;
    assetTypeDetails:Asset_type|null;
  }
  export interface SaveAssets {
    id: string;
    serialNumber: string;
    status: string;
    statusName:string|null;
    manufacturer: string;
    assetType: string;
    assetTypeName:string|null;
    model: string;
    moreDetails: string;
    purchaseDate: Date;
    warrantyDueDate: Date;
    warranty: string|null; 
    currentOwner: string|null;
    currentOwnerFullName:string|null;
    previousOwner: string|null;
    previousOwnerFullName:string|null;
    note: string;
    // currentOwnerDetails:employee|null;
    // previousOwnerDetails:employee|null;
    statusDetails:Asset_Status|null;
    assetTypeDetails:Asset_type|null;
    action:number
  }
