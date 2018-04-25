using System;
using InSimDotNet.Packets;

namespace ServerDriftInSim
{
    public class StatsAttribute : Attribute
    {
        private string _name = "";
        private bool _update = true;
        public StatsAttribute(string name)
        {
            _name = name;
            _update = true;
        }
        public StatsAttribute(string name,bool update)
        {
            _name = name;
            _update = update;
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public bool Update
        {
            get
            {
                return Update;
            }
            set
            {
                Update = value;
            }
        }
    }
	
    public class Connection
	{

    	public bool HasVoted;

        public string Abbreviation;

        public int driftCombo;
        public int driftLap;

        public int bestDriftCombo;
        public int bestDriftLap;

        public int straightenDriftCounter;

        public int wwWarnings;

        public int RacePoints;
    	
    	public string UserIP;
        
        // Cruise Bits
        public int Cash;
        public int TotalDistance;
        public int TripMeter;
        public int BankBalance;
     
        public int BonusDistance;
    
        public int TotalBonusDone;
        
    
        public string Cars;
        public uint CarsON;
    
     
        public bool XRG_Rent;
        public bool FBM_Rent;
        
        public int RouletteTimer;
        public int RouletteTimer2;
        public string RedorBlack;
        public int RouletteBet;
        
        // ANTI-BOT
        public int n1;
        public int n2;
        public int result;
        
        
        public Random PlySeed;
        
    
        public byte TotalHealth;
        public int BankBonusTimer;
        public int BankBonus;
        public int HealthDist;
        public byte HealthWarn;
        public byte BankruptWarn;
        public bool IsAFK;
        public int AFKTick;
        public int AFKTimer;
        public int TotalSale;
        
        public int LastLotto;
        public int LastCasino;
        public int TotalJobsDone;

      

        // Cruise Way
        public byte ExitZone;

        public string Location;
        

        public byte IsSpeeder;

        // Places
        public int InHouse1Dist;
        public int InHouse2Dist;
        public int InHouse3Dist;
        public int InHouse4Dist;
        public int InHouse5Dist;
        public int InHouse6Dist;
        public int InHouse7Dist;
        public int InSchoolDist;
        public int InCasinoDist;
        public int InShopDist;
    
        public int InStoreDist;
        public int InBankDist;
    
        public int InDoctorDist;
        public int InNoleggioDist;
        public int InRouletteDist;
        public int InCarDealerDist;
        
        public int InBoxMatchDist;
        

        public string LocationBox;
        public string SpeedBox;

        public byte Electronics;
        public byte Furniture;

        public int SellElectronics;
        public int SellFurniture;

        public bool DisplaysOpen;

        public bool JobToHouse1;
        public bool JobToHouse2;
        public bool JobToHouse3;
        public bool JobToHouse4;
        public bool JobToHouse5;
        public bool JobToHouse6;
        public bool JobToHouse7;
        public bool JobToRoulette;
        public bool JobToCarDealer;
        public bool JobToNoleggio;
        public bool JobToDoctor;
        public bool JobToBank;
        public bool JobToStore;
        public bool JobToShop;
        public bool JobToSchool;
        public bool JobToCasino;
        
        public bool JobToBoxMatch;
        
        public bool JobFromJobCenter;

        public bool InHouse1;
        public bool InHouse2;
        public bool InHouse3;
        public bool InHouse4;
        public bool InHouse5;
        public bool InHouse6;
        public bool InHouse7;

        public bool InStore;
        public bool InSchool;
        public bool InCasino;
        public bool InBank;
        public bool InShop;
    
        public bool InFastPit;
        public bool InDoctor;
        public bool InNoleggio;
        public bool InRoulette;
        public bool InCarDealer;

        public bool InBoxMatch;

  
        
        
        public bool HidePLate;
        public int HidePlateTimer;
        
        // Game Settings
     

        
        public byte InGame;
        public byte Penalty;
        public byte WaitCMD;
        public byte WaitBTN;
        
        public byte StreetSign;
        // Membership Status
        public byte CanBeOfficer;
        public byte CanBeCadet;
        public byte CanBeTowTruck;
        public bool IsOfficer;
        public bool IsCadet;
        public bool IsTowTruck;

        // Officer/Cadet Bits
        public bool IsSuspect;
        public byte Panel;
        public int Chasee;
        public bool InChaseProgress;
   
        
        public bool TowSirenShown;
        public int TowSirenDistance;
       
        public int TowSiren;
        
        
        public byte ChaseCondition;
        public int CopSiren;
      
        public bool CopSirenShown;
        public int DistanceFromCop;
        public int DistanceFromCop2;
        public bool Busted;
        public bool JoinedChase;
        public byte BustedTimer;
        public int SirenDistance;
   
        public bool SpeederClocked;
        public byte OldIDClocked;
        public byte CopInChase;
        public byte BumpButton;
     
        public bool IsBeingBusted;
        public byte AcceptTicket;
        
        public int SuspectMS;

        public bool InFineMenu;
        public string TicketReason;
        public int TicketAmount;
        public bool TicketReasonSet;
        public bool TicketAmountSet;
        public byte TicketRefuse;
     
        
        
        
        // TOW SYSTEM
        
        // Customers variables
        public bool CalledTow;
        public byte TowRequestID; // Only for ClickIDs!
        public bool CallAccepted;
        public bool IsBeingTowed;
        
        // Tows variables
        public int TowClientID;
        public bool InTowProgress;
        
       
       

        // Trap
        public bool InTrap;
        public int TrapX;
        public int TrapY;
        public int TrapSpeed;
        public bool TrapSetted;

        // Moderation System
        public byte InModerationMenu;
        public bool ModReasonSet;
        public string ModReason;
        public string ModUsername;
        public byte ModerationWarn;

        // Send Test
    

        // Player Bits
		public byte FailCon;
		public byte PlayerID;
        public byte UniqueID;
      
        public string Username;
        public string PlayerName;
     
        public byte IsAdmin;
        public byte IsSuperAdmin;
        public byte IsModerator;
      
        public string NoColPlyName;
        public string LastName;
        public string CurrentCar;
        public InSimDotNet.Packets.IS_NPL PlayerPacket;
        public string Plate;
        public string SkinName;
    //    protected byte IntakeRestriction;
        
        public bool Coord;
        
        
        public byte Underground;
        public byte OutOfRing;
        

     /*   public enum enuPType : byte
        {
            Female = 0,
            AI = 1,
            Remote = 2,
        } */

        //CompCar Packet
        public CarPack CompCar = new CarPack();
        
        public struct CarPack
        {
        //    public short AngVel;
            public int Direction;
            public int Heading;
        //    public int Heading;
        //    public CompCarFlags Info;
        //    public int Lap;
            public int Node;
            public int Speed;
        //    public byte PLID;
        //    public byte Position;
            public int X;
            public int Y;
            public int Z;
        }
	}
}