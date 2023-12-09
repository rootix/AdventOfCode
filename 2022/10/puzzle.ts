import {execute, getInput, splitLinesIntoArray} from "../utils";

const simulate = (): [number, string] => {
    const instructions = splitLinesIntoArray(getInput());
    let x = 1;
    let signal = 0;
    let cycle = 0;
    let crt = '';
    for(const instruction of instructions) {
        const cycles = instruction === "noop" ? 1 : 2;
        const append = cycles === 2 ? Number(instruction.substring(5)) : 0;
        for (let _ = 0; _ < cycles; _++) {
            if (Math.abs(x - cycle % 40) < 2) {
                crt += '#';
            } else {
                crt += ".";
            }

            cycle++;
            if (cycle % 40 === 20) {
                signal += cycle * x;
            }
        }

        x += append;
    }

    return [signal, crt];
}

const part1 = (): number => {
    const [signal, crt] = simulate();
    return signal
}

const part2 = (): string => {
    const [_, crt] = simulate();
    return '\n' + crt.split(/(.{40})/).filter(x => x.length == 40).join('\n');
}

execute([part1, part2]);