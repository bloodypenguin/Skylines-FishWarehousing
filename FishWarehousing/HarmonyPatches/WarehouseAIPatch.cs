using ColossalFramework;
using ColossalFramework.Math;
using HarmonyLib;

namespace FishWarehousing.HarmonyPatches
{
    [HarmonyPatch(typeof(WarehouseAI))]
    public static class WarehouseAIPatch
    {
        [HarmonyPatch(typeof(WarehouseAI), "GetTransferVehicleService")]
        [HarmonyPrefix]
        public static bool Prefix(TransferManager.TransferReason material, ItemClass.Level level, ref Randomizer randomizer, ref VehicleInfo __result)
        {
            if (material == TransferManager.TransferReason.Fish)
            {
                __result = GetTransferVehicleService(ref randomizer);
                return false;
            }

            return true;
        }

        private static VehicleInfo GetTransferVehicleService(ref Randomizer randomizer)
        {
            return Singleton<VehicleManager>.instance.GetRandomVehicleInfo(ref randomizer, ItemClass.Service.Fishing, ItemClass.SubService.None, ItemClass.Level.Level1, VehicleInfo.VehicleType.Car);
        }
    }
}