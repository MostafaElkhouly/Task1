

namespace Infrastructure.ViewModel.Base
{
    public abstract class ProfileBase : AutoMapper.Profile
    {
        public ProfileBase()
        {
            Response();
            Request();
        }
        public abstract void Response();
        public abstract void Request();
    }
}
