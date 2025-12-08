import { getInput, execute, splitLinesIntoArray } from '../utils';

type Box = {
    x: number;
    y: number;
    z: number;
};

type Pair = {
    a: Box;
    b: Box;
    distance: number;
};

const part1 = (): number => {
    const boxes = parseInput();
    const allPairs = buildSortedPairs(boxes);
    const circuits = boxes.map((b) => new Set<Box>([b]));
    for (const pair of allPairs.splice(0, boxes.length === 20 ? 10 : 1000)) {
        const ai = circuits.findIndex((c) => c.has(pair.a));
        const bi = circuits.findIndex((c) => c.has(pair.b));
        if (ai === bi) {
            continue;
        }

        for (const box of circuits[bi]) {
            circuits[ai].add(box);
        }

        circuits.splice(bi, 1);
    }

    circuits.sort((a, b) => b.size - a.size);

    return circuits.slice(0, 3).reduce((acc, circuit) => acc * circuit.size, 1);
};

const part2 = (): number => {
    const boxes = parseInput();
    const allPairs = buildSortedPairs(boxes);
    const circuits = boxes.map((b) => new Set<Box>([b]));
    while (circuits.length > 1) {
        for (const pair of allPairs) {
            const ai = circuits.findIndex((c) => c.has(pair.a));
            const bi = circuits.findIndex((c) => c.has(pair.b));

            if (ai === bi) {
                continue;
            }

            for (const box of circuits[bi]) {
                circuits[ai].add(box);
            }

            if (circuits.length === 2) {
                return pair.a.x * pair.b.x;
            }

            circuits.splice(bi, 1);
        }
    }

    throw new Error('No pairs found to connect all boxes');
};

const parseInput = () => {
    return splitLinesIntoArray(getInput()).map((line) => {
        const [x, y, z] = line.split(',').map(Number);
        return { x, y, z };
    });
};

const buildSortedPairs = (boxes: Box[]) => {
    const allPairs: Pair[] = [];
    for (let i = 0; i < boxes.length; i++) {
        for (let j = i + 1; j < boxes.length; j++) {
            const a = boxes[i];
            const b = boxes[j];
            const dist = Math.sqrt(Math.pow(a.x - b.x, 2) + Math.pow(a.y - b.y, 2) + Math.pow(a.z - b.z, 2));
            allPairs.push({ a, b, distance: dist });
        }
    }

    allPairs.sort((p, q) => p.distance - q.distance);

    return allPairs;
};

execute([part1, part2]);
