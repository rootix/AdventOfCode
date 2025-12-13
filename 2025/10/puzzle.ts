import { getInput, execute, splitLinesIntoArray, sum } from '../utils';

type Machine = {
    lights: string;
    schematics: number[][];
    joltageRequirements: number[];
};

const part1 = (): number => {
    const machines = parseInput();
    return sum(machines.map(getMinPresses));
};

const part2 = (): number => {
    return NaN; // No Chance, too stupid ¯\_(ツ)_/¯
};

const parseInput = (): Machine[] => {
    return splitLinesIntoArray(getInput()).map((line) => {
        const parts = line.split(' ');
        const lights = parts.splice(0, 1)[0].slice(1, -1);
        const joltageRequirements = parts
            .splice(parts.length - 1, 1)[0]
            .slice(1, -1)
            .split(',')
            .map(Number);
        const schematics = parts.map((p) => p.slice(1, -1).split(',').map(Number));
        return { lights, schematics, joltageRequirements };
    });
};

const getMinPresses = (machine: Machine) => {
    const queue: string[] = [];
    const visited = new Map<string, number>();
    queue.push('.'.repeat(machine.lights.length));

    while (queue.length > 0) {
        var lightState = queue.shift()!;
        var schematicsApplied = visited.get(lightState) ?? 0;

        for (const schematic of machine.schematics) {
            const lights = lightState.split('');
            for (const button of schematic) {
                lights[button] = lights[button] === '#' ? '.' : '#';
            }

            const toggledLights = lights.join('');
            if (toggledLights === machine.lights) {
                return schematicsApplied + 1;
            }

            if (visited.has(toggledLights)) {
                continue;
            }

            visited.set(toggledLights, schematicsApplied + 1);
            queue.push(toggledLights);
        }
    }

    return Infinity;
};

execute([part1, part2]);
