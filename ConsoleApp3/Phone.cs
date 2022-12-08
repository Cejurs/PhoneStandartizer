using System.Text;
using System.Text.RegularExpressions;

namespace PhoneNumbersStandartizer
{
    public class Phone
    {
        public int Id { get; set; }
        public IEnumerable<string> Numbers { get; set; }

        public Phone(int id, IEnumerable<string> numbers)
        {
            Id = id;
            Numbers = numbers;
        }
    }
}
