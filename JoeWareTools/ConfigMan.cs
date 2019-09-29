#region Copyright © 2017 JoeWare
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Xml;
using System.Text;
using System.Xml.XPath;
using System.Collections;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;

using JoeWare.Tools.Logging;

// --------------------------------------------------------
/// <summary>
///     The ConfigMan is designed to provide access
///     to a config file in much the same way that web.config
///     and app.config are used. The difference being that
///     with ConfigMan, one may have access to multiple
///     config files in the same application.
///     
///     Also, Without a custom configuration file reader, a DLL
///     would be required to rely on the client application's config
///     file for storage. This is due to the fact that the DLL
///     is loaded into another application's namespace.
///     
///     With ConfigMan, a dll may retain access to its own config file.
/// </summary>
/// <remarks>
///     Next available error ID: 10006
/// </remarks>

namespace JoeWare.Tools
{
    public class ConfigMan
    {
        private XmlDocument xDoc;

        private const string EVENT_SOURCE = "JoeWareTools";

        // ------------------------------------------------
        /// <summary>
        ///     Access to the path/name for the file loaded
        ///     as a config file.
        /// </summary>

        public string FileName { set; get; }

        // ------------------------------------------------
        /// <summary>
        ///     Accessor
        /// </summary>

        public IDictionary AppSettings
        {
            get;
            private set;
        }

        // ------------------------------------------------
        /// <summary>
        ///     The Default Constructor will attempt to reference
        ///     the running assembly's own config file.
        /// </summary>

        public ConfigMan()
        {
            if(System.Web.HttpContext.Current == null)
            {
                Initialize(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);
            }
            else
            {
                try
                {
                    Initialize(WebConfigurationManager.OpenWebConfiguration("~").FilePath);
                }
                catch(Exception)
                {
                    // ------------------------------------------------------------------
                    // If we fail to get a Web Config, at least try to get an App Config.

                    Initialize(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath);
                }
            }
        }

        // ------------------------------------------------
        /// <summary>
        ///     Constructor using a parameter to identify the location
        ///     and name of the Config file.
        /// </summary>
        /// <param name="cfgFileName"></param>

        public ConfigMan(string cfgFileName)
        {
            Initialize(cfgFileName);
        }

        // ------------------------------------------------

        private void Initialize(string cfgFileName)
        {
            FileName = cfgFileName;

            if(string.IsNullOrEmpty(cfgFileName))
            {
                string msg = string.Format("Unable to load the configuration file: '{0}'", FileName);

                var evt = new JWareEvent(10003, EVENT_SOURCE, msg);

                evt.MethodName = "Initialize()";
                evt.SourceFile = "ConfigMan.cs";
                evt.Log();

                throw (evt);
            }
            else
            {
                xDoc = new XmlDocument();

                try
                {
                    XmlTextReader reader = new XmlTextReader(FileName);
                    xDoc.Load(reader);
                    reader.Close();
                    AppSettings = GetConfig("appSettings");
                }
                catch(Exception ex)
                {
                    string msg = string.Format("Unable to load the configuration file: '{0}'", FileName);

                    var evt = new JWareEvent(10002, EVENT_SOURCE, msg, ex)
                    {
                        MethodName = "Initialize()",
                        SourceFile = "ConfigMan.cs"
                    };
                    
                    evt.Log();
                    throw (evt);
                }
            }
        }

        // ------------------------------------------------
        /// <summary>
        ///     ReadList requires that a Section Definition 
        ///     be added to the Config File configSections
        ///     
        ///     <configSections>
        ///         <section name="ListSection" type="JoeWare.Tools.ConfigList.ListSection" />
        ///     </configSections>
        ///    
        ///     Then a new Section can be added to the config
        ///     File:
        ///     
        ///     <ItemList name="AddedProperties">
        ///         <Item>DN</Item>
        ///         <Item>UID</Item>
        ///         <Item>AuthSource</Item>
        ///         <Item>ProfileType</Item>
        ///         <Item>DisplayName</Item>
        ///         <Item>EntityLinks</Item>
        ///     </ItemList>
        ///     
        ///     Then the List of Items can be read with code like this:
        ///     
        ///     ReadList(|"ItemListName"|)
        ///     ReadList("AddedProperties") for the list above.
        /// </summary>
        /// <param name="listName"></param>
        /// <returns>
        ///     StringCollection containing the items from the Config File
        /// </returns>

