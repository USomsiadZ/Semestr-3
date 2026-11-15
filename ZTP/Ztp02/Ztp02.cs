using System;
using System.Collections.Generic;
using System.Text;

abstract class Cell
{
    public abstract override string ToString();
}

class StringCell : Cell
{
    private string value;

    public StringCell(string value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return value.PadRight(15);
    }
}

class NumberCell : Cell
{
    private double value;

    public NumberCell(double value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        return value.ToString("F2").PadLeft(15);
    }
}

class BoolCell : Cell
{
    private bool value;

    public BoolCell(bool value)
    {
        this.value = value;
    }

    public override string ToString()
    {
        if (value)
        {
            return "YES".PadRight(15);
        }

        return "NO".PadRight(15);
    }
}

abstract class CellFactory
{
    public abstract Cell CreateCell(object value);
    public abstract Cell CreateDefaultCell();
}

class StringCellFactory : CellFactory
{
    public override Cell CreateCell(object value)
    {
        if (!(value is string))
        {
            throw new ArgumentException("Kolumna wymaga wartości tekstowej.");
        }

        return new StringCell((string)value);
    }

    public override Cell CreateDefaultCell()
    {
        return new StringCell("");
    }
}

class NumberCellFactory : CellFactory
{
    public override Cell CreateCell(object value)
    {
        if (!(value is double))
        {
            throw new ArgumentException("Kolumna wymaga wartości liczbowej typu double.");
        }

        return new NumberCell((double)value);
    }

    public override Cell CreateDefaultCell()
    {
        return new NumberCell(0.0);
    }
}

class BoolCellFactory : CellFactory
{
    public override Cell CreateCell(object value)
    {
        if (!(value is bool))
        {
            throw new ArgumentException("Kolumna wymaga wartości logicznej.");
        }

        return new BoolCell((bool)value);
    }

    public override Cell CreateDefaultCell()
    {
        return new BoolCell(false);
    }
}

class Header
{
    public string Name { get; private set; }
    private CellFactory factory;

    public Header(string name, CellFactory factory)
    {
        Name = name;
        this.factory = factory;
    }

    public Cell CreateCell(object value)
    {
        return factory.CreateCell(value);
    }

    public Cell CreateDefaultCell()
    {
        return factory.CreateDefaultCell();
    }
}

class Table
{
    private List<Header> headers;
    private List<List<Cell>> rows;

    public Table()
    {
        headers = new List<Header>();
        rows = new List<List<Cell>>();
    }

    public void AddColumn(Header header)
    {
        headers.Add(header);

        foreach (List<Cell> row in rows)
        {
            row.Add(header.CreateDefaultCell());
        }
    }

    public void AddRow(params object[] values)
    {
        if (values.Length != headers.Count)
        {
            throw new ArgumentException("Liczba wartości nie zgadza się z liczbą kolumn.");
        }

        List<Cell> newRow = new List<Cell>();

        for (int i = 0; i < values.Length; i++)
        {
            newRow.Add(headers[i].CreateCell(values[i]));
        }

        rows.Add(newRow);
    }

    public override string ToString()
    {
        StringBuilder text = new StringBuilder();

        foreach (Header header in headers)
        {
            text.Append(header.Name.PadRight(15));
        }

        text.AppendLine();
        text.AppendLine(new string('-', headers.Count * 15));

        foreach (List<Cell> row in rows)
        {
            foreach (Cell cell in row)
            {
                text.Append(cell.ToString());
            }

            text.AppendLine();
        }

        return text.ToString();
    }
}

class Program
{
    static void Main(string[] args)
    {
        Table table = new Table();

        table.AddColumn(new Header("Name", new StringCellFactory()));
        table.AddColumn(new Header("Age", new NumberCellFactory()));
        table.AddColumn(new Header("Is Student", new BoolCellFactory()));

        table.AddRow("Alice", 30.5, true);
        table.AddRow("Bob", 25.0, false);
        table.AddRow("Charlie", 35.75, true);

        Console.WriteLine(table.ToString());

        table.AddColumn(new Header("Salary", new NumberCellFactory()));
        Console.WriteLine(table.ToString());

        try
        {
            table.AddRow("David", "zly typ", true, 4000.0);
        }
        catch (ArgumentException error)
        {
            Console.WriteLine(error.Message);
        }
    }
}
