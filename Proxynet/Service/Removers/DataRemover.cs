using UsersLib.DbControllers;

namespace Proxynet.Service.Removers
{
    public class DataRemover : IDataRemover
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly IDbGroupController _dbGroupController;
        private readonly IDbUserController _dbUserController;

        public DataRemover(IDbSiteController dbSiteController, 
            IDbGroupController dbGroupController, 
            IDbUserController dbUserController)
        {
            _dbSiteController = dbSiteController;
            _dbGroupController = dbGroupController;
            _dbUserController = dbUserController;
        }

        public void RemoveSite(int siteId)
        {
            _dbSiteController.DeleteSite(siteId);
        }

        public void RemoveUser(int userId)
        {
            _dbUserController.DeleteUser(userId);
        }

        public void RemoveGroup(int groupId)
        {
            _dbGroupController.DeleteGroup(groupId);
        }
    }
}