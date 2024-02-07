using System.Globalization;
using Bogus;

namespace BogusDotNetEight;

public class PrimaryCostructorTest
{
    [Test]
    public void Should_Create_Model()
    {
        var modelGenerator = new Faker<Model>()
            .CustomInstantiator(f => new Model(
                f.Name.FullName(),
                f.Address.StreetAddress(),
                f.Date.Soon(10).ToString("yyyy-MM-dd"),
                f.Date.Future().ToString("yyyy-MM-dd"),
                f.Random.Number(1, 1000).ToString()))
            .RuleFor(m => m.EndDate,
                (f, m) => f.Date.Future(2,
                        DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                            DateTimeStyles.None))
                    .ToString("yyyy-MM-dd"));
        
        var modelFaker = new ModelFaker();
        
        var modelClass = modelFaker.Generate(10);
        
        var model = modelGenerator.Generate(10);
        Assert.Multiple(() =>
        {
            Assert.That(model, Has.Count.EqualTo(10));
            Assert.That(modelClass, Has.Count.EqualTo(10));
        });
    }
}

public class ModelFaker : Faker<Model>
{
    public ModelFaker()
    {
        CustomInstantiator(f => new Model(
            f.Name.FullName(),
            f.Address.StreetAddress(),
            f.Date.Soon(10).ToString("yyyy-MM-dd"),
            f.Date.Future().ToString("yyyy-MM-dd"),
            f.Random.Number(1, 1000).ToString()))
            .RuleFor(m => m.EndDate,
                (f, m) => f.Date.Future(2,
                        DateTime.ParseExact(m.StartDate, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                            DateTimeStyles.None))
                    .ToString("yyyy-MM-dd"));
    }
}

public class Model(string name, string address, string startDate, string endDate, string number)
{
    public string Name { get; init; } = name;
    public string Address { get; init; } = address;
    public string StartDate { get; init; } = startDate;
    public string EndDate { get; init; } = endDate;
    public string Number { get; init; } = number;
}