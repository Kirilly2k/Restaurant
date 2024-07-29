using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant
{
    class Product : IIDHolder, INameHolder, IPrintable
    {
        public enum ProductAmountType
        {
            Gramms,
            Pieces,
            Millilitters
        }
        public int Id { get; }
        public string Name { get; set; }
        public int calories;
        public float price;
        public ProductAmountType amountType;
        public float amount;

        public Product(int id,string name, int calories, float price, ProductAmountType amountType, float amount)
        {
            this.Name = name;
            this.calories = calories;
            this.price = price;
            this.amount = amount;
            this.amountType = amountType;
            Id = id;
        }


        public override string ToString()
        {
            return string.Join("|",Id, Name, amountType, $"Количество: {amount}");
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }
}
