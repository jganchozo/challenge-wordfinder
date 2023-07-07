
namespace WordFinderChallenge
{
    public class WordFinderValidator : IWordFinderValidator
    {
        public bool ValidateSizesBetweenTwoValues(int firstValue, int secondValue)
        {
            bool isValid = false;

            if (firstValue != secondValue)
            {
                isValid = true;
            }

            return isValid;
        }
    }
}
