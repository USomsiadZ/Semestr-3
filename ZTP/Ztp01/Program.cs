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
// Singleton
class ConnectionManager{
    private static readonly ConnectionManager instance = new(); // tworzy jedną instancję
    public static ConnectionManager Instancje => instance;// daje dostep do singleton
    private ConnectionManager(){}//Prywatny konstruktor
    public IDatabaseConnection GetDatabaseConnection(string databaseName){
        return Database.GetInstance(databaseName).GetConnection();
    }
}
// Prosta baza danych
class Database
{   
    //Multiton
    private static readonly Dictionary<string, Database> instances = new();
    public static Database GetInstance(string name){
        if(!instances.ContainsKey(name)){
            instances[name] = new Database();
        }
        return instances[name];
    }
    private readonly List<Record> records; // Lista przechowująca rekordy
    private int nextId = 1; // Licznik do generowania unikalnych ID

    private Database()
    {
        records = new();
    }

    // Zwraca implementację interfejsu DatabaseConnection
    // public IDatabaseConnection GetConnection()
    // {
    //     return new DatabaseConnection(this);
    // }
    // Object pool
    private readonly List<IDatabaseConnection> pool = new();
    private int index = 0;
    public IDatabaseConnection GetConnection()
    {
        if(pool.Count < 3){
            pool.Add(new DatabaseConnection(this));
        }
        IDatabaseConnection connection = pool[index % pool.Count];
        index++;
        return connection;
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
        /*
        // Tworzenie bazy danych
        // Database database = new Database();
        Database db1 = Database.GetInstance("student");
        Database db2 = Database.GetInstance("nauczyciel");
        Database db3 = Database.GetInstance("student");

        // Tworzenie połączenia z bazą danych (klient operuje na interfejsie)
        IDatabaseConnection connection = db1.GetConnection();

        // Operacje na bazie danych za pośrednictwem połączenia
        connection.AddRecord("Piotr", 25);
        connection.ShowAllRecords();
        // Sprawdzenie multiton
        Console.WriteLine(ReferenceEquals(db1,db2));//false
        Console.WriteLine(ReferenceEquals(db1,db3));//true
        */
        // Sprawdzenie object pool
        var manager = ConnectionManager.Instancje;
        var c1 = manager.GetDatabaseConnection("Student");
        var c2 = manager.GetDatabaseConnection("Student");
        var c3 = manager.GetDatabaseConnection("Student");
        var c4 = manager.GetDatabaseConnection("Student");
        Console.WriteLine(ReferenceEquals(c1, c2));//false
        Console.WriteLine(ReferenceEquals(c1, c3));//false
        Console.WriteLine(ReferenceEquals(c1, c4));//true
        c1.AddRecord("Piotr",25);
        c2.ShowAllRecords();
        var c5 = manager.GetDatabaseConnection("Student");
        c5.ShowAllRecords();
        var nauczyciel = manager.GetDatabaseConnection("Nauczyciel");
        Console.WriteLine("---------");
        nauczyciel.AddRecord("Anna",40);
        c1.ShowAllRecords();
        Console.WriteLine("---------");
        nauczyciel.ShowAllRecords();
    }
}
// Baza - multiton ✓
// Main -> ConnectionManager - singleton ✓
// Połączeniami - object pool: max3-cyklicznie