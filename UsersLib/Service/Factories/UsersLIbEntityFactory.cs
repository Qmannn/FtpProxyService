namespace UsersLib.Service.Factories
{
    public static class UsersLIbEntityFactory
    {
        public static IUsersLIbEntityFactory Instance = new UsersLIbEntityFactoryImpl();
    }
}