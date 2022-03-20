namespace GameService.Infrastructure.Logger
{
    public static class EventId
    {
        public const int GameService = 0;
        public const int ServerController = 1;
        public const int GameServer = 2;
        public const int ThreadCount = 99;
        public const int Handle = 15;

        private static int _id = 1000;
        public static int New()
        {
            return _id++;
        }
    }
}