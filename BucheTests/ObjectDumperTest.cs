using System;
using System.Collections.Generic;
using Buche;
using Xunit;

namespace BucheTests
{
    public class ObjectDumperTest
    {

        public class Dump
        {
            public string Password { get; set; }
            public string Passwd { get; set; }
            public string Whatever { get; set; }
            public string securityanswer { get; set; }
            public string SecurityAnswer { get; set; }
            public string Secret { get; set; }
            public string secret { get; set; }
            public string Nothing { get; set; }
            public List<string> Strings { get; set; }
            public List<DateTime> DateTimes { get; set; } 
        }

		private class DumpTestObject
		{
			public string fieldString;
			public int fieldInt;
			public bool fieldBool;
			
			public List<long> fieldListLong;
			public List<string> fieldListString;
			public List<DumpTestObject> fieldListDumpTestObject;

			public Dictionary<string, int> fieldDictionary;

			public string PropertyString { get; set; }
			public int PropertyInt { get; set; }
			public bool PropertyBool { get; set; }

			public List<long> PropertyListLong { get; set; }
			public List<string> PropertyListString { get; set; }
			public Dictionary<string, int> PropertyDictionary { get; set; }
		}

		[Fact]
		public void TestObject()
		{
			List<long> listLong = new List<long>();
			listLong.Add(1);
			listLong.Add(2);

			List<string> listString = new List<string>();
			listString.Add("a");
			listString.Add(null);
			listString.Add("v");

			List<DumpTestObject> listDumpTestObject = new List<DumpTestObject>();
			listDumpTestObject.Add(new DumpTestObject
			                       	{
			                       		fieldString = "List dump 1",
			                       		fieldInt = 987564,
			                       		fieldBool = true,
			                       		fieldListLong = listLong,
			                       		fieldListString = null,
			                       		fieldDictionary = new Dictionary<string, int>()
			                       	});
			listDumpTestObject.Add(null);
			listDumpTestObject.Add(new DumpTestObject
			                       	{
			                       		fieldString = "list dump 3",
			                       		fieldListString = new List<string>(),
			                       		PropertyInt = 23432
			                       	});

			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary["1"] = 10;
			dictionary["2"] = 20;

			DumpTestObject obj = new DumpTestObject
			                     	{
			                     		fieldString = "Naksdfoiweur",
			                     		fieldInt = 123123213,
			                     		fieldBool = false,
										fieldListLong = listLong,
										fieldListString = listString,
										fieldDictionary = dictionary,
										fieldListDumpTestObject = listDumpTestObject
			                     	};

			string dump = ObjectDumper.Log(obj);
            Console.WriteLine(dump);
			Assert.Contains(obj.fieldString, dump);
		}
    }
}
