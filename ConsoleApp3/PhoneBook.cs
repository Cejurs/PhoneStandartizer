using System.Text;
using System.Text.RegularExpressions;

namespace PhoneNumbersStandartizer
{
    public class PhoneBook
    {
        private List<Phone> phones;
        public IEnumerable<Phone> Phones { get => phones; }

        public PhoneBook(string path)
        {
            phones = LoadPhones(path);
        }
        private static List<Phone> LoadPhones(string path)
        {
            var phones = new List<Phone>();
            if (File.Exists(path) == false) return phones;
            using (var stream = new StreamReader(path))
            {
                var line = stream.ReadLine(); // Считывание заголовка
                line = stream.ReadLine(); // Считываем 1 строку.
                while (line != null)
                {
                    var arr = line.Split(';');
                    var id = int.Parse(arr[0]);
                    var phoneNumbers = new List<string>();
                    for (int i = 1; i < arr.Length; i++)
                    {
                        var numbers = FindNumbersByRegex(arr[i]);
                        for (int j = 0; j < numbers.Count; j++) // Удаление всех лишних знаков
                        {
                            var str = numbers[j].Value;
                            var sb = new StringBuilder();
                            for (int k = 0; k < str.Length; k++)
                            {
                                if (char.IsDigit(str[k]))
                                {
                                    sb.Append(str[k]);
                                }
                            }
                            var number = sb.ToString();
                            if (string.IsNullOrEmpty(number)) continue;
                            if (number[0] == '8') number = '7' + number.Substring(1);
                            if (number.Length == 10) number = '7' + number;
                            phoneNumbers.Add(number);
                        }
                    }
                    phones.Add(new Phone(id, phoneNumbers));
                    line = stream.ReadLine();
                }
            }
            return phones;

        }
        public void Save(string path)
        {
            if (File.Exists(path) == false)
            {
                var file = File.Create(path);
                file.Close();
            }
            using (var stream = new StreamWriter(path))
            {
                foreach (var phone in phones)
                {
                    var line = new StringBuilder();
                    var id = phone.Id + ";";
                    line.Append(id);
                    var flag = phone.Numbers.Count() > 1 ? "\"" : "";
                    line.Append(flag);
                    foreach (var number in phone.Numbers)
                    {
                        var str = number + ";";
                        line.Append(str);
                    }
                    line.Append(flag);
                    stream.WriteLine(line.ToString());
                }
            }
        }
        private static MatchCollection FindNumbersByRegex(string str)
        {
            return Regex.Matches(str, "((8|\\+7)[\\- ]?)?(\\(?\\d{3}\\)?[\\- ]?)?[\\d\\- ]{7,10}");
        }
    }
}
