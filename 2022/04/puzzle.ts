import {execute, getInput, splitLinesIntoArray, sum} from "../utils";

interface Range {
    from: number,
    to: number
}

const parseRange = (input: string): Range => {
    const parts = input.split('-');
    return {from: parseInt(parts[0]), to: parseInt(parts[1])};
};

const contains = (first: Range, second: Range): boolean => first.from <= second.from && first.to >= second.to ||
    second.from <= first.from && second.to >= first.to;

const overlaps = (first: Range, second: Range): boolean => second.to >= first.from && first.to >= second.from;

const part1 = (): number => {
    return sum(splitLinesIntoArray(getInput())
        .map(pairings => pairings.split(',').map(parseRange))
        .map(pairings => contains(pairings[0], pairings[1]) ? 1 : 0));
}

const part2 = (): number => {
    return sum(splitLinesIntoArray(getInput())
        .map(pairings => pairings.split(',').map(parseRange))
        .map(pairings => overlaps(pairings[0], pairings[1]) ? 1 : 0));
}

execute([part1, part2]);