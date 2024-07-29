using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant
{
    class Program
    {
        static void Main()
        {
            Manager manager = new Manager();


            Dictionary<string, Action> orderMenu = new Dictionary<string, Action>()
            {
                {"Добавить заказ", manager.AddNewOrder},
                {"Проверить заказ", manager.CheckOrder}
            };
            Dictionary<string, Action> outputMenu = new Dictionary<string, Action>()
            {
                {"Список продуктов", DisplayProducts},
                {"Список блюд", DisplayDishes}
            };

            Dictionary<string, Action> databaseMenu = new Dictionary<string, Action>()
            {
                {"Добавить продукт", manager.AddNewProduct },
                {"Добавить блюдо", manager.AddNewDish},
            };

            Dictionary<string, Action> mainMenu = new Dictionary<string, Action>()
            {
                {"Работа с базой данных", DatabaseActions },
                {"Заказы", OrderActions},
                {"Вывод", Output}
            };


            void DisplayProducts()
            {
                manager.PrintList(manager.products);
            }

            void DisplayDishes()
            {
                manager.PrintList(manager.dishes);
            }


            DisplayDictionaryAndChooseAction(mainMenu);

            void CallMethodFromDictionary(Dictionary<string, Action> dictionary, int index)
            {
                dictionary.ElementAt(index-1).Value();
            }

            void DisplayDictionaryAndChooseAction(Dictionary<string, Action> dictionary)
            {
                do
                {
                    for (int i = 0; i < dictionary.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}:{dictionary.ElementAt(i).Key}");
                    }
                    Console.WriteLine($"{dictionary.Count + 1}:Выйти");

                    Console.Write("Введите номер действия: ");
                    if (!int.TryParse(Console.ReadLine(), out int res) || res > (dictionary.Count + 1) || res < 0)
                    {
                        continue;
                    }

                    if (res == (dictionary.Count + 1))
                    {
                        return;
                    }
                    CallMethodFromDictionary(dictionary, res);
                }
                while (true);
            }

            void Output()
            {
                DisplayDictionaryAndChooseAction(outputMenu);
            }
            void OrderActions()
            {
                DisplayDictionaryAndChooseAction(orderMenu);
            }
            void DatabaseActions()
            {
                DisplayDictionaryAndChooseAction(databaseMenu);
            }

        }
    }
}
