import { Pipe, PipeTransform } from '@angular/core';

export enum ReportStatus {
  Pending = 0,
  Reviewed = 1,
  Rejected = 2,
  Blocked,
}
@Pipe({
  standalone: true,
  name: 'statusTranslate'
})
export class StatusTranslatePipe implements PipeTransform {
  transform(value: ReportStatus | number): string {
    switch (value) {
      case ReportStatus.Pending:
        return 'В обработке';
      case ReportStatus.Reviewed:
        return 'Обработана';
      case ReportStatus.Rejected:
        return 'Отклонена';
      case ReportStatus.Blocked:
        return 'Пользователь заблокирован';
      default:
        return '';
    }
  }
}
