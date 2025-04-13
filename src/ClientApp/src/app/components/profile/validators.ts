import { AbstractControl, FormGroup } from '@angular/forms';

export function minimumAge(minAge: number) {
  return (control: AbstractControl) => {
    const birthDate = new Date(control.value);
    const today = new Date();
    const age = today.getFullYear() - birthDate.getFullYear();

    if (age < minAge) {
      return { minimumAge: true };
    }
    return null;
  };
}

export function rangeValidator(min: number, max: number) {
  return (control: AbstractControl) => {

    if (control.value < min || control.value > max) {
      return { range: true };
    }
    return null;
  };
}

export function minValue(min: number) {
  return (control: AbstractControl) => {

    if (control.value < min) {
      return { minValue: true };
    }
    return null;
  };
}

export function ageFromLessThanOrEqualAgeTo(ageFromKey: string, ageToKey: string) {
  return (formGroup: FormGroup) => {
    const ageFrom = formGroup.controls[ageFromKey];
    const ageTo = formGroup.controls[ageToKey];

    if (ageFrom.value > ageTo.value) {
      ageTo.setErrors({ ageFromLessThanAgeTo: true });
    } else {
      ageTo.setErrors(null);
    }
  };
}

export function getErrorMessage(controlName: string, form: FormGroup): string {
  const control = form.get(controlName);
  if (control && control.errors) {
    if (control.errors['required']) {
      return 'Поле обязательно для заполнения';
    } else if (control.errors['minlength']) {
      return `Минимальное количество символов ${control.errors['minlength'].requiredLength}`;
    } else if (control.errors['maxlength']) {
      return `Максимальное количество символов ${control.errors['maxlength'].requiredLength}`;
    } else if (control.errors['minimumAge']) {
      return `Минимальный возраст 16`;
    } else if (control.errors['range']) {
      return 'Значения вне диапазона';
    } else if (control.errors['minValue']) {
      return 'Значение должно быть больше 0';
    } else if (control.errors['ageFromLessThanAgeTo']) {
      return 'Возраст от должен быть меньше возраста до';
    }
  }
  return '';
}
