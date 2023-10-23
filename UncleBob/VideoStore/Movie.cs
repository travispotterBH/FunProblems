using NUnit.Framework.Constraints;

namespace VideoStore
{
    public abstract class Movie
    {
        public string Title { get; private set; }
        public abstract double RentalAmount(int daysRented);
        public abstract int FrequentRenterPoints(int daysRented);

        public Movie(string title)
        {
            Title = title;
        }
    }

    public class RegularMovie : Movie
    {
        public RegularMovie(string name) : base(name) { }
        public override int FrequentRenterPoints(int _) => 1;
        public override double RentalAmount(int daysRented) => 2 + (daysRented > 2 ? (daysRented - 2) * 1.5 : 0);
    }
    public class ChildrensMovie : Movie
    {
        public ChildrensMovie(string name) : base(name) { }
        public override int FrequentRenterPoints(int _) => 1;
        public override double RentalAmount(int daysRented) => 1.5 + (daysRented > 3 ? (daysRented - 3) * 1.5 : 0);
    }

    public class NewReleaseMovie : Movie
    {
        public NewReleaseMovie(string name) : base(name) { }
        public override int FrequentRenterPoints(int daysRented) => daysRented > 1 ? 2 : 1;
        public override double RentalAmount(int daysRented) => daysRented * 3;
    }
}