using ColossalFramework;
using ColossalFramework.Math;

namespace FishWarehousing.HarmonyPatches.WarehouseAIPatches
{
    internal static class GetTransferVehicleServicePatch
    {
        internal static void Apply()
        {
            PatchUtil.Patch(
                new PatchUtil.MethodDefinition(typeof(WarehouseAI), nameof(WarehouseAI.GetTransferVehicleService)),
                new PatchUtil.MethodDefinition(typeof(GetTransferVehicleServicePatch), (nameof(Prefix))));
        }

        internal static void Undo()
        {
            PatchUtil.Unpatch(new PatchUtil.MethodDefinition(typeof(WarehouseAI), nameof(WarehouseAI.GetTransferVehicleService)));
        }

        private static bool Prefix(TransferManager.TransferReason material,
            ItemClass.Level level,
            ref Randomizer randomizer, ref VehicleInfo __result)
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
            return Singleton<VehicleManager>.instance.GetRandomVehicleInfo(ref randomizer, 
                ItemClass.Service.Fishing,
                ItemClass.SubService.None, ItemClass.Level.Level1, VehicleInfo.VehicleType.Car);
        }
    }
}