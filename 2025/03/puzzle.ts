import { getInput, execute, splitLinesIntoArray } from '../utils';

const part1 = (): number => {
    return calculateMaxPower(2);
};

const part2 = (): number => {
    return calculateMaxPower(12);
};

const calculateMaxPower = (numberOfBatteriesToTurnOn: number): number => {
    const banks = splitLinesIntoArray(getInput()).map((line) => line.split('').map(Number));
    let power = 0;

    for (const bank of banks) {
        const relevantBatteries = [...bank];
        while (relevantBatteries.length > numberOfBatteriesToTurnOn) {
            let removedBattery = false;
            for (let i = 0; i < relevantBatteries.length - 1; i++) {
                if (relevantBatteries[i] < relevantBatteries[i + 1]) {
                    relevantBatteries.splice(i, 1);
                    removedBattery = true;
                    break;
                }
            }

            if (!removedBattery) {
                relevantBatteries.splice(relevantBatteries.length - 1, 1);
            }
        }

        power += Number(relevantBatteries.reduce((acc, curr) => acc + curr.toString(), ''));
    }

    return power;
};

execute([part1, part2]);
