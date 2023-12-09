import {splitLinesByEmptyLines, splitLinesIntoArray, sum, sortDesc, isSampleInput, getInput, execute} from '../utils'

const part1 = (): number => {
    return Math.max(...splitLinesByEmptyLines(getInput())
        .map(splitLinesIntoArray)
        .map(group => group.map(Number))
        .map(group => sum(group)));
}

const part2 = (): number => {
    return sum(sortDesc(splitLinesByEmptyLines(getInput())
        .map(splitLinesIntoArray)
        .map(group => group.map(Number))
        .map(group => sum(group)))
        .slice(0, 3));
}

execute([part1, part2]);