import { getInput, execute, mod } from '../utils';

const parseInput = (input: string) => {
    return input.split(',').map((range) => {
        const parts = range.split('-');
        return { start: Number(parts[0]), end: Number(parts[1]) };
    });
};

const isRepeatingNumber = (num: number) => {
    const numStr = num.toString();
    const half = numStr.length / 2;
    return numStr.slice(0, half) === numStr.slice(half);
};

const isRepeatingSequence = (num: number) => {
    const numStr = num.toString();
    const numLen = numStr.length;

    for (let seqLength = 1; seqLength <= Math.floor(numStr.length / 2); seqLength++) {
        if (mod(numLen, seqLength) !== 0) {
            continue;
        }

        const sequence = numStr.slice(0, seqLength);
        const fullSeq = sequence.repeat(numLen / seqLength);
        if (fullSeq === numStr) {
            return true;
        }
    }

    return false;
};

const sumRelevantNumbers = (isRelevantNumberFunc: (num: number) => boolean) => {
    const ranges = parseInput(getInput());
    let count = 0;
    for (const range of ranges) {
        for (let i = range.start; i <= range.end; i++) {
            if (isRelevantNumberFunc(i)) {
                count = count + i;
            }
        }
    }

    return count;
};

const part1 = (): number => {
    return sumRelevantNumbers(isRepeatingNumber);
};

const part2 = (): number => {
    return sumRelevantNumbers(isRepeatingSequence);
};

execute([part1, part2]);
