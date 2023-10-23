using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoStore
{

    public class Statement
    {
        public double TotalAmount { get; set; } = 0.0;
        public int FrequentRenterPoints { get; set; } = 0;
        public string CustomerName { get; private set; } = "";
        private readonly List<Rental> Rentals = new();

        public Statement(string customerName) { CustomerName = customerName; }

        public void AddRental(Rental rental) => Rentals.Add(rental);

        public string Generate()
        {
            ClearTotals();
            return StatementText();

            void ClearTotals() => (TotalAmount, FrequentRenterPoints) = (0, 0);

            string StatementText()
            {
                string statementText = Header();
                statementText += RentalLines();
                statementText += Footer();
                return statementText;

                string Header() => string.Format("Rental Record for {0}\n", CustomerName);

                string RentalLines() => Rentals.Aggregate("", (rentalLines, rental) => rentalLines + RentalLine(rental));

                string RentalLine(Rental rental)
                {
                    double rentalAmount = rental.RentalAmount();
                    TotalAmount += rentalAmount;
                    FrequentRenterPoints += rental.FrequentRenterPoints();
                    return string.Format("\t{0}\t{1}\n", rental.Title, rentalAmount.ToString("F1"));
                }

                string Footer() => string.Format(
                    "You owed {0}\n" +
                    "You earned {1} frequent renter points\n",
                    TotalAmount.ToString("0.0"), FrequentRenterPoints.ToString());
            }
        }
    }
}