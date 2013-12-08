// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TLShemaCompilerFacts.cs">
//   Copyright (c) 2013 Alexander Logger. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using SharpTL.TLSchema;

namespace SharpTL.Tests
{
    [TestFixture]
    public class TLShemaCompilerFacts
    {
        [Test]
        public void Should_compile_TL_schema()
        {
            var compiler = new TLSchemaCompiler();
            var sharpTLSchema = compiler.Compile(GetTestTLSchema());
            sharpTLSchema.Should().NotBeNullOrEmpty();
        }

        private static string GetTestTLSchema()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TL-schemas", "Test.tl"));
        }
    }
}