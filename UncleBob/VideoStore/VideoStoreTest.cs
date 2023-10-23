using NUnit.Framework;

namespace VideoStore
{
    [TestFixture]
    public class VideoStoreTest : TestBase
    {
        private const double DELTA = 0.001;
        private List<Movie> Movies = new();
        private Movie newReleaseMovie1;
        private Movie newReleaseMovie2;
        private Movie childrensMovie1;
        private Movie regularMovie1;
        private Movie regularMovie2;
        private Movie regularMovie3;
        private Statement statement;

        public VideoStoreTest() : base("Customer") { }

        public override void SetUp()
        {
            statement = new Statement("Customer");
            newReleaseMovie1 = new NewReleaseMovie("New Release 1");
            newReleaseMovie2 = new NewReleaseMovie("New Release 2");
            childrensMovie1 = new ChildrensMovie("Children's Movie");
            regularMovie1 = new RegularMovie("Regular Movie 1");
            regularMovie2 = new RegularMovie("Regular Movie 2");
            regularMovie3 = new RegularMovie("Regular Movie 3");
        }

        [Test]
        public void TestSingleNewReleaseStatementTotals()
        {
            statement.AddRental(new Rental(newReleaseMovie1, 3));
            statement.Generate();
            Assert.AreEqual(9.0, statement.TotalAmount, DELTA);
            Assert.AreEqual(2, statement.FrequentRenterPoints);
        }

        [Test]
        public void TestDualNewReleaseStatementTotals()
        {
            statement.AddRental(new Rental(newReleaseMovie1, 3));
            statement.AddRental(new Rental(newReleaseMovie2, 3));
            statement.Generate();
            Assert.AreEqual(18.0, statement.TotalAmount, DELTA);
            Assert.AreEqual(4, statement.FrequentRenterPoints);
        }

        [Test]
        public void TestSingleChildrensStatementTotals()
        {
            statement.AddRental(new Rental(childrensMovie1, 3));
            statement.Generate();
            Assert.AreEqual(1.5, statement.TotalAmount, DELTA);
            Assert.AreEqual(1, statement.FrequentRenterPoints);
        }

        [Test]
        public void TestMultipleRegularStatementTotals()
        {
            statement.AddRental(new Rental(regularMovie1, 1));
            statement.AddRental(new Rental(regularMovie2, 2));
            statement.AddRental(new Rental(regularMovie3, 3));
            statement.Generate();
            Assert.AreEqual(7.5, statement.TotalAmount, DELTA);
            Assert.AreEqual(3, statement.FrequentRenterPoints);
        }

        [Test]
        public void TestMultipleRegularStatementFormat()
        {
            statement.AddRental(new Rental(regularMovie1, 1));
            statement.AddRental(new Rental(regularMovie2, 2));
            statement.AddRental(new Rental(regularMovie3, 3));
            Assert.AreEqual(
                "Rental Record for Customer\n" +
                "\tRegular Movie 1\t2.0\n" +
                "\tRegular Movie 2\t2.0\n" +
                "\tRegular Movie 3\t3.5\n" +
                "You owed 7.5\n" +
                "You earned 3 frequent renter points\n",
                statement.Generate());
        }
    }

    [TestFixture]
    public abstract class TestBase
    {
        protected Statement? statement;
        protected string Name;

        public TestBase(string name) { Name = name; }

        [SetUp]
        public virtual void SetUp() { statement = new Statement(Name); }

        [TearDown]
        public virtual void TearDown() { /* Cleanup code here */ }
    }
}