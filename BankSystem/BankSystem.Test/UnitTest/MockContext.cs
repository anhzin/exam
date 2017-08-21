using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankSystem.Test.UnitTest
{
    public class MockContext
    {
        protected MockRepository MockRepository { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseUnitTest"/> class.
        /// </summary>
        protected MockContext()
        {
            MockRepository = new MockRepository(MockBehavior.Default);
        }



        /// <summary>
        /// Creates the mock set.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public Mock<DbSet<T>> CreateMockSet<T>(IEnumerable<T> data)
           where T : class
        {
            var queryableData = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider)
                    .Returns(() => queryableData.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression)
                    .Returns(() => queryableData.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType)
                    .Returns(() => queryableData.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                    .Returns(() => queryableData.GetEnumerator());
            return mockSet;
        }
    }
}
