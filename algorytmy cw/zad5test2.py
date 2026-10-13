def fi(n):
    n %= 60
    a = b = 1
    c = 0
    if n < 1:
        return 0
    if n < 3:
        return 1
    for i in range(2,n):
        c = (a+b)%10
        a = b
        b = c
    return c