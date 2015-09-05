/****************************** Module Header ******************************\
* Module Name:    networkTalker.svc.cs
* Project:        CSASPNETAJAXWebwakeup
* Copyright (c) Microsoft Corporation
*
* The project illustrates how to design a simple AJAX web wakeup application. 
* We use jQuery, ASP.NET AJAX at client side and Linq to SQL at server side.
* In this sample, we could create a wakeup network and invite someone
* else to join in the network and start to wakeup.
* 
* In this file, we create an Ajax-enabled WCF service which used to be called
* from the client side.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
*
\*****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using Webwakeup.Data;
using Webwakeup.Logic;

namespace Webwakeup.Services
{
    [ServiceContract(Namespace = "http://CSASPNETAJAXWebwakeup",
        SessionMode = SessionMode.Allowed)]
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Transition
    {

        [OperationContract]
        public void Createwakeupnetwork(string clientalias,
            string networkName,
            string password,
            int maxclient,
            bool needPassword)
        {

            if (maxclient < 2)
            {
                maxclient = 2;
            }

            Guid networkid = wakeupManager.Createwakeupnetwork(
                networkName, password, false, maxclient, needPassword);
        }

        [OperationContract]
        public wakeupnetwork Joinwakeupnetwork(string networkid, string alias)
        {
            Guid rid;
            if (Guid.TryParse(networkid, out rid))
            {
                wakeupManager.Joinwakeupnetwork(rid, HttpContext.Current, alias);
                return new wakeupnetwork(rid);

            }
            else
            {
                return null;
            }

        }

        [OperationContract]
        public void Leavewakeupnetwork(string networkid)
        {
            if (networkid == null)
                networkid = GetGUIDFromQuery(
                    HttpContext.Current.Request.UrlReferrer.Query).ToString();
            Guid rid;
            if (Guid.TryParse(networkid, out rid))
            {
                wakeupManager.Leavewakeupnetwork(rid, HttpContext.Current);
            }
            else
            {
                return;
            }
        }

        [OperationContract]
        public List<wakeupnetwork> GetwakeupnetworkList()
        {
            List<tblwakeupnetwork> list = wakeupManager.GetwakeupnetworkList();
            List<wakeupnetwork> result = new List<wakeupnetwork>();
            foreach (tblwakeupnetwork network in list)
            {
                result.Add(new wakeupnetwork(network.wakeupnetworkID));
            }
            return result;
        }

        [OperationContract]
        public wakeupnetwork GetwakeupnetworkInfo(string networkID)
        {
            Guid rim;
            if (Guid.TryParse(networkID, out rim))
            {
                return new wakeupnetwork(rim);
            }
            else
            {
                return null;
            }

        }

        [OperationContract]
        public List<networkTalker> GetnetworkTalkerList()
        {

            List<networkTalker> result = new List<networkTalker>();
            Guid networkid = GetGUIDFromQuery(
                HttpContext.Current.Request.UrlReferrer.Query);
            if (networkid != Guid.Empty)
            {
                List<tblTalker> talkerList =
                    wakeupManager.GetnetworkTalkerList(networkid);
                foreach (tblTalker talker in talkerList)
                {
                    result.Add(new networkTalker(talker, HttpContext.Current));
                }
            }
            return result;

        }

        [OperationContract]
        public bool SendPacket(string Packet)
        {
            Guid networkid = GetGUIDFromQuery(
                HttpContext.Current.Request.UrlReferrer.Query);

            if (networkid != Guid.Empty)
            {
                tblTalker talker = 
                    wakeupManager.FindTalker(networkid, HttpContext.Current);
                wakeupManager.SendPacket(talker, Packet);
                return true;
            }
            else
            {
                return false;

            }
        }

        [OperationContract]
        public List<Packet> RecievePacket()
        {
            List<Packet> result = new List<Packet>();
            Guid networkid = GetGUIDFromQuery(
                HttpContext.Current.Request.UrlReferrer.Query);
            if (networkid != Guid.Empty)
            {
                List<tblPacketPool> PacketList =
                    wakeupManager.RecievePacket(
                        wakeupManager.Getwakeupnetwork(networkid));

                foreach (tblPacketPool msg in PacketList)
                {
                    result.Add(new Packet(msg, HttpContext.Current));
                }
            }
            return result;

        }

        private Guid GetGUIDFromQuery(string query)
        {
            Guid rim;
            if (string.IsNullOrEmpty(query))
            {
                return Guid.Empty;
            }
            Regex reg = new Regex(
            "i=([0-9a-z]{8}-[0-9a-z]{4}-[0-9a-z]{4}-[0-9a-z]{4}-[0-9a-z]{12})");
            string gid = reg.Match(query).Groups[1].Value;
            if (Guid.TryParse(gid, out rim))
            {
                return rim;
            }
            else
            {
                return Guid.Empty;
            }

        }
    }
}
