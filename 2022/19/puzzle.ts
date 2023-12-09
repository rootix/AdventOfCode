import {execute, getInput, multiply, splitLinesIntoArray, sum} from "../utils";

interface Blueprint {
    id: number,
    oreRobotCost: { ore: number },
    clayRobotCost: { ore: number },
    obsidianRobotCost: { ore: number, clay: number },
    geodeRobotCost: { ore: number, obsidian: number },
}

interface State {
    remaining: number,
    ores: number,
    clay: number,
    obsidian: number,
    geodes: number,
    oreRobots: number,
    clayRobots: number,
    obsidianRobots: number,
    didNotBuild: string[]
}

const parseBlueprints = (input: string): Blueprint[] => splitLinesIntoArray(input)
    .map(line => line.match(/Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.$/).slice(1))
    .map(matches => {
        return {
            id: Number(matches[0]),
            oreRobotCost: {ore: Number(matches[1])},
            clayRobotCost: {ore: Number(matches[2])},
            obsidianRobotCost: {ore: Number(matches[3]), clay: Number(matches[4])},
            geodeRobotCost: {ore: Number(matches[5]), obsidian: Number(matches[6])},
        }
    });

/* This optimization reduces the search space by:
       1. capping the number of robots to not more than necessary for any given simulation round
       2. capping the amount of resources to whats possibly ever needed till the time limit is reached
   With these changes, the cache hit ratio increases massively. */
const optimizeStateForCache = (state: State, maxOreCosts: number, maxClayCosts: number, maxObsidianCosts: number) => {
    if (state.oreRobots >= maxOreCosts) {
        state.oreRobots = maxOreCosts;
    }

    if (state.clayRobots >= maxClayCosts) {
        state.clayRobots = maxClayCosts;
    }

    if (state.obsidianRobots >= maxObsidianCosts) {
        state.obsidianRobots = maxObsidianCosts;
    }

    const oreCap = state.remaining * maxOreCosts - state.oreRobots * (state.remaining - 1);
    const clayCap = state.remaining * maxClayCosts - state.clayRobots * (state.remaining - 1);
    const obsidianCap = state.remaining * maxObsidianCosts - state.obsidianRobots * (state.remaining - 1);

    if (state.ores >= oreCap) {
        state.ores = oreCap;
    }

    if (state.clay >= clayCap) {
        state.clay = clayCap;
    }

    if (state.obsidian >= obsidianCap) {
        state.obsidian = obsidianCap;
    }
};

const getMaxGeodes = (blueprint: Blueprint, remaining: number): number => {
    const cache = new Set<string>();
    const queue: State[] = [{
        remaining,
        ores: 0,
        clay: 0,
        obsidian: 0,
        geodes: 0,
        oreRobots: 1,
        clayRobots: 0,
        obsidianRobots: 0,
        didNotBuild: []
    }];

    // We don't have to build robots of a given type if we can build the most expensive dependent robot within one turn
    const maxOreCosts = Math.max(blueprint.oreRobotCost.ore, blueprint.clayRobotCost.ore, blueprint.obsidianRobotCost.ore, blueprint.geodeRobotCost.ore);
    const maxClayCosts = blueprint.obsidianRobotCost.clay;
    const maxObsidianCosts = blueprint.geodeRobotCost.obsidian;

    let maxGeodes = 0;
    while (queue.length) {
        const state = queue.pop();

        optimizeStateForCache(state, maxOreCosts, maxClayCosts, maxObsidianCosts);
        const cacheKey = `${state.remaining}|${state.ores}|${state.clay}|${state.obsidian}|${state.geodes}|${state.oreRobots}|${state.clayRobots}|${state.obsidianRobots}`;
        if (cache.has(cacheKey)) {
            continue;
        }

        cache.add(cacheKey);

        // Collect all resources and decrease the time for the next state. Done here to simplify the construction of the different states below
        const nextState = {
            ...state,
            remaining: state.remaining - 1,
            ores: state.ores + state.oreRobots,
            clay: state.clay + state.clayRobots,
            obsidian: state.obsidian + state.obsidianRobots,
            didNotBuild: []
        };

        if (nextState.remaining === 0) {
            maxGeodes = Math.max(maxGeodes, nextState.geodes);
            continue;
        }

        const canBuild = [];
        if (state.ores >= blueprint.geodeRobotCost.ore && state.obsidian >= blueprint.geodeRobotCost.obsidian && !state.didNotBuild.includes('geode')) {
            canBuild.push('geode');
            queue.push({
                ...nextState,
                ores: nextState.ores - blueprint.geodeRobotCost.ore,
                obsidian: nextState.obsidian - blueprint.geodeRobotCost.obsidian,
                geodes: nextState.geodes + nextState.remaining // This way we don't have to keep track of geode robots :)
            });
        }

        if (state.obsidianRobots < maxObsidianCosts && state.ores >= blueprint.obsidianRobotCost.ore && state.clay >= blueprint.obsidianRobotCost.clay && !state.didNotBuild.includes('obsidian')) {
            canBuild.push('obsidian');
            queue.push({
                ...nextState,
                ores: nextState.ores - blueprint.obsidianRobotCost.ore,
                clay: nextState.clay - blueprint.obsidianRobotCost.clay,
                obsidianRobots: nextState.obsidianRobots + 1
            });
        }

        if (state.clayRobots < maxClayCosts && state.ores >= blueprint.clayRobotCost.ore && !state.didNotBuild.includes('clay')) {
            canBuild.push('clay');
            queue.push({
                ...nextState,
                ores: nextState.ores - blueprint.clayRobotCost.ore,
                clayRobots: nextState.clayRobots + 1
            });
        }

        if (state.oreRobots < maxOreCosts && state.ores >= blueprint.oreRobotCost.ore && !state.didNotBuild.includes('ore')) {
            canBuild.push('ore');
            queue.push({
                ...nextState,
                ores: nextState.ores - blueprint.oreRobotCost.ore,
                oreRobots: nextState.oreRobots + 1
            });
        }

        /* If we don't have enough resources to satisfy our limits, simulate one turn in which we didn't built any robot at all, but tell this turn which ones we could have.
           We then build this robot type only if another robot was built in the mean time */
        if (state.ores < maxOreCosts || state.clay < maxClayCosts || state.obsidian < maxObsidianCosts) {
            queue.push({...nextState, didNotBuild: canBuild});
        }
    }

    return maxGeodes;
};

const part1 = (): number => {
    const blueprints = parseBlueprints(getInput());
    return sum(blueprints.map(b => getMaxGeodes(b, 24) * b.id));
}

const part2 = (): number => {
    const blueprints = parseBlueprints(getInput()).slice(0, 3);
    return multiply(blueprints.map(b => getMaxGeodes(b, 32)));
}

execute([part1, part2]);