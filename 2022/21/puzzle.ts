import {execute, getInput, splitLinesIntoArray} from "../utils";

const rootMonkey = "root";
const humnMonkey = "humn";

type Monkeys = Record<string, number | string[]>;

const calculate: Record<string, (a: number, b: number) => number> = {
    '+': (a, b) => a + b,
    '-': (a, b) => a - b,
    '*': (a, b) => a * b,
    '/': (a, b) => a / b
}

const parseInput = (input: string) => {
    const monkeys: Monkeys = {};
    splitLinesIntoArray(input).forEach(line => {
        const [monkey, rest] = line.split(': ');
        monkeys[monkey] = !isNaN(parseInt(rest, 10)) ? Number(rest) : rest.split(' ');
    })

    return monkeys;
};

const yell = (monkey: string, monkeys: Monkeys): number => {
    const value = monkeys[monkey];
    if (!Array.isArray(value)) {
        return value;
    }

    const [left, op, right] = value;
    return calculate[op](yell(left, monkeys), yell(right, monkeys));
};

const hasDescendent = (monkey: string, descendent: string, monkeys: Monkeys): boolean => {
    const value = monkeys[monkey];
    if (!Array.isArray(value)) {
        return false;
    }

    const [left, _, right] = value;
    if (left === descendent || right === descendent) {
        return true;
    }

    return hasDescendent(left, descendent, monkeys) || hasDescendent(right, descendent, monkeys);
};

const findHumnValue = (monkeys: Monkeys) => {
    const rootValue = monkeys[rootMonkey];
    if (typeof rootValue === 'number') {
        throw new Error("root already has a number assigned!");
    }

    const [rootLeft, _, rootRight] = rootValue;
    const humnIsLeft = hasDescendent(rootLeft, humnMonkey, monkeys);

    monkeys[humnMonkey] = 0;
    const firstYell = humnIsLeft ? yell(rootLeft, monkeys) : yell(rootRight, monkeys);
    monkeys[humnMonkey] = 1;
    const secondYell = humnIsLeft ? yell(rootLeft, monkeys) : yell(rootRight, monkeys);
    const yellProbeIncreased = secondYell > firstYell;

    const otherBranchYell = humnIsLeft ? yell(rootRight, monkeys) : yell(rootLeft, monkeys);
    const humnBranch = humnIsLeft ? rootLeft : rootRight;

    let min = 0;
    let max = Number.MAX_SAFE_INTEGER;
    while (min < max) {
        const humnValue = (min + max) / 2;
        monkeys[humnMonkey] = humnValue;

        const compare = yellProbeIncreased
            ? yell(humnBranch, monkeys) - otherBranchYell
            : otherBranchYell - yell(humnBranch, monkeys);

        if (compare === 0) {
            return humnValue;
        } else if (compare > 0) {
            max = humnValue;
        } else {
            min = humnValue;
        }
    }
};

const part1 = (): number => {
    const monkeys = parseInput(getInput());
    return yell(rootMonkey, monkeys);
}

const part2 = (): number => {
    const monkeys = parseInput(getInput());
    return findHumnValue(monkeys);
}

execute([part1, part2]);