import { EducationLevel } from "./education-level";
import { SaveEducation } from "../constant/api.const";
import { employee } from "./employee";
import { City } from "./city";
import { State } from "./state";
import { Country } from "./country";

export interface Education{
    id:string;
    educationLevels:string;
    educationLevelName:string;
    subject:string;
    institute:string;
    city:string;
    state:string;
    country:string;
    completionDate:Date;
    certificate:string;
    anabin:string|null;
    employee:string;
    educationlevelDetails:EducationLevel|null,
    cityDetails:City|null;
    stateDetails:State|null;
    countryDetails:Country|null;
    employeeDetails:employee|null;
    filename?:any;
}

export interface SaveEducation{
    id:string;
    educationLevels:string;
    educationLevelName:string;
    subject:string;
    institute:string;
    city:string;
    state:string;
    country:string;
    completionDate:Date;
    certificate:string;
    anabin:string|null;
    employee:string;
    educationlevelDetails:EducationLevel|null,
    cityDetails:City|null;
    stateDetails:State|null;
    countryDetails:Country|null;
    employeeDetails:employee|null;
    action:number;
     filename?:any;
}