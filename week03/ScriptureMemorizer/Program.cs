using System;
using System.Collections.Generic;
using System.Linq;

namespace ScriptureMemorizer
{
    // Represents the scripture reference (e.g., "John 3:16" or "Proverbs 3:5-6")
    public class ScriptureReference
    {
        public string Reference { get; private set; }

        // Constructor for single verse (e.g., "John 3:16")
        public ScriptureReference(string reference)
        {
            Reference = reference;
        }

        // Constructor for verse range (e.g., "Proverbs 3:5-6")
        public ScriptureReference(string book, int chapter, int startVerse, int? endVerse = null)
        {
            if (endVerse.HasValue)
            {
                Reference = $"{book} {chapter}:{startVerse}-{endVerse.Value}";
            }
            else
            {
                Reference = $"{book} {chapter}:{startVerse}";
            }
        }

        public override string ToString()
        {
            return Reference;
        }
    }

    // Represents one word in the scripture text with ability to hide it
    public class Word
    {
        public string Text { get; private set; }
        public bool IsHidden { get; private set; }

        public Word(string text)
        {
            Text = text;
            IsHidden = false;
        }

        public void Hide()
        {
            IsHidden = true;
        }

        public override string ToString()
        {
            if (IsHidden)
            {
                return new string('_', Text.Length);
            }
            else
            {
                return Text;
            }
        }
    }

    // Represents the scripture comprising a reference and list of words
    public class Scripture
    {
        public ScriptureReference Reference { get; private set; }
        private List<Word> words;

        public Scripture(ScriptureReference reference, string text)
        {
            Reference = reference;
            words = new List<Word>();
            string[] splitWords = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var word in splitWords)
            {
                words.Add(new Word(word));
            }
        }

        // Hide a specified number of random words that are not already hidden
        public bool HideRandomWords(int count = 3)
        {
            var notHiddenWords = words.Where(w => !w.IsHidden).ToList();

            if (notHiddenWords.Count == 0)
            {
                return false; // all words are hidden
            }

            int toHideCount = Math.Min(count, notHiddenWords.Count);
            Random rand = new Random();

            while (toHideCount > 0)
            {
                int index = rand.Next(notHiddenWords.Count);
                notHiddenWords[index].Hide();
                notHiddenWords.RemoveAt(index);
                toHideCount--;
            }
            return true;
        }

        public bool AllWordsHidden()
        {
            return words.All(w => w.IsHidden);
        }

        public override string ToString()
        {
            return Reference.ToString() + Environment.NewLine + string.Join(" ", words.Select(w => w.ToString()));
        }
    }

    // Main Program class to run the Scripture Memorizer logic
    public class Program
    {
        // A library of scriptures to select from
        private static readonly List<(ScriptureReference reference, string text)> ScriptureLibrary = new List<(ScriptureReference, string)>
        {
            (new ScriptureReference("John 3:16"), "For God so loved the world that he gave his one and only Son, that whoever believes in him shall not perish but have eternal life."),
            (new ScriptureReference("Proverbs 3:5-6"), "Trust in the Lord with all your heart and lean not on your own understanding; in all your ways submit to him, and he will make your paths straight."),
            (new ScriptureReference("Psalm 23:1-3"), "The Lord is my shepherd, I lack nothing. He makes me lie down in green pastures, he leads me beside quiet waters, he refreshes my soul."),
            (new ScriptureReference("Philippians 4:13"), "I can do all this through him who gives me strength.")
        };

        static void ClearScreen()
        {
            Console.Clear();
        }

        static void Main(string[] args)
        {
            var rand = new Random();
            var (reference, text) = ScriptureLibrary[rand.Next(ScriptureLibrary.Count)];

            Scripture scripture = new Scripture(reference, text);

            while (true)
            {
                ClearScreen();
                Console.WriteLine(scripture);
                Console.WriteLine();
                Console.Write("Press Enter to hide some words or type 'quit' to exit: ");
                string input = Console.ReadLine();

                if (input.Trim().ToLower() == "quit")
                {
                    break;
                }

                if (!scripture.HideRandomWords())
                {
                    ClearScreen();
                    Console.WriteLine(scripture);
                    Console.WriteLine();
                    Console.WriteLine("All words are now hidden! Well done!");
                    break;
                }
            }

            Console.WriteLine("Thanks for using Scripture Memorizer. Keep practicing!");
        }
    }
}

/*
Exceeding core requirements:
- Added a scripture library with multiple scriptures to randomly select from, providing variety.
- The program hides multiple random words (default 3) at a time to increase memorization challenge.
- Proper encapsulation with classes: ScriptureReference, Word, Scripture, and Program.
- Multiple constructors for ScriptureReference handle single verses and verse ranges.
- Clear screen before each display to improve user experience.
- Extensive in-code comments describe design decisions and logic.
*/

