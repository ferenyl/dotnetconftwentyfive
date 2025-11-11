using System.Numerics;
using System.Text.RegularExpressions;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
//builder.Services.AddHostedService<Worker>();

// null-propagating assignment in property access
var person = User.TryGet();
person?.Name = "New Name";

// lambda with out parameter
var lambda = (out string hello) => { hello = "yey"; };

var list = new List<int> { 1, 2, 3 };

// extension method and property
var first = list.MyFirst();
var seccond = list.SelectSeccond();
var halfCount = list.CountHalf;
// or
MyListExtensions.get_CountHalf(list);

// static abstract members in interfaces and operator overloading med extensionblocks
var r = MyListExtensions.RangeFromOne<int>(5) * 48;
foreach (var item in r)
{
    Console.WriteLine(item);
}

var host = builder.Build();
host.Run();

public class User(string name)
{
    // backing field for the Friends property med null-propagating assignment
    public IReadOnlyList<User> Friends => field ??= [];

    // backing field for the Name property
    public string Name
    {
        get => field;
        set => field = value;
    } = name;

    public static User? TryGet()
    {
        return null;
    }
}

// extensionblocks
static class MyListExtensions
{
    extension<TSource>(List<TSource> source)
    {
        public TSource MyFirst() { return source[0]; }
        public TSource SelectSeccond() { return source[1]; }
        public double CountHalf => source.Count / 2;

    }

    extension<T>(T source) where T : INumber<T>
    {

        public static IEnumerable<T> operator *(IEnumerable<T> vector, T Scalar)
        {
            return vector.Select(v => v * Scalar);
        }
        public static IEnumerable<T> RangeFromOne(int count)
        {
            var start = T.One;
            for (var i = 0; i < count; i++)
            {
                yield return start++;
            }
        }
    }

}