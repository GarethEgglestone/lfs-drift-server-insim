using System;
using InSimDotNet.Packets;

namespace ServerDriftInSim
{
	public class Connection
	{
	public int driftCombo;
	public int driftLap;

	public int bestDriftCombo;
	public int bestDriftLap;

	public int straightenDriftCounter;
	}

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
