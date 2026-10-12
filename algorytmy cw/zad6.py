# Podaj możliwe najefektywniejsze czasowo rozwiązanie problemu znajdywania różnicy zbiorów
# Przykład:
# Wejście: A={5, 3, 8, 7, 2, 6} B={7, 6, 3, 4, 1, 2, 0, 9}
# Wyjście: A\B={5, 8}
# a) zakres liczb to <0, 10^18>
# b) zakres liczb to <-10^6, 10^6>


def algorytm(A,B):
    C = []
    for i in A:
        if i not in B:
            C.append(A[i])
    return



if __name__ == "__main__":
    A = [5, 3, 8, 7, 2, 6]
    B = [7, 6, 3, 4, 1, 2, 0, 9]

# Dokonczyc implementacje
