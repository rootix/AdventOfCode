import {execute, getInput, sortAsc, splitLinesIntoArray, sum} from "../utils";

interface Directory {
    name: string,
    parent?: Directory,
    subDirs: Directory[],
    files: File[]
}

interface File {
    name: string,
    size: number
}

const parseFileSystem = (input: string) => {
    let rootDir: Directory;
    let currentDir: Directory;
    for (const line of splitLinesIntoArray(input)) {
        if (line.startsWith("$ cd")) {
            const param = line.substring(5);
            if (param !== "..") {
                const newDir = <Directory>{name: param, subDirs: [], files: []};
                if (rootDir === undefined) {
                    rootDir = newDir
                } else {
                    newDir.parent = currentDir;
                    currentDir.subDirs.push(newDir)
                }
                currentDir = newDir;
            } else {
                currentDir = currentDir.parent;
            }
        } else if (Number(line.charAt(0))) {
            const [size, name] = line.split(" ");
            currentDir.files.push({name, size: Number(size)})
        }
    }

    return rootDir;
};

const calculateDirSize = (dir: Directory) => sum(dir.files.map(f => f.size)) + sum(dir.subDirs.map(d => calculateDirSize(d)));

const getQ1Size = (dir: Directory) => {
    let total = 0;
    const size = calculateDirSize(dir);
    if (size < 100000) {
        total += size;
    }
    dir.subDirs.map(d => total += getQ1Size(d))
    return total;
};

const flattenDirSizes = (dir: Directory) => {
    const sizes = [calculateDirSize(dir)];
    dir.subDirs.map(subDir => sizes.push(...flattenDirSizes(subDir)))
    return sizes;
};

const part1 = (): number => {
    const rootDir = parseFileSystem(getInput());
    return getQ1Size(rootDir);
}

const part2 = (): number => {
    const rootDir = parseFileSystem(getInput());
    const allDirSizes = sortAsc(flattenDirSizes(rootDir));
    const unusedSpace = 70000000 - allDirSizes[allDirSizes.length - 1];
    const needsToBeFreedUp = 30000000 - unusedSpace;
    return allDirSizes.find(s => s > needsToBeFreedUp);
}

execute([part1, part2]);