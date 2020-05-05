namespace UAParser.Abstraction
{
    internal interface IParser<out T>
    {
        T Parse(string input);
    }
}
