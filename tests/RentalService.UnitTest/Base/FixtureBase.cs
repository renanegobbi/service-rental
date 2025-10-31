using Moq;
using System.Linq.Expressions;

namespace RentalService.UnitTest.Base
{
    /// <summary>
    /// Base fixture for tests, ensures full isolation between executions.
    /// </summary>
    public abstract class FixtureBase : IDisposable
    {
        private readonly MockRepository _repository;

        protected FixtureBase()
        {
            // Initializes with Strict to detect unexpected behaviors
            _repository = new MockRepository(MockBehavior.Strict);
            Reset();
        }

        /// <summary>
        /// Creates a new isolated mock.
        /// </summary>
        protected Mock<T> CreateMock<T>() where T : class
            => _repository.Create<T>();

        /// <summary>
        /// Clears invocations, setups, and recreates fixture-specific mocks.
        /// </summary>
        public virtual void Reset()
        {
        }

        /// <summary>
        /// Releases resources and enforces verification of Strict mocks.
        /// </summary>
        public void Dispose()
        {
            _repository.VerifyAll();
        }

        /// <summary>
        /// Allows setups of void methods without warnings.
        /// </summary>
        protected void AllowVoid<T>(Mock<T> mock, Expression<Action<T>> expr) where T : class
        {
            mock.Setup(expr);
        }
    }
}

