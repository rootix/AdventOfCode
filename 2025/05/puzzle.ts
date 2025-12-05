import { getInput, execute, splitLinesByEmptyLines } from '../utils';

const part1 = (): number => {
    const parts = splitLinesByEmptyLines(getInput());
    const freshItemRanges = parts[0].split('\n').map(range => range.split('-').map(Number));

    let freshItemCount = 0;
    for (const item of parts[1].split('\n')) {
        const value = Number(item);
        for (const [min, max] of freshItemRanges) {
            if (value >= min && value <= max) {
                freshItemCount++;
                break;
            }
        }
    }

    return freshItemCount;
};

const part2 = (): number => {
    const parts = splitLinesByEmptyLines(getInput());
    const freshItemRanges = parts[0].split('\n').map(range => range.split('-').map(Number)).sort((a, b) => a[0] - b[0]);

    let rangePos = 1;
    while (rangePos < freshItemRanges.length) {
        const [currentMin, currentMax] = freshItemRanges[rangePos];
        const prevMax = freshItemRanges[rangePos - 1][1];

        if (currentMin <= prevMax) {
            freshItemRanges[rangePos - 1][1] = Math.max(prevMax, currentMax);
            freshItemRanges.splice(rangePos, 1);
        } else {
            rangePos++;
        }
    }

    return freshItemRanges.reduce((sum, [min, max]) => sum + (max - min + 1), 0);
};

execute([part1, part2]);
