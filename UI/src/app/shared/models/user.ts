export interface user {
    id:string
    name: string;
    token: string | null;
    expiration: boolean;
    email:string;
    signInDate: Date;
    luanguge:string;
    roleId:string
    role:string
}

export interface tokenRefresh {
  token?: string |null;
  expiration?: boolean;
  email?:string;

}

