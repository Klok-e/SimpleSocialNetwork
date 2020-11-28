using NUnit.Framework;

namespace Tests.Business
{
    [TestFixture]
    public class OpMessageServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            _db = new ServicesHelper();
        }

        [TearDown]
        public void TearDown()
        {
            _db.Dispose();
        }

        private ServicesHelper _db = null!;
    }
}