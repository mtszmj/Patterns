namespace Patterns.Creation
{
    public class BuilderFaceted
    {
        public void Test()
        {
            var pb = new APersonBuilder();
            APerson person = pb
                .Works.At("Amazon")
                      .AsA("Programmer")
                      .Earning(12345)
                .Lives.At("12 Manchester Road")
                      .In("Liverpool")
                      .WithPostcode("12-345");

            System.Console.WriteLine(person);

        }
    }

    public class APerson
    {
        // address
        public string StreetAddress, Postcode, City;

        // employment
        public string CompanyName, Position;
        public int AnnualIncome;

        public override string ToString()
        {
            return $"{nameof(StreetAddress)}: {StreetAddress}, {nameof(Postcode)}: {Postcode}, {nameof(City)}: {City}, " +
                $"{nameof(CompanyName)}: {CompanyName}, {nameof(Position)}: {Position}, {nameof(AnnualIncome)}: {AnnualIncome}";
        }
    }

    public class APersonBuilder // facade
    {
        // reference!
        protected APerson person = new APerson();

        public APersonJobBuilder Works => new APersonJobBuilder(person);
        public APersonAddressBuilder Lives => new APersonAddressBuilder(person);

        public static implicit operator APerson(APersonBuilder pb)
        {
            return pb.person;
        }
    }

    public class APersonJobBuilder : APersonBuilder
    {
        public APersonJobBuilder(APerson person)
        {
            this.person = person;
        }

        public APersonJobBuilder At(string companyName)
        {
            person.CompanyName = companyName;
            return this;
        }

        public APersonJobBuilder AsA(string position)
        {
            person.Position = position;
            return this;
        }

        public APersonJobBuilder Earning(int amount)
        {
            person.AnnualIncome = amount;
            return this;
        }
    }

    public class APersonAddressBuilder : APersonBuilder
    {
        public APersonAddressBuilder(APerson person)
        {
            this.person = person;
        }

        public APersonAddressBuilder At(string streetAddress)
        {
            person.StreetAddress = streetAddress;
            return this;
        }

        public APersonAddressBuilder WithPostcode(string postcode)
        {
            person.Postcode = postcode;
            return this;
        }

        public APersonAddressBuilder In(string city)
        {
            person.City = city;
            return this;
        }
    }
}
