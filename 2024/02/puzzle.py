from helpers import run


def part1(data):
    reports = parse_input(data)
    safe_reports = sum(1 for report in reports if is_safe(report))

    return safe_reports


def part2(data):
    reports = parse_input(data)
    safe_reports = sum(1 for report in reports if is_safe_with_tolerance(report))

    return safe_reports


def is_safe(report):
    increasing = report[1] > report[0]
    for i in range(len(report) - 1):
        left = report[i]
        right = report[i + 1]
        diff = left - right
        if abs(diff) < 1 or abs(diff) > 3:
            return False

        if increasing and left > right:
            return False

        if not increasing and left < right:
            return False

    return True


def is_safe_with_tolerance(report):
    if is_safe(report):
        return True

    for iteration in [[report[i] for i in range(len(report)) if i != index] for index in range(len(report))]:
        if is_safe(iteration):
            return True

    return False


def parse_input(data):
    reports = []
    for line in data:
        reports.append(list(map(int, line.split())))

    return reports


run(part1, "example.txt")
run(part1, "input.txt")
run(part2, "example.txt")
run(part2, "input.txt")
