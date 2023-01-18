using ColossalFramework;
using HarmonyLib;
using System;
using UnityEngine;

namespace FishWarehousing.HarmonyPatches
{
	[HarmonyPatch(typeof(CommonBuildingAI))]
	public static class CommonBuildingAIPatch
	{
		[HarmonyPatch(typeof(CommonBuildingAI), "CalculateOwnVehicles")]
        [HarmonyPrefix]
        public static bool Prefix(ushort buildingID, ref Building data, TransferManager.TransferReason material, ref int count, ref int cargo, ref int capacity, ref int outside)
        {
            if (material == TransferManager.TransferReason.Fish &&  data.Info.GetAI() is WarehouseAI)
            {
                VehicleManager instance = Singleton<VehicleManager>.instance;
				ushort num = data.m_ownVehicles;
				int num2 = 0;
				while (num != 0)
				{
					if ((TransferManager.TransferReason)instance.m_vehicles.m_buffer[num].m_transferType == material)
					{
						VehicleInfo info = instance.m_vehicles.m_buffer[num].Info;
						if(info.m_vehicleType == VehicleInfo.VehicleType.Car)
						{
							info.m_vehicleAI.GetSize(num, ref instance.m_vehicles.m_buffer[num], out var size, out var max);
							cargo += Mathf.Min(size, max);
							capacity += max;
							count++;
							if ((instance.m_vehicles.m_buffer[num].m_flags & (Vehicle.Flags.Importing | Vehicle.Flags.Exporting)) != 0)
							{
								outside++;
							}
						}
						
					}
					num = instance.m_vehicles.m_buffer[num].m_nextOwnVehicle;
					if (++num2 > 16384)
					{
						CODebugBase<LogChannel>.Error(LogChannel.Core, "Invalid list detected!\n" + Environment.StackTrace);
						break;
					}
				}
                return false;
            }

            return true;
        }

	}
}
