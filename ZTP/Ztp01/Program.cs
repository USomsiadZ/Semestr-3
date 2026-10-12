// Ćwiczenie 1 — wzorce projektowe: Singleton, Multiton i Object Pool.
// Należy zaimplementować bazy danych jako Multiton: dla każdej nazwy może istnieć
// najwyżej jedna baza, ale aplikacja może jednocześnie korzystać z wielu nazwanych baz.
// Dostęp do baz ma odbywać się wyłącznie przez ConnectionManager będący Singletonem.
// Połączenia do każdej bazy należy przechowywać w puli maksymalnie trzech obiektów
// i zwracać je cyklicznie — czwarte pobranie ma zwrócić pierwsze połączenie.
// W metodzie Main trzeba przetestować kilka baz i wykazać, że połączenia do tej samej
// bazy współdzielą jej obiekt oraz że pierwsze i czwarte połączenie są tym samym obiektem.
using System.Linq;
using System;
interface IDatabaseConnection
{
    int AddRecord(string name, int age);

    void UpdateRecord(int id, string newName, int newAge);

    void DeleteRecord(int id);

    Record? GetRecord(int id);

    void ShowAllRecords();
}

// Prosty rekord w bazie danych
class Record
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }

    public Record(int id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }

    public override string ToString()
    {
        return $"Record [ID={Id}, Name={Name}, Age={Age}]";
    }
}

// Prosta baza danych
class Database
{

    private readonly List<Record> records; // Lista przechowująca rekordy
    private int nextId = 1; // Licznik do generowania unikalnych ID

    public Database()
    {
        records = new();
    }

    // Zwraca implementację interfejsu DatabaseConnection
    public IDatabaseConnection GetConnection()
    {
        return new DatabaseConnection(this);
    }

    // Prywatna klasa wewnętrzna implementująca interfejs DatabaseConnection
    // W Javie korzystamy z cech klas wewnętrznych, w C# ta klasa musiałaby posiadać
    // referencję na obiekt klasy Database
    private class DatabaseConnection : IDatabaseConnection
    {
        private readonly Database db;

        public DatabaseConnection(Database database)
        {
            db = database;
        }

        // Dodawanie nowego rekordu
        public int AddRecord(string name, int age)
        {
            Record newRecord = new(db.nextId++, name, age);
            db.records.Add(newRecord);
            Console.WriteLine($"Inserted: {newRecord}");
            return db.nextId - 1; // zwracamy id dodanego rekordu
        }

        // Pobieranie rekordu po ID
        public Record? GetRecord(int id)
        {
            return db.records.Where(rec => rec.Id == id).FirstOrDefault();
        }

        // Aktualizowanie rekordu po ID
        public void UpdateRecord(int id, string newName, int newAge)
        {
            Record? optionalRecord = GetRecord(id);

            if (optionalRecord != null)
            {
                Record record = optionalRecord;
                record.Name = newName;
                record.Age = newAge;
                Console.WriteLine($"Updated: {record}");
            } else
            {
                Console.WriteLine($"Record with ID {id} not found.");
            }
        }

        // Usuwanie rekordu po ID
        public void DeleteRecord(int id)
        {
            Record? optionalRecord = GetRecord(id);

            if (optionalRecord != null)
            {
                db.records.Remove(optionalRecord);
                Console.WriteLine($"Deleted record with ID {id}");
            } else
            {
                Console.WriteLine($"Record with ID {id} not found.");
            }
        }

        // Wyświetlanie wszystkich rekordów
        public void ShowAllRecords()
        {
            if (db.records.Any())
            {
                Console.WriteLine("All records:");
                db.records.ForEach(r => Console.WriteLine(r));
            } else
            {
                Console.WriteLine("No records in the database.");
            }
        }
    }
}

public class Ztp01
{
    public static void Main(string[] args)
    {
        // Tworzenie bazy danych
        Database database = new Database();

        // Tworzenie połączenia z bazą danych (klient operuje na interfejsie)
        IDatabaseConnection connection = database.GetConnection();

        // Operacje na bazie danych za pośrednictwem połączenia
        connection.AddRecord("Piotr", 25);
        connection.ShowAllRecords();
    }
}
