import {HttpContextToken} from "@angular/common/http";

export const _IGNORED_STATUSES = new HttpContextToken<boolean>(() => false);
