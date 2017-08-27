namespace Proxynet.Service.Removers
{
    public interface IDataRemover
    {
        void RemoveGroup(int groupId);
        void RemoveSite(int siteId);
    }
}