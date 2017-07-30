using System.Collections.Generic;
using UsersLib.DbControllers;
using UsersLib.Entity;
using UsersLib.Secure.Finders.Results;

namespace UsersLib.Secure.Passwork
{
    public class PassworkController : IPassworkController
    {
        private const string MemoryCacheKey = "TreeReplyModel";
        private const int DefaultFtpPort = 21;

        public int Update()
        {
            List<Site> remoteSites = GetSitesFromStorage();
            IDbSiteController siteController = new DbSiteController();
            return siteController.UpdateSites( remoteSites );
        }

        public SiteSecureDataFinderResult GetSiteData( string storageId )
        {
            return null;

            //TreeReplyModel treeReply = GetTreeFromCache() ?? GetTreeFromStorage();

            //GroupReplyModel storageGroup =
            //    treeReply?.groups.FirstOrDefault( item => item.name == Config.PassworkGroupName );

            //PasswordReplyModel passwordReply = storageGroup?.passwords.FirstOrDefault( item => item.id == storageId );
            //if ( passwordReply == null )
            //{
            //    return null;
            //}
            
            //return new SiteSecureDataFinderResult
            //{
            //    UrlAddress = GetUrl( passwordReply.url ),
            //    Login = passwordReply.login,
            //    Password = passwordReply.cryptedPassword,
            //    Port = GetPort( passwordReply.url )
            //};
        }

        private List<Site> GetSitesFromStorage()
        {
            return null;
            //List<Site> sites = new List<Site>();

            //TreeReplyModel treeReply = GetTreeFromCache() ?? GetTreeFromStorage();

            //GroupReplyModel storageGroup =
            //    treeReply.groups.FirstOrDefault( item => item.name == Config.PassworkGroupName );
            //if ( storageGroup == null )
            //{
            //    return sites;
            //}

            //foreach ( PasswordReplyModel password in storageGroup.passwords )
            //{
            //    sites.Add( new Site
            //    {
            //        StorageId = password.id,
            //        Description = password.description,
            //        SiteKey = password.name
            //    } );
            //}

            //return sites;
        }

        //private TreeReplyModel GetTreeFromCache()
        //{
        //    return MemoryCache.Default[ MemoryCacheKey ] as TreeReplyModel;
        //}

        //private TreeReplyModel GetTreeFromStorage()
        //{
        //    CoreOptionModel coreOptionModel = new CoreOptionModel
        //    {
        //        Url = Config.PassworkApiUrl
        //    };
        //    Api passworkApi = new Api( coreOptionModel );
        //    passworkApi.SetMasterPassword( Config.PassworkMasterWord );
        //    passworkApi.Login( Config.PassworkLogin, Config.PassworkPassword );

        //    TreeReplyModel treeReply = passworkApi.GetTree();

        //    MemoryCache.Default.Set( MemoryCacheKey, treeReply, DateTime.Now.AddMinutes( 1 ) );

        //    return treeReply;
        //}

        //private int GetPort( string url )
        //{
        //    if ( String.IsNullOrEmpty( url ) )
        //    {
        //        return DefaultFtpPort;
        //    }

        //    string[] urlParts = url.Split( ':' );
        //    if ( urlParts.Length != 2 )
        //    {
        //        return DefaultFtpPort;
        //    }
        //    int parsedPort;
        //    if ( Int32.TryParse( urlParts.Last(), out parsedPort ) )
        //    {
        //        return parsedPort;
        //    }
        //    return DefaultFtpPort;
        //}

        //private string GetUrl( string storageUrl )
        //{
        //    if ( String.IsNullOrEmpty( storageUrl ) )
        //    {
        //        return String.Empty;
        //    }

        //    string[] urlParts = storageUrl.Split( ':' );
        //    if ( urlParts.Length != 2 )
        //    {
        //        return String.Empty;
        //    }

        //    return urlParts.First();
        //}
    }
}