import {execute, getInput, splitLinesIntoArray} from "../utils";

const getPossibleAdjacents = (cube: number[]) => {
    const [x, y, z] = cube;
    return [
        [x - 1, y, z],
        [x + 1, y, z],
        [x, y - 1, z],
        [x, y + 1, z],
        [x, y, z - 1],
        [x, y, z + 1]
    ]
};

const part1 = (): number => {
    const cubes = splitLinesIntoArray(getInput()).map(cube => cube.split(',').map(Number));
    const processed = new Set<string>();
    let surfaceArea = 0;
    for (const cube of cubes) {
        surfaceArea += 6;
        for (const adjacent of getPossibleAdjacents(cube)) {
            if (processed.has(`${adjacent[0]},${adjacent[1]},${adjacent[2]}`)) {
                surfaceArea -= 2;
            }
        }

        processed.add(`${cube[0]},${cube[1]},${cube[2]}`)
    }

    return surfaceArea;
}

const part2 = (): number => {
    const cubesAsString = splitLinesIntoArray(getInput());
    const cubes = cubesAsString.map(cube => cube.split(',').map(Number));
    const minX = cubes.reduce((min, cube) => Math.min(min, cube[0]), Infinity) - 1;
    const maxX = cubes.reduce((max, cube) => Math.max(max, cube[0]), -Infinity) + 1;
    const minY = cubes.reduce((min, cube) => Math.min(min, cube[1]), Infinity) - 1;
    const maxY = cubes.reduce((max, cube) => Math.max(max, cube[1]), -Infinity) + 1;
    const minZ = cubes.reduce((min, cube) => Math.min(min, cube[2]), Infinity) - 1;
    const maxZ = cubes.reduce((max, cube) => Math.max(max, cube[2]), -Infinity) + 1;

    const bigCube = {};
    for (let z = minZ; z <= maxZ; z++) {
        for (let y = minY; y <= maxY; y++) {
            for (let x = minX; x <= maxX; x++) {
                bigCube[`${x},${y},${z}`] = cubesAsString.includes(`${x},${y},${z}`);
            }
        }
    }

    const work = [[minX, minY, minZ]];
    while (work.length) {
        const cube = work.pop();
        bigCube[`${cube[0]},${cube[1]},${cube[2]}`] = null;
        for (const adj of getPossibleAdjacents(cube)) {
            if (bigCube[`${adj[0]},${adj[1]},${adj[2]}`] === false) {
                work.push([adj[0], adj[1], adj[2]]);
            }
        }
    }

    const exteriorSurfaceArea = Object.keys(bigCube).reduce((area, pos) => {
        if (bigCube[pos] === null) {
            return area;
        }

        const cube = pos.split(',').map(Number);
        getPossibleAdjacents(cube).forEach(adj => {
            if (bigCube[`${adj[0]},${adj[1]},${adj[2]}`] === null) {
                area++;
            }
        });

        return area;
    }, 0);

    return exteriorSurfaceArea;
}

execute([part1, part2]);