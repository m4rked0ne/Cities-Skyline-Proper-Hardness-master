﻿using ColossalFramework;
using ColossalFramework.Globalization;
using ColossalFramework.Math;
using System;
using System.Runtime.InteropServices;
using UnityEngine;
namespace DifficultyMod
{
    public class WBCargoTruckAI : CargoTruckAI
    {
        private static byte[] blockCounter = new byte[16384u];
        public override void SimulationStep(ushort vehicleID, ref Vehicle data, Vector3 physicsLodRefPos)
        {
            var bc = blockCounter[vehicleID];
            if (data.m_blockCounter == 0)
            {
                bc = 0;
            }
            else if (data.m_blockCounter > 1)
            {
                bc = (byte)Mathf.Min(bc + 1, 0xff);
            }
            // MarkedOne fixed updated this line for the newer patches based on his interpretation.
            //if ((data.m_flags & Vehicle.Flags.Congestion) != Vehicle.Flags.None)
            if ((data.m_flags & Vehicle.Flags.Congestion) == Vehicle.Flags.Congestion)
            {
                bc = (byte)Mathf.Min(bc + 5, 0xff);
                data.m_flags &= ~Vehicle.Flags.Congestion;
            }
            data.m_blockCounter = 1;
            blockCounter[vehicleID] = bc;

            if (bc == 0xff)
            {
                blockCounter[vehicleID] = 0;
                Singleton<VehicleManager>.instance.ReleaseVehicle(vehicleID);
            }
            else
            {
                base.SimulationStep(vehicleID, ref data, physicsLodRefPos);
            }
        }
    }

}
