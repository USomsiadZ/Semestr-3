using System.Collections.Generic;
using System;
class Circle
{
    private double radius;

    public Circle(double radius)
    {
        this.radius = radius;
    }

    public double CalculatePerimeter()
    {
        return 2 * Math.PI * radius;
    }

    public double CalculateArea()
    {
        return Math.PI * radius * radius;
    }
}

class Rectangle
{
    private double width, height;

    public Rectangle(double width, double height)
    {
        this.width = width;
        this.height = height;
    }

    public double Perimeter()
    {
        return 2 * (width + height);
    }

    public double Area()
    {
        return width * height;
    }
}

class Trojkat
{
    private int a, b, c;

    public Trojkat(int a, int b, int c)
    {
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public double GetPerimeter()
    {
        return a + b + c;
    }

    public double GetArea()
    {
        return a * b * c;
    }
}

class Program
{
    static void Main()
    {
        List<object> figures = new();
        int choice = 0;
        do
        {
            Console.WriteLine("Choose a figure to add (1 - Circle, 2 - Rectangle, 3 - Triangle, 0 - Exit): ");
            choice = Convert.ToInt32(Console.ReadLine());

            if (choice == 1)
            {
                double radius = Convert.ToDouble(Console.ReadLine());
                figures.Add(new Circle(radius));
            }
            else if (choice == 2)
            {
                double width = Convert.ToDouble(Console.ReadLine());
                double height = Convert.ToDouble(Console.ReadLine());
                figures.Add(new Rectangle(width, height));
            }
            else if (choice == 3)
            {
                double a = Convert.ToDouble(Console.ReadLine());
                double b = Convert.ToDouble(Console.ReadLine());
                double c = Convert.ToDouble(Console.ReadLine());
                figures.Add(new Trojkat((int)a, (int)b, (int)c));
            }
        } while (choice != 0);

        Console.WriteLine("\nFigures in the collection:");
        foreach (object figure in figures)
        {
            if (figure is Circle circle)
            {
                Console.WriteLine($"Perimeter: {circle.CalculatePerimeter()}, Area: {circle.CalculateArea()}");
            }
            else if (figure is Rectangle rectangle)
            {
                Console.WriteLine($"Perimeter: {rectangle.Perimeter()}, Area: {rectangle.Area()}");
            }
            else if (figure is Trojkat trojkat)
            {
                Console.WriteLine($"Perimeter: {trojkat.GetPerimeter()}, Area: {trojkat.GetArea()}");
            }
        }
    }
}
