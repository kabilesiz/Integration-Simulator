using System.Text.RegularExpressions;
using Bogus.DataSets;

namespace Integration.Helpers;

public static class DataHelper
{
    public static async Task<List<string>> GetItems()
    {
        var (frequencies, length )= await GetUserInput();
        var lorem = new Lorem();
        
        return GenerateItemsWithFrequencies(lorem, frequencies, length);
    }
    
    private static async Task<(Dictionary<string, int>, int)> GetUserInput()
    {
        while (true)
        {
            try
            {
                Console.Write("Please enter the number of items/words you wish to work on : ");
                
                var lengthOfItems = int.Parse(Console.ReadLine() ?? string.Empty);
                
                if (lengthOfItems <= 0)
                {
                    await Error();
                }
                
                Console.Write("Enter word frequencies (word:count), separate each pair with a comma (e.g., lorem:3,ipsum:2):");
                
                var frequencyResponse = Console.ReadLine();
                var trimmedFrequencyResponse = Regex.Replace(frequencyResponse!, @"\s+", string.Empty);
                var keyValuePattern = @"([^,]+):(\d+)";
                var regex = new Regex(keyValuePattern);
                var matches = regex.Matches(trimmedFrequencyResponse);
                var allItems = trimmedFrequencyResponse.Split(',');
                var frequencies = new Dictionary<string, int>();

                if (allItems.Length <= 0)
                {
                    return (frequencies, lengthOfItems); 
                }
                
                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        var key = match.Groups[1].Value;
                        var value = int.Parse(match.Groups[2].Value);
                        frequencies[key] = value;
                    }
                }
                
                foreach (var item in allItems)
                {
                    if (!frequencies.ContainsKey(item.Split(':')[0]))
                    {
                        Console.WriteLine($"Invalid input: [{item}]. This input will have its frequency value set to 0.");
                    }
                }

                var sum = frequencies.Sum(x => x.Value);
                
                if (sum > lengthOfItems)
                {
                    Console.WriteLine("The sum of frequencies can not be larger than items length!");
                    await AgainMenu();
                }
                else
                {
                    return (frequencies, lengthOfItems);
                }
                
            }
            catch (Exception)
            {
                await Error();
            }
        }
    }
    
    private static List<string> GenerateItemsWithFrequencies(Lorem lorem, Dictionary<string, int> frequencies, int countOfItems)
    {
        var words = new List<string>();

        foreach (var kvp in frequencies)
        {
            words.AddRange(Enumerable.Repeat(kvp.Key, kvp.Value));
        }

        var remainingWordsCount = countOfItems - words.Count;
        var randomWords = lorem.Words(remainingWordsCount);

        words.AddRange(randomWords);

        words = ShuffleList(words);

        return words;
    }
    
    private static List<T> ShuffleList<T>(List<T> list)
    {
        var random = new Random();
        for (var i = list.Count - 1; i > 0; i--)
        {
            var j = random.Next(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
        return list;
    }
    
    public static async Task Error()
    {
        Console.WriteLine("\t    An attempt was made with an input type other than specified.");
        await AgainMenu();
    }

    public static Task AgainMenu()
    {
        Console.Write("Would you like to try again? [Y/y] :");
        var response = Console.ReadLine();
        if (!string.Equals(response, "Y", StringComparison.InvariantCultureIgnoreCase))
        {
            Environment.Exit(0);
        }

        return Task.CompletedTask;
    }
}