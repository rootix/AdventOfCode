import {execute, getInput, mod, splitLinesByEmptyLines, splitLinesIntoArray} from '../utils';

enum Direction {
    RIGHT = 0,
    DOWN = 1,
    LEFT = 2,
    UP = 3
}

interface MinMax {
    min: number;
    max: number;
}

interface Position {
    row: number,
    col: number,
    face?: number,
    facing: Direction
}

const DIRECTIONS: Record<Direction, [number, number]> = {
    [Direction.UP]: [-1, 0],
    [Direction.RIGHT]: [0, 1],
    [Direction.DOWN]: [1, 0],
    [Direction.LEFT]: [0, -1]
};

const CUBE_SIDE_LEN = 50;

const CUBE_SIDES: Record<number, [number, number]> = { // start row, start col
    [1]: [0, CUBE_SIDE_LEN],
    [2]: [0, CUBE_SIDE_LEN * 2],
    [3]: [CUBE_SIDE_LEN, CUBE_SIDE_LEN],
    [4]: [CUBE_SIDE_LEN * 2, 0],
    [5]: [CUBE_SIDE_LEN * 2, CUBE_SIDE_LEN],
    [6]: [CUBE_SIDE_LEN * 3, 0]
}

// This only works for the real input. I was too lazy to do it for the sample too
const CUBE_SIDE_TRANSITIONS: Record<number, Record<Direction, (pos: Position) => Position>> = {
    [1]: {
        [Direction.RIGHT]: pos => <Position>{ row: pos.row, col: CUBE_SIDES[2][1], face: 2, facing: Direction.RIGHT },
        [Direction.DOWN]: pos => <Position>{ row: CUBE_SIDES[3][0], col: pos.col, face: 3, facing: Direction.DOWN },
        [Direction.LEFT]: pos => <Position>{ row: CUBE_SIDES[4][0] + CUBE_SIDE_LEN - mod(pos.row, CUBE_SIDE_LEN) -1, col: CUBE_SIDES[4][1], face: 4, facing: Direction.RIGHT },
        [Direction.UP]: pos => <Position>{ row: CUBE_SIDES[6][0] + mod(pos.col, CUBE_SIDE_LEN), col: CUBE_SIDES[6][1], face: 6, facing: Direction.RIGHT }
    },
    [2]: {
        [Direction.RIGHT]: pos => <Position>{ row: CUBE_SIDES[5][0] + CUBE_SIDE_LEN - mod(pos.row, CUBE_SIDE_LEN) - 1, col: CUBE_SIDES[5][1] + CUBE_SIDE_LEN - 1, face: 5, facing: Direction.LEFT },
        [Direction.DOWN]: pos => <Position>{ row: CUBE_SIDES[3][0] + mod(pos.col, CUBE_SIDE_LEN), col: CUBE_SIDES[3][1] + CUBE_SIDE_LEN - 1, face: 3, facing: Direction.LEFT },
        [Direction.LEFT]: pos => <Position>{ row: pos.row, col: CUBE_SIDES[1][1] + CUBE_SIDE_LEN - 1, face: 1, facing: Direction.LEFT },
        [Direction.UP]: pos => <Position>{ row: CUBE_SIDES[6][0] + CUBE_SIDE_LEN - 1, col: CUBE_SIDES[6][1] + mod(pos.col, CUBE_SIDE_LEN), face: 6, facing: Direction.UP }
    },
    [3]: {
        [Direction.RIGHT]: pos => <Position>{ row: CUBE_SIDES[2][0] + CUBE_SIDE_LEN - 1, col: CUBE_SIDES[2][1] + mod(pos.row, CUBE_SIDE_LEN), face: 2, facing: Direction.UP },
        [Direction.DOWN]: pos => <Position>{ row: CUBE_SIDES[5][0], col: pos.col, face: 5, facing: Direction.DOWN },
        [Direction.LEFT]: pos => <Position>{ row: CUBE_SIDES[4][0], col: mod(pos.row, CUBE_SIDE_LEN), face: 4, facing: Direction.DOWN },
        [Direction.UP]: pos => <Position>{ row: CUBE_SIDES[1][0] + CUBE_SIDE_LEN - 1, col: pos.col, face: 1, facing: Direction.UP },
    },
    [4]: {
        [Direction.RIGHT]: pos => <Position>{ row: pos.row, col: CUBE_SIDES[5][1], face: 5, facing: Direction.RIGHT },
        [Direction.DOWN]: pos => <Position>{ row: CUBE_SIDES[6][0], col: pos.col, face: 6, facing: Direction.DOWN },
        [Direction.LEFT]: pos => <Position>{ row: CUBE_SIDES[1][0] + CUBE_SIDE_LEN - mod(pos.row, CUBE_SIDE_LEN) - 1, col: CUBE_SIDES[1][1], face: 1, facing: Direction.RIGHT },
        [Direction.UP]: pos => <Position>{ row: CUBE_SIDES[3][0] + mod(pos.col, CUBE_SIDE_LEN), col: CUBE_SIDES[3][1], face: 3, facing: Direction.RIGHT },
    },
    [5]: {
        [Direction.RIGHT]: pos => <Position>{ row: CUBE_SIDES[2][0] + CUBE_SIDE_LEN - mod(pos.row, CUBE_SIDE_LEN) - 1, col: CUBE_SIDES[2][1] + CUBE_SIDE_LEN - 1, face: 2, facing: Direction.LEFT },
        [Direction.DOWN]: pos => <Position>{ row: CUBE_SIDES[6][0] + mod(pos.col, CUBE_SIDE_LEN), col: CUBE_SIDES[6][1] + CUBE_SIDE_LEN - 1, face: 6, facing: Direction.LEFT },
        [Direction.LEFT]: pos => <Position>{ row: pos.row, col: CUBE_SIDES[4][1] + CUBE_SIDE_LEN - 1, face: 4, facing: Direction.LEFT },
        [Direction.UP]: pos => <Position>{ row: CUBE_SIDES[3][0] + CUBE_SIDE_LEN - 1, col: pos.col, face: 3, facing: Direction.UP },
    },
    [6]: {
        [Direction.RIGHT]: pos => <Position>{ row: CUBE_SIDES[5][0] + CUBE_SIDE_LEN - 1, col: CUBE_SIDES[5][1] + mod(pos.row, CUBE_SIDE_LEN), face: 5, facing: Direction.UP },
        [Direction.DOWN]: pos => <Position>{ row: CUBE_SIDES[2][0], col: CUBE_SIDES[2][1] + mod(pos.col, CUBE_SIDE_LEN), face: 2, facing: Direction.DOWN },
        [Direction.LEFT]: pos => <Position>{ row: CUBE_SIDES[1][0], col: CUBE_SIDES[1][1] + mod(pos.row, CUBE_SIDE_LEN), face: 1, facing: Direction.DOWN },
        [Direction.UP]: pos => <Position>{ row: CUBE_SIDES[4][0] + CUBE_SIDE_LEN - 1, col: pos.col, face: 4, facing: Direction.UP },
    }
}

