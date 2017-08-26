using Ninject;

namespace UsersLib.Dependency.Ninject
{
    public interface INinjectKernelFactory
    {
        IKernel GetNinjectKernel();
    }
}