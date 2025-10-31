using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PolybiusCipher
{
    public class PolybiusCipher
    {
        private static readonly Dictionary<char, string> polybiusSquare = new()
        {
            {'A', "11"}, {'B', "12"}, {'C', "13"}, {'D', "14"}, {'E', "15"},
            {'F', "21"}, {'G', "22"}, {'H', "23"}, {'I', "24"}, {'J', "24"}, {'K', "25"},
            {'L', "31"}, {'M', "32"}, {'N', "33"}, {'O', "34"}, {'P', "35"},
            {'Q', "41"}, {'R', "42"}, {'S', "43"}, {'T', "44"}, {'U', "45"},
            {'V', "51"}, {'W', "52"}, {'X', "53"}, {'Y', "54"}, {'Z', "55"}
        };

        private static readonly Dictionary<string, char> reverseSquare;

        static PolybiusCipher()
        {
            reverseSquare = polybiusSquare
                .GroupBy(kvp => kvp.Value)
                .ToDictionary(g => g.Key, g => g.First().Key);
        }

        public static string Encrypt(string text)
        {
            var encrypted = new List<string>();

            foreach (char character in text.ToUpper())
            {
                if (polybiusSquare.ContainsKey(character))
                {
                    encrypted.Add(polybiusSquare[character]);
                }
                else if (character == ' ')
                {
                    encrypted.Add(" ");
                }
            }

            return string.Join(" ", encrypted);
        }

        public static string Decrypt(string encodedText)
        {
            var decrypted = new List<char>();
            var codes = encodedText.Split(' ');

            foreach (string code in codes)
            {
                if (reverseSquare.ContainsKey(code))
                {
                    decrypted.Add(reverseSquare[code]);
                }
                else if (string.IsNullOrEmpty(code))
                {
                    decrypted.Add(' ');
                }
            }

            return new string(decrypted.ToArray());
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Шифр Полибия ===");

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1 - Зашифровать текст");
                Console.WriteLine("2 - Расшифровать текст");
                Console.WriteLine("3 - Выход");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        EncryptText();
                        break;
                    case "2":
                        DecryptText();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        private static void EncryptText()
        {
            try
            {
                Console.Write("Введите текст для шифрования: ");
                string text = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(text))
                {
                    Console.WriteLine("Текст не может быть пустым!");
                    return;
                }

                string encryptedText = PolybiusCipher.Encrypt(text);

                // Сохранение в файл
                File.WriteAllText("encrypted.txt", encryptedText);
                Console.WriteLine($"Зашифрованный текст: {encryptedText}");
                Console.WriteLine("Текст сохранён в файл 'encrypted.txt'");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при шифровании: {ex.Message}");
            }
        }

        private static void DecryptText()
        {
            try
            {
                if (!File.Exists("encrypted.txt"))
                {
                    Console.WriteLine("Файл 'encrypted.txt' не найден!");
                    return;
                }

                string encryptedText = File.ReadAllText("encrypted.txt");
                string decryptedText = PolybiusCipher.Decrypt(encryptedText);

                Console.WriteLine($"Зашифрованный текст из файла: {encryptedText}");
                Console.WriteLine($"Расшифрованный текст: {decryptedText}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при расшифровке: {ex.Message}");
            }
        }
    }
}