import re

from helpers import run


def part1(data):
    pattern = r'mul\((\d+),(\d+)\)'
    section = parse_input(data)
    matches = re.findall(pattern, section)
    result = sum(int(match[0]) * int(match[1]) for match in matches)

    return result


def part2(data):
    pattern = r"(don't\(\))|(do\(\))|(mul\((\d+),(\d+)\))"
    section = parse_input(data)
    result = 0
    skip = False
    matches = re.findall(pattern, section)
    for match in matches:
        if match[0]:
            skip = True
        elif match[1]:
            skip = False
        elif not skip:
            result += int(match[3]) * int(match[4])

    return result


def parse_input(data):
    return ''.join(data)


run(part1, "example.txt")
run(part1, "input.txt")
run(part2, "example.txt")
run(part2, "input.txt")
