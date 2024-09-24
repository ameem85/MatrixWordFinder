namespace WordFinderTests;

[TestClass]
public class WordFinderTests
{
    [TestMethod]
    public void Find_ReturnsFoundWords_WithAtLeastOneOccurrence()
    {
        // arrange
        var matrix = new List<string>
        {
            "abcdc",
            "fgwio",
            "chill",
            "pqnsd",
            "uvdxy",
        };
        var words = new List<string>
        {
            "cold",
            "wind",
            "snow",
            "chill"
        };
        var wordFinder = new WordFinder(matrix);

        // act
        var result = wordFinder.Find(words).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "cold", "wind", "chill" }, result);
    }

    [TestMethod]
    public void Find_ReturnsFoundWords_InDescendingOccurenceOrder()
    {
        // arrange
        var matrix = new List<string>
        {
            "abcdc",
            "fgwio",
            "chill",
            "pqnsd",
            "uvdxy",
            "fgwio",
            "chill",
            "pqnsd",
            "uvdxy",
        };
        var words = new List<string>
        {
            "cold",
            "wind",
            "snow",
            "chill"
        };
        var wordFinder = new WordFinder(matrix);

        // act
        var result = wordFinder.Find(words).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "wind", "chill", "cold" }, result);
    }

    [TestMethod]
    public void Find_ReturnsFoundWords_IncludingRepeatsInSameRowOrColumn()
    {
        // arrange
        var matrix = new List<string>
        {
            "ABABC",
            "PABAB",
            "EABAB",
        };
        var words = new List<string>
        {
           "APE",
           "AB",
        };
        var wordFinder = new WordFinder(matrix);

        // act
        var result = wordFinder.Find(words).ToArray();

        // assert
        CollectionAssert.AreEqual(new[] { "AB", "APE"}, result);
    }
}