namespace VideoStore
{
    public class Rental
    {
        public Movie Movie { get; private set; }
        public int DaysRented { get; private set; }
        public string Title => Movie.Title;

        public Rental(Movie movie, int daysRented)
        {
            Movie = movie;
            DaysRented = daysRented;
        }

        public double RentalAmount() => Movie.RentalAmount(DaysRented);
        public int FrequentRenterPoints() => Movie.FrequentRenterPoints(DaysRented);
    }
}