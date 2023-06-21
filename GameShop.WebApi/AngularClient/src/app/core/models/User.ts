import { BaseModel } from "./BaseModel";

export interface User extends BaseModel {

    NickName?: string;

    Password?: string;

    Role?: string;

    BanOption?: string;
}
