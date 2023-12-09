import {execute, getInput, mod, move, splitLinesIntoArray, sum} from "../utils";

interface Num {
    value: number
}

const mix = (file: string, times = 1, multiplier = 1) => {
    const initial = splitLinesIntoArray(file).map(n => <Num>{value: Number(n) * multiplier});
    const mixed = [...initial];
    for (let _ = 0; _ < times; _++) {
        for (const num of initial) {
            const mixedIndex = mixed.indexOf(num);
            let newMixedIndex = mod(mixedIndex + num.value, mixed.length - 1);
            newMixedIndex = newMixedIndex === 0 ? mixed.length - 1 : newMixedIndex;
            move(mixedIndex, newMixedIndex, mixed);
        }
    }

    return mixed;
};

const getGroveCoordinates = (mixed: Num[]) => {
    const zeroIndex = mixed.findIndex(n => n.value === 0);
    return sum([1000, 2000, 3000].map(number => mixed[mod((zeroIndex + number), mixed.length)].value));
};

const part1 = (): number => {
    const mixed = mix(getInput());
    return getGroveCoordinates(mixed);
}

const part2 = (): number => {
    const mixed = mix(getInput(), 10, 811589153);
    return getGroveCoordinates(mixed);
}

execute([part1, part2]);