﻿using EnumerableExtensions;
using EnumerableExtensionsTests.Commons;
using NUnit.Framework;
using System.Linq;

namespace EnumerableExtensionsTests
{
    internal class Chain
        : Base
    {
        #region Public Methods

        [Test]
        public void Consecutive()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .Consecutive((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Count() == 3);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void Paired()
        {
            disposalCount = 0;

            var result = GetWithDisposal("a", "b", "c")
                .Paired((x, y) => new { x, y }).ToArray();

            Assert.IsTrue(result.Count() == 2);

            Assert.True(disposalCount == 1);
        }

        [Test]
        public void WithoutLast()
        {
            var result = GetWithDisposal("a", "b", "c").WithoutLast(2);

            Assert.IsTrue(result.Count() == 1);
        }

        [Test]
        public void WithoutLastTooMuch()
        {
            var result = GetWithDisposal("a", "b", "c").WithoutLast(4);

            Assert.IsFalse(result.Any());
        }

        #endregion Public Methods
    }
}