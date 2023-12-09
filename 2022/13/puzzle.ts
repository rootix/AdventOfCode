import {execute, getInput, splitLinesByEmptyLines, splitLinesIntoArray, sum} from "../utils";

const compare = (left: any, right: any): number => {
    if (Number.isInteger(left) && Number.isInteger(right)) {
        return left < right ? -1 : left > right ? 1 : 0;
    }

    if (Number.isInteger(left)) {
        left = [left];
    }

    if (Number.isInteger(right)) {
        right = [right];
    }

    for (let index = 0; index < left.length; index++) {
        if (index >= right.length) {
            return 1;
        } else {
            const result = compare(left[index], right[index]);
            if (result !== 0) {
                return result;
            }
        }
    }

    return left.length === right.length ? 0 : -1;
};

const part1 = (): number => {
    const pairs = splitLinesByEmptyLines(getInput()).map(pair => splitLinesIntoArray(pair).map(part => JSON.parse(part)));
    return sum(pairs.map(((pair, index) => compare(pair[0], pair[1]) === -1 ? index + 1 : 0)));
}

const part2 = (): number => {
    const packets = splitLinesByEmptyLines(getInput()).flatMap(pair => splitLinesIntoArray(pair).map(part => JSON.parse(part)));

    const firstDivider = [[2]];
    const secondDivider = [[6]];
    packets.push(firstDivider, secondDivider);

    const sortedPairs = packets.sort(compare);

    const indexOfFirstDivider = sortedPairs.indexOf(firstDivider) + 1;
    const indexOfSecondDivider = sortedPairs.indexOf(secondDivider) + 1;

    return indexOfFirstDivider * indexOfSecondDivider;
}

execute([part1, part2]);