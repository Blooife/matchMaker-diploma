
import { Pipe, PipeTransform } from '@angular/core';

export enum Gender {
  Undefined = 0,
  Male = 1,
  Female = 2,
}
@Pipe({
  standalone: true,
  name: 'genderTranslate'
})
export class GenderTranslatePipe implements PipeTransform {
  transform(value: Gender | number): string {
    switch (value) {
      case Gender.Male:
        return 'Мужской';
      case Gender.Female:
        return 'Женский';
      case Gender.Undefined:
      default:
        return 'Не указан';
    }
  }
}
