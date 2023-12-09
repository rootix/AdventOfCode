import {execute, getInput, splitLinesByEmptyLines, splitLinesIntoArray} from "../utils";

const buildStacks = (inventory: string): string[][] => {
    const lines = splitLinesIntoArray(inventory);
    const stacks = [];
    const crateNumberLine = lines[lines.length - 1];
    const stackIndex = [];
    for (let i = 0; i < crateNumberLine.length; i++) {
        if (crateNumberLine[i] !== " ") {
            stackIndex.push(i);
            stacks.push([]);
        }
    }

    for (let l = 0; l < lines.length - 1; l++) {
        const line = lines[l];
        for (let c = 0; c < stackIndex.length; c++) {
            if (line[stackIndex[c]] !== " ") {
                stacks[c].push(line[stackIndex[c]]);
            }
        }
    }

    return stacks;
};

const operateCrane = (model: "9000" | "9001", instructions: string, stacks: string[][]): string[][] => {
    const instructionRegex = /move (\d+) from (\d+) to (\d+)/
    const lines = splitLinesIntoArray(instructions);
    for (const instruction of lines) {
        const matches = instructionRegex.exec(instruction);
        const times = parseInt(matches[1]);
        const from = parseInt(matches[2]) - 1;
        const to = parseInt(matches[3]) - 1;

        for (let a = 0; a < times; a++) {
            const crate = stacks[from].shift();
            if (model === "9000") {
                stacks[to].unshift(crate)
            } else {
                stacks[to].splice(a, 0, crate)
            }
        }
    }

    return stacks;
};

const getTopCrateString = (stacks: string[][]) => stacks.map(stack => stack[0]).join('');

const part1 = (): string => {
    const [inventory, instructions] = splitLinesByEmptyLines(getInput());
    const stacks = buildStacks(inventory);
    operateCrane("9000", instructions, stacks);
    return getTopCrateString(stacks);
}

const part2 = (): string => {
    const [inventory, instructions] = splitLinesByEmptyLines(getInput());
    const stacks = buildStacks(inventory);
    operateCrane("9001", instructions, stacks);
    return getTopCrateString(stacks);
}

execute([part1, part2]);