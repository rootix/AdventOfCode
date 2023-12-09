import {execute, getInput, splitLinesIntoArray} from "../utils";

interface Vector {
    x: number;
    y: number;
}

const moves: { [key: string]: Vector } = {'R': {x: 1, y: 0}, 'D': {x: 0, y: -1}, 'L': {x: -1, y: 0}, 'U': {x: 0, y: 1}}

const followLeader = (follower: Vector, leader: Vector) => {
    if (Math.abs(follower.x - leader.x) > 1) {
        follower.x += leader.x > follower.x ? 1 : -1;
        if (follower.y != leader.y) {
            follower.y += leader.y > follower.y ? 1 : -1;
        }
    }

    if (Math.abs(follower.y - leader.y) > 1) {
        follower.y += leader.y > follower.y ? 1 : -1;
        if (follower.x != leader.x) {
            follower.x += leader.x > follower.x ? 1 : -1;
        }
    }
};

const processInstructions = (input: string, numberOfKnots: number) => {
    const knots = new Array(numberOfKnots).fill(0).map(_ => <Vector>{x: 0, y: 0});
    const visited = new Set();
    const instructions = splitLinesIntoArray(input);
    for (const instruction of instructions) {
        const move = moves[instruction.substring(0, 1)];
        const times = Number(instruction.substring(2));
        for (let _ = 0; _ < times; _++) {
            knots[0].x += move.x;
            knots[0].y += move.y;

            for (let f = 1; f < knots.length; f++) {
                followLeader(knots[f], knots[f - 1]);
            }

            visited.add(`${knots[numberOfKnots - 1].x};${knots[numberOfKnots - 1].y}`);
        }
    }

    return visited.size;
};

const part1 = (): number => {
    return processInstructions(getInput(), 2);
}

const part2 = (): number => {
    return processInstructions(getInput(), 10);
}

execute([part1, part2]);