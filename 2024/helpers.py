import time

def run(func, input_file):
    with open(input_file, 'r') as file:
        data = file.readlines()

    start_time = time.perf_counter()
    result = func(data)
    end_time = time.perf_counter()

    print(f"{func.__name__}({input_file}) // Result: {result}, took {format_duration(start_time, end_time)}")

def format_duration(start, end):
    duration = end - start  # in seconds
    if duration < 1:
        return "{:.0f}ms".format(duration * 1000)
    else:
        return "{:.3f}s".format(duration)
