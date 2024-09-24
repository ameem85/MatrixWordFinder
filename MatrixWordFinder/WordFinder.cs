using System.Text;

public class WordFinder
{
    public IList<string> Matrix { get; }

    public WordFinder(IEnumerable<string> matrix)
    {
        //convert to list to avoid potential multiple enumerations
        Matrix = matrix.ToList();
    }

    public IEnumerable<string> Find(IEnumerable<string> words)
    {
        const int topNWords = 10;

        IDictionary<string,int> inputWordCount = new Dictionary<string, int>();

        //get the horizontal words
        var horizontalWords = Matrix;

        //get the vertical words
        var verticalWords = GetVerticalWords(Matrix);

        //combine the horizontal and vertical words into a single collection
        var allWords = horizontalWords.Concat(verticalWords).ToArray();

        //iterate through each input word and count the occurrences
        foreach (var inputWord in words)
        {
            //var occurrenceCount = allWords.Count(w => w.Contains(inputWord));
            var occurrenceCount = allWords.Sum(w => GetOccurrenceCountInWord(w, inputWord));

            //add the word to the dictionary if it occurs at least once
            if (occurrenceCount > 0)
            {
                inputWordCount[inputWord] = occurrenceCount;
            }
        }

        //order the input words by the count of occurrences
        var orderedWords = inputWordCount
            .OrderByDescending(w => w.Value)
            .Select(p=>p.Key).Take(topNWords);
        return orderedWords;

        ////filter out the words that are not in the list of words, without removing duplicates to ensure the occurrence of the word is counted correctly
        //var filteredWords = allWords
        //    .GroupJoin(words, o => o, i => i, (key, group) => (key, group.FirstOrDefault()))
        //    .Where(a => a.Item2 != null) //filter out the words that are not in the list of words
        //    .Select(a => a.key);    //select the word

        //group by the words and count the occurrences, order by count and take the top N word
        //return filteredWords.GroupBy(
        //        keySelector: s => s,
        //        elementSelector: s => s,
        //        resultSelector: (key, elements) => new { Key = key, Count = elements.Count() })
        //    .OrderBy(a => a.Count)
        //    .Take(topNWords)
        //    .Select(a => a.Key);

    }

    private int GetOccurrenceCountInWord(string word, string inputWord)
    {
        var startIndex = 0;
        var occurrenceCount = 0;
        while (startIndex < word.Length)
        {
            var index = word.IndexOf(inputWord, startIndex, StringComparison.Ordinal);
            if (index == -1)
            {
                break;
            }
            occurrenceCount++;
            startIndex = index + 1;
        }

        return occurrenceCount;
    }


    /// <summary>
    /// Gets the enumerable collection of words read as columns.
    /// </summary>
    /// <param name="matrix">List of words representing the word matrix.</param>
    /// <returns></returns>
    private IEnumerable<string> GetVerticalWords(IList<string> matrix)
    {
        //retrieve the biggest word length
        var biggestWordLength = matrix.Max(word => word.Length);

        //iterate through the columns, up to the biggest word length
        for (var columnNo = 0; columnNo < biggestWordLength; columnNo++)
        {
            var verticalWord = new StringBuilder();

            //iterate through the rows and aggregate as word, any empty cell is ignored
            var columnWiseString = matrix.Aggregate(verticalWord, (word, row) =>
            {
                //get the character at the column number
                var wordChar = row.ElementAtOrDefault(columnNo);

                //append the character to the word
                if (wordChar != default(char))
                {
                    word.Append(wordChar);
                }

                return word;
            }, word => word.ToString());
            yield return columnWiseString;
        }
    }
}