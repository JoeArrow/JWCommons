#region Copyright © 2017 JWCommons
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using JWCommons.Tools;

namespace JWCommonsTools_UT
{
    // ----------------------------------------------------
    /// <summary>
    ///     Summary description for JWCommonsTools_UT
    /// </summary>

    [TestClass]
    public class StringUtil_UT
    {
        public StringUtil_UT() { }

        private TestContext testContextInstance;

        // ------------------------------------------------
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        ///</summary>

        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        // ------------------------------------------------

        [DeploymentItem("Test Data\\StringUtil_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\StringUtil_TD.xml",
                    "ParseValue",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void ParseValue_JWCommonsTools_Extracts_A_String_From_Within_A_Larger_String()
        {
            // -------
            // Arrange

            var ordinal = Convert.ToInt32(TestContext.DataRow["Ordinal"]);
            var delimiter = Convert.ToChar(TestContext.DataRow["Delimiter"]);
            var expected = Convert.ToString(TestContext.DataRow["Expected"]);
            var fullString = Convert.ToString(TestContext.DataRow["FullString"]);
            var parseValue = Convert.ToString(TestContext.DataRow["ValueToParse"]);

            // ---
            // Log

            Console.WriteLine("Full String: {1}{0}", Environment.NewLine, fullString);

            // ---
            // Act

            var res = StringUtil.ParseValue(fullString, parseValue, delimiter, ordinal);

            // ---
            // Log

            Console.WriteLine("Search String:\t{1}{0}Ordinal:\t\t{2}{0}Expected:\t{3}{0}Result:\t\t{4}",
                              Environment.NewLine, parseValue, ordinal, expected, res);
            // ------
            // Assert

            Assert.AreEqual(expected, res);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\StringUtil_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\StringUtil_TD.xml",
                    "IsDistinguishedName",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void IsDistinguishedName_JWCommonsTools_Determines_If_A_Given_String_Matches_The_Format_Of_A_DistinguishedName_In_LDAP()
        {
            // -------
            // Arrange
            
            var expected = Convert.ToBoolean(TestContext.DataRow["Expected"]);
            var testValue = Convert.ToString(TestContext.DataRow["TestValue"]);

            if(string.IsNullOrEmpty(testValue))
            {
                testValue = null;
            }

            // ---
            // Log

            Console.WriteLine("Test String: {1}{0}", Environment.NewLine, testValue);

            // ---
            // Act

            var res = StringUtil.IsDistinguishedName(testValue);

            // ---
            // Log

            Console.WriteLine("Expected:\t{1}{0}Result:\t\t{2}", Environment.NewLine, expected, res);

            // ------
            // Assert

            Assert.AreEqual(expected, res);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\StringUtil_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\StringUtil_TD.xml",
                    "IsGUID",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void IsGUID_JWCommonsTools_Determines_If_A_Given_String_Matches_The_Format_Of_A_GUID()
        {
            // -------
            // Arrange

            var expected = Convert.ToBoolean(TestContext.DataRow["Expected"]);
            var testValue = Convert.ToString(TestContext.DataRow["TestValue"]);

            if(string.IsNullOrEmpty(testValue))
            {
                testValue = null;
            }

            // ---
            // Log

            Console.WriteLine("Test String: {1}{0}", Environment.NewLine, testValue);

            // ---
            // Act

            var res = StringUtil.IsGUID(testValue);

            // ---
            // Log

            Console.WriteLine("Expected:\t{1}{0}Result:\t\t{2}", Environment.NewLine, expected, res);

            // ------
            // Assert

            Assert.AreEqual(expected, res);
        }

        // ------------------------------------------------

        [DeploymentItem("Test Data\\StringUtil_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\StringUtil_TD.xml",
                    "IsExcludedSSN",
                    DataAccessMethod.Sequential),
         TestMethod]
        public void IsExcludedSSN_JWCommonsTools_Determines_If_A_Given_SSN_Should_Be_Exlcuded_From_Use()
        {
            // -------
            // Arrange

            var testValue = Convert.ToString(TestContext.DataRow["SSN"]);
            var expected = Convert.ToBoolean(TestContext.DataRow["Expected"]);

            // ---
            // Log

            Console.WriteLine("Test String: {1}{0}", Environment.NewLine, testValue);

            // ---
            // Act

            var res = StringUtil.IsExcludedSSN(testValue);

            // ---
            // Log

            Console.WriteLine("Expected:\t{1}{0}Result:\t\t{2}", Environment.NewLine, expected, res);

            // ------
            // Assert

            Assert.AreEqual(expected, res);
        }
    }
}

/*

        // ------------------------------------------------

        [DeploymentItem("Test Data\\StringUtil_TD.xml"),
         DataSource("Microsoft.VisualStudio.TestTools.DataSource.XML",
                    "|DataDirectory|\\StringUtil_TD.xml",
                    "IsDistinguishedName",
                    DataAccessMethod.Sequential),
         TestCategory("Nonpernicious"), TestMethod]
        public void IsDistinguishedName_JWCommonsTools_Determines_If_A_Given_String_Matches_The_Format_Of_A_DistinguishedName_In_LDAP()
        {
            // -------
            // Arrange
            

            // ---
            // Log
            
            Console.WriteLine("{0}", Environment.NewLine);

            // ---
            // Act
            

            // ---
            // Log

            Console.WriteLine("{0}", Environment.NewLine);

            // ------
            // Assert
            
        }
 */
