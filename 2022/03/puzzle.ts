import {execute, getInput, intersect, splitLinesIntoArray, sum} from "../utils";

const calculatePriority = (char: string) => {
    if (char >= 'A' && char <= 'Z') {
        return char.charCodeAt(0) - 38;
    } else if (char >= 'a' && char <= 'z') {
        return char.charCodeAt(0) - 96;
    }

    return 0;
};

const findPriority = (first: string, second: string) => {
    for (const char of first) {
        if (second.indexOf(char) != -1) {
            return calculatePriority(char);
        }
    }

    return 0;
};

const part1 = (): number => {
    return sum(splitLinesIntoArray(getInput())
        .map(line => [line.substring(0, line.length / 2), line.substring(line.length / 2)])
        .map(compartments => findPriority(compartments[0], compartments[1])));
}

const part2 = (): number => {
    let result = 0;
    const backpacks = splitLinesIntoArray(getInput());
    for (let offset = 0; offset < backpacks.length; offset += 3) {
        result += calculatePriority(intersect(intersect(backpacks[offset].split(''), backpacks[offset + 1].split('')), backpacks[offset + 2].split(''))[0]);
    }

    return result;
}

execute([part1, part2]);