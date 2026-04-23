using System.Text;

Point p1 = new Point(1, 2);


Point p2 = p1 with { X = 5};
Console.WriteLine($"{p2.X} and {p2.Y}");

public record Point(double X, double Y);