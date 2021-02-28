using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace caxxperts
{
    // Display the content of a text file on the screen.
    // Function uses ReadToEnd() method.
    class Caxxpertstask
    {
        static void Main()
        {
            string ASCIIString =
                @"
                ---       ---      |    |  |     -----   
                 /         _|      |    |__|     |___    
                 \        |        |       |         |   
                --        ---      |       |     ____|  ";

            string[] linesupdate =
                File
                    .ReadAllLines(@"C:\Users\NETZONE 2\Desktop\caxxperts\NumberParserExtended.txt");

            //foreach (string line in linesupdate)
            //          Console.WriteLine (linesupdate);
            Console.WriteLine("Recognized numbers");

            string[] lines =
                ASCIIString
                    .Split(new [] { "\n", "\r\n" },
                    StringSplitOptions.RemoveEmptyEntries);

            lines = lines.ReplaceSpacesWithSeparator("$");

            ASCIINumbersParser parser = new ASCIINumbersParser(lines, "$");

            // Try to find all numbers contained in the ASCII string
            foreach (string[] parsernumber in parser.parsernumbersList)
            {
                for (int i = 1; i < 10; ++i)
                {
                    string[] num =
                        ASCIINumberHelper.GetASCIIRepresentationForNumber(i);
                    if (
                        ASCIINumberHelper
                            .ASCIIRepresentationMatch(num, parsernumber)
                    )
                        Console
                            .WriteLine("Number {0} was found in the string.",
                            i);
                }
            }
        }
    }

    public static class StringHelperClass
    {
        // Extension method to remove any unnecessary white-space and put a separator char instead.
        public static string[]
        ReplaceSpacesWithSeparator(this string[] text, string separator)
        {
            // Create an array of StringBuilder, one for every line in the text.
            StringBuilder[] stringBuilders = new StringBuilder[text.Length];

            // Initialize stringBuilders.
            for (int n = 0; n < text.Length; n++)
            stringBuilders[n] = new StringBuilder().Append(separator);

            // Get shortest line in the text, in order to avoid Out Of Range Exception.
            int shorterstLine = text.Min(line => line.Length);

            // Temporary variables.
            int lastSeparatorIndex = 0;
            bool previousCharWasSpace = false;

            // Start processing the text, char after char.
            for (int n = 0; n < shorterstLine; ++n)
            {
                // Look for white-spaces on the same position on
                // all the lines of the text.
                if (text.All(line => line[n] == ' '))
                {
                    // Go to next char if previous char was also a white-space,
                    // or if this is the first white-space char of the text.
                    if (previousCharWasSpace || n == 0)
                    {
                        previousCharWasSpace = true;
                        lastSeparatorIndex = n + 1;
                        continue;
                    }
                    previousCharWasSpace = true;

                    // Append non white-space chars to the StringBuilder
                    // of each line, for later use.
                    for (int i = lastSeparatorIndex; i < n; ++i)
                    {
                        for (int j = 0; j < text.Length; j++)
                        stringBuilders[j].Append(text[j][i]);
                    }

                    // Append separator char.
                    for (int j = 0; j < text.Length; j++)
                    stringBuilders[j].Append(separator);

                    lastSeparatorIndex = n + 1;
                }
                else
                    previousCharWasSpace = false;
            }

            for (int j = 0; j < text.Length; j++)
            text[j] = stringBuilders[j].ToString();

            // Return formatted text.
            return text;
        }
    }

    public static class ASCIINumberHelper
    {
        // Get an ASCII art representation of a number.
        public static string[] GetASCIIRepresentationForNumber(int number)
        {
            switch (number)
            {
                case 1:
                    return new [] { "|", "|", "|", "|" };
                case 2:
                    return new [] { "---", " _|", "|  ", "---" };
                case 3:
                    return new [] { "---", " / ", @" \ ", "-- " };
                case 4:
                    return new [] { "|  |", "|__|", "   |", "   |" };
                case 5:
                    return new [] { "-----", "|___ ", "    |", "____|" };
                default:
                    return null;
            }
        }

        // See if two numbers represented as ASCII art are equal.
        public static bool
        ASCIIRepresentationMatch(string[] number1, string[] number2)
        {
            // Return false if the any of the two numbers is null
            // or their lenght is different.
            // if (number1 == null || number2 == null)
            //     return false;
            // if (number1.Length != number2.Length)
            //     return false;
            if (number1?.Length != number2?.Length) return false;

            try
            {
                for (int n = 0; n < number1.Length; ++n)
                {
                    if (number1[n].CompareTo(number2[n]) != 0) return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return false;
            }

            return true;
        }
    }

    public class ASCIINumbersParser
    {
        // Will store a list of all the possible numbers
        // found in the text.
        public List<string[]> parsernumbersList { get; }

        public ASCIINumbersParser(string[] text, string separator)
        {
            parsernumbersList = new List<string[]>();

            string[][] parsernumbers = new string[text.Length][];

            for (int n = 0; n < text.Length; ++n)
            {
                // Split each line in the text, using the separator char/string.
                parsernumbers[n] =
                    text[n]
                        .Split(new [] { separator },
                        StringSplitOptions.RemoveEmptyEntries);
            }

            // Put the strings in such a way that each parsernumberList item
            // contains only one possible number found in the text.
            for (int i = 0; i < parsernumbers[0].Length; ++i)
            parsernumbersList.Add(parsernumbers.Select(c => c[i]).ToArray());
        }
    }
}
