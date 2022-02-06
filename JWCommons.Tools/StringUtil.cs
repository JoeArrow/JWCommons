#region Copyright © 2017 JWCommons
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Text.RegularExpressions;

// --------------------------------------------------------
/// <summary>
///     StringUtil is a helper class that provides 
///     specialized string handling functionality.
/// </summary>

namespace JWCommons.Tools
{
    public static class StringUtil
    {
        // ------------------------------------------------
        /// <summary>
        ///     This static method is used to parse a string 
        ///     in a very specific and useful way.
        /// </summary>
        /// <param name="fullString">String to be parsed</param>
        /// <param name="parseVal">string to look for</param>
        /// <param name="delimiter">the delimiter within the string</param>
        /// <param name="ordinal">which instance of the search string to return<br/>
        ///      1 returns the first instance.
        ///      2 returns the second instance.
        ///     -1 returns the last instance.
        /// </param>
        /// <returns>The found value or an empty string if the value was not found.</returns>
        /// <example>
        /// If a string needs to be parsed to retrieve:<br/><br/>
        /// <list type="bullet">
        /// <item>The first instance of a given value in the string</item>
        /// <item>One of multiple instances of a given value in the string</item>
        /// <item>The last instance of a given value in the string</item>
        /// </list><br/><br/>
        /// The string to be parsed is str = "TestValue=test;application=appName;TestValue=anotherValue;stage=1;TestValue=yetAnotherValue"<br/><br/>
        /// 
        /// The <b>first</b> TestValue may be found using:<br/>
        /// val = StringUtil.ParseValue(str, "TestValue=", ';', 1);<br/><br/>
        /// 
        /// The <b>second</b> TestValue may be found using:<br/>
        /// val = StringUtil.ParseValue(str, "TestValue=", ';', 2);<br/><br/>
        /// 
        /// The <b>last</b> TestValue may be found using:<br/>
        /// val = StringUtil.ParseValue(str, "TestValue=", ';', -1);
        /// </example>

