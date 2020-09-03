namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using ICSSoft.Services;

    public class TestUser : CurrentUserService.IUser
    {
        /// <inheritdoc/>
        public string Login { get => "Vasiliev"; set => throw new System.NotImplementedException(); }

        /// <inheritdoc/>
        public string Domain { get => "Home"; set => throw new System.NotImplementedException(); }

        /// <inheritdoc/>
        public string FriendlyName { get => "Vasilii Vasiliev"; set => throw new System.NotImplementedException(); }
    }
}
