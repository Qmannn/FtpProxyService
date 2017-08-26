using Ninject;
using UsersLib.Dependency.Ninject.Modules;

namespace UsersLib.Dependency.Ninject
{
    internal class NinjectKernelFactory : INinjectKernelFactory
    {
        public IKernel GetNinjectKernel()
        {
            return new StandardKernel(new ServiceNinjectModule());
        }
    }
}