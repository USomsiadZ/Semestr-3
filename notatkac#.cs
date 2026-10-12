// C# — SKŁADNIA W MAŁYCH PORCJACH
// Od klas wzwyż. Każda porcja: zasada -> przykład -> pułapka.
// Ucz się 2–3 porcje naraz; odtwórz przykład z pamięci i zmień jeden element.
// Kod jest poza komentarzami, aby edytor podświetlał składnię. To niezależne fragmenty.
// Fragmenty nie tworzą wspólnego programu; nie wymagają kompilacji jako całość.
// Baza: wykłady PO (bez po-10) i ztp-01. Przykłady skrócone i doprecyzowane.
// C# 12 / .NET 8+. Regiony można zwijać w edytorze.

class Punkt
{
    public int X;
    public Punkt(int x) { this.X = x; } // this = bieżący obiekt
    public Punkt() : this(0) { }       // inny konstruktor tej klasy
}
var p = new Punkt(3);                  // var: kompilator ustala typ Punkt
// Po dodaniu własnego konstruktora klasa nie dostaje automatycznie bezparametrowego.

// 02. WŁAŚCIWOŚĆ Z LOGIKĄ
// Pole przechowuje dane; property może kontrolować ich odczyt i zmianę.
private int wiek;
public int Wiek
{
    get => wiek;                      // skrót: return wiek;
    set
    {                             // value = nowa wartość
        if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
        wiek = value;
    }
}
osoba.Wiek = 20;                       // setter; osoba.Wiek przy odczycie -> getter
// nameof(value) zwraca nazwę „value”, nie zawartość zmiennej.

// 03. GET / SET / INIT
public int Id { get; private set; }    // zmiana tylko z kodu tego typu
public string Miasto { get; init; } = ""; // ustawianie w fazie inicjalizacji
public bool Dorosly => Wiek >= 18;     // obliczana, bez settera
var osoba = new Osoba { Miasto = "Łomża" }; // konstruktor, potem initializer
// init blokuje późniejsze przypisanie, ale nie zamraża obiektu w property.

// 04. DOSTĘP
// public: wszędzie, o ile typ też jest dostępny; private: wewnątrz typu.
// protected: typ i jego pochodne; internal: ten sam assembly (.dll/.exe).
// protected internal: internal LUB protected; private protected: protected W TYM assembly.
// Klasa w namespace domyślnie jest internal; składowe klasy są private.
// Zwykła deklaracja metody interfejsu jest domyślnie public.

// 05. STATIC / CONST / READONLY
public static int Licznik;            // wspólny dla instancji; Typ.Licznik
public readonly List<int> Dane = new(); // pole przypisywalne w deklaracji/konstruktorze
public static readonly DateTime Start = DateTime.Now;
// Dane.Add(1) działa; późniejsze Dane = new() nie. readonly nie zamraża listy.
static Typ() { Licznik = 0; }        // inicjalizacja składowych statycznych
// Inicjalizuje typ raz; nie ma parametrów ani modyfikatora dostępu.
// static class ma tylko składowe statyczne; nie można tworzyć jej instancji.

// 06. WARTOŚĆ CZY REFERENCJA?
int a = 1; int b = a; b = 2;         // a nadal 1: kopia wartości
var a = new Osoba(); var b = a;       // jedna instancja, dwie referencje
b.Imie = "Jan";                       // a.Imie też „Jan”
// struct, enum, int: wartościowe; class, tablica, List: referencyjne.
// struct kopiuje swoje pola; pole referencyjne nadal wskazuje ten sam obiekt.
// string jest referencyjny, lecz niemutowalny: operacje tworzą nowe teksty.
// Nie zakładaj: „wartościowy zawsze na stosie”. var nie oznacza dynamic.
#endregion

#region B — Metody i parametry
// 07. METODA + PRZECIĄŻANIE
int Dodaj(int a, int b) => a + b;
double Dodaj(double a, double b) => a + b;
Dodaj(2, 3);                          // wersja int
// Ta sama nazwa, inne parametry. Sama zmiana typu wyniku/nazwy parametru nie wystarczy.
// => jest skrótem ciała; void oznacza brak wyniku; return zwraca wynik/kończy metodę.

