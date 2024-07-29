using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Restaurant
{
    public interface IIDHolder
    {
        public int Id { get; }
    }

    public interface INameHolder
    {
        public string Name { get; set; }
    }

    public interface IPrintable
    {
        public void Print();
    }

    class Manager
    {

        public List<Product> products;
        public List<Dish> dishes;
        public List<Order> orders;

        public static float minimumDishPrice = 1000;

        public Manager()
        {
            products = new List<Product>()
            {
                new Product(0,"Огурец", 10, 1.19f, Product.ProductAmountType.Gramms, 500000),
                new Product(1,"Колбаса", 12, 8, Product.ProductAmountType.Gramms, 100000),
                new Product(2,"Масло", 71, 6, Product.ProductAmountType.Millilitters, 50000)
            };

            dishes = new List<Dish>()
            {
                new Dish("Оливье", new List<Product>(){new Product(1,"Колбаса", 12, 8, Product.ProductAmountType.Gramms, 1000) },new TimeSpan(0,10,0),1.32f)
            };
            orders = new List<Order>();
        }

        public void AddNewDish()
        {
            Console.Write("Введите наименование блюда: ");
            string dishName = Console.ReadLine();

            if (dishes.Any(x => x.Name == dishName))
            {
                Console.WriteLine("Данное блюдо уже существует в списке!");
                return;
            }

            PrintList(products);
            List<Product> prods = new List<Product>();
            do
            {
                Console.Write("Введите название или идентификатор продукта (оставьте пустым для завершения): ");
                string input = Console.ReadLine();

                if(input == "")
                {
                    break;
                }

                if (int.TryParse(input, out int res) && FindObject(products, res) != null)
                {
                    try
                    {

                        float amount;
                        do
                        {
                            Console.Write($"Введите нужное количество продукта {input}: ");
                            if (!float.TryParse(Console.ReadLine(), out amount))
                            {
                                continue;
                            }
                            break;
                        }
                        while (true);

                        prods.Add(GetProduct(res, amount));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                    continue;
                }

                if(FindObject(products, input) != null)
                {
                    try
                    {
                        float amount;
                        do
                        {
                            Console.Write($"Введите нужное количество продукта {input}: ");
                            if (!float.TryParse(Console.ReadLine(), out amount))
                            {
                                continue;
                            }
                            break;
                        }
                        while (true);

                        prods.Add(GetProduct(input, amount));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }


            }
            while (true);

            TimeSpan timeToCook;
            do
            {
                Console.Write("Введите время приготовления блюда: ");
                if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan t))
                {
                    continue;
                }
                timeToCook = t;
                break;
            }
            while (true);

            float koef;
            do
            {
                Console.Write("Введите коэффициент стоимости блюда: ");
                if (!float.TryParse(Console.ReadLine(), out float t))
                {
                    continue;
                }
                koef = t;
                break;
            }
            while (true);
            dishes.Add(new Dish(dishName, prods, timeToCook, koef));
            Console.WriteLine($"Новое блюдо {dishName} было успешно добавлено в базу данных!");
        }

        public void AddNewProduct()
        {
            Console.Write("Введите наименование продукта: ");
            string prodName = Console.ReadLine();
            if (products.Any(x => x.Name == prodName))
            {
                Product oldProd = (FindObject(products, prodName) as Product);
                Console.WriteLine(oldProd);
                Console.WriteLine("Данный продукт уже существует в списке! Желаете ли вы увеличить его количество? Введите новое количество (оставьте пустым для отмены): ");

                string answer = Console.ReadLine();

                if(answer == "")
                {
                    return;
                }

                if(float.TryParse(answer, out float res))
                {
                    oldProd.amount = res;
                    Console.WriteLine("Количество продукта было успешно обновлено!");
                    return;
                }
            }

            Product.ProductAmountType prodType;
            do
            {
                Console.Write("Введите тип количества продукта (Граммы, Литры, Штуки): ");
                string type = Console.ReadLine();

                if(type == "Граммы")
                {
                    prodType = Product.ProductAmountType.Gramms;
                    break;
                }
                else if (type == "Миллилитры")
                {
                    prodType = Product.ProductAmountType.Millilitters;
                    break;
                }
                else if (type == "Штуки")
                {
                    prodType = Product.ProductAmountType.Pieces;
                    break;
                }
            }
            while (true);

            int calories;
            do
            {
                Console.Write("Введите количество калорий продукта: ");
                if (!int.TryParse(Console.ReadLine(), out int t))
                {
                    continue;
                }
                calories = t;
                break;
            }
            while (true);


            float price;
            do
            {
                Console.Write($"Введите цену продукта за {prodType}: ");
                if (!float.TryParse(Console.ReadLine(), out float t))
                {
                    continue;
                }
                price = t;
                break;
            }
            while (true);


            float amount;
            do
            {
                Console.Write($"Введите количество продукта: ");
                if (!float.TryParse(Console.ReadLine(), out float t))
                {
                    continue;
                }
                amount = t;
                break;
            }
            while (true);

            Console.Write("Введите id продукта: ");
            int id = int.Parse(Console.ReadLine());

            products.Add(new Product(id,prodName, calories, price, prodType, amount));
            Console.WriteLine($"Продукт {prodName} был успешно добавлен в базу данных!");
        }

        public void AddNewOrder()
        {
            PrintList(dishes);

            Console.Write("Здравствуйте! ");

            if (dishes.Count == 0)
            {
                Console.WriteLine("К сожалению, в данный момент мы не можем принять заказ, так как у нас нет ни одного блюда в базе данных. Заходите позже!");
                return;
            }
            List<Dish> orderDishes = new List<Dish>();
            do
            {
                Console.Write("Введите название желаемого блюда (или оставьте пустым для завершения заказа): ");
                string dishName = Console.ReadLine();

                if(dishName == "")
                {
                    break;
                }

                if (!dishes.Any(x => x.Name == dishName))
                {
                    Console.WriteLine("Данного блюда нет в списке!");
                    return;
                }

                Dish dish = (Dish)FindObject(dishes, dishName);

                bool canCook = true;
                foreach (Product t in dish.products)
                {
                    if (t.amount > (FindObject(products, t.Name) as Product).amount)
                    {
                        Console.WriteLine("К сожалению, в данный момент мы не можем приготовить данное блюдо. Может, вы желаете что-то другое?");
                        canCook = false;
                    }
                }
                if (!canCook)
                {
                    continue;
                }

                int amount;
                do
                {
                    Console.Write("В каком количестве приготовить данное блюдо? Введите количество: ");
                    if (!int.TryParse(Console.ReadLine(), out int t))
                    {
                        continue;
                    }
                    amount = t;
                    break;
                }
                while (true);

                for(int i = 0; i < amount; i++)
                {
                    orderDishes.Add(dish);
                }
            }
            while (true);

            if(orderDishes.Count == 0)
            {
                Console.WriteLine("Нам жаль, что вы ничего не заказали... Приходите ещё, мы вам всегда рады!");
                return;
            }

            Console.WriteLine("Ваш заказ принят! Давайте повторим всё, что вы заказали: ");
            List<Tuple<int, Dish>> outputList = new List<Tuple<int, Dish>>();
            foreach(Dish dish in orderDishes)
            {
                if (!outputList.Any(x => x.Item2 == dish))
                {
                    outputList.Add(new Tuple<int, Dish>(1,dish));
                    continue;
                }
                Tuple<int, Dish> found = outputList.Find(x => x.Item2 == dish);
                int count = found.Item1;
                outputList.Remove(found);
                count += 1;
                Tuple<int, Dish> newTuple = new Tuple<int, Dish>(count, dish);
                outputList.Add(newTuple);
            }
            outputList.ForEach(dish => Console.WriteLine("\t-" + dish.Item1 + " x " + dish.Item2));
            float finalPrice = 0;
            orderDishes.ForEach(dish => finalPrice += dish.GetPrice());
            Console.WriteLine($"Итоговая цена за заказ: {finalPrice}");

            Console.Write("Введите id заказа: ");
            int id = int.Parse(Console.ReadLine());

            do
            {
                Console.Write("Завершить выполнение заказа и начать его готовить? (1 - завершить, 0 - отменить заказ): ");
                string check = Console.ReadLine();
                if (check == "1")
                {
                    break;
                }
                else if (check == "0")
                {
                    return;
                }
            }
            while (true);

            orders.Add(new Order(id, orderDishes));

            Console.WriteLine($"Заказ {id} был успешно оформлен. Вы можете проверить статус приготовления в меню Заказы -> Проверить заказ!");
            foreach(Dish d in orderDishes)
            {
                StartCookingTheDish(d);
            }

        }

        public void CheckOrder()
        {
            bool status = false;
            do
            {
                Console.Write("Введите идентификатор заказа: ");
                if (!int.TryParse(Console.ReadLine(), out int res))
                {
                    continue;
                }

                if(!orders.Any(order => order.Id == res))
                {
                    Console.WriteLine("Заказ с данным идентификатором не был найден!");
                    continue;
                }
                Order ord = (FindObject(orders, res) as Order);
                status = ord.IsOrderDone();
                Console.WriteLine($"{(status ? "Готов" : "Ещё не готово")}");

                if (!status)
                {
                    Console.WriteLine("Оставшееся время: " + ord.RemainingTime());
                    return;
                }
                orders.Remove(ord);
                Console.WriteLine("Заказ выдан! Приятного аппетита!");
                break;
            }
            while (true);
        }

        public Product GetProduct(Product foundProduct, float amount)
        {
            if (foundProduct == null)
            {
                throw new Exception("Продукт с таким именем не был найден!");
            }

            if (amount > foundProduct.amount)
            {
                throw new Exception("На складе отсутствует данное количество продукта!");
            }
            //foundProduct.amount -= amount;
            Product newProd = new Product(foundProduct.Id, foundProduct.Name, foundProduct.calories, foundProduct.price, foundProduct.amountType, amount);
            return newProd;
        }
        public Product GetProduct(int id, float amount)
        {
            Product foundProd = (Product)FindObject(products, id);
            return GetProduct(foundProd, amount);
        }

        public Product GetProduct(string name, float amount)
        {
            Product foundProd = (Product)FindObject(products, name);
            return GetProduct(foundProd, amount);
        }

        public void StartCookingTheDish(Dish dish)
        {
            dish.SetTime();
            foreach(Product t in dish.products)
            {
                (FindObject(products, t.Name) as Product).amount -= t.amount;
            }
        }

        public object FindObject<T>(List<T> list, int id)
        {
            if (!list.Any(x => (x as IIDHolder).Id == id))
            {
                return null;
            }
            return list.Find(x => (x as IIDHolder).Id == id);
        }

        public object FindObject<T>(List<T> list, string name)
        {
            if (!list.Any(x => (x as INameHolder).Name == name))
            {
                return null;
            }
            return list.Find(x => (x as INameHolder).Name == name);
        }

        public List<T> FindAllObjects<T>(List<T> list, int id)
        {
            if (!list.Any(x => (x as IIDHolder).Id == id))
            {
                return null;
            }
            return list.FindAll(x => (x as IIDHolder).Id == id);
        }

        public List<T> FindAllObjects<T>(List<T> list, string name)
        {
            if (!list.Any(x => (x as INameHolder).Name == name))
            {
                return null;
            }
            return list.FindAll(x => (x as INameHolder).Name == name);
        }


        public void PrintList<T>(List<T> list)
        {
            list.ForEach(t => (t as IPrintable).Print());
        }
    }
}
