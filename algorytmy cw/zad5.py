# Zaproponuj możliwie najefektywniejszy algorytm rozwiązujący problem wyznaczania ostatniej
# cyfry w wartości n-tej liczby Fibonacciego (Fn) dla danej wartości n. Określ pesymistyczną
# złożoność czasową swojego rozwiązania.
def fibonacci(n):
    n %= 60
    if n == 0:
        return 0
    a = 1
    b = 1
    c = 0
    if n < 3:
        return 1
    for i in range(2,n):
        c = (a+b)%10
        a = b
        b = c
    return c

if __name__ == "__main__":
    print(fibonacci(1))
    print(fibonacci(2))
    print(fibonacci(3))
    print(fibonacci(4))
    print(fibonacci(5))

# Program wykonuje co najwyżej 59 iteracji
# O(1)