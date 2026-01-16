#region Copyright © 2017 Joeware
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System.Collections;
using System.Collections.Generic;

namespace JWCommons.Tools
{
    public interface IConfigMan
    {
        string FileName { get; set; }
        IDictionary AppSettings { get; }
        string this[string key] { get; }

        T GetJSONItem<T>(string itemName) where T : class;
        string ReadFromKeyedList(string listName, string key);
        T ReadFromKeyedObjList<T>(string listName, string key) where T : class;
        IList ReadList(string listName);
        List<T> ReadObjList<T>(string listName);
    }
}