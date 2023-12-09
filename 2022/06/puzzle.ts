import {execute, getInput} from "../utils";

const getMarkerPosition = (input: string, distinct: number) => {
    for (let i = distinct; i < input.length; i++) {
        if (new Set(input.slice(i - distinct, i)).size == distinct) {
            return i;
        }
    }

    return -1;
};

const part1 = (): number => {
    return getMarkerPosition(getInput(), 4);
}

const part2 = (): number => {
    return getMarkerPosition(getInput(), 14);
}

execute([part1, part2]);