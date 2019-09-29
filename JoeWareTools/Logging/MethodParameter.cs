#region Copyright © 2017 JoeWare
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Diagnostics.CodeAnalysis;

// --------------------------------------------------------
/// <summary>
/// 
/// </summary>

namespace JoeWare.Tools.Logging
{
    public class MethodParameter
    {
        [ExcludeFromCodeCoverage]
        public string Name { set; get; }
        [ExcludeFromCodeCoverage]
        public string Value { get; set; }

        // ------------------------------------------------

        public override string ToString()
        {
            return string.Format("Name:\t{1}{0}\tValue:\t{2}", Environment.NewLine, Name, Value);
        }
    }
}
