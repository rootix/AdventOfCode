import {execute, getInput, multiply, splitLinesIntoArray, sum} from "../utils";

const getView = (row: number, column: number, grid: number[][]) => {
    const up = grid.slice(0, row).map(row => row[column]).reverse();
    const down = grid.slice(row + 1).map(row => row[column]);
    const left = grid[row].slice(0, column).reverse();
    const right = grid[row].slice(column + 1);
    return [up, left, right, down];
};

const isVisible = (row: number, column: number, grid: number[][]) => {
    const tree = grid[row][column];
    const view = getView(row, column, grid);
    return view.filter(v => tree > Math.max(...v)).length > 0;
};

const getScore = (row: number, column: number, grid: number[][]) => {
    const tree = grid[row][column];
    const view = getView(row, column, grid);

    return multiply(view.map(v => {
        const distance = v.findIndex(other => tree <= other);
        return distance == -1 ? v.length : distance + 1;
    }));
};

const part1 = (): number => {
    const grid = splitLinesIntoArray(getInput()).map(row => row.split('').map(Number));
    return sum(grid.flatMap((_, row) => grid[row].map((_, col) => isVisible(row, col, grid) ? 1 : 0)));
}

const part2 = (): number => {
    const grid = splitLinesIntoArray(getInput()).map(row => row.split('').map(Number));
    return Math.max(...grid.flatMap((_, row) => grid[row].map((_, col) => getScore(row, col, grid))));
}

execute([part1, part2]);