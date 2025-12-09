namespace ICSSoft.STORMNET.Business.Audit.Tests
{
    using NewPlatform.Flexberry.ORM.CurrentUserService;

    public class TestUser : ICurrentUser
    {
        /// <inheritdoc/>
        public string Login { get => "Vasiliev"; set => throw new System.NotImplementedException(); }

        /// <inheritdoc/>
        public string Domain { get => "Home"; set => throw new System.NotImplementedException(); }

        /// <inheritdoc/>
        public string FriendlyName { get => "Vasilii Vasiliev"; set => throw new System.NotImplementedException(); }
    }
}
