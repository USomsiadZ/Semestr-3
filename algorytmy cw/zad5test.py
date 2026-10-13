def fibonacci(n):
    n %= 60
    if n < 0:
        return 0
    if n < 3:
        return 1
    a = 1
    b = 1
    c = 0
    for i in range(2,n):
        c = (a+b) % 10
        a = b
        b = c
    return c
print(fibonacci(3))