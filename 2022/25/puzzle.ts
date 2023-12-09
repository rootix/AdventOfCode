import {execute, getInput, splitLinesIntoArray, sum} from "../utils";

const SNAFU_TABLE = {
    '2': 2,
    '1': 1,
    '0': 0,
    '-': -1,
    '=': -2
}

const intToSnafu = (int: number) => {
    if (int === 0) return '';

    const nextDigit = int % 5;
    const snafuTable = Object.entries(SNAFU_TABLE);
    for (let i = 0; i < snafuTable.length; i++) {
        if ((snafuTable[i][1] + 5) % 5 === nextDigit) {
            const newDigit = Math.floor(int - snafuTable[i][1]) / 5;
            return intToSnafu(newDigit) + snafuTable[i][0];
        }
    }
};

const snafuToInt = (snafu: string) => {
    let int = 0;
    for (let i = 0; i < snafu.length; i++) {
        const snafuDigit = SNAFU_TABLE[snafu[i]];
        int += Math.pow(5, snafu.length - i - 1) * snafuDigit;
    }

    return int;
};

const part1 = (): number => {
    const fuelRequirement = sum(splitLinesIntoArray(getInput()).map(snafuToInt));
    return intToSnafu(fuelRequirement);
}

execute([part1]);