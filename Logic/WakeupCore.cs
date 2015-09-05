/****************************** Module Header ******************************\
* Module Name:    wakeupnetwork.cs
* Project:        CSASPNETAJAXWebwakeup
* Copyright (c) Microsoft Corporation
*
* The project illustrates how to design a simple AJAX web wakeup application. 
* We use jQuery, ASP.NET AJAX at client side and Linq to SQL at server side.
* In this sample, we could create a wakeup network and invite someone
* else to join in the network and start to wakeup.
* 
* In this file, we create a DataContract class which used to serialize the
* wakeupnetwork data to the client side.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
*
\*****************************************************************************/

using System;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Webwakeup.Logic
{
    [DataContract]
    public class wakeupnetwork
    {
        [DataMember]
        public Guid networkID { get; private set; }
        [DataMember]
        public string networkName { get; private set; }
        [DataMember]
        public int Maxclient { get; private set; }
        [DataMember]
        public int Currentclient { get; private set; }

        public wakeupnetwork(Guid id)
        {
            Webwakeup.Data.SessionDBDataContext db = new Data.SessionDBDataContext();
            var network = db.tblwakeupnetworks.SingleOrDefault(r => r.wakeupnetworkID == id);
            if (network != null)
            {
                networkID = id;
                networkName = network.wakeupnetworkName;
                Maxclient = network.MaxclientNumber;
                Currentclient = network.tblTalkers.Count(t => t.CheckOutTime == null);
            }
        }

    }
}