// 08. OPCJONALNE / NAZWANE / PARAMS
void Log(string tekst, string poziom = "INFO") => Console.WriteLine($"[{poziom}] {tekst}");
Log(poziom: "WARN", tekst: "Błąd");    // nazwane: wskazujesz, co przekazujesz
int Suma(params int[] liczby) => liczby.Sum(); // wymaga System.Linq
Suma(1, 2, 3);                        // params: ostatni parametr, wiele argumentów
Suma();                              // pusta tablica; można też przekazać tablicę
// Parametry opcjonalne umieszczamy po wymaganych.

// 09. REF / OUT / IN
void Zmien(ref int x) { x = 10; }     // dostęp do zmiennej; wcześniej zainicjalizuj
int a = 1; Zmien(ref a);              // a = 10; ref po obu stronach
bool ok = int.TryParse("12", out int n); // out: wynik, bez wcześniejszej inicjalizacji
void Pokaz(in int x) => Console.WriteLine(x);          // tylko odczyt zmiennej przez referencję
void PokazRef(ref readonly int x) => Console.WriteLine(x); // C# 12; wywołanie zalecane z in/ref
// out musi być przypisane przed normalnym powrotem. in przy wywołaniu jest opcjonalne.
// Bez modyfikatora kopiujesz wartość; dla klasy tą wartością jest REFERENCJA.
// Zmiana obj.Imie działa na wspólnym obiekcie; obj = new() podmienia tylko lokalną kopię.
// ref pozwala podmienić zmienną wywołującego. in nie zamraża obiektu klasy.
// Nie przeciążaj wyłącznie różnicą ref/out/in/ref readonly.
#endregion

#region C — Współpraca i dziedziczenie
// 10. DZIEDZICZENIE + BASE
class Pies : Zwierze
{
    public Pies(string imie) : base(imie) { } // konstruktor bazowy
}
// Jedna klasa bazowa; konstruktory nie są dziedziczone.
// Ciało konstruktora bazowego działa przed ciałem pochodnego.
// base.Metoda() wywołuje implementację bazową; this(...) deleguje w tej samej klasie.

// 11. VIRTUAL / OVERRIDE / NEW / SEALED
// w bazowej:
public virtual string Glos() => "?";
    // w pochodnej:
    public override string Glos() => "Hau";
Zwierze z = new Pies("Reks"); z.Glos();     // „Hau”: rzeczywisty typ obiektu
// new przy metodzie UKRYWA ją: wybór zależy od typu zmiennej/wyrażenia.
// sealed class blokuje dziedziczenie; sealed override blokuje dalsze nadpisanie.
// overload = inne parametry; override = nowa implementacja metody wirtualnej.
public override string ToString() => $"Imię: {Imie}"; // opis; $ = interpolacja

// 12. ABSTRACT + INTERFACE
abstract class Figura { public abstract double Pole(); } // bez ciała, z ;
interface IDruk { void Drukuj(); }   // kontrakt zachowania
class Raport : Bazowa, IDruk
{
    public void Drukuj() => Console.WriteLine("Raport"); // zwykła implementacja
}
// Nie tworzysz new Figura(); konkretna klasa musi nadpisać abstract.
// Klasa może implementować wiele interfejsów; interface I : IA, IB łączy kontrakty.
void IDruk.Drukuj () => Console.WriteLine("Raport"); // alternatywa: implementacja jawna
// Do implementacji jawnej dostęp przez ((IDruk)raport).Drukuj().
// Interfejs może deklarować property; może też mieć domyślne implementacje.

// 13. IS / AS / TYP / BOXING
object x = new Pies("Reks");
if (x is Pies p) { p.Glos(); }       // sprawdzenie + zmienna właściwego typu
Pies? mozePies = x as Pies;                 // brak dopasowania -> null; cast (Pies)x -> wyjątek
Type typBazowy = typeof(Zwierze); Type typObiektu = x.GetType();        // typ wskazany w kodzie / rzeczywisty typ obiektu
object box = 7; int n = (int)box;    // boxing / unboxing
// (long)box jest błędne w runtime: najpierw (int)box, potem konwersja do long.
// as działa dla referencji i nullable; nie używaj „as int”.

