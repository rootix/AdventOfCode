import {execute, getInput, splitLinesIntoArray} from "../utils";

interface Point {
    x: number,
    y: number
}

const newSand: Point = {x: 500, y: 0};

const parseWalls = (input: string): Set<string> => {
    const walls = new Set<string>();
    splitLinesIntoArray(getInput())
        .map(wall => wall.split(" -> ").map(edge => {
            const edgeSplit = edge.split(',');
            return <Point>{x: Number(edgeSplit[0]), y: Number(edgeSplit[1])}
        }))
        .forEach(wall => {
            for (let i = 1; i < wall.length; i++) {
                let minX = Math.min(wall[i - 1].x, wall[i].x);
                let maxX = Math.max(wall[i - 1].x, wall[i].x);
                let minY = Math.min(wall[i - 1].y, wall[i].y);
                let maxY = Math.max(wall[i - 1].y, wall[i].y);
                for (let y = minY; y <= maxY; y++) {
                    for (let x = minX; x <= maxX; x++) {
                        walls.add(`${x},${y}`);
                    }
                }
            }
        });
    return walls;
};

const calculateBottom = (rocks: string[]) => rocks.reduce((lowest: number, rock) => Math.max(Number(rock.split(',')[1]), lowest), -Infinity);

const fall = (currentSand: Point, occupied: Set<string>): boolean => {
    if (occupied.has(`${currentSand.x},${currentSand.y + 1}`)) {
        if (!occupied.has(`${currentSand.x - 1},${currentSand.y + 1}`)) {
            currentSand.x--;
            currentSand.y++;
        } else if (!occupied.has(`${currentSand.x + 1},${currentSand.y + 1}`)) {
            currentSand.x++;
            currentSand.y++;
        } else {
            occupied.add(`${currentSand.x},${currentSand.y}`);
            return true;
        }
    } else {
        currentSand.y++;
    }

    return false;
};

const part1 = (): number => {
    const occupied = parseWalls(getInput());
    const bottom = calculateBottom([...occupied]);
    let endlessVoidReached = false;
    let sandGrains = 0;
    let currentSand: Point = {x: newSand.x, y: newSand.y};

    while (!endlessVoidReached) {
        while (!endlessVoidReached) {
            const cameToRest = fall(currentSand, occupied);
            if (cameToRest) {
                break;
            }

            if (currentSand.y > bottom) {
                endlessVoidReached = true;
            }
        }

        sandGrains++;
        currentSand = {x: newSand.x, y: newSand.y};
    }

    return (sandGrains - 1);
}

const part2 = (): number => {
    const occupied = parseWalls(getInput());
    const bottom = calculateBottom([...occupied]);
    let sandGrains = 0;
    let currentSand: Point = {x: newSand.x, y: newSand.y};

    while (!occupied.has(`${newSand.x},${newSand.y}`)) {
        while (true) {
            const cameToRest = fall(currentSand, occupied);
            if (cameToRest) {
                break;
            }

            if (currentSand.y == bottom + 1) {
                occupied.add(`${currentSand.x},${currentSand.y}`);
                break;
            }
        }

        sandGrains++;
        currentSand = {x: newSand.x, y: newSand.y};
    }

    return sandGrains;
}

execute([part1, part2]);