using System;
using System.Linq;

interface IDatabaseConnection
{
    int AddRecord(string name, int age);

    void UpdateRecord(int id, string newName, int newAge);

    void DeleteRecord(int id);

    Record? GetRecord(int id);

    void ShowAllRecords();
}
interface ConnectionManager
{

    DatabaseConnection getConnection(String databaseName);

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

    //-Multiton
    // Dodanie słownika
    // Prywatny konstruktor
    // Multiton-GetInstance
    private static readonly Dictionary<string, Database> instances = new();
    private readonly List<Record> records; // Lista przechowująca rekordy
    private int nextId = 1; // Licznik do generowania unikalnych ID

    private Database()
    {
        records = new();
    }

    // Zwraca implementację interfejsu DatabaseConnection
    public IDatabaseConnection GetConnection()
    {
        return new DatabaseConnection(this);
    }
    // Dodanie multiton
    public static Database GetInstance(string name)
    {
        if(!instances.ContainsKey(name)){
            instances[name] = new();
        }
        return instances[name];
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
            }
            else
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
            }
            else
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
            }
            else
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
        // Database database = new();
        Database db1 = Database.GetInstance("Student");
        Database db2 = Database.GetInstance("Pracownicy");
        Database db3 = Database.GetInstance("Student");

        // Tworzenie połączenia z bazą danych (klient operuje na interfejsie)
        IDatabaseConnection connection = db1.GetConnection();

        // Operacje na bazie danych za pośrednictwem połączenia
        connection.AddRecord("Piotr", 25);
        connection.ShowAllRecords();
        Console.WriteLine(ReferenceEquals(db1,db2));//false
        Console.WriteLine(ReferenceEquals(db1,db3));//true

    }
}
// Dodać singleton ConnectionManager 
// Baza ma być multitonem Database ✓
// Połączeniami powinien zarządzać pula obiektów ConnectionPool