        public IList ReadList(string listName)
        {
            var retVal = new StringCollection();
            var xPath = new XPathDocument(FileName);

            var nav = xPath.CreateNavigator();
            var manager = new XmlNamespaceManager(nav.NameTable);

            var expression = string.Format("//ItemList[@name=\"{0}\"]/Item", listName);
            var myIt = nav.Select(expression);

            while(myIt.MoveNext())
            {
                retVal.Add(myIt.Current.Value);
            }

            return retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     The intent is for the list of items in the 
        ///     config file to hace unique keys on each Item.
        ///     
        ///     ReadFromKeyedList is intenede to read the single 
        ///     unique Item from the list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="key"></param>
        /// <returns>
        ///     A string with either the value associated with 
        ///     the given Key, or an empty string
        /// </returns>

        public string ReadFromKeyedList(string listName, string key)
        {
            var retVal = string.Empty;
            var foundItems = new StringCollection();
            var xPath = new XPathDocument(FileName);

            var nav = xPath.CreateNavigator();
            var manager = new XmlNamespaceManager(nav.NameTable);

            var expression = string.Format("//ItemList[@name=\"{0}\"]/Item[@key=\"{1}\"]", listName, key);
            var myIt = nav.Select(expression);

            while(myIt.MoveNext())
            {
                foundItems.Add(myIt.Current.Value);
            }

            if(foundItems.Count == 1)
            {
                retVal = foundItems[0];
            }
            else
            {
                var msg = new StringBuilder(string.Format("{1} items found in Keyed List.{0}", Environment.NewLine, foundItems.Count));

                foreach(var item in foundItems)
                {
                    msg.AppendFormat("\t\tItem: '{1}'{0}", Environment.NewLine, item);
                }

                var parameters = new List<MethodParameter>();

                parameters.Add(new MethodParameter() { Name = "listName", Value = listName });
                parameters.Add(new MethodParameter() { Name = "key", Value = key });

                var evt = new JWareEvent(10005, EVENT_SOURCE, msg.ToString())
                {
                    Parameters = parameters,
                    MethodName = "ReadFromKeyedList()",
                };

                evt.Log();
            }

            return retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     The intent is for the list of items in the 
        ///     config file to hace unique keys on each Item.
        ///     
        ///     ReadFromKeyedList is intenede to read the single 
        ///     unique Item from the list.
        /// </summary>
        /// <param name="listName"></param>
        /// <param name="key"></param>
        /// <returns>
        ///     A string with either the value associated with 
        ///     the given Key, or an empty string
        /// </returns>

        public T ReadFromKeyedObjList<T>(string listName, string key) where T : class
        {
            var retVal = null as T;
            var foundItems = new List<T>();
            var xPath = new XPathDocument(FileName);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var nav = xPath.CreateNavigator();
            var manager = new XmlNamespaceManager(nav.NameTable);

            var expression = string.Format("//ItemList[@name=\"{0}\"]/Item[@key=\"{1}\"]", listName, key);
            var myIt = nav.Select(expression);

            while(myIt.MoveNext())
            {
                foundItems.Add(serializer.Deserialize<T>(myIt.Current.Value));
            }

            if(foundItems.Count == 1)
            {
                retVal = foundItems[0];
            }
            else
            {
                var msg = new StringBuilder(string.Format("{1} items found in Keyed List.{0}", Environment.NewLine, foundItems.Count));

                foreach(var item in foundItems)
                {
                    msg.AppendFormat("\t\tItem: '{1}'{0}", Environment.NewLine, item);
                }

                var parameters = new List<MethodParameter>();

                parameters.Add(new MethodParameter() { Name = "listName", Value = listName });
                parameters.Add(new MethodParameter() { Name = "key", Value = key });

                var evt = new JWareEvent(10006, EVENT_SOURCE, msg.ToString())
                {
                    Parameters = parameters,
                    MethodName = "ReadFromKeyedObjList()",
                };

                evt.Log();
            }

            return retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     ReadJSONList requires that a Section Definition 
        ///     be added to the Config File configSections
        ///     
        ///     <configSections>
        ///         <section name="ListSection" type="JoeWare.Tools.ConfigList.ListSection" />
        ///     </configSections>
        ///     
        ///     In the Client code, a class should be created to deserialize into:
        ///     
        ///     public class TestClass
        ///     {
        ///         public string Name { get; set; }
        ///         public string Value { get; set; }
        ///     }
        ///    
        ///     Then a new Section can be added to the Config File:
        ///     
        ///     <JSONList name="NameForThisList">
        ///         <Item>{ "Name":"object 0", "Value":"Object with a Value of 0" }</Item>
        ///         <Item>{ "Name":"object 1", "Value":"Object with a Value of 1" }</Item>
        ///         <Item>{ "Name":"object 2", "Value":"Object with a Value of 2" }</Item>
        ///         <Item>{ "Name":"object 3", "Value":"Object with a Value of 3" }</Item>
        ///     </JSONList>
        ///     
        ///     Then the List of Items can be read with code like this:
        ///<!--     
        ///     ReadJSONList<T>(|"ItemListName"|)
        ///     ReadJSONList<TestClass>("NameForThisList") for the list above.
        ///-->
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>

        public List<T> ReadObjList<T>(string listName)
        {
            var retVal = new List<T>();
            var xPath = new XPathDocument(FileName);
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            var nav = xPath.CreateNavigator();
            var manager = new XmlNamespaceManager(nav.NameTable);

            var expression = string.Format("//ItemList[@name=\"{0}\"]/Item", listName);
            var myIt = nav.Select(expression);

            while(myIt.MoveNext())
            {
                try
                {
                    retVal.Add(serializer.Deserialize<T>(myIt.Current.Value));
                }
                catch(Exception exp)
                {
                    var evt = new JWareEvent(10007, EVENT_SOURCE, exp.Message, exp)
                    {
                        SourceFile = "ConfigMan.cs",
                        MethodName = "ReadListJSON()",
                        TechMessage = string.Format("Failed to deserialize value: '{0}'", myIt.Current.Value)
                    };

                    evt.Parameters.Add(new MethodParameter() { Name = "listName", Value = listName });
                    evt.Parameters.Add(new MethodParameter() { Name = "type", Value = typeof(T).FullName });

                    evt.Log();
                    throw evt;
                }
            }

            return retVal;
        }

        // ------------------------------------------------

        public T GetJSONItem<T>(string itemName) where T : class
        {
            T retVal = (T) null;
            var objDef = AppSettings[itemName] as string;
            var serializer = new JavaScriptSerializer();

            try
            {
                retVal = serializer.Deserialize<T>(objDef);
            }
            catch(Exception exp)
            {
                var evt = new JWareEvent(10004, EVENT_SOURCE, exp.Message, exp)
                {
                    SourceFile = "ConfigMan.cs",
                    MethodName = "ReadListJSON()",
                    TechMessage = string.Format("Failed to deserialize object: '{0}'", objDef)
                };

                evt.Parameters.Add(new MethodParameter() { Name = "ItemName", Value = itemName });
                evt.Parameters.Add(new MethodParameter() { Name = "type", Value = typeof(T).FullName });

                evt.Log();
                throw evt;
            }

            return (T) retVal;
        }

        // ------------------------------------------------
        /// <summary>
        ///     Searches the config file for a node that 
        ///     matches the elementName, and returns it's 
        ///     children in a Dictionary object.
        /// </summary>
        /// <param name="elementName">Element to retrieve</param>

        private IDictionary GetConfig(string elementName)
        {
            IDictionary retVal = null;

            try
            {
                XmlNodeList nodes = xDoc.GetElementsByTagName(elementName);

                foreach(XmlNode node in nodes)
                {
                    // ------------------------------------------------------------
                    // CurrentCultureIgnoreCase makes this search case insensitive.

                    if(node.LocalName.Trim().Equals(elementName.Trim(), StringComparison.CurrentCultureIgnoreCase))
                    {
                        DictionarySectionHandler handler = new DictionarySectionHandler();
                        retVal = (IDictionary)handler.Create(null, null, node);
                    }
                }
            }
            catch(Exception ex)
            {
                string msg = string.Format("The Config file entry is missing for '{0}'", elementName);

                var evt = new JWareEvent(10000, EVENT_SOURCE, msg, ex)
                {
                    SourceFile = "ConfigMan.cs",
                    MethodName = "string this[ string key ]",
                    TechMessage = string.Format("The value for the key '{0}' was requested from '{1}', and this key is not present in this file.",
                                                elementName,
                                                xDoc.BaseURI)
                };

                evt.Parameters.Add(new Logging.MethodParameter() { Name = "elementName", Value = elementName });
                evt.Log();

                // -----------------------------------
                // Event was intentionally not thrown...
            }

            return retVal;
        }
    }
}
