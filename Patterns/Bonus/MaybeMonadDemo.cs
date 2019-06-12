using System;

namespace Patterns.Bonus.Maybe
{
    public static class MaybeMonadDemo
    {
        public static void Test()
        {

        }

        private static bool HasMedicalRecord(Person person)
        {
            throw new NotImplementedException();
        }

        private static void CheckAddress(Address address)
        {

        }


        public class Person
        {
            public Address Address { get; set; }


        }

        public class Address
        {
            public string PostCode { get; set; }
        }



        public static void MyMethod(Person p)
        {
            string postcode = "UNKNOWN";
            //if(p != null && p.Address != null && p.Address.PostCode != null) { }
            postcode = p?.Address?.PostCode;

            string postcode2;
            if (p != null)
            {
                if (HasMedicalRecord(p) && p.Address != null)
                {
                    CheckAddress(p.Address);
                    if (p.Address.PostCode != null)
                    {
                        postcode2 = p.Address.PostCode.ToString();
                    }
                    else
                    {
                        postcode2 = "UNKNOWN";
                    }
                }
            }


            string postcode3 = p.With(x => x.Address).With(x => x.PostCode);
            postcode3 = p
                .If(HasMedicalRecord)
                .With(x => x.Address)
                .Do(CheckAddress)
                .Return(x => x.PostCode, "UNKNOWN");
        }
    }

    public static class Maybe
    {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator)
            where TResult : class
            where TInput : class
        {
            if (o == null) return null;
            else return evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null; 
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action)
            where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            if (o == null) return failureValue;
            return evaluator(o);
        }
    }


}
