import { getInput, execute, sum, multiply, splitLinesIntoArray } from '../utils';

const part1 = (): number => {
    const manifold = splitLinesIntoArray(getInput()).map((line) => line.split(''));
    let beamSplits = 0;
    for (let row = 1; row < manifold.length; row++) {
        const previousRow = manifold[row - 1];
        const currentRow = manifold[row];
        for (let col = 0; col < currentRow.length; col++) {
            if (currentRow[col] === '.') {
                if (previousRow[col] === 'S' || previousRow[col] === '|') {
                    currentRow[col] = '|';
                }
            } else if (currentRow[col] === '^' && previousRow[col] === '|') {
                currentRow[col - 1] = '|';
                currentRow[col + 1] = '|';
                beamSplits++;
            }
        }
    }

    return beamSplits;
};

const part2 = (): number => {
    const manifold: (string | number)[][] = splitLinesIntoArray(getInput()).map((line) => line.split(''));

    for (let row = 1; row < manifold.length; row++) {
        const previousRow = manifold[row - 1];
        const currentRow = manifold[row];

        for (let col = 0; col < currentRow.length; col++) {
            if (previousRow[col] === 'S' && currentRow[col] === '.') {
                currentRow[col] = 1; // First row creates first beam and is the only pass
            } else if (typeof previousRow[col] === 'number' && currentRow[col] === '.') {
                currentRow[col] = previousRow[col]; // Continue the beam downwards
            } else if (typeof previousRow[col] === 'number' && typeof currentRow[col] === 'number') {
                currentRow[col] = (currentRow[col] as number) + (previousRow[col] as number); // Merge beams
            } else if (currentRow[col] === '^' && typeof previousRow[col] === 'number') {
                // Handle left split
                if (currentRow[col - 1] === '.') {
                    currentRow[col - 1] = previousRow[col];
                } else if (typeof currentRow[col - 1] === 'number') {
                    currentRow[col - 1] = (currentRow[col - 1] as number) + (previousRow[col] as number);
                }
                // Right split always continues the beam downwards, since we go from left to right.
                if (currentRow[col + 1] === '.') {
                    currentRow[col + 1] = previousRow[col];
                }
            }
        }
    }

    // console.log('Manifold:');
    // for (const row of manifold) {
    //     console.log(row.map((cell) => cell.toString()).join(' '));
    // }

    return manifold[manifold.length - 1]
        .filter((cell) => typeof cell === 'number')
        .reduce((acc, val) => {
            return acc + val;
        }, 0);
};

execute([part1, part2]);
