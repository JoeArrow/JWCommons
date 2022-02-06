#region Copyright © 2017 JoeWare
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System.Configuration;
using System.Diagnostics.CodeAnalysis;

// --------------------------------------------------------
/// <summary>
/// 
/// </summary>

namespace JoeWare.Tools.ConfigList
{
    [ExcludeFromCodeCoverage]
    public class ListSection : ConfigurationSection
    {
        public ListSection() { }

        // ------------------------------------------------

        [ConfigurationProperty("itemCollection")]
        public ConfigurationElementCollection Items
        {
            get
            {
                return this["itemCollection"] as ConfigurationElementCollection;
            }
        }

        // ------------------------------------------------

        public static ListSection GetListSection()
        {
            return ConfigurationManager.GetSection("ListSection") as ListSection;
        }
    }
}
