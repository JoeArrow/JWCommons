#region Copyright © 2017 JWCommons
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;

// --------------------------------------------------------
/// <summary>
/// 
/// </summary>

namespace JWCommonsTools_UT.DTO
{
    public class EntryDTO
    {
        public int Age { get; set; }
        public string Name { get; set; }

        // ------------------------------------------------

        public override string ToString()
        {
            return string.Format("Name:{0}\t{1}{0}Age:{0}\t{2}", Environment.NewLine, Name, Age);
        }
    }
}
