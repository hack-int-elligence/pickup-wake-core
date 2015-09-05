/****************************** Module Header ******************************\
* Module Name:    Packet.cs
* Project:        CSASPNETAJAXWebwakeup
* Copyright (c) Microsoft Corporation
*
* The project illustrates how to design a simple AJAX web wakeup application.
* We use jQuery, ASP.NET AJAX at client side and Linq to SQL at server side.
* In this sample, we could create a wakeup network and invite someone
* else to join in the network and start to wakeup.
*
* In this file, we create a DataContract class which used to serialize the
* Packet data to the client side.
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
    public class Packet
    {
        [DataMember]
        public string Talker { get; private set; }
        [DataMember]
        public string PacketData { get; private set; }
        [DataMember]
        public DateTime SendTime { get; private set; }

        [DataMember]
        public bool Islinked { get; private set; }

        public Packet(Webwakeup.Data.tblPacketPool Packet,HttpContext session)
        {
            Talker = Packet.tblTalker.tblSession.clientAlias;
            PacketData = Packet.Packet;
            SendTime = Packet.SendTime;
            Islinked = (Packet.tblTalker.tblSession.SessionID != session.Session.SessionID);
        }
    }
}
