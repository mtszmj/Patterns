using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Proxy
{
    class ProtectionProxy
    {
        public static void Test()
        {
            ICar car = new CarProxy(new Driver { Age = 12 });
            car.Drive();

            car = new CarProxy(new Driver { Age = 22 });
            car.Drive();
        }
    }

    public interface ICar
    {
        void Drive();
    }

    public class Car : ICar
    {
        public void Drive()
        {
            Console.WriteLine("Car is being driven.");
        }
    }

    public class CarProxy : ICar
    {
        private Driver driver;
        private Car car = new Car();

        public CarProxy(Driver driver)
        {
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
        }
        public void Drive()
        {
            if (driver.Age >= 16)
                car.Drive();
            else
                Console.WriteLine("You are too young.");
        }
    }

    public class Driver
    {
        public int Age { get; set; }
    }
}
