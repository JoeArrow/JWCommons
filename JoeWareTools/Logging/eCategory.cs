#region Copyright © 2017 JoeWare
//
// All rights reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical, or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
#endregion

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

// --------------------------------------------------------
/// <summary>
/// 
/// </summary>

namespace JoeWare.Tools.Logging
{
    public enum eCategory
    {
        /// <summary>
        ///     No Category listed
        /// </summary>

        None,

        /// <summary>
        ///     A device related event
        /// </summary>

        Devices,

        /// <summary>
        ///     A Disk related event
        /// </summary>

        Disk,

        /// <summary>
        ///     A printer related event
        /// </summary>

        Printers,

        /// <summary>
        ///     A Service related event
        /// </summary>

        Services,

        /// <summary>
        ///     A Shell related event
        /// </summary>

        Shell,

        /// <summary>
        ///     A System related event
        /// </summary>

        SystemEvent,

        /// <summary>
        ///     A Network related event
        /// </summary>

        Network,

        /// <summary>
        ///     Critical situation, Call for support.
        /// </summary>

        Critical = 100,

        /// <summary>
        ///     Minor situation, Email support team.
        /// </summary>

        Moderate = 200,

        /// <summary>
        ///     For information only.
        /// </summary>

        Benign = 300
    }
}
