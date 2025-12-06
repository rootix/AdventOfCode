import { getInput, execute, sum, multiply, splitLinesIntoArray } from '../utils';

const part1 = (): number => {
    const sheet = splitLinesIntoArray(getInput()).map((line) => line.split(/\s+/).filter((part) => part !== ''));
    const operands = sheet[sheet.length - 1];
    const calculationResults: number[] = [];

    for (let i = 0; i < operands.length; i++) {
        const numbers: number[] = [];
        for (let j = 0; j < sheet.length - 1; j++) {
            numbers.push(Number(sheet[j][i]));
        }

        calculationResults.push(executeOperation(operands[i], numbers));
    }

    return sum(calculationResults);
};

const part2 = (): number => {
    const rows = splitLinesIntoArray(getInput());
    const operandRow = rows[rows.length - 1];
    const calculationResults: number[] = [];

    let nums: number[] = [];
    for (let i = operandRow.length - 1; i >= 0; i--) {
        let numAsString = '';
        for (let j = 0; j < rows.length - 1; j++) {
            if (rows[j][i] !== ' ') {
                numAsString += rows[j][i];
            }
        }

        nums.push(Number(numAsString));
        if (operandRow[i] !== ' ') {
            calculationResults.push(executeOperation(operandRow[i], nums));
            nums = [];
            i--; // skip empty col on i-1
        }
    }

    return sum(calculationResults);
};

const executeOperation = (operand: string, numbers: number[]): number => {
    if (operand === '+') {
        return sum(numbers);
    } else {
        return multiply(numbers);
    }
};

execute([part1, part2]);
