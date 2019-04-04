using AutoFixture;
using AutoFixture.AutoMoq;
using VirtoCommerce.Platform.Testing.Bases;

namespace VirtoCommerce.OrderModule.Test.Base
{
    public abstract class AutofixtureTestBase : TestBase
    {
        private IFixture _fixture;

        protected IFixture AutoFixture => this._fixture ?? (this._fixture = new Fixture().Customize(new AutoMoqCustomization()));
    }
}
