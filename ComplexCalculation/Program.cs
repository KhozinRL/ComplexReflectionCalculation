using System;
using System.IO;
using System.Reflection;
using System.Numerics;

namespace ComplexCalculation
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly asm = null;
            string assemblyName = "Complex.dll";

            Console.WriteLine(assemblyName);

            try
            {
                 asm = Assembly.LoadFile(Path.GetFullPath(assemblyName));
            }
            catch {
                Console.WriteLine("Сборка {0} не найдена!", assemblyName);
                Complex e = new Complex(1, 2);
                Complex f = Complex.FromPolarCoordinates(3, Math.PI / 8);
                Complex d = 34 + Complex.Pow(e, f);
                Console.WriteLine("Результат: {0:f4}", d);
                Console.WriteLine("В триганометрическом виде: abs = {0:f4}, arg = {1:f4}", d.Magnitude, d.Phase);
                return;
            }
            
            Type complexType = asm.GetType("Complex.Complex");


            Console.WriteLine("Без dynamic: ");
         //Создали первый экземпляр через контруктор
            object x = Activator.CreateInstance(complexType, 2, 3);
         //Получили static метод и создали второй экземпляр
            MethodInfo createComplex = complexType.GetMethod("CreateComplex");
            object y = createComplex?.Invoke(null, new object[] { 14, Math.PI / 4 });
         //Получили операторы для действий с комплексными числами
            MethodInfo sum = complexType.GetMethod("op_Addition");
            MethodInfo division = complexType.GetMethod("op_Division");
            MethodInfo multiply = complexType.GetMethod("op_Multiply");
         //Получаем Property
            PropertyInfo abs = complexType.GetProperty("Abs");
            PropertyInfo arg = complexType.GetProperty("Arg");
         //Выполняем действия
            object z = sum?.Invoke(null, new object[] { x, y });
            z = multiply?.Invoke(null, new object[] {z, z });
            z = division?.Invoke(null, new object[] { z, Activator.CreateInstance(complexType, 27) });
         //Выводим результат
            Console.WriteLine("Результат: {0:f4}", z);
            Console.WriteLine("В триганометрическом виде: abs = {0:f4}, arg = {1:f4}", abs?.GetValue(z), arg?.GetValue(z));

         //С использованием dynamic
            Console.WriteLine("\nС использаванием dynamic: ");
         //Создали первый экземпляр через контруктор
            dynamic a = Activator.CreateInstance(complexType, 4, 1);
         //Создали второй экземпляр с помощью статического метода
            dynamic b = createComplex?.Invoke(null, new object[] { 2, Math.PI / 3 });
         //Вычисляем результат и выводим
            dynamic res = a * a + b * b;
            dynamic c = res*res / (3 * b);
            Console.WriteLine("Результат: {0:f4}", c);
            Console.WriteLine("В триганометрическом виде: abs = {0:f4}, arg = {1:f4}", c.Abs, c.Arg);

        }

    }

}