// 14. NULL I OPERATORY
string? imie = null;                 // referencja może być null
int? wiek = null;                    // Nullable<int>: brak wartości liczbowej
int dlugosc = imie?.Length ?? 0;     // ?. pomija dostęp dla null; ?? wartość zastępcza
imie ??= "Jan";                      // przypisz, tylko jeśli null
// imie! tłumi ostrzeżenie kompilatora, NIE usuwa null. Włącz #nullable enable.
// T? dla nieograniczonego generycznego T nie gwarantuje Nullable<T> dla int.

// 15. KOMPOZYCJA / ENUM / ZAGNIEŻDŻENIE
class Auto { private readonly Silnik silnik = new(); } // „ma”; dziedziczenie: „jest”
enum Status : byte { Nowe = 0, Gotowe = 1 }
Status s = Status.Gotowe;            // nazwane wartości; (Status)99 też jest możliwe!
class A { public class B { } }       // tworzenie: new A.B()
// B nie musi być static. Nie ma automatycznej referencji do instancji A;
// aby odczytać jej stan, potrzebuje konkretnego obiektu A.
#endregion

#region D — Kolekcje i własne typy
// 16. WYBÓR KOLEKCJI — System.Collections.Generic
var lista = new List<int> { 1, 2 };  // kolejność, indeks; lista[0], Count
lista.Add(3); lista.Remove(2);       // po wartości; RemoveAt(0) po indeksie
var mapa = new Dictionary<string, int> { ["Jan"] = 20 };
mapa["Jan"] = 21;                    // dodaje lub nadpisuje
if (mapa.TryGetValue("Jan", out int wiek)) { Console.WriteLine(wiek); }
var zbior = new HashSet<int> { 1, 1, 2 }; // unikalność; Count = 2, bez indeksów
var q = new Queue<int>(); q.Enqueue(1); q.Dequeue(); // FIFO
var s = new Stack<int>(); s.Push(1); s.Pop();       // LIFO
// Peek odczytuje bez usuwania; pusta kolejka/stos -> użyj TryDequeue/TryPop.
// Odczyt mapa[brak] rzuca wyjątek; Add z istniejącym kluczem również.
// Tablica ma Length, lista Count. new Osoba[3] nie tworzy trzech obiektów Osoba.
// HashSet/Dictionary wymagają spójnych Equals/GetHashCode; nie zmieniaj hasza klucza.

// 17. GENERYKI: <T> + WHERE
class Box<T> { public T Value { get; set; } public Box(T x) { Value = x; } }
var box = new Box<int>(3);           // T = int; Box<string> to inny typ
T Identity<T>(T x) => x;
T Nowy<T>() where T : new() => new T(); // publiczny konstruktor bezparametrowy
// Ograniczenia: class (referencja), struct (wartość), Bazowa, IKontrakt.
// class i struct nie łączymy.
class Repo<T> where T : Bazowa, IKontrakt, new() { } // new() ostatnie
T Max<T>(T a, T b) where T : IComparable<T> => a.CompareTo(b) >= 0 ? a : b;
// default(T): int -> 0, bool -> false, referencja -> null. T to typ, nie wartość.
T Dodaj<T>(T a, T b) where T : IAdditionOperators<T, T, T> => a + b; // System.Numerics

// 18. WARIANCJA IN / OUT
IEnumerable<string> napisy = new List<string>();
IEnumerable<object> obiekty = napisy; // out T: kowariancja (wyniki)
Action<object> pokaz = x => Console.WriteLine(x);
Action<string> pokazNapis = pokaz;    // in T: kontrawariancja (wejście)
// List<string> nie staje się List<object>. Wariancja: interfejsy/delegaty, typy referencyjne.
// Własne deklaracje:
interface IZrodlo<out T> { T Pobierz(); }
interface IOdbiorca<in T>
{
    void Przyjmij(T x); }
// To inne in/out niż przy przekazywaniu parametrów metod!

// 19. OPERATOR / KONWERSJA / INDEKSATOR — deklaracje we własnym typie
public static Wektor operator +(Wektor a, Wektor b) => new(a.X + b.X, a.Y + b.Y);
    public static implicit operator double(Liczba n) => n.Value; // bez castu
    public static explicit operator int(Liczba n) => (int)n.Value; // wymaga (int)n
    public int this[int i] { get => dane[i]; set => dane[i] = value; } // obj[i]
