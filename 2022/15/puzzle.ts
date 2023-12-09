import {execute, getInput, splitLinesIntoArray} from "../utils";

const getSensorsAndBeacons = (input: string) => splitLinesIntoArray(input).map(pair => pair.match(/^Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)$/).slice(1).map(Number));

const distance = (sX: number, sY: number, bX: number, bY: number) => Math.abs(sX - bX) + Math.abs(sY - bY);

const getXBoundaries = (sensorsAndBeacons: number[][]) => sensorsAndBeacons.reduce((acc, [sX, sY, bX, bY]) => {
        const range = distance(sX, sY, bX, bY);
        acc.min = Math.min(acc.min, sX - range, bX);
        acc.max = Math.max(acc.max, sX + range, bX);
        return acc;
    },
    {min: Infinity, max: 0});

const countCoveredPositions = (sensorsAndBeacons: number[][], y: number) => {
    const xBoundaries = getXBoundaries(sensorsAndBeacons);
    let coveredPositions = 0;
    for (let x = xBoundaries.min; x <= xBoundaries.max; x++) {
        for (const [sX, sY, bX, bY] of sensorsAndBeacons) {
            if (x === bX && y === bY) continue;
            const sensorRange = distance(sX, sY, bX, bY);
            const isInRange = distance(x, y, sX, sY) <= sensorRange;
            if (isInRange) {
                coveredPositions++;
                break;
            }
        }
    }
    return coveredPositions;
};

const findDistressBeacon = (sensorsAndBeacons: number[][], max: number) => {
    let x = 0;
    let y = 0;
    while (true) {
        if (y > max) throw Error("distress beacon not found!");
        const sensorInRange = sensorsAndBeacons.find(([sX, sY, bX, bY]) => distance(sX, sY, bX, bY) >= distance(x, y, sX, sY));
        if (!sensorInRange) {
            return {x, y};
        }

        const [sX, sY, bX, bY] = sensorInRange;
        const sensorRange = distance(sX, sY, bX, bY);
        const distanceToSensor = distance(x, y, sX, sY);

        // Optimization: Skip the x range for which we know the sensor in range is covering
        const nextX = x + sensorRange - distanceToSensor + 1;
        x = nextX > max ? 0 : nextX;
        y = nextX > max ? y + 1 : y;
    }
};

const part1 = (): number => {
    const sensorsAndBeacons = getSensorsAndBeacons(getInput());
    return countCoveredPositions(sensorsAndBeacons, 2000000);
}

const part2 = (): number => {
    const sensorsAndBeacons = getSensorsAndBeacons(getInput());
    const max = 4000000;
    const beacon = findDistressBeacon(sensorsAndBeacons, max);
    return beacon.x * max + beacon.y;
}

execute([part1, part2]);