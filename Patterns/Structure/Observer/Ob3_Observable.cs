using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Patterns.Structure.Observer
{
    class Ob3_Observable
    {
        public static void Test()
        {
            // OnPropertyChanged
            var market = new Market();

            market.PropertyChanged += (sender, eventArgs) =>
            {
                if (eventArgs.PropertyName == "Volatility")
                {
                    Console.WriteLine("Volatility changed.");
                }
            };

            market.Volatility++;

            // Invoke new element in the list
            var marketWithSequence = new MarketWithSequence();
            marketWithSequence.PriceAdded += (sender, f) =>
            {
                Console.WriteLine($"We've got a price of {f}");
            };

            marketWithSequence.AddPrice(123);
            marketWithSequence.AddPrice(123);

            // binding list
            var marketWithBindingList = new MarketWithBindingList();
            marketWithBindingList.Prices.ListChanged += (sender, eventArgs) =>
            {
                if(eventArgs.ListChangedType == ListChangedType.ItemAdded)
                {
                    float price = ((BindingList<float>)sender)[eventArgs.NewIndex];
                    Console.WriteLine($"Binding list got {price}");
                }                
            };

            marketWithBindingList.AddPrice(321);
        }
    }

    public class MarketWithBindingList
    {
        public BindingList<float> Prices = new BindingList<float>();

        public void AddPrice(float price)
        {
            Prices.Add(price);
        }
    }

    public class MarketWithSequence
    {
        private List<float> prices = new List<float>();

        public void AddPrice(float price)
        {
            prices.Add(price);
            PriceAdded?.Invoke(this, price);
        }

        public event EventHandler<float> PriceAdded;
    }

    public class Market : INotifyPropertyChanged
    {
        private float volatility;

        public float Volatility
        {
            get => volatility;
            set
            {
                if (value.Equals(volatility)) return;
                volatility = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
