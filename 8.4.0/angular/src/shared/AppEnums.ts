import { TenantAvailabilityState } from '@shared/service-proxies/service-proxies';


export class AppTenantAvailabilityState {
    static Available: number = TenantAvailabilityState._1;
    static InActive: number = TenantAvailabilityState._2;
    static NotFound: number = TenantAvailabilityState._3;
}

export enum FILE_TYPE_ENUMS {
    JPG = 0,
    JPGE = 1,
    GIF = 2,
    PNG = 3,
    JPE = 4,
    TIF = 5,
}
export interface FileOption {
    value: string;
    name: string;
}

export enum JOB_TYPE_ENUMS {
    None = 0,
    Teacher = 1,
    Student = 2,
    Developer = 3,
}
export interface JobTypeOption {
    value: string;
    name: string;
}