// = nie jest przeciążalne. Dla class kopiuje referencję, dla struct wartość.
// W C# 12 a += b korzysta z +. == i != deklaruj parami i uzgodnij z Equals/GetHashCode.
// Domyślne == zwykłej klasy porównuje referencje; string porównuje treść.
    #endregion

    #region E — Błędy, zdarzenia i dane
    // 20. TRY / CATCH / FINALLY / THROW
try { Operacja(); }
catch (ArgumentException e) when (e.ParamName == "wiek") { Console.WriteLine(e.Message); }
finally { Sprzatanie(); }
throw new ArgumentException("Opis błędu"); // zgłoszenie
throw;                                    // wewnątrz catch: ponowne rzucenie
// Najpierw catch szczegółowy, potem ogólny. when dodaje warunek.
// throw e zmienia ślad stosu; throw zachowuje. Nie zostawiaj pustego catch.
// Opakowanie z przyczyną: new MojException("Opis", e), konstruktor : base(opis, e).
// finally działa przy zwykłym wyjściu, także return/wyjątku; nie np. po zabiciu procesu.

// 21. USING: NAZWY A ZASOBY
using System.IO;                     // skraca nazwy, nie instaluje biblioteki
namespace Nauka;                     // grupuje typy; nie określa assembly
using Tekst = System.String;         // alias nazwy
using var r = new StreamReader("plik.txt"); // Dispose przy wyjściu z zakresu
using (var r = new StreamReader("plik.txt")) { Console.WriteLine(r.ReadLine()); } // Dispose po bloku
// Dispose zamyka zasoby, GC odzyskuje pamięć. Finalizator nie zastępuje using.

// 22. DELEGAT / LAMBDA / EVENT
delegate int Operacja(int a, int b); // typ opisujący sygnaturę metody
Func<int, int> kwadrat = x => x * x;  // ostatni typ Func to wynik
Action<string> pokaz = x => Console.WriteLine(x); // Action -> void
Predicate<int> dodatnia = x => x > 0; // Predicate -> bool
public event EventHandler? Zmiana;   // deklaracja w klasie
Zmiana?.Invoke(this, EventArgs.Empty); // emitowanie wewnątrz tej klasy
obj.Zmiana += Handler; obj.Zmiana -= Handler; // podpinanie/odpinanie odbiorcy
// Handler ma argumenty (object? sender, EventArgs e). Odpinaj ten sam delegat.
// Windows Forms (wymaga Windows):
button.Click += (s, e) => label.Text = "OK";

// 23. LINQ + JSON + PLIKI
// LINQ jest uzupełnieniem do lambd; using System.Linq:
var wynik = liczby.Where(x => x > 0).Select(x => x * x).ToList();
// Where filtruje, Select przekształca, ToList materializuje; zapytanie zwykle jest odroczone.
// using System.Text.Json oraz System.IO:
string json = JsonSerializer.Serialize(osoby);
var dane = JsonSerializer.Deserialize<List<Osoba>>(json) ?? new();
File.WriteAllText("osoby.json", json); // tworzy lub NADPISUJE
string tekst = File.ReadAllText("osoby.json");
// Domyślnie JSON obejmuje publiczne properties. Deserialize może zwrócić null/rzucić JsonException.
// Ścieżka względna dotyczy katalogu procesu, nie pliku .cs. ReadLine() -> null przy końcu pliku.
#endregion

#region F — Składnia z ZTP, po opanowaniu A–E
// 24. DI + WZORCE: CZEGO SZUKAĆ W KODZIE?
private readonly IPlatnosc platnosc;
public Zamowienie(IPlatnosc p) { platnosc = p; } // DI: zależność przez konstruktor
// Klient korzysta z kontraktu; implementację wybiera kod tworzący obiekt.
// Singleton: private konstruktor + static readonly instancja + public static property.
// Multiton: prywatny konstruktor + Dictionary<klucz, instancja> + metoda Pobierz(klucz).
// Object Pool: Queue<T> wolnych + Func<T> fabryka; Pobierz/Zwroc zamiast ciągłego new.
// To sposoby projektowania, nie osobne słowa kluczowe C#. Same wzorce nie gwarantują
// bezpieczeństwa wielowątkowego, resetu stanu czy poprawnego zwalniania zasobów.
#endregion
