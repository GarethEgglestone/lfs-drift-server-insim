using System;
using System.Linq;
using System.Collections.Generic;
using InSimDotNet;
using InSimDotNet.Packets;
using InSimDotNet.Helpers;

namespace ServerDriftInSim
{
    class Program
    {
        public List<Connection> Connections = new List<Connection>();

        static void Main(string[] args)
        {
            Program _ = new Program();
            _.Start();
        }

        void Start()
        {
            InSim insim = new InSim();

            insim.Initialize(new InSimSettings
            {
                Host = "127.0.0.1",
                Admin = "",
                Flags = InSimFlags.ISF_MCI | InSimFlags.ISF_OBH | InSimFlags.ISF_CON,
                Port = 54333,
                Prefix = '!',
                Interval = 500,
            });

            insim.Bind<IS_MSO>(MSO_Handler);
            insim.Bind<IS_NCN>(NCN_NewConnection);
            insim.Bind<IS_CNL>(NCL_ConnectionLeft);
            insim.Bind<IS_MCI>(MCI_MCIUpdate);
            insim.Bind<IS_NPL>(NPL_NewPlayer);
            insim.Bind<IS_PLP>(PlayerPits);
            insim.Bind<IS_PLL>(PLL_PlayerLeavesRace);
            insim.Bind<IS_CPR>(CPR_ClientPlayerRename);

            insim.Send(new IS_TINY { SubT = TinyType.TINY_NCN, ReqI = 1 });
            insim.Send(new IS_TINY { SubT = TinyType.TINY_NPL, ReqI = 1 });

            while (true)
            {
                var move = 0;

                for (byte x = 1; x < Connections.Count; x++)
                {
                    string _;
                    if (Connections[x].PlayerID != 0) _ = $"^1{Connections[x].PlayerName} ^1| ^7{Connections[x].driftCombo} points";
                    else _ = $"^1{Connections[x].PlayerName} ^1| ^7PIT/SPEC";

                    insim.Send(new IS_BTN
                    {
                        H = 4,
                        L = 170,
                        W = 30,
                        T = (byte)(104 + move),

                        BStyle = ButtonStyles.ISB_RIGHT,
                        
                        Text = _,

                        ReqI = 1,
                        ClickID = (byte)(120 + move),

                        UCID = 255,
                    });

                    move += 4;
                }

                System.Threading.Thread.Sleep(250);
            }
        }

        private void CPR_ClientPlayerRename(InSim insim, IS_CPR packet)
        {
            Connection Conn = Connections[GetConnIdx(packet.UCID)];

            Conn.Plate = packet.Plate;
            Conn.PlayerName = packet.PName;
        }

        private void PLL_PlayerLeavesRace(InSim insim, IS_PLL packet)
        {
            Connection Conn = Connections[GetConnIdx2(packet.PLID)];

            Conn.PlayerID = 0;

            SendMessageToPlayer(insim, "^1> ^7You lost your points!", Conn.UniqueID);
            SendMessageToPlayer(insim, "^1> ^7Combo reset to zero!", Conn.UniqueID);

            Conn.straightenDriftCounter = 0;
            Conn.driftCombo = 0;
        }

        private void PlayerPits(InSim insim, IS_PLP packet)
        {
            Connection Conn = Connections[GetConnIdx2(packet.PLID)];

            Conn.PlayerID = 0;

            SendMessageToPlayer(insim, "^1> ^7You lost your points!", Conn.UniqueID);
            SendMessageToPlayer(insim, "^1> ^7Combo reset to zero!", Conn.UniqueID);

            Conn.straightenDriftCounter = 0;
            Conn.driftCombo = 0;
        }

        private void MSO_Handler(InSim insim, IS_MSO packet)
        {

        }

        private void NPL_NewPlayer(InSim insim, IS_NPL packet)
        {
            Connections[GetConnIdx(packet.UCID)].PlayerID = packet.PLID;
        }

