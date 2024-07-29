using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurant
{
    class Order: IIDHolder, IPrintable
    {
        public int Id { get; }
        public List<Dish> dishes = new List<Dish>();

        public Order(int id, List<Dish> dishes)
        {
            Id = id;
            this.dishes = dishes;
        }

        public TimeSpan RemainingTime()
        {
            TimeSpan tmp = new TimeSpan(0,0,0);
            foreach (Dish t in dishes)
            {
                //Console.WriteLine(t.GetRemainingTime());
                if (!t.IsFinished())
                    tmp += t.GetRemainingTime();
            }
            return tmp;
        }

        public bool IsOrderDone()
        {
            foreach (Dish t in dishes)
            {
                if (!t.IsFinished())
                    return false;
            }
            return true;
        }

        public void Print()
        {
            Console.WriteLine(string.Join("|", Id, string.Join(", ", dishes)));
        }
    }
}
