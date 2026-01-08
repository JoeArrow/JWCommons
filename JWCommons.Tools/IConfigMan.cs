using System.Collections;
using System.Collections.Generic;

namespace JWCommons.Tools
{
    public interface IConfigMan
    {
        IDictionary AppSettings { get; }
        string FileName { get; set; }

        T GetJSONItem<T>(string itemName) where T : class;
        string ReadFromKeyedList(string listName, string key);
        T ReadFromKeyedObjList<T>(string listName, string key) where T : class;
        IList ReadList(string listName);
        List<T> ReadObjList<T>(string listName);
    }
}