import { getInput, execute, splitLinesIntoArray } from '../utils';

const directions = [
    [0, 1], // east
    [1, 1], // southeast
    [1, 0], // south
    [1, -1], // southwest
    [0, -1], // west
    [-1, -1], // northwest
    [-1, 0], // north
    [-1, 1], // northeast
];

const parseDiagram = (): string[][] => {
    return splitLinesIntoArray(getInput()).map((line) => line.split(''));
};

const walkDiagram = (diagram: string[][], onAccessible?: (row: number, col: number) => void): number => {
    const maxRow = diagram.length;
    const maxCol = diagram[0].length;
    let accessableRolls = 0;

    for (let row = 0; row < maxRow; row++) {
        for (let col = 0; col < maxCol; col++) {
            const cell = diagram[row][col];
            if (cell === '.') {
                continue;
            }

            let adjacentRollCount = 0;
            for (const [x, y] of directions) {
                if (row + x < 0 || row + x >= maxRow || col + y < 0 || col + y >= maxCol) {
                    continue;
                }

                if (diagram[row + x][col + y] === '@') {
                    adjacentRollCount++;
                }
            }

            if (adjacentRollCount < 4) {
                accessableRolls++;
                if (onAccessible) {
                    onAccessible(row, col);
                }
            }
        }
    }

    return accessableRolls;
};

const part1 = (): number => {
    const diagram = parseDiagram();
    return walkDiagram(diagram);
};

const part2 = (): number => {
    const diagram = parseDiagram();
    let allRemovedRolls = 0;

    while (true) {
        const removedRolls = walkDiagram(diagram, (row, col) => (diagram[row][col] = '.'));
        if (removedRolls === 0) {
            break;
        }

        allRemovedRolls += removedRolls;
    }

    return allRemovedRolls;
};

execute([part1, part2]);
