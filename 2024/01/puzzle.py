from helpers import run


def part1(data):
    left, right = parse_input(data)
    total_distance = sum(abs(l - r) for l, r in zip(left, right))

    return total_distance


def part2(data):
    left, right = parse_input(data)
    similarity = sum(l * right.count(l) for l in left)

    return similarity


def parse_input(data):
    left, right = [], []

    for line in data:
        if line.strip():
            l, r = map(int, line.split())
            left.append(l)
            right.append(r)

    left.sort()
    right.sort()

    return left, right


run(part1, "example.txt")
run(part1, "input.txt")
run(part2, "example.txt")
run(part2, "input.txt")
