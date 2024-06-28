namespace Motk.Matchmaking.New;

public static class RandomUtils
{
  private const string PossibleChars = "abcdefghijklmnopqrstuvwxyz0123456789";

  public static string RandomString(Random random, int num = 10)
  {
    var result = new char[num];
      
    while(num-- > 0)
      result[num] = PossibleChars[random.Next(0, PossibleChars.Length)];

    return new string(result);
  }
}