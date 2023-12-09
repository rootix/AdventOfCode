import {execute, getInput, splitLinesIntoArray} from "../utils";

type Position = [number, number];
type Direction = '^' | '>' | 'v' | '<';
type Blizzards = Map<string, Direction[]>;

interface State {
    move: number,
    position: Position
}

const DIRECTIONS: Record<Direction, Position> = {
    ['^']: [-1, 0],
    ['>']: [0, 1],
    ['v']: [1, 0],
    ['<']: [0, -1]
};

const walls = new Set<string>();
const initialBlizzards = new Map<string, Direction[]>();

let topWallRow = Infinity;
let leftWallRow = Infinity;
let rightWallRow = -Infinity;
let bottomWallRow = -Infinity;

const getPosKey = (pos: Position) => `${pos[0]},${pos[1]}`;

splitLinesIntoArray(getInput()).forEach((row, rowIndex) => {
    row.split('').forEach((col, colIndex) => {
        if (col === '#') {
            walls.add(getPosKey([rowIndex, colIndex]));
            topWallRow = Math.min(topWallRow, rowIndex);
            bottomWallRow = Math.max(bottomWallRow, rowIndex);
            leftWallRow = Math.min(leftWallRow, colIndex);
            rightWallRow = Math.max(rightWallRow, colIndex);
        } else if (col !== '.') {
            const position: Position = [rowIndex, colIndex];
            initialBlizzards.set(getPosKey(position), [col as Direction]);
        }
    });
});

const startPos: Position = [topWallRow, leftWallRow + 1];
const endPos: Position = [bottomWallRow, rightWallRow - 1];

const getPosFromKey = (key: string) => {
    const parts = key.split(',');
    return [Number(parts[0]), Number(parts[1])];
};

const moveBlizzards = (blizzards: Blizzards) => {
    const movedBlizzards = new Map<string, Direction[]>();

    for (const [key, values] of blizzards) {
        const blizzardPos = getPosFromKey(key);
        for (const blizzard of values) {
            const dir = DIRECTIONS[blizzard];
            const newPos: Position = [blizzardPos[0] + dir[0], blizzardPos[1] + dir[1]];
            if (newPos[0] >= bottomWallRow) {
                newPos[0] = topWallRow + 1;
            } else if (newPos[0] <= topWallRow) {
                newPos[0] = bottomWallRow - 1;
            } else if (newPos[1] >= rightWallRow) {
                newPos[1] = leftWallRow + 1;
            } else if (newPos[1] <= leftWallRow) {
                newPos[1] = rightWallRow - 1;
            }

            const newPosKey = getPosKey(newPos);
            if (movedBlizzards.has(newPosKey)) {
                movedBlizzards.get(newPosKey).push(blizzard);
            } else {
                movedBlizzards.set(newPosKey, [blizzard]);
            }
        }
    }

    return movedBlizzards;
};

const move = (from: Position, to: Position, startingBlizzards: Blizzards): [number, Blizzards] => {
    const toPosKey = getPosKey(to);
    const blizzardCache = new Map<number, Blizzards>();
    blizzardCache.set(0, startingBlizzards);
    const queue: State[] = [{move: 0, position: from}];
    const visited = new Set<string>();

    while (queue.length) {
        const {move, position} = queue.shift();

        const visitedKey = `${move},${getPosKey(position)}`;
        if (visited.has(visitedKey)) {
            continue;
        }

        visited.add(visitedKey);

        let blizzards = blizzardCache.get(move);
        if (!blizzards) {
            blizzards = moveBlizzards(blizzardCache.get(Math.max(0, move - 1)));
            blizzardCache.set(move, blizzards);
        }

        for (const dir of Object.values(DIRECTIONS)) {
            const newPos: Position = [position[0] + dir[0], position[1] + dir[1]];
            const newPosKey = getPosKey(newPos);

            if (newPosKey === toPosKey) {
                return [move, blizzards];
            }

            if (blizzards.has(newPosKey) ||
                walls.has(newPosKey) ||
                newPos[0] < topWallRow ||
                newPos[0] > bottomWallRow ||
                newPos[1] < leftWallRow ||
                newPos[1] > rightWallRow) {
                continue;
            }

            queue.push({move: move + 1, position: newPos});
        }

        if (!blizzards.has(getPosKey(position))) {
            queue.push({move: move + 1, position});
        }
    }
};

const part1 = (): number => {
    const [moves, _] = move(startPos, endPos, initialBlizzards);
    return moves;
}

const part2 = (): number => {
    const [firstMoves, firstBlizzards] = move(startPos, endPos, initialBlizzards);
    const [secondMoves, secondBlizzards] = move(endPos, startPos, firstBlizzards);
    const [thirdMoves, _] = move(startPos, endPos, secondBlizzards);
    return firstMoves + secondMoves + thirdMoves;
}

execute([part1, part2]);