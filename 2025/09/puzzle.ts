import { getInput, execute, splitLinesIntoArray } from '../utils';

const part1 = (): number => {
    const redTiles = parseInput();
    let maxArea = 0;
    for (let i = 0; i < redTiles.length; i++) {
        for (let j = i + 1; j < redTiles.length; j++) {
            const a = redTiles[i];
            const b = redTiles[j];

            const area = Math.abs(a.x - b.x + 1) * Math.abs(a.y - b.y + 1);
            maxArea = Math.max(maxArea, area);
        }
    }

    return maxArea;
};

const part2 = (): number => {
    const redTiles = parseInput();
    let maxArea = 0;
    for (let i = 0; i < redTiles.length; i++) {
        for (let j = i + 1; j < redTiles.length; j++) {
            const a = redTiles[i];
            const b = redTiles[j];

            const minX = Math.min(a.x, b.x);
            const maxX = Math.max(a.x, b.x);
            const minY = Math.min(a.y, b.y);
            const maxY = Math.max(a.y, b.y);

            let isIntersecting = false;

            // Check if any polygon edge cuts through the rectangle's interior
            for (let k = 0; k < redTiles.length; k++) {
                const c = redTiles[k];
                const d = k === redTiles.length - 1 ? redTiles[0] : redTiles[k + 1];

                // Edge intersects rectangle interior if its bounding box overlaps with the rectangle's interior on both axes
                if (
                    Math.max(c.x, d.x) > minX &&
                    Math.min(c.x, d.x) < maxX &&
                    Math.max(c.y, d.y) > minY &&
                    Math.min(c.y, d.y) < maxY
                ) {
                    isIntersecting = true;
                    break;
                }
            }

            if (!isIntersecting) {
                const area = (maxX - minX + 1) * (maxY - minY + 1);
                maxArea = Math.max(maxArea, area);
            }
        }
    }

    return maxArea;
};

const parseInput = () => {
    return splitLinesIntoArray(getInput()).map((line) => {
        const [x, y] = line.split(',').map(Number);
        return { x, y };
    });
};

execute([part1, part2]);
