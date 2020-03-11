using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace t2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Division_as_is divisor = new Division_as_is();
            int firstNumber = 0;
            int secondNumber = 0;
            Console.WriteLine("Ділення як є");
            do
            {
                Console.Write("Ваше перше число (у межах -127 → +127): ");
                firstNumber = int.Parse(Console.ReadLine());
            } while (firstNumber > 127 || firstNumber < -127);
            do
            {
                Console.Write("Ваше друге число (у межах -127 → +127): ");
                secondNumber = int.Parse(Console.ReadLine());
            } while (secondNumber > 127 || secondNumber < -127);
            int[] result = divisor.Division(firstNumber, secondNumber);

            Console.ReadKey();
        }
    }

    public class Division_as_is
    {
        public Division_as_is()
        {

        }

        //ділення як є
        public int[] Division(int first, int second)
        {
            int[] a = BinaryConverter(first);
            int[] b = BinaryConverter(second);
            Console.WriteLine("Перше число: " + Print(a));
            Console.WriteLine("Друге число: " + Print(b));

            //визначення знаку, яке застосується вкінці операції
            int sign = Convert.ToInt16(Convert.ToBoolean(a[0]) ^ Convert.ToBoolean(b[0]));
            int resultSign = sign == 0 ? 1 : -1;

            a[0] = 0;
            b[0] = 0;

            // dividend - ділюване
            int[] dividend = a;
            // divisor - дільник
            int[] divisor = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // reminder - залишок
            int[] reminder = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            // quotient - частка
            int[] quotient = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

            //записую дільник у регістр залишку
            for (int i = 0; i < dividend.Length; i++)
            {
                reminder[i + dividend.Length] = dividend[i];
            }
            for (int i = 0; i < b.Length; i++)
            {
                divisor[i] = b[i];
            }

            for (int i = 0; i < dividend.Length + 1; i++)
            {
                Console.WriteLine();
                Console.WriteLine("Крок (ітерація)    : " + i);
                Console.WriteLine("Ділюване (dividend): " + Print(dividend));
                Console.WriteLine("Залишок (reminder) : " + Print(reminder));
                Console.WriteLine("Дільник (divisor)  : " + Print(divisor));
                Console.WriteLine("Частка (quotient)  : " + Print(quotient));

                //Від регістру залишку відняти дільник - та записати результат в регістр залишку

                int[] div = divisor.Select((x) => { if (x == 1) return 0; else return 1; }).ToArray();
                int[] negativeDivisor = AdditionNoSign(div, new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 });

                reminder = AdditionNoSign(reminder, negativeDivisor);
                Console.WriteLine("Додавання без знаку (reminder та negativeDivisor)");

                if (reminder[0] == 0)
                {
                    quotient = ShiftLeft(quotient);
                    Console.WriteLine("Здвиг вправо (quotient)");
                }
                else
                {
                    reminder = AdditionNoSign(divisor, reminder);
                    Console.WriteLine("Додавання без знаку (divisor та reminder)");
                    quotient = ShiftLeftWithSign(quotient);
                    Console.WriteLine("Здвиг вліво без знаку (quotient)");
                }
                divisor = ShiftRight(divisor);
            }
            Console.WriteLine();
            Console.WriteLine("Остання ітерація: ");
            Console.WriteLine("Ділюване (dividend): " + Print(dividend));
            Console.WriteLine("Залишок (reminder) : " + Print(reminder));
            Console.WriteLine("Дільник (divisor)  : " + Print(divisor));
            Console.WriteLine("Частка (quotient)  : " + Print(quotient));

            Console.WriteLine();
            Console.WriteLine("Ділюване (dividend) перше число :" + first.ToString());
            Console.WriteLine("Дільник (divisor) друге число   :" + second.ToString());
            Console.WriteLine("Результат ділення   :" + (resultSign * DecimalConverter(quotient)).ToString());
            Console.WriteLine("Залишок від ділення :" + DecimalConverter(reminder).ToString());

            return quotient;
        }

        public string Print(int[] numberToPrint)
        {
            string number = "";
            for (int i = 0; i < numberToPrint.Length; i++)
            {
                number += numberToPrint[i].ToString();
            }
            return number;
        }

        public int[] BinaryConverter(int numberToConvert)
        {
            int[] binaryForm = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            //визначення знаку
            if (numberToConvert < 0)
            {
                binaryForm[0] = 1;
            }
            else
            {
                binaryForm[0] = 0;
            }
            //прибираю знак
            numberToConvert = Math.Abs(numberToConvert);
            //перетворення у двійкову систему
            for (int i = 0; i < binaryForm.Length && numberToConvert != 0; i++)
            {
                binaryForm[binaryForm.Length - 1 - i] = numberToConvert % 2;
                numberToConvert /= 2;
            }
            return binaryForm;
        }
        //допоміжний код
        public int[] HelpCodeConverter(int number)
        {
            int[] binaryForm = BinaryConverter(number);
            if (binaryForm[0] == 0)
            {
                return binaryForm;
            }
            int[] helpCode = new int[15];
            int[] inverted = new int[8] { 1, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 1; i < binaryForm.Length; i++)
            {
                inverted[i] = (binaryForm[i] == 0 ? 1 : 0);
            }
            helpCode = Addition(inverted, BinaryConverter(1));
            return helpCode;
        }

        public int[] NegativeConverter(int[] binaryForm)
        {
            int[] helpCode = new int[binaryForm.Length];
            int[] inverted = new int[8] { 1, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 1; i < binaryForm.Length; i++)
            {
                inverted[i] = (binaryForm[i] == 0 ? 1 : 0);
            }
            helpCode = Addition(inverted, BinaryConverter(1));
            return helpCode;
        }
        //додавання двійкових чисел
        public int[] Addition(int[] first, int[] second)
        {
            int[] number = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
            int temp = 0;
            for (int i = first.Length - 1; i >= 1; i--)
            {
                int result = first[i] + second[i] + temp;
                switch (result)
                {
                    case 0:
                        number[i] = result;
                        temp = 0;
                        break;
                    case 1:
                        number[i] = result;
                        temp = 0;
                        break;
                    case 2:
                        number[i] = 0;
                        temp = 1;
                        break;
                    case 3:
                        number[i] = 1;
                        temp = 1;
                        break;
                }
            }
            return number;
        }
        //додавання двійкових чисел беззнаково
        public int[] AdditionNoSign(int[] first, int[] second)
        {
            int[] number = new int[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int temp = 0;
            for (int i = first.Length - 1; i >= 0; i--)
            {
                int result = first[i] + second[i] + temp;
                switch (result)
                {
                    case 0:
                        number[i] = result;
                        temp = 0;
                        break;
                    case 1:
                        number[i] = result;
                        temp = 0;
                        break;
                    case 2:
                        number[i] = 0;
                        temp = 1;
                        break;
                    case 3:
                        number[i] = 1;
                        temp = 1;
                        break;
                }
            }
            return number;
        }

        public int[] ShiftRight(int[] number)
        {
            int previous_bit = number[0];
            int next_bit = number[0];
            number[0] = 0;
            //починаю з 2 біта зліва, бо 1 - знаковий, його не чіпаємо
            for (int i = 1; i < number.Length; i++)
            {
                next_bit = number[i];
                number[i] = previous_bit;
                previous_bit = next_bit;
            }
            return number;
        }

        public int[] ShiftLeft(int[] number)
        {
            int previous_bit = 1;
            int next_bit = 0;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                next_bit = number[i];
                number[i] = previous_bit;
                previous_bit = next_bit;
            }
            return number;
        }

        public int[] ShiftLeftWithSign(int[] number)
        {
            int previous_bit = 0;
            int next_bit = 0;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                next_bit = number[i];
                number[i] = previous_bit;
                previous_bit = next_bit;
            }
            return number;
        }

        public int DecimalConverter(int[] helpCode)
        {
            if (helpCode[0] != 0)
            {
                helpCode = Addition(helpCode, BinaryConverter(-1));
                for (int i = 1; i < helpCode.Length; i++)
                {
                    helpCode[i] = (helpCode[i] == 0 ? 1 : 0);
                }
            }
            int number = 0;
            for (int i = helpCode.Length - 1; i >= 1; i--)
            {
                number += helpCode[i] * (int)Math.Pow(2, helpCode.Length - 1 - i);
            }
            return number;
        }
    }
}

