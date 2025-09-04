using System;
using System.Collections.Generic;
using NUnit.Framework;
using Runtime.Models;
using Runtime.Services;

namespace Tests
{
    public class TestsClass
    {
        [Test]
        public void TestGen()
        {
            var releaseGen = new ReleaseGenerator();
            var generate = releaseGen.Generate(new DateTime(2025, 08, 20), new DateTime(2025, 09, 24), 46, 4);

            Assert.That(generate, Is.Not.Empty);
        }
    }
}