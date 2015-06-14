using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CanonicalEquation.Logic;

namespace CanonicalEquation.Console
{
    class Program
    {
        private static readonly EquationLogic Logic = new EquationLogic();

        static void Main(string[] args)
        {
            if (args.Any())
            {
                //файловый режим
                string path = args.First();
                if (File.Exists(path))
                {
                    string outFilePath = Path.GetDirectoryName(path) + Path.GetFileNameWithoutExtension(path) + ".out";
                    using (var writer = new StreamWriter(outFilePath))
                    {
                        using (var reader = new StreamReader(path))
                        {
                            string equation = reader.ReadToEnd();
                            try
                            {
                                string result = Logic.Process(equation);
                                writer.WriteLine(result);
                            }
                            catch (Exception ex)
                            {
                                writer.WriteLine("Не удалось преобразовать выражение. Ошибка: " + ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    System.Console.WriteLine("Не удалось найти файл - " + path);
                }
            }
            else
            {
                //Интерактивный режим
                while (true)
                {
                    System.Console.WriteLine("Введите выражение для преобразования");
                    string equation = System.Console.ReadLine();
                    try
                    {
                        string result = Logic.Process(equation);
                        System.Console.WriteLine(result);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Не удалось преобразовать выражение. Ошибка: " + ex.Message);
                    }
                }
            }
        }
    }
}
