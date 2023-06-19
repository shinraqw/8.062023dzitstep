using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace _8._062023dzitstep
{
    public class Journal
    {
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string PublicationDate { get; set; }
        public int PageCount { get; set; }
        public List<Article> Articles { get; set; }

        public Journal()
        {
            Articles = new List<Article>();
        }

        public override string ToString()
        {
            string journalInfo = $"Назва журналу: {Name}\nНазва видавництва: {Publisher}\nДата видання: {PublicationDate}\nКількість сторінок: {PageCount}";

            string articlesInfo = "\nСтатті:";
            foreach (Article article in Articles)
            {
                articlesInfo += $"\nНазва статті: {article.Title}\nКількість символів: {article.CharacterCount}\nАнонс статті: {article.Preview}\n";
            }

            return journalInfo + articlesInfo;
        }
    }


    public class Article
    {
        public string Title { get; set; }
        public int CharacterCount { get; set; }
        public string Preview { get; set; }

        public Article(string title, int characterCount, string preview)
        {
            Title = title;
            CharacterCount = characterCount;
            Preview = preview;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //1
            Fraction[] fractions;

            Console.WriteLine("Введіть кількість дробів:");
            int count = int.Parse(Console.ReadLine());

            fractions = new Fraction[count];

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"Дріб #{i + 1}:");
                Console.Write("Чисельник: ");
                int numerator = int.Parse(Console.ReadLine());
                Console.Write("Знаменник: ");
                int denominator = int.Parse(Console.ReadLine());

                fractions[i] = new Fraction(numerator, denominator);
            }

            // Збереження серіалізованого масиву у файл
            SaveFractionsToFile(fractions, "fractions.txt");

            // Завантаження серіалізованого масиву з файлу та десеріалізація
            Fraction[] loadedFractions = LoadFractionsFromFile("fractions.txt");

            Console.WriteLine("Завантажені дроби:");
            foreach (Fraction fraction in loadedFractions)
            {
                Console.WriteLine(fraction);
            }
            //2-3-4
            List<Journal> journals = new List<Journal>();

            while (true)
            {
                Console.WriteLine("\nОберіть опцію:");
                Console.WriteLine("1. Створити новий журнал");
                Console.WriteLine("2. Вивести інформацію про журнали");
                Console.WriteLine("3. Серіалізувати та зберегти журнали у файл");
                Console.WriteLine("4. Завантажити та десеріалізувати журнали з файлу");
                Console.WriteLine("5. Вийти з програми");

                int option = int.Parse(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Journal newJournal = CreateJournal();
                        journals.Add(newJournal);
                        Console.WriteLine("Журнал створено.");
                        break;
                    case 2:
                        DisplayJournals(journals);
                        break;
                    case 3:
                        SerializeJournals(journals);
                        break;
                    case 4:
                        journals = DeserializeJournals();
                        Console.WriteLine("Журнали завантажено.");
                        break;
                    case 5:
                        return;
                    default:
                        Console.WriteLine("Невірна опція. Спробуйте ще раз.");
                        break;
                }
            }
            static void SaveFractionsToFile(Fraction[] fractions, string filename)
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    foreach (Fraction fraction in fractions)
                    {
                        writer.WriteLine($"{fraction.Numerator},{fraction.Denominator}");
                    }
                }
            }

            static Fraction[] LoadFractionsFromFile(string filename)
            {
                string[] lines = File.ReadAllLines(filename);
                Fraction[] fractions = new Fraction[lines.Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] parts = lines[i].Split(',');
                    int numerator = int.Parse(parts[0]);
                    int denominator = int.Parse(parts[1]);

                    fractions[i] = new Fraction(numerator, denominator);
                }

                return fractions;
            }
        }
        public class Fraction
        {
            public int Numerator { get; set; }
            public int Denominator { get; set; }

            public Fraction(int numerator, int denominator)
            {
                Numerator = numerator;
                Denominator = denominator;
            }

            public override string ToString()
            {
                return $"{Numerator}/{Denominator}";
            }
        }
        static Journal CreateJournal()
        {
            Journal journal = new Journal();

            Console.WriteLine("Введіть інформацію про журнал:");
            Console.Write("Назва журналу: ");
            journal.Name = Console.ReadLine();
            Console.Write("Назва видавництва: ");
            journal.Publisher = Console.ReadLine();
            Console.Write("Дата видання: ");
            journal.PublicationDate = Console.ReadLine();
            Console.Write("Кількість сторінок: ");
            journal.PageCount = int.Parse(Console.ReadLine());

            // Введення інформації про статті
            Console.WriteLine("\nВведіть інформацію про статті (введіть 'done', щоб завершити введення):");
            while (true)
            {
                Console.WriteLine("\nСтаття:");
                Console.Write("Назва статті: ");
                string articleTitle = Console.ReadLine();
                if (articleTitle.ToLower() == "done")
                    break;

                Console.Write("Кількість символів: ");
                int characterCount = int.Parse(Console.ReadLine());

                Console.Write("Анонс статті: ");
                string articlePreview = Console.ReadLine();

                Article article = new Article(articleTitle, characterCount, articlePreview);
                journal.Articles.Add(article);
            }

            return journal;
        }

        static void DisplayJournals(List<Journal> journals)
        {
            Console.WriteLine("\nІнформація про журнали:");

            foreach (Journal journal in journals)
            {
                Console.WriteLine(journal.ToString());
            }
        }

        static void SerializeJournals(List<Journal> journals)
        {
            Console.Write("Введіть назву файлу для збереження журналів: ");
            string fileName = Console.ReadLine();

            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(fileStream, journals);
                }
                Console.WriteLine($"Журнали збережено у файл {fileName}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час збереження журналів: {ex.Message}");
            }
        }

        static List<Journal> DeserializeJournals()
        {
            Console.Write("Введіть назву файлу для завантаження журналів: ");
            string fileName = Console.ReadLine();

            try
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    List<Journal> journals = (List<Journal>)binaryFormatter.Deserialize(fileStream);
                    return journals;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка під час завантаження журналів: {ex.Message}");
                return new List<Journal>();
            }
        }
    }

}