        private void MCI_MCIUpdate(InSim insim, IS_MCI packet)
        {
            try
            {
                byte InGame = 0;
                if (packet.NumC == 8)
                {
                    foreach (Connection u in Connections)
                    {
                        if (u.PlayerID != 0)
                        {
                            InGame += 1;
                        }

                    }
                }
                
                for (int n = 0; n < packet.NumC; n++)
                {
                    int IDX = -1;
                    for (int i = 0; i < Connections.Count; i++)
                    {
                        if (Connections[i].PlayerID == packet.Info[n].PLID)
                        {
                            IDX = i;
                            break;
                        }
                    }
                    if (IDX == -1)
                        continue;

                    Connection Conn = Connections[IDX];

                    if (Conn.PlayerID != 0)
                    {
                        Conn.CompCar.Direction = packet.Info[n].Direction;
                        Conn.CompCar.Node = packet.Info[n].Node;
                        Conn.CompCar.Speed = packet.Info[n].Speed;
                        Conn.CompCar.X = packet.Info[n].X;
                        Conn.CompCar.Y = packet.Info[n].Y;
                        Conn.CompCar.Z = packet.Info[n].Z;
                        Conn.CompCar.Heading = packet.Info[n].Heading;
                    }
                }
                
                if (packet.NumC < 8 || InGame == 8)
                {
                    foreach (Connection Conn in Connections)
                    {
                        if (Conn.PlayerID != 0) 
                        {
                            var kmh = Conn.CompCar.Speed / 91;
                            var direction = Conn.CompCar.Direction / 182;
                            var node = Conn.CompCar.Node;
                            double heading = Conn.CompCar.Heading * 180d / 32768;

                            double speed = MathHelper.SpeedToKph(Conn.CompCar.Speed);

                            double driftAngle = Math.Abs(direction - heading);

                            if (driftAngle > 0 && speed < 5)
                                driftAngle = 0;

                            if (driftAngle > 10 && driftAngle < 130 && speed > 25)
                            {
                                Conn.driftCombo += (int)(2 * driftAngle + speed) / 3;

                                Conn.straightenDriftCounter = 0;
                            }

                            else
                            {
                                if (Conn.driftCombo > 0)
                                {
                                    if (Conn.straightenDriftCounter < 10)
                                        Conn.straightenDriftCounter++;

                                    else
                                    {
                                        SendMessageToPlayer(insim, "^1> ^7You lost your points!", Conn.UniqueID);
                                        SendMessageToPlayer(insim, "^1> ^7Combo reset to zero!", Conn.UniqueID);

                                        Conn.straightenDriftCounter = 0;
                                        Conn.driftCombo = 0;
                                    }
                                }
                            }

                            insim.Send(new IS_BTN
                            {
                                H = 4,
                                L = 180,
                                W = 20,
                                T = 96,

                                BStyle = ButtonStyles.ISB_RIGHT,

                                Text = $"^1{(int)driftAngle}° ^1@ {(int)speed} ^1km/h",

                                ReqI = 1,
                                ClickID = 99,

                                UCID = Conn.UniqueID,
                            });

                            insim.Send(new IS_BTN
                            {
                                H = 4,
                                L = 180,
                                W = 20,
                                T = 100,

                                BStyle = ButtonStyles.ISB_RIGHT,

                                Text = $"^7Combo: ^1{Conn.driftCombo}",

                                ReqI = 1,
                                ClickID = 100,

                                UCID = Conn.UniqueID,
                            });                            
                        }
                    }
                }
            }
            catch { }
        }

        private void NCL_ConnectionLeft(InSim insim, IS_CNL packet)
        {
            var Conn = Connections[GetConnIdx(packet.UCID)];

            RemoveFromConnectionsList(packet.UCID);

            for (byte x = 120; x < byte.MaxValue; x++)
                insim.Send(new IS_BFN { ClickID = x, UCID = 255 });
        }

        private void NCN_NewConnection(InSim insim, IS_NCN packet)
        {
            AddToConnectionsList(packet);

            SendMessageToPlayer(insim, "^1> ^7Welcome to Gaz's drift server,", packet.UCID);
            SendMessageToPlayer(insim, "^1> ^7this server was just done for fun when I was bored.", packet.UCID);
            SendMessageToPlayer(insim, "^1> ^7Follow the rules, and enjoy!", packet.UCID);
        }

        #region Helper Functions
        private void RemoveFromConnectionsList(byte ucid)
        {
            // Copy of item to remove
            Connection RemoveItem = new Connection();

            // Check what item the connection had
            foreach (Connection Conn in Connections)
            {
                if (ucid == Conn.UniqueID)
                {
                    // Copy item (Can't delete it here)
                    RemoveItem = Conn;
                    continue;
                }
            }

            // Remove item
            Connections.Remove(RemoveItem);
        }

        private void AddToConnectionsList(IS_NCN NCN)
        {
            bool InList = false;
            
            foreach (Connection Conn in Connections)
            {
                if (Conn.UniqueID == NCN.UCID)
                {
                    InList = true;
                    continue;
                }
            }
            
            if (!InList)
            {
                Connection NewConn = new Connection();

                NewConn.UniqueID = NCN.UCID;
                NewConn.Username = NCN.UName;
                NewConn.PlayerName = NCN.PName;

                if (NCN.Admin)
                {
                    NewConn.IsAdmin = 1;
                }

                else
                {
                    NewConn.IsAdmin = 0;
                }

                Connections.Add(NewConn);
            }
        }

        public int GetConnIdx(int UNID)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].UniqueID == UNID) { return i; }
            }
            return 0;
        }

        public int GetConnIdx2(int PLID)
        {
            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].PlayerID == PLID) { return i; }
            }
            return 0;
        }

        public void SendMessageToAll(InSim insim, string message)
        {
            insim.Send(new IS_MTC {
                Msg = message,
                ReqI = 0,
                UCID = 255,
                Sound = MessageSound.SND_SYSMESSAGE
            });
        }

        public void SendMessageToPlayer(InSim insim, string message, byte UCID)
        {
            insim.Send(new IS_MTC
            {
                Msg = message,
                ReqI = 0,
                UCID = UCID,
                Sound = MessageSound.SND_ERROR
            });
        }

        #endregion
    }
}
