# Wariant B – „Największy spójny fragment”
# WP: Ciąg liczb całkowitych A ={ai} o długości n (0=<n<=100000), spełniających warunek
# -100000<=ai <=+100000.
# WK: Spójny fragment ciągu A o największej sumie. Jeżeli w ciągu występuje więcej niż jeden
# spójny fragment o największej sumie, to wynikiem jest fragment, który w ciągu zaczyna
# się w najmniejszym indeksie.
# Opis wejścia i wyjścia: Na wejściu, w pliku tekstowym o nazwie in_B_grupa_nazwisko.txt
# podawana jest w pierwszej linii długość ciągu A. W kolejnych n liniach wejścia zapisane są
# kolejne wyrazy ciągu A. Na wyjściu, w pliku tekstowym out_B_grupa_nazwisko.txt
# ASD, zestaw zadań nr 1, pracownia specjalistyczna, informatyka, studia stacjonarne
# J.Koszelew
# 2
# podawane są w poszczególnych liniach (co najmniej dwóch), wyniki rozwiązań złożone z
# dwóch wartości p i k, gdzie p to indeks początkowy wynikowego fragmentu, a k to indeks
# końcowy wynikowego fragmentu ciągu A. Indeksy ciągu zaczynają się od zera.
# Przykład
# in_B_1_Nowak.txt:
# 6
# 5
# 2
# -13
# 5
# -5
# 6
# Out_B_1_Nowak.txt:
# 0 1
# 0 1


from random import randint


def pierwszy(dlugosc,tablica):
    max_suma = tablica[0]
    poczatek = koniec = 0
    operacje_elementarne = 0
    if dlugosc < 1 or not tablica:
        return 0, 0
    for i in range(dlugosc):
        suma = 0
        for j in range(i,dlugosc):
            suma += tablica[j]
            operacje_elementarne += 2
            if suma > max_suma:
                max_suma = suma
                poczatek = i
                koniec = j
    print(f"Operację elementarne {operacje_elementarne}")
    return poczatek,koniec
    # output (3,7)✓
    # O(dlugosc) = n^2

# iteracja
# if suma < 0: suma = 0 i reset x,y
# best = max(best,suma)
def drugi(dlugosc,tablica):
    suma = 0
    best = tablica[0]
    operacje_elementarne = 0
    poczatek = sub_poczatek = koniec = 0
    if dlugosc < 1 or not tablica:
        return 0,0
    for i in range(dlugosc):
        suma += tablica[i]
        operacje_elementarne += 2
        if best < suma:
            best = suma
            koniec = i
            poczatek = sub_poczatek
        if suma < 0:
            suma = 0
            sub_poczatek = i + 1
    print(f"Operację elementarne {operacje_elementarne}")
    return poczatek,koniec
    # output (3,7)✓
    # O(dlugosc) = n


def main():
    statycznie = 0
    if statycznie == 1:
        with open("in_B_1_Parzych.txt") as plik:
            dane = plik.read().split()
        dlugosc = int(dane[0])
        tablica = [int(x) for x in dane[1:dlugosc + 1]]
        # print(dlugosc,tablica)
        print(pierwszy(dlugosc,tablica))
        print(drugi(dlugosc,tablica))
    else:
        dlugosc = randint(0,100)
        tablica = []
        for i in range(dlugosc):
            tablica.append(randint(-100000, 100000))
        print(pierwszy(dlugosc, tablica))
        print(drugi(dlugosc, tablica))


if __name__ == "__main__":
    main()
