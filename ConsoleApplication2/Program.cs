using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Работает цикл до ввода пользователем ключевого слова 'exit'
            while (true) {
                //Вывод в консоль текста приглашения для ввода
                Console.Write("Для выхода введите 'exit'.\n\nВведите заказ: ");
                string order = Console.ReadLine(); //В переменную order записывается ввод пользователя
                //Выход из цикла, в случае ввода 'exit'
                if (order.Equals("exit"))   
                    break;
                /* Проверка правильности ввода заказа пользователем: только буквы a, b, c 
                 * в любом порядке и количестве, регистр не учитывается */
                if (Regex.IsMatch(order, "[^abc]", RegexOptions.IgnoreCase)){
                    Console.WriteLine("Неверный заказ! Проверьте правильность ввода.");
                    continue; // В случае неправильного ввода, цикл перезапускается
                }

                int cost = 0;   //Поле для хранения суммы заказа
                int A=0, B=0, C=0;  //Переменные для хранения количества каждого вида товара в заказе
                // В цикле определяется количество каждого вида товара в заказе
                foreach (char c in order.ToUpper())
                {
                    switch (c) 
                    {
                        case 'A': 
                            A++;
                            break;

                        case 'B': 
                            B++;
                            break;

                        case 'C': 
                            C++;
                            break;
                    }
                }

                try
                {
                    //Поля для хранения данных о ценах из конфигурационного файла
                    string[] getNumOfA = ConfigurationManager.AppSettings.Get("NumOfA").Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] getNumOfB = ConfigurationManager.AppSettings.Get("NumOfB").Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] getNumOfC = ConfigurationManager.AppSettings.Get("NumOfC").Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] getCostForA = ConfigurationManager.AppSettings.Get("CostForA").Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] getCostForB = ConfigurationManager.AppSettings.Get("CostForB").Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    string[] getCostForC = ConfigurationManager.AppSettings.Get("CostForC").Split(new Char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    //Поле для хранения информации о количестве товаров  вида A, для которых предусмотрена определенная цена
                    int[] numOfA = { Int32.Parse(getNumOfA[0]), Int32.Parse(getNumOfA[1]), Int32.Parse(getNumOfA[2]) };
                    //Поле для хранения всех категорий цены товаров вида A для каждого количества товаров соответственно
                    int[] costForA = { Int32.Parse(getCostForA[0]), Int32.Parse(getCostForA[1]), Int32.Parse(getCostForA[2]) };
                    //Поле для хранения информации о количестве товаров  вида B, для которых предусмотрена определенная цена
                    int[] numOfB = { Int32.Parse(getNumOfB[0]), Int32.Parse(getNumOfB[1]), Int32.Parse(getNumOfB[2]) };
                    //Поле для хранения всех категорий цены товаров вида B для каждого количества товаров соответственно
                    int[] costForB = { Int32.Parse(getCostForB[0]), Int32.Parse(getCostForB[1]), Int32.Parse(getCostForB[2]) };
                    //Поле для хранения информации о количестве товаров  вида C, для которых предусмотрена определенная цена
                    int[] numOfC = { Int32.Parse(getNumOfC[0]), Int32.Parse(getNumOfC[1]), Int32.Parse(getNumOfC[2]) };
                    //Поле для хранения всех категорий цены товаров вида C для каждого количества товаров соответственно
                    int[] costForC = { Int32.Parse(getCostForC[0]), Int32.Parse(getCostForC[1]), Int32.Parse(getCostForC[2]) };

                    //Считается количество товаров каждого вида для каждой категории цены и их стоимость прибавляется к сумме заказа
                    cost += Cost(A, numOfA, costForA);
                    cost += Cost(B, numOfB, costForB);
                    cost += Cost(C, numOfC, costForC);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: " + e.Message);
                    continue;
                }
                
                Console.WriteLine("Стоимость заказа: " + cost); //Вывод в консоль стоимости заказа
            }
        }
        // Метод для определения стоимости товара, возвращает итоговую стоимость
        //Параметры: наименование товара, количество товаров со скидкой, стоимость каждой категории товаров
        static int Cost(int Item, int[] numOfItem, int[] costForItem) 
        {
            int itemCost = 0; //Поле для хранения итоговой стоимости, изначально равна нулю
            int tmp = 0; //Вспомогательно поле
            tmp = Item / numOfItem[2]; //Определяется, есть ли достаточное количество товаров с определенной ценой
            itemCost += tmp * costForItem[2]; //Если есть, то к итоговой стоимости прибавляется соотвествующая сумма
            Item -= tmp * numOfItem[2]; //От общего количества товаров отнимается количество учтенных
            //Пункты, описанные выше, повторяются для оставшихся категорий цен
            tmp = Item / numOfItem[1];
            itemCost += tmp * costForItem[1];
            Item -= tmp * numOfItem[1];
            tmp = Item / numOfItem[0];
            itemCost += tmp * costForItem[0];
            return itemCost; //Результатом метода является общая сумма стоимости данного товара
        }  
    }
}
