using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace TelephoneDictWind
{
    class OperationWithDict
    {
        #region поля
        /// <summary>
        /// Приватный словарь, хранит телефонный справочник.
        /// </summary>
        private Dictionary<string, string> telephoneDirectory = new Dictionary<string, string>();
        #endregion

        #region Свойства

        /// <summary>
        /// Публичное свойство для доступа на чтение и запись к телефонному справочнику.
        /// </summary>
        public Dictionary<string, string> TelephoneDirectory { get { return telephoneDirectory; } set { telephoneDirectory =  value; } }
        #endregion

        #region Методы

        /// <summary>
        /// Метод для дополнения справочника парой телефон - ФИО
        /// 
        /// Данный метод в оконном приложении не используется
        /// 
        /// </summary>
        private void InputData()
        {
            var flag = true;
            while (flag)
            {
                Console.WriteLine("Введите номер телефона человека и его ФИО через запятую. " +
                    "Для прекращения ввода нажмите enter\nПример ввода +70001112233, Иванов Иван Иванович");
                var inputString = Console.ReadLine();

                if (String.IsNullOrWhiteSpace(inputString))
                {
                    flag = false;
                }
                else
                {
                    var arg = inputString.Split(',');
                    var key = arg[0];

                    bool convertOk = true;
                    FormatPhone(ref key, ref convertOk);

                    if (convertOk)
                    {
                        var value = arg[1];
                       // if (!telephoneDirectory.TryAdd(key, value))
                        {
                            Console.WriteLine("Введенный номер уже присутствует в базе " +
                                "данных и не будет добавлен повторно.");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Данная пара телефон - ФИО записана не будет. " +
                            "Номер телефона не может содержать буквы и спецсимволы.");
                    }

                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Метод для форматирования номера телефона
        /// </summary>
        /// <param name="phone">номер телефона в строковом формате</param>
        /// <param name="convertOk">индикатор корректности номера - если номер содержит 
        /// запрещенные символы он не будет отредактирован
        /// и будет возвращено значение false
        ///  
        /// Данный метод в оконном приложении не используется
        /// 
        /// </param>
        private void FormatPhone(ref string phone, ref bool convertOk)
        {
            UInt64 number;
            bool flag = UInt64.TryParse(phone, out number);
            if (flag)
            {
                if (phone.Length < 13 && phone.Length > 9)
                {
                    if ((phone[0] == '7') || (phone[0] == '8'))
                    {
                        phone = "+7-" + phone[1] + phone[2] + phone[3] + "-" + phone[4] 
                            + phone[5] + phone[6] + "-" + phone[7] + phone[8] + "-" + phone[9] + phone[10];
                    }
                    else
                    {
                        if ((phone[0] == '+') && (phone[1] == '7'))
                        {
                            phone = "+7-" + phone[2] + phone[3] + phone[4] + "-" 
                                + phone[5] + phone[6] + phone[7] + "-" + phone[8] + phone[9] + 
                                "-" + phone[10] + phone[11];
                        }
                        else
                        {
                            phone = "+7-" + phone[0] + phone[1] + phone[2] + "-" + phone[3] + 
                                phone[4] + phone[5] + "-" + phone[6] + phone[7] + "-" + phone[8] + phone[9];
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Номер введен некорректно.");
                convertOk = false;
            }
        }

        /// <summary>
        /// Вывод на консоль всего справочника.
        ///  
        /// Данный метод в оконном приложении не используется
        /// 
        /// </summary>
        private void PrintDictionary()
        {
            foreach (KeyValuePair<string, string> e in telephoneDirectory)
            {
                Console.WriteLine($"{e.Key,13}, {e.Value}");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Метод сохраняет словарик в файл
        /// </summary>
        /// <param name="dict"></param>
        public void Save(Dictionary<string, string> dict, string path, bool append)
        {
            try
            {
                using (var writer = new StreamWriter(path, append)) //открыт поток на запись
                {
                    foreach (var kvp in dict)
                    {
                        writer.WriteLine($"{kvp.Key},{kvp.Value}"); // kvp- key value pair
                    }
                }

            }
            catch (Exception exeption)
            {
                MessageBox.Show(exeption.Message, "Error !!!", MessageBoxButtons.OK);
            }
            
        }

        /// <summary>
        /// Метод читает файл и отправляет в словарь всё что есть в файле уже. 
        /// </summary>
        public void Read(string path)
        {
            telephoneDirectory.Clear();
            telephoneDirectory = File.ReadAllLines(path, Encoding.UTF8)
                .Select(line => line.Split(new[] { ',' }))
                .ToDictionary(arr => arr[0], arr => arr[1]);
        }

        /// <summary>
        /// Метод выводит на консоль пользователю всё, что было записано в файле ранее. 
        /// Считывает данные из файла и выводит на экран.
        /// Воспринимает как словарь
        /// 
        /// В оконном приложении данный метод не используется
        /// 
        /// </summary>
        public void FileOutput()
        {
            using (var read = new StreamReader("telephone.txt"))
            {
                string line;
                while ((line = read.ReadLine()) != null) //построчное чтение до конца файла
                {
                    var ar = line.Split(',');
                    var key = ar[0];
                    var value = ar[1];
                    telephoneDirectory.Add(key, value);
                }
            }

            PrintDictionary();
        }

        /// <summary>
        /// Вызывается в main для того, чтобы показать что мы записали в файл, старое (записанное в файле) не сохраняется
        /// 
        /// Данный метод в оконном приложении не используется
        /// 
        /// </summary>
        public void Demonstration()
        {
            InputData();
            Console.WriteLine("Справочник содержит:");
            PrintDictionary();
            Save(telephoneDirectory, "name.json", true);
        }

        #endregion

    }
}
