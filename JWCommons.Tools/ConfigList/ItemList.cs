#region Copyright © 2017 JoeWare
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

// --------------------------------------------------------
/// <summary>
/// 
/// </summary>

namespace JoeWare.Tools.ConfigList
{
    [ExcludeFromCodeCoverage]
    public class ItemList : ConfigurationElementCollection, IEnumerable<string>
    {
        public ConfigurationElement this[int index]
        {
            get
            {
                return BaseGet(index) as ConfigurationElement;
            }
        }

        // ------------------------------------------------

        protected override ConfigurationElement CreateNewElement()
        {
            return new Item();
        }

        // ------------------------------------------------

        private IEnumerable<string> Items()
        {
            foreach(var item in this)
            {
                yield return item;
            }
        }

        // ------------------------------------------------
        // Must be implemented as part of the abstract Base 
        // Class ConfigurationElementCollection

        protected override object GetElementKey(ConfigurationElement element)
        {
            throw new NotImplementedException();
        }

        // ------------------------------------------------

        public new IEnumerator<string> GetEnumerator()
        {
            return Items().GetEnumerator() as IEnumerator<string>;
        }
    }
}
