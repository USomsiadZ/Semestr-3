# Wariant B – „Najwikszy spójny fragment”
# WP: Cig liczb całkowitych A ={ai} o długoci n (0=<n<=100000), spełniajcych warunek
# -100000<=ai <=+100000.
# WK: Spójny fragment cigu A o najwikszej sumie. Jeeli w cigu wystpuje wicej ni jeden
# spójny fragment o najwikszej sumie, to wynikiem jest fragment, który w cigu zaczyna
# si w najmniejszym indeksie.
# Opis wejcia i wyjcia: Na wejciu, w pliku tekstowym o nazwie in_B_grupa_nazwisko.txt
# podawana jest w pierwszej linii długocigu A. W kolejnych n liniach wejcia zapisane s
# kolejne wyrazy cigu A. Na wyjciu, w pliku tekstowym out_B_grupa_nazwisko.txt
# ASD, zestaw zada nr 1, pracownia specjalistyczna, informatyka, studia stacjonarne
# J.Koszelew
# 2
# podawane s w poszczególnych liniach (co najmniej dwóch), wyniki rozwiza złoone z
# dwóch wartoci p i k, gdzie p to indeks pocztkowy wynikowego fragmentu, a k to indeks
# kocowy wynikowego fragmentu cigu A. Indeksy cigu zaczynaj si od zera.
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


def pierwszy(dlugosc,tablica):
    max_suma = tablica[0]
    poczatek = koniec = 0
    for i in range(dlugosc):
        suma = 0
        for j in range(i,dlugosc):
            suma += tablica[j]
            if suma > max_suma:
                max_suma = suma
                poczatek = i
                koniec = j
    return poczatek,koniec
    #output (3,7)✓

#iteracja
#if suma < 0: suma = 0 i reset x,y
#best = max(best,suma)
def drugi(dlugosc,tablica):
    suma = 0
    best = tablica[0]
    poczatek = sub_poczatek = koniec = 0
    if dlugosc == 0:
        return 0,0
    for i in range(dlugosc):
        suma += tablica[i]
        if best < suma:
            best = suma
            koniec = i
            poczatek = sub_poczatek
        if suma < 0:
            suma = 0
            sub_poczatek = i + 1
    return poczatek,koniec


def main():
    with open("in_B_1_Parzych.txt") as plik:
        dane = plik.read().split()

    dlugosc = int(dane[0])
    tablica = [int(x) for x in dane[1:dlugosc + 1]]
    # print(dlugosc,tablica)
    print(pierwszy(dlugosc,tablica))
    print(drugi(dlugosc,tablica))
    


if __name__ == "__main__":
    main()