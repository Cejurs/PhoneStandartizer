using PhoneNumbersStandartizer;

var book = new PhoneBook(@"..\..\..\Files\phones.csv");
book.Save(@"..\..\..\Files\phones2.csv");