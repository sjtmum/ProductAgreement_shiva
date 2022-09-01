namespace AgMg_SandeepTrivedi.Extentions
{
    public static class Extentions
    {
        public static int ToInteger(this string value)
        {
            if (int.TryParse(value, out int result))
                return result;

            return 0;
        }
        public static decimal ToDecimal(this string value)
        {
            if (decimal.TryParse(value, out decimal result))
                return result;

            return 0;
        }
    }
}
