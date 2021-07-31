using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace NArgs.Tests
{
	public class BasicTests
	{
		public class TestArgs : IArgumentCollection
		{
			public IList<string> Values { get; set; }

			[CommandDefinition("s", "stringArg")]
			public string StringArg { get; set; }

			[CommandDefinition("l", "stringListArg")]
			public IList<string> StringListArg { get; set; }

			[CommandDefinition("b", "boolArg")]
			public bool BoolArg { get; set; }
		}

		public class RequiredTestArgs : IArgumentCollection
		{
			public IList<string> Values { get; set; }

			[CommandDefinition("s", "stringArg", Required = true)]
			public string StringArg { get; set; }
		}
		
		[Test]
		public void BlankArgTest()
		{
			var testArgs = Arguments.Parse<TestArgs>(new string[0]);

			Assert.IsNull(testArgs.StringArg);

			Assert.IsNotNull(testArgs.StringListArg);
			Assert.IsEmpty(testArgs.StringListArg);

			Assert.IsFalse(testArgs.BoolArg);

			Assert.IsNotNull(testArgs.Values);
			Assert.IsEmpty(testArgs.Values);
		}
		
		[TestCase("test", "--stringArg", "test")]
		[TestCase("test", "-s", "test")]
		public void StringArgTest(string expectedValue, params string[] args)
		{
			var testArgs = Arguments.Parse<TestArgs>(args);

			Assert.AreEqual(expectedValue, testArgs.StringArg);
		}
		
		[TestCase(true, "--boolArg")]
		[TestCase(true, "-b")]
		public void BoolArgTest(bool expectedValue, params string[] args)
		{
			var testArgs = Arguments.Parse<TestArgs>(args);

			Assert.AreEqual(expectedValue, testArgs.BoolArg);
		}
		
		[TestCase("test;test2", "--stringListArg", "test", "--stringListArg", "test2")]
		[TestCase("test;test2", "-l", "test", "-l", "test2")]
		[TestCase("test;test2", "-l", "test", "--stringListArg", "test2")]
		public void StringListArgTest(string expectedValues, params string[] args)
		{
			var testArgs = Arguments.Parse<TestArgs>(args);

			CollectionAssert.AreEqual(expectedValues.Split(";"), testArgs.StringListArg);
		}
		
		[TestCase("test", "test")]
		[TestCase("test;test2", "test", "test2")]
		[TestCase("test;test2", "-l", "test", "test", "test2")]
		public void ValueTest(string expectedValues, params string[] args)
		{
			var testArgs = Arguments.Parse<TestArgs>(args);

			CollectionAssert.AreEqual(expectedValues.Split(";"), testArgs.Values);
		}

		[Test]
		public void RequiredTest()
		{
			Assert.Throws<ArgumentException>(() =>
			{
				Arguments.Parse<RequiredTestArgs>(new string[0]);
			});

			Assert.DoesNotThrow(() => {
				Arguments.Parse<RequiredTestArgs>(new [] { "-s", "value" });
			});
		}
	}
}