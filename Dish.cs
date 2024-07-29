using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant
{
    class Dish : IIDHolder, INameHolder, IPrintable
    {
        public int Id { get; }
        public string Name { get; set; }
        public List<Product> products = new List<Product>();
        public TimeSpan timeToDo;
        public float priceKoef;
        DateTime cookingFinishDate;

        public Dish(string name, List<Product> products, TimeSpan timeToDo, float priceKoef)
        {
            this.Name = name;
            this.products = products;
            this.timeToDo = timeToDo;
            this.priceKoef = priceKoef;
        }

        public void SetTime()
        {
            cookingFinishDate = DateTime.Now.Add(timeToDo);
        }

        //public void StartCook()
        //{
        //    cookingFinishDate = DateTime.Now.Add(timeToDo);
        //    foreach(Product t in manager.products)
        //    {
                
        //    }
        //}
        public int GetCalories()
        {
            int cal = 0;
            products.ForEach(product => { cal += product.calories; });
            return cal;
        }

        public float GetFinalCalories()
        {
            float cal = 0;
            products.ForEach(product => { cal += product.calories * product.amount; });
            return (int)MathF.Round(cal);
        }

        public float GetPrice()
        {
            float prodPrice = 0;
            products.ForEach(product => { prodPrice += product.price; });
            return Manager.minimumDishPrice * priceKoef + prodPrice;
        }

        public bool IsFinished()
        {
            return DateTime.Now >= cookingFinishDate;
        }

        public TimeSpan GetRemainingTime()
        {
            return cookingFinishDate - DateTime.Now;
        }

        public override string ToString()
        {
            return string.Join("|", Id, Name, "Ингредиенты: (" + string.Join(", ", products) + ")", "Время приготовления: " + timeToDo, "Калории: " + GetCalories(), "Общее количество калорий: ~" + GetFinalCalories(), "Итоговая цена: " + GetPrice() + $" (Коэффциент: {priceKoef})");
        }

        public void Print()
        {
            Console.WriteLine(ToString());
        }
    }
}
