namespace DiabetsAPI.DB
{
    public static class DbInitializer
    {
        public static void Initialize(DiabetsContext context)
        {
            if (context.Database.EnsureCreated())
            {

            }
        }
    }
}