        public static string ParseValue(string fullString, string parseVal, char delimiter, int ordinal)
        {
            if(fullString == string.Empty || fullString == null)
            {
                return string.Empty;
            }

            var strRetVal = string.Empty;
            var iLength = parseVal.Length;

            if(ordinal == -1)
            {
                // -----------------------------------------------
                // Find the starting position of the last instance
                // of the parse value.

                int iStartPos = fullString.LastIndexOf(parseVal, fullString.Length) + iLength;

                // -------------------------------------------
                // Find the index of the end of our substring.          

                if((iStartPos - iLength) > -1)
                {
                    // -------------------------------------------
                    // Find the index of the end of our substring.

                    int iEndPos = fullString.IndexOf(delimiter, iStartPos);

                    // -------------------------------------------------------------
                    // If the parse value is the last datum in the header, there may 
                    // be no separator to read to. This results in a return of -1.
                    // If this happens, the length of the string is the end position.

                    if(iEndPos < 0)
                    {
                        iEndPos = fullString.Length;
                    }

                    strRetVal = fullString.Substring(iStartPos, iEndPos - iStartPos);
                }
            }
            else
            {
                // ------------------------------------------------
                // Find the starting position of the first instance
                // of the parse value.

                int iStartPos = fullString.IndexOf(parseVal, 0) + iLength;

                // -------------------------------------------
                // By subtracting 1 from the ordinal, we allow
                // the client code to more naturally identify
                // which instance of the parse value within the
                // full string is being sought.
                // (1 means the first, 2 means the second, etc.)

                for(int i = 0; i < ordinal - 1; i++)
                {
                    iStartPos = fullString.IndexOf(parseVal, iStartPos) + iLength;
                }

                if((iStartPos - iLength) > -1)
                {
                    // -------------------------------------------
                    // Find the index of the end of our substring.

                    int iEndPos = fullString.IndexOf(delimiter, iStartPos);

                    // -------------------------------------------------------------
                    // If the parse value is the last datum in the header, there may 
                    // be no separator to read to. This results in a return of -1.
                    // If this happens, the length of the string is the end position.

                    if(iEndPos < 0)
                    {
                        iEndPos = fullString.Length;
                    }

                    strRetVal = fullString.Substring(iStartPos, iEndPos - iStartPos);
                }
            }

            return strRetVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     Used to determine if the given string contains 
        ///     "DC=" AND "OU=". If so, then the test value is
        ///     said to be a Distinguished Name.
        /// </summary>
        /// <param name="testVal"></param>
        /// <returns></returns>

        public static bool IsDistinguishedName(string testVal)
        {
            var retVal = false;
            var r = new Regex(@"^(([OoCc][UuNn][=]['\w\d\s\-\@.&amp;]+,)+([Oo][Uu][=]['\w\d\s\-\&amp;]+,)*([Dd][Cc][=]['\w\d\s\-\&amp;]+[,]*){2,})$");

            try
            {
                var m = r.Match(testVal);
                retVal = m.Success;
            }
            catch(Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     Used to determine if the given string has a
        ///     format that is consistent with a GUID.
        /// </summary>
        /// <param name="testVal"></param>
        /// <returns></returns>

        public static bool IsGUID(string testVal)
        {
            bool retVal = false;

            Regex r = new Regex(@"^[a-fA-F0-9]{8}(-[a-fA-F0-9]{4}){3}-[a-fA-F0-9]{12}$");
            Match m = null;

            try
            {
                m = r.Match(testVal);
                retVal = m.Success;
            }
            catch(Exception)
            {
                retVal = false;
            }

            return retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     Used to exclude any SSN that is a series of 
        ///     any single number.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>

        public static bool IsExcludedSSN(string val)
        {
            bool retVal = false;

            // ----------------------------------------------------------------------------------------------
            // This Regex will match any three same digits with any two same digits with any four same digits
            // This picks up any combination including all of the same digit
            // I wish that I had a shorthand for this match...

            var sameDigitRegex = new Regex(@"([0]{3}|[1]{3}|[2]{3}|[3]{3}|[4]{3}|[5]{3}|[6]{3}|[7]{3}|[8]{3}|[9]{3})" +
                                           @"([0]{2}|[1]{2}|[2]{2}|[3]{2}|[4]{2}|[5]{2}|[6]{2}|[7]{2}|[8]{2}|[9]{2})" +
                                           @"([0]{4}|[1]{4}|[2]{4}|[3]{4}|[4]{4}|[5]{4}|[6]{4}|[7]{4}|[8]{4}|[9]{4})");

            var startWithSixes = new Regex(@"^([6]{3})([\d]{2})([\d]{4})");     // 666-XX-XXXX MOTB!

            var starstWithZero = new Regex(@"^([0]{3})([\d]{2})([\d]{4})");     // 000-XX-XXXX
            var middleZero = new Regex(@"^([\d]{3})([0]{2})([\d]{4})");         // XXX-00-XXXX
            var endsWithZero = new Regex(@"^([\d]{3})([\d]{2})([0]{4})");       // XXX-XX-0000

            var alternatingDigits = new Regex(@"^(\d{2})\1{3,}");               // Alternating digits ex. XYX-YX-YXYX

            if(string.IsNullOrEmpty(val) ||                                     // Value Null or Empty
               val.Length > 9 ||                                                // Value too long
               val.Length < 6 ||                                                // Value too short               
               middleZero.Match(val).Success ||
               endsWithZero.Match(val).Success ||
               starstWithZero.Match(val).Success ||
               sameDigitRegex.Match(val).Success ||
               startWithSixes.Match(val).Success ||
               alternatingDigits.Match(val).Success)
            {
                retVal = true;
            }
            else
            {
                switch(val)
                {
                    case "100000000":
                    case "123456789":
                    case "987654321":
                    case "123121234":
                    case "234232345":
                    case "345343456":
                    case "456454567":
                    case "567565678":
                    case "678676789":
                    case "789787890":
                    case "890898901":
                    case "901909012":
                        retVal = true;
                        break;
                }
            }

            return retVal;
        }
    }
}
