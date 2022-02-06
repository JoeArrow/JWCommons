using System;
using System.Text;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using JWCommons.Tools;
using JWCommonsTools_UT.DTO;
using JWCommons.Tools.Logging;

namespace JWCommonsTools_UT
{
    // ----------------------------------------------------
    /// <summary>
    ///     Summary description for ConfigMan
    /// </summary>

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class ConfigMan_UT
    {
        public ConfigMan_UT() { }

        // ------------------------------------------------
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the 
        ///     current test run.
        ///</summary>

        public TestContext TestContext { get; set; }

        #region Additional test attributes
        // ------------------------------------------------
        //  You can use the following additional attributes 
        //  as you write your tests:

        // ------------------------------------------------
        //  Use ClassInitialize to run code before running 
        //  the first test in the class

        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }

        // ------------------------------------------------
        //  Use ClassCleanup to run code after all tests in 
        //  a class have run

        // [ClassCleanup()]
        // public static void MyClassCleanup() { }

        // ------------------------------------------------
        //  Use TestInitialize to run code before running 
        //  each test 

        // [TestInitialize()]
        // public void MyTestInitialize() { }

        // ------------------------------------------------
        //  Use TestCleanup to run code after each test has 
        //  run

        // [TestCleanup()]
        // public void MyTestCleanup() { }

        #endregion

        // ------------------------------------------------

        [TestMethod]
        public void Constructor_Null_Argument_Throws()
        {
            Console.Write("An Exception is expected because we are providing a NULL ConfigFile name.");

            // ---
            // Act

            Assert.ThrowsException<JWEvent>(() => new ConfigMan(null));
        }

        // ------------------------------------------------
        
        [TestMethod, 
         DeploymentItem("Test Data\\Test.config"),
         DataRow("Test.config", "TestEntry", "Custom Test Value")]
        public void ReadEntry_Custom_JWCommonsTools_ConfigMan_Reads_An_Entry_From_A_Custom_Config_File(string cfgFile, 
                                                                                                     string entryName,
                                                                                                     string expected)
        {
            // -------
            // Arrange

            var cfg = new ConfigMan(cfgFile);

            // ---
            // Log

            Console.WriteLine("Config File:\t{1}{0}Entry Name:\t{2}", Environment.NewLine, cfgFile, entryName);

            // ---
            // Act

            var element = cfg.AppSettings[entryName];

            // ---
            // Log

            Console.WriteLine("{0}Expected:\t'{1}'{0}Actual:\t\t'{2}'", Environment.NewLine, expected, element);

            // ------
            // Assert

            Assert.AreEqual(expected, element);
        }

        // ------------------------------------------------

