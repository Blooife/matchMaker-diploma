import {AbstractControl, FormGroup, ValidationErrors, ValidatorFn} from '@angular/forms';

export function minimumAge(minAge: number) {
  return (control: AbstractControl) => {
    const birthDate = new Date(control.value);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
      age--;
    }

    if (age < minAge) {
      return { minimumAge: { requiredAge: minAge, actualAge: age } };
    }
    return null;
  };
}

export function rangeValidator(min: number, max: number) {
  return (control: AbstractControl) => {
    const value = control.value;
    if (value < min || value > max) {
      return { range: { min, max, actual: value } };
    }
    return null;
  };
}

export function minValue(min: number) {
  return (control: AbstractControl) => {
    if (control.value < min) {
      return { minValue: { requiredMin: min, actual: control.value } };
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

export function futureDateValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const inputDate = new Date(value);
    const now = new Date();

    if (isNaN(inputDate.getTime())) {
      return null;
    }

    if (inputDate <= now) {
      return { futureDate: true };
    }

    return null;
  };
}

export function passwordMatchValidator(passwordKey: string, confPasswordKey: string) {
  return (formGroup: FormGroup) => {
    const curPassword = formGroup.controls[passwordKey];
    const newPassword = formGroup.controls[confPasswordKey];
    if (curPassword.value != newPassword.value) {
      newPassword.setErrors({ mismatch: true });
    } else {
      newPassword.setErrors(null);
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
      return `Минимальный возраст ${control.errors['minimumAge'].requiredAge}`;
    } else if (control.errors['range']) {
      return `Значение должно быть от ${control.errors['range'].min} до ${control.errors['range'].max}`;
    } else if (control.errors['minValue']) {
      return `Значение должно быть не меньше ${control.errors['minValue'].requiredMin}`;
    } else if (control.errors['ageFromLessThanAgeTo']) {
      return 'Возраст от должен быть меньше возраста до';
    } else if (control.errors['futureDate']) {
      return 'Дата должна быть в будущем';
    }else if (control.errors['mismatch']) {
      return 'Пароли не совпадают';
    }
  }
  return '';
}

