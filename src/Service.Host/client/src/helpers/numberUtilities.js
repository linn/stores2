import Decimal from 'decimal.js';

const numberOrZero = x => {
    if (!x || x.toString().trim() === '') {
        return 0;
    }
    return x;
};

export const add = (x, y) => new Decimal(numberOrZero(x)).add(new Decimal(numberOrZero(y)));
export const subtract = (x, y) => new Decimal(numberOrZero(x)).sub(new Decimal(numberOrZero(y)));
export const multiply = (x, y) =>
    new Decimal(numberOrZero(x))
        .mul(new Decimal(numberOrZero(y)))
        .toDecimalPlaces(2, Decimal.ROUND_HALF_UP);
export const divide = (x, y) =>
    new Decimal(numberOrZero(x))
        .div(new Decimal(numberOrZero(y)))
        .toDecimalPlaces(2, Decimal.ROUND_HALF_UP);

export const equals = (x, y) => new Decimal(numberOrZero(x)).equals(new Decimal(numberOrZero(y)));
export const lessThan = (x, y) =>
    new Decimal(numberOrZero(x)).lessThan(new Decimal(numberOrZero(y)));
export const lessThanOrEqualTo = (x, y) =>
    new Decimal(numberOrZero(x)).lessThanOrEqualTo(new Decimal(numberOrZero(y)));
export const greaterThan = (x, y) =>
    new Decimal(numberOrZero(x)).greaterThan(new Decimal(numberOrZero(y)));
export const greaterThanOrEqualTo = (x, y) =>
    new Decimal(numberOrZero(x)).greaterThanOrEqualTo(new Decimal(numberOrZero(y)));

export const currencyFormat = new Intl.NumberFormat('en-GB', {
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
});