        [TestMethod, DataRow("EntryDTO")]
        public void GetJSONEntry_Native_JWCommonsTools_ConfigMan_Deserializes_An_Entry_From_The_Native_Config_File(string entryName)
        {
            // -------
            // Arrange

            var cfg = new ConfigMan();

            // ---
            // Act

            var entry = cfg.GetJSONItem<EntryDTO>(entryName);

            // ---
            // Log

            Console.WriteLine(entry.ToString());

            // ------
            // Assert

            Assert.IsNotNull(entry);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadEntryNative",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadEntry_Native_JWCommonsTools_ConfigMan_Reads_An_Entry_From_The_Native_Config_File()
        {
            // -------
            // Arrange

            var entryName = Convert.ToString(TestContext.DataRow["EntryName"]);
            var expected = Convert.ToString(TestContext.DataRow["ExpectedValue"]);

            var cfg = new ConfigMan();

            // ---
            // Log

            Console.WriteLine("Config File:\t<Native>{0}Entry Name:\t{1}", Environment.NewLine, entryName);

            // ---
            // Act

            var element = cfg.AppSettings[entryName];

            // ---
            // Log

            Console.WriteLine("{0}Expected:\t'{1}'{0}Actual:\t\t'{2}'", Environment.NewLine, expected, element);

            // ------
            // Assert

            Assert.AreEqual(expected, element);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DeploymentItem("Test Data\\Test.config"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadList",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadList_JWCommonsTools_ConfigMan_Reads_A_List_Of_Entries_From_The_Config_File()
        {
            // -------
            // Arrange

            var index = Convert.ToInt32(TestContext.DataRow["Index"]);
            var listName = Convert.ToString(TestContext.DataRow["ListName"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var cfgFile = Convert.ToString(TestContext.DataRow["ConfigFile"]);

            var cfg = new ConfigMan(cfgFile);

            // ---
            // Log

            Console.WriteLine("Config File:\t{1}{0}List Name:\t{2}", Environment.NewLine, cfgFile, listName);

            // ---
            // Act

            var elements = cfg.ReadList(listName);

            // ------
            // Assert
            
            Assert.IsTrue(elements.Contains(expected));
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DeploymentItem("Test Data\\Test.config"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadFromKeyedListNative",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadFromKeyedList_JWCommonsTools_Native_ConfigMan_Reads_A_List_Of_Entries_To_Return_The_Item_Identified_By_a_Key()
        {
            // -------
            // Arrange

            var key = Convert.ToString(TestContext.DataRow["Key"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var listName = Convert.ToString(TestContext.DataRow["ListName"]);

            var cfg = new ConfigMan();

            // ---
            // Log

            Console.WriteLine("Config File:\tapp.config{0}List Name:\t{1}{0}", Environment.NewLine, listName);

            // ---
            // Act

            var element = cfg.ReadFromKeyedList(listName, key);

            // ---
            // Log

            Console.WriteLine("Key:\t\t'{1}'{0}Expected:\t'{2}'{0}Actual:\t\t'{3}'", Environment.NewLine, key, expected, element);

            // ------
            // Assert

            Assert.AreEqual(expected, element);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DeploymentItem("Test Data\\Test.config"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadFromKeyedListCustom",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadFromKeyedList_JWCommonsTools_Custom_ConfigMan_Reads_A_List_Of_Entries_To_Return_The_Item_Identified_By_a_Key()
        {
            // -------
            // Arrange

            var key = Convert.ToString(TestContext.DataRow["Key"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var listName = Convert.ToString(TestContext.DataRow["ListName"]);
            var cfgFile = Convert.ToString(TestContext.DataRow["ConfigFile"]);

            var cfg = new ConfigMan(cfgFile);

            // ---
            // Log

            Console.WriteLine("Config File:\t{1}{0}List Name:\t{2}{0}", Environment.NewLine, cfgFile, listName);

            // ---
            // Act

            var element = cfg.ReadFromKeyedList(listName, key);

            // ---
            // Log

            Console.WriteLine("Key:\t\t'{1}'{0}Expected:\t'{2}'{0}Actual:\t\t'{3}'", Environment.NewLine, key, expected, element);

            // ------
            // Assert

            Assert.AreEqual(expected, element);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DeploymentItem("Test Data\\Test.config"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadFromKeyedObjListCustom",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadFromKeyedObjList_JWCommonsTools_Custom_ConfigMan_Reads_A_List_Of_JSON_Entries_To_Return_The_Item_Identified_By_a_Key()
        {
            // -------
            // Arrange

            var key = Convert.ToString(TestContext.DataRow["Key"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var listName = Convert.ToString(TestContext.DataRow["ListName"]);
            var cfgFile = Convert.ToString(TestContext.DataRow["ConfigFile"]);

            var cfg = new ConfigMan(cfgFile);

            // ---
            // Log

            Console.WriteLine("Config File:\t{1}{0}List Name:\t{2}{0}", Environment.NewLine, cfgFile, listName);

            // ---
            // Act

            var element = cfg.ReadFromKeyedObjList<TestClass_DTO>(listName, key);

            // ---
            // Log

            if(element != null)
            {
                Console.WriteLine("Key:\t\t'{1}'{0}Expected:\t'{2}'{0}Actual:\t\t'{3}'", Environment.NewLine, key, expected, element.Value);

                // ------
                // Assert

                Assert.AreEqual(expected, element.Value);
            }
            else
            {
                Assert.AreEqual(expected, string.Empty);
            }
        }

        // ------------------------------------------------
        /// <summary>
        ///     For this test the term "Local" means the 
        ///     native app.config rather than a custom
        ///     config file.
        /// </summary>

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadListNative",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadListLocal_JWCommonsTools_ConfigMan_Reads_A_List_Of_Entries_From_The_App_Config_File()
        {
            // -------
            // Arrange

            var index = Convert.ToInt32(TestContext.DataRow["Index"]);
            var listName = Convert.ToString(TestContext.DataRow["ListName"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);

            var cfg = new ConfigMan();

            // ---
            // Log

            Console.WriteLine("List Name:\t'{1}'{0}", Environment.NewLine, listName);
            
            // ---
            // Act

            var elements = cfg.ReadList(listName);

            // ---
            // Log

            Console.WriteLine("Elements Read:{0}", Environment.NewLine);

            foreach(var element in elements)
            {
                Console.WriteLine("'{0}'", element);
            }

            // ------
            // Assert

            Assert.AreEqual(expected, elements[index]);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DeploymentItem("Test Data\\Test.config"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadObjList",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadObjList_JWCommonsTools_ConfigMan_Reads_A_List_Of_JSON_Entries_From_The_App_Config_File()
        {
            // -------
            // Arrange

            var index = Convert.ToInt32(TestContext.DataRow["Index"]);
            var listName = Convert.ToString(TestContext.DataRow["ListName"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);

            var cfg = new ConfigMan();

            // ---
            // Log

            Console.WriteLine("List Name:\t'{1}'{0}", Environment.NewLine, listName);

            // ---
            // Act

            var elements = cfg.ReadObjList<TestClass_DTO>(listName);

            // ---
            // Log

            Console.WriteLine("Elements Read:{0}", Environment.NewLine);

            foreach(var element in elements)
            {
                Log(element);
            }

            // ------
            // Assert

            Assert.AreEqual(expected, elements[index].Value );
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\ConfigMan_TD.xml"),
         DeploymentItem("Test Data\\Test.config"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\ConfigMan_TD.xml",
                    "ReadCustomObjList",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ReadCustomObjList_JWCommonsTools_ConfigMan_Reads_A_List_Of_JSON_Entries_From_a_Custom_Config_File()
        {
            // -------
            // Arrange

            var index = Convert.ToInt32(TestContext.DataRow["Index"]);
            var listName = Convert.ToString(TestContext.DataRow["ListName"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var configFile = Convert.ToString(TestContext.DataRow["ConfigName"]);

            var cfg = new ConfigMan(configFile);

            // ---
            // Log

            Console.WriteLine("List Name:\t'{1}'{0}", Environment.NewLine, listName);

            // ---
            // Act

            var elements = cfg.ReadObjList<TestClass_DTO>(listName);

            // ---
            // Log

            Console.WriteLine("Elements Read:{0}", Environment.NewLine);

            foreach(var element in elements)
            {
                Log(element);
            }

            // ------
            // Assert

            Assert.AreEqual(expected, elements[index].Value);
        }

        // ------------------------------------------------

        private void Log(object element)
        {
            // ------------------------------------------------
            // Each property will be output in a common format.

            var outputFormat = "{0}:\t{1}\t";

            var sb = new StringBuilder();

            Type obj = element.GetType();
            var properties = obj.GetProperties();

            // -----------------------------------------------
            // Step through each property in the event object.

            foreach(PropertyInfo prop in properties)
            {
                object val = prop.GetValue(element, null);

                if(val != null && val.ToString() != string.Empty && val.ToString() != "0")
                {
                    // -------------------------------------------
                    // Only output the property if it has a value.

                    sb.AppendFormat(outputFormat, prop.Name, val.ToString());
                }
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