const [mapPart, instructionPart] = splitLinesByEmptyLines(getInput());
const instructions = instructionPart.split(/([LR])/).map(part => !isNaN(parseInt(part, 10)) ? Number(part) : part as 'R' | 'L');

const grid = new Set<string>();
const stones = new Set<string>();
const xEdges: MinMax[] = [];
const yEdges: MinMax[] = [];

const turn = (pos: Position, direction: 'R' | 'L') => {
    pos.facing = direction === 'R' ? mod(pos.facing + 1, 4) : mod(pos.facing - 1, 4);
};

const wrapOrStay = (pos: Position) => {
    if (grid.has(`${pos.row},${pos.col}`)) {
        return pos;
    }

    switch (pos.facing) {
        case Direction.UP:
            return {...pos, row: yEdges[pos.col].max}
        case Direction.RIGHT:
            return {...pos, col: xEdges[pos.row].min}
        case Direction.DOWN:
            return {...pos, row: yEdges[pos.col].min}
        case Direction.LEFT:
            return {...pos, col: xEdges[pos.row].max}
    }
};

const changeFaceOrStay = (currPos: Position, newPos: Position) => {
    const cubeFace = CUBE_SIDES[currPos.face];
    if (newPos.row >= cubeFace[0] + CUBE_SIDE_LEN) {
        return CUBE_SIDE_TRANSITIONS[currPos.face][Direction.DOWN](currPos);
    } else if (newPos.row < cubeFace[0]) {
        return CUBE_SIDE_TRANSITIONS[currPos.face][Direction.UP](currPos);
    } else if (newPos.col >= cubeFace[1] + CUBE_SIDE_LEN) {
        return CUBE_SIDE_TRANSITIONS[currPos.face][Direction.RIGHT](currPos);
    } else if (newPos.col < cubeFace[1]) {
        return CUBE_SIDE_TRANSITIONS[currPos.face][Direction.LEFT](currPos);
    }

    return newPos;
};

const move = (pos: Position, times: number, cubic: boolean) => {
    for (let t = 0; t < times; t++) {
        const dirVector = DIRECTIONS[pos.facing];
        let newPos = {...pos, row: pos.row + dirVector[0], col: pos.col + dirVector[1]};
        newPos = cubic ? changeFaceOrStay(pos, newPos) : wrapOrStay(newPos);
        if (stones.has(`${newPos.row},${newPos.col}`)) {
            break;
        }

        Object.assign(pos, newPos);
    }
};

const followInstructions = (pos: Position, cubic: boolean) => {
    for (const instruction of instructions) {
        if (typeof instruction !== "number") {
            turn(pos, instruction);
        } else {
            move(pos, instruction, cubic);
        }
    }

    return 1000 * (pos.row + 1) + 4 * (pos.col + 1) + pos.facing;
};


splitLinesIntoArray(mapPart).forEach((row, rowIndex) => {
    const xEdge: MinMax = {min: Infinity, max: -Infinity};
    row.split('').forEach((col, colIndex) => {
        let yEdge: MinMax;
        if (yEdges.length <= colIndex) {
            yEdge = {min: Infinity, max: -Infinity};
            yEdges[colIndex] = yEdge;
        } else {
            yEdge = yEdges[colIndex];
        }

        if (col !== ' ') {
            grid.add(`${rowIndex},${colIndex}`);
            xEdge.min = Math.min(xEdge.min, colIndex);
            xEdge.max = Math.max(xEdge.max, colIndex);
            yEdge.min = Math.min(yEdge.min, rowIndex);
            yEdge.max = Math.max(yEdge.max, rowIndex);
        }
        if (col === '#') {
            stones.add(`${rowIndex},${colIndex}`);
        }
    });

    xEdges[rowIndex] = xEdge;
});

const part1 = (): number => {
    const pos: Position = {row: 0, col: xEdges[0].min, facing: Direction.RIGHT};
    return followInstructions(pos, false);
}

const part2 = (): number => {
    const pos: Position = {row: 0, col: xEdges[0].min, face: 1, facing: Direction.RIGHT};
    return followInstructions(pos, true);
}

execute([part1, part2]);