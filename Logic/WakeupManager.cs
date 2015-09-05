/****************************** Module Header ******************************\
* Module Name:    wakeupManager.cs
* Project:        CSASPNETAJAXWebwakeup
* Copyright (c) Microsoft Corporation
*
* The project illustrates how to design a simple AJAX web wakeup application. 
* We use jQuery, ASP.NET AJAX at client side and Linq to SQL at server side.
* In this sample, we could create a wakeup network and invite someone
* else to join in the network and start to wakeup.
* 
* In this file, we use Linq to control the data in the database.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
*
\*****************************************************************************/

using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Webwakeup.Data;

namespace Webwakeup.Logic
{
    public class wakeupManager
    {
        #region Send & Recieve Packet

        public static bool SendPacket(tblTalker talker, string Packet)
        {
            try
            {
                SessionDBDataContext db = new SessionDBDataContext();
                tblPacketPool msgpool = new tblPacketPool();
                msgpool.Packet = Packet;
                msgpool.SendTime = DateTime.Now;
                msgpool.talkerID = talker.TalkerID;
                db.tblPacketPools.InsertOnSubmit(msgpool);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static List<tblPacketPool> RecievePacket(tblwakeupnetwork network)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            if (db.tblPacketPools.Count(
                msg => network.tblTalkers.Contains(msg.tblTalker)) > 0)
            {
                return (from Packets in db.tblPacketPools
                        where Packets.tblTalker.wakeupnetworkID == network.wakeupnetworkID
                        select Packets).ToList();
            }
            else
            {
                return null;
            }
        }

        private static void TryToDeletewakeupPacketList(Guid networkid)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            var wakeupnetwork = Getwakeupnetwork(networkid);
            if (wakeupnetwork.tblTalkers.Count(t => t.CheckOutTime == null) == 0)
            {
                var list = from m in db.tblPacketPools
                           where m.tblTalker.wakeupnetworkID == networkid
                           select m;
                db.tblPacketPools.DeleteAllOnSubmit(list);
                db.SubmitChanges();
            }
        }

        #endregion

        #region wakeupnetwork Management

        public static Guid Createwakeupnetwork(string networkName, string password,
            bool isLock, int maxclientNumber, bool needPassword)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            tblwakeupnetwork network = new tblwakeupnetwork();
            network.wakeupnetworkID = Guid.NewGuid();
            network.wakeupnetworkName = networkName;
            network.wakeupnetworkPassword = password;
            network.IsLock = isLock;
            network.MaxclientNumber = maxclientNumber;
            network.NeedPassword = needPassword;
            db.tblwakeupnetworks.InsertOnSubmit(network);
            db.SubmitChanges();
            return network.wakeupnetworkID;
        }

        public static tblwakeupnetwork Getwakeupnetwork(Guid networkid)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            return db.tblwakeupnetworks.SingleOrDefault(r => r.wakeupnetworkID == networkid);

        }

        public static bool IsnetworkFull(Guid networkID)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            var rsl = db.tblwakeupnetworks.Single(network => network.wakeupnetworkID == networkID);
            if (rsl != null)
            {
                return rsl.MaxclientNumber == rsl.tblTalkers.Count(
                    t => t.CheckOutTime == null);
            }
            else
            {
                return false;
            }

        }

        public static List<tblwakeupnetwork> GetwakeupnetworkList()
        {
            SessionDBDataContext db = new SessionDBDataContext();
            return db.tblwakeupnetworks.ToList();
        }

        public static bool Joinwakeupnetwork(Guid wakeupnetworkID, HttpContext context,
            string alias)
        {
            if (!wakeupManager.IsnetworkFull(wakeupnetworkID))
            {
                SessionDBDataContext db = new SessionDBDataContext();
                if (db.tblSessions.Count(
                    s => s.SessionID == context.Session.SessionID) == 0)
                {
                    wakeupManager.CreateSession(context, alias);
                }
                var session = wakeupManager.GetSession(context);
                if (db.tblTalkers.Count(t => t.wakeupnetworkID == wakeupnetworkID && 
                    t.SessionID == session.UID && t.CheckOutTime == null) > 0)
                {
                    return false;
                }
                else
                {
                    tblTalker talker = new tblTalker();
                    talker.wakeupnetworkID = wakeupnetworkID;
                    talker.CheckInTime = DateTime.Now;
                    talker.CheckOutTime = null;
                    talker.SessionID = session.UID;
                    db.tblTalkers.InsertOnSubmit(talker);
                    db.SubmitChanges();
                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public static tblTalker FindTalker(Guid wakeupnetworkID, HttpContext context)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            var rsl = db.tblTalkers.FirstOrDefault(
                t => t.wakeupnetworkID == wakeupnetworkID && 
                t.SessionID == wakeupManager.GetSession(context).UID);
            return rsl;

        }

        public static List<tblTalker> GetnetworkTalkerList(Guid wakeupnetworkID)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            var rsl = from d in db.tblTalkers
                      where d.CheckOutTime == null && d.wakeupnetworkID == wakeupnetworkID
                      select d;
            return rsl.ToList();

        }

        public static void Leavewakeupnetwork(Guid wakeupnetworkID, HttpContext context)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            tblSession session = wakeupManager.GetSession(context);
            if (session != null)
            {
                var talker = db.tblTalkers.FirstOrDefault(
                    t => t.wakeupnetworkID == wakeupnetworkID &&
                    t.SessionID == session.UID && t.CheckOutTime == null);

                if (talker != null)
                {
                    talker.CheckOutTime = DateTime.Now;
                    db.SubmitChanges();
                }
            }
            TryToDeletewakeupPacketList(wakeupnetworkID);
        }
        #endregion

        #region wakeup Session Management

        public static tblSession GetSession(HttpContext context)
        {
            SessionDBDataContext db = new SessionDBDataContext();
            var session = db.tblSessions.FirstOrDefault(
                s => s.SessionID == context.Session.SessionID);
            return session;
        }

        public static bool SessionExist(HttpContext context)
        {
            return wakeupManager.GetSession(context) != null;
        }

        public static bool CreateSession(HttpContext context,
            string clientAlias)
        {
            try
            {
                SessionDBDataContext db = new SessionDBDataContext();

                tblSession session = new tblSession();
                session.SessionID = context.Session.SessionID;
                session.IP = context.Request.clientHostAddress;
                if (string.IsNullOrEmpty(clientAlias))
                    clientAlias = session.IP;
                session.clientAlias = clientAlias;
                db.tblSessions.InsertOnSubmit(session);
                db.SubmitChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}
