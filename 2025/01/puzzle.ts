import { getInput, execute, splitLinesIntoArray, mod } from '../utils';

const dialSteps = 100;

const turns = (rotations: string[]) => {
    let dial = 50;

    return rotations.map((rotation) => {
        const value = Number(rotation.slice(1));
        const newAbsDialValue = dial + (rotation[0] === 'L' ? -1 : 1) * value;
        const zeros = Math.floor(Math.abs(newAbsDialValue) / dialSteps) + (newAbsDialValue <= 0 && dial > 0 ? 1 : 0);

        dial = mod(newAbsDialValue, dialSteps);

        return { dialValue: dial, zeros };
    });
};

const part1 = (): number => {
    const rotations = splitLinesIntoArray(getInput());
    return turns(rotations).reduce((acc, d) => acc + (d.dialValue === 0 ? 1 : 0), 0);
};
const part2 = (): number => {
    const rotations = splitLinesIntoArray(getInput());
    return turns(rotations).reduce((acc, d) => acc + d.zeros, 0);
};

execute([part1, part2]);