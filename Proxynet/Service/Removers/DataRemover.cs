using UsersLib.DbControllers;

namespace Proxynet.Service.Removers
{
    public class DataRemover : IDataRemover
    {
        private readonly IDbSiteController _dbSiteController;
        private readonly IDbGroupController _dbGroupController;

        public DataRemover(IDbSiteController dbSiteController, IDbGroupController dbGroupController)
        {
            _dbSiteController = dbSiteController;
            _dbGroupController = dbGroupController;
        }

        public void RemoveSite(int siteId)
        {
            _dbSiteController.DeleteSite(siteId);
        }

        public void RemoveGroup(int groupId)
        {
            _dbGroupController.DeleteGroup(groupId);
        }
    }
}