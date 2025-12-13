import { getInput, execute, splitLinesIntoArray } from '../utils';

type DeviceGraph = Record<string, string[]>;

const part1 = (): number => {
    const deviceOutputGraph = parseInput(1);

    const followPath = (graph: DeviceGraph, start: string) => {
        let pathCount = 0;
        for (const output of graph[start]) {
            if (output === 'out') {
                return 1;
            }

            pathCount += followPath(graph, output);
        }

        return pathCount;
    };

    return followPath(deviceOutputGraph, 'you');
};

const part2 = (): number => {
    const deviceOutputGraph = parseInput(2);

    const followPath = (
        graph: DeviceGraph,
        start: string,
        dac: boolean,
        fft: boolean,
        visited: Map<string, number> = new Map()
    ): number => {
        const cacheKey = `${start}|${dac}|${fft}`;
        if (visited.has(cacheKey)) {
            return visited.get(cacheKey) ?? 0;
        }
        let pathCount = 0;
        for (const output of graph[start]) {
            if (output === 'out') {
                return dac && fft ? 1 : 0;
            }

            pathCount += followPath(graph, output, dac || output === 'dac', fft || output === 'fft', visited);
        }

        visited.set(cacheKey, pathCount);
        return pathCount;
    };

    return followPath(deviceOutputGraph, 'svr', false, false);
};

const parseInput = (part: 1 | 2) => {
    const deviceOutputGraph: DeviceGraph = {};
    splitLinesIntoArray(getInput(part)).forEach((line) => {
        const [device, outputString] = line.split(': ');
        deviceOutputGraph[device] = outputString.split(' ');
    });

    return deviceOutputGraph;
};

execute([part1, part2]);
