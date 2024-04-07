namespace BookStore.Models.Exceptions;

[Serializable]
public class YearException : Exception
{
    internal object parameter;

    public YearException(string message, string parameter) : base(message) => this.parameter = parameter;
}