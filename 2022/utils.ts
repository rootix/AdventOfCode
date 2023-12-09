// @ts-ignore
import { readFileSync } from 'fs';

export const isSampleInput = (): boolean => {
    // @ts-ignore
    return process.argv[2] === 'sample';
};

export const getInput = (fileName?: string): string => {
    if (!fileName) {
        fileName = isSampleInput() ? 'sample.txt' : 'input.txt';
    }

    return readFileSync(fileName).toString();
};

export const execute = (parts: (() => number|string)[]): void => {
    parts.forEach((part, i) => {
        const start = new Date();

        const result = part();

        const end = new Date();
        const duration = formatDuration(start, end);

        console.log(`Part ${i + 1}: ${result}, took ${duration}`);
    });
};

const formatDuration = (start: Date, end: Date): string => {
    const duration = end.valueOf() - start.valueOf(); // in milliseconds
    if (duration < 1000) {
        return `${duration}ms`;
    } else {
        return `${duration / 1000}s`;
    }
};

export const splitLinesByEmptyLines = (input: string) => input.split('\n\n');

export const splitLinesIntoArray = (input: string) => input.split('\n');

export const sum = (array: number[]) => {
    if (!array.length) {
        return 0;
    }

    return array.reduce((total, current) => total + current);
};

export const multiply = (array: number[]) => array.reduce((total, current) => total * current);

// At this exact moment i learned that % does not mean modulo in Javascript. Thanks Day 20!
export const mod = (n: number, m: number) => (n % m + m) % m;

export const sortAsc = (array: number[]) => array.sort((a, b) => b > a ? -1 : b < a ? 1 : 0);

export const sortDesc = (array: number[]) => array.sort((a, b) => a > b ? -1 : a < b ? 1 : 0);

export const intersect = <T>(first: T[], second: T[]): T[] => first.filter(val => second.includes(val));

export const move = <T>(fromIndex: number, toIndex: number, array: T[]) => {
    array.splice(toIndex, 0, array.splice(fromIndex, 1)[0]);
};