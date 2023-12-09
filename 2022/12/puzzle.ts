import {execute, getInput, splitLinesIntoArray} from "../utils";

interface Position {
    r: number;
    c: number;
    d?: number;
}

const directions: Position[] = [{r: 1, c: 0}, {r: 0, c: -1}, {r: -1, c: 0}, {r: 0, c: 1}];

const parseHeightMap = (input: string): { start: Position, end: Position, map: number[][] } => {
    let start: Position;
    let end: Position;
    const map = splitLinesIntoArray(input)
        .map((row, r) => row.split('')
            .map((char, c) => {
                if (char === "S") {
                    start = {r, c, d: 0};
                    return 0; // a
                } else if (char === "E") {
                    end = {r, c}
                    return 25; // z
                }
                return char.charCodeAt(0) - 'a'.charCodeAt(0);
            }));

    return {start, end, map};
};

const hikeToE = (map: number[][], starts: Position[], end: Position) => {
    const visited = new Set();
    const searchQueue: Position[] = starts;
    while (searchQueue.length) {
        const item = searchQueue.shift();
        if (visited.has(`${item.r};${item.c}`)) {
            continue;
        }

        visited.add(`${item.r};${item.c}`);

        if (item.r === end.r && item.c === end.c) {
            return item.d;
        }

        directions.forEach(dir => {
            const newRow = item.r + dir.r;
            const newCol = item.c + dir.c;
            if (newRow >= 0 && newRow < map.length &&
                newCol >= 0 && newCol < map[0].length &&
                map[newRow][newCol] <= map[item.r][item.c] + 1) {
                searchQueue.push({r: newRow, c: newCol, d: item.d + 1});
            }
        })
    }
};

const getStarts = (map: number[][]) => {
    const starts: Position[] = [];
    map.forEach((row, rowIndex) => row.map((height, colIndex) => {
        if (height === 0) { // a
            starts.push({r: rowIndex, c: colIndex, d: 0})
        }
    }));
    return starts;
};

const part1 = (): number => {
    const {start, end, map} = parseHeightMap(getInput());
    return hikeToE(map, [start], end);
}

const part2 = (): number => {
    const {end, map} = parseHeightMap(getInput());
    return hikeToE(map, getStarts(map), end);
}

execute([part1, part2]);