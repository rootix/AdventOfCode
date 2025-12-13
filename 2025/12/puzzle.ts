import { getInput, execute, splitLinesIntoArray, splitLinesByEmptyLines } from '../utils';

/* Since i was late to the Day 12 party, i knew this is enough to get the result and that it does not work for the sample.
   I expected to parse everything and do some dfs algorithm with all rotations and combinatories with many necessary optimizations to reduce the problem space.
   This count of whether the shapes can fit the area at all, would have been the fist optimization in my mind. 
   I'm not mad that i don't need to do this :D */
const part1 = (): number => {
    const { shapeSizes: presents, regions } = parseInput();
    let fits = 0;
    for (const region of regions) {
        const area = region.x * region.y;
        let occupied = 0;
        for (let i = 0; i < region.packages.length; i++) {
            const shapeSize = presents[i];
            const times = region.packages[i];
            occupied += shapeSize * times;
        }

        if (occupied <= area) {
            fits++;
        }
    }

    return fits;
};

const parseInput = () => {
    const parts = splitLinesByEmptyLines(getInput());
    const regions = parts.splice(parts.length - 1).flatMap((part) =>
        splitLinesIntoArray(part).map((line) => {
            const regionSplit = line.split(': ');
            const areaSplit = regionSplit[0].split('x');
            const presentSplit = regionSplit[1].split(' ').map(Number);
            return { x: Number(areaSplit[0]), y: Number(areaSplit[1]), packages: presentSplit };
        })
    );

    const shapeSizes = parts.map((packagePart) => {
        const shapeLines = splitLinesIntoArray(packagePart).slice(1);
        let size = 0;
        for (const line of shapeLines) {
            for (const part of line.split('')) {
                if (part === '#') {
                    size++;
                }
            }
        }

        return size;
    });

    return { shapeSizes, regions };
};

execute([part1]);