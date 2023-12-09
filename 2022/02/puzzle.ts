import {execute, getInput, isSampleInput, splitLinesIntoArray, sum} from "../utils";

enum Shape {
    UNKNOWN = 0,
    ROCK = 1,
    PAPER = 2,
    SCISSOR = 3
}

interface Round {
    opponent: Shape,
    me: Shape
}

const getShape = (input: string): Shape => {
    switch (input) {
        case "A":
        case "X":
            return Shape.ROCK;
        case "B":
        case "Y":
            return Shape.PAPER;
        case "C":
        case "Z":
            return Shape.SCISSOR;
    }

    return Shape.UNKNOWN;
};

const getShapeForOutcome = (opponent: Shape, outcome: string) => {
    if (outcome == "Y") {
        return opponent;
    } else if (outcome == "X") {
        switch (opponent) {
            case Shape.PAPER:
                return Shape.ROCK;
            case Shape.SCISSOR:
                return Shape.PAPER;
            case Shape.ROCK:
                return Shape.SCISSOR;
        }
    } else if (outcome == "Z") {
        switch (opponent) {
            case Shape.PAPER:
                return Shape.SCISSOR;
            case Shape.SCISSOR:
                return Shape.ROCK;
            case Shape.ROCK:
                return Shape.PAPER;
        }
    }

    return Shape.UNKNOWN;
};

const playRound = (round: Round): number => {
    let score = round.me;
    if (round.me == round.opponent) {
        score += 3;
    } else if (round.me == Shape.PAPER && round.opponent == Shape.ROCK
        || round.me == Shape.ROCK && round.opponent == Shape.SCISSOR
        || round.me == Shape.SCISSOR && round.opponent == Shape.PAPER) {
        score += 6;
    }

    return score
};

const part1 = (): number => {
    return sum(splitLinesIntoArray(getInput())
        .map(round => round.split(" "))
        .map(hands => <Round>{opponent: getShape(hands[0]), me: getShape(hands[1])})
        .map(playRound));
}

const part2 = (): number => {
    return sum(splitLinesIntoArray(getInput())
        .map(round => round.split(" "))
        .map(hands => <Round>{opponent: getShape(hands[0]), me: getShapeForOutcome(getShape(hands[0]), hands[1])})
        .map(playRound));
}

execute([part1, part2]);