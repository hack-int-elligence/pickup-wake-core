/****************************** Module Header ******************************\
* Module Name:    networkTalker.cs
* Project:        CSASPNETAJAXWebwakeup
* Copyright (c) Microsoft Corporation
*
* The project illustrates how to design a simple AJAX web wakeup application.
* We use jQuery, ASP.NET AJAX at client side and Linq to SQL at server side.
* In this sample, we could create a wakeup network and invite someone
* else to join in the network and start to wakeup.
*
* In this file, we create a DataContract class which used to serialize the
* talker data to the client side.
*
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
*
\*****************************************************************************/

using System;
using System.Web;
using System.Runtime.Serialization;

namespace Webwakeup.Logic
{
    [DataContract]
    public class networkTalker
    {
        [DataMember]
        public string TalkerAlias { get; private set; }
        [DataMember]
        public string TalkerSession { get; private set; }
        [DataMember]
        public string TalkerIP { get; private set; }
        [DataMember]
        public bool Islinked { get; private set; }

        public networkTalker(Webwakeup.Data.tblTalker talker, HttpContext context)
        {
            TalkerAlias = talker.tblSession.clientAlias;
            TalkerIP = talker.tblSession.IP;
            TalkerSession = talker.tblSession.SessionID;
            Islinked = (TalkerSession != context.Session.SessionID);
        }
    }
}
