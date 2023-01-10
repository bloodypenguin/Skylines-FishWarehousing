using HarmonyLib;

namespace FishWarehousing.HarmonyPatches
{
    public static class RefreshDropdownListsPatch
    {
        private static TransferManager.TransferReason[] m_transferReasons = new TransferManager.TransferReason[16]
        {
            TransferManager.TransferReason.None,
            TransferManager.TransferReason.Fish,
            TransferManager.TransferReason.AnimalProducts,
            TransferManager.TransferReason.Flours,
            TransferManager.TransferReason.Paper,
            TransferManager.TransferReason.PlanedTimber,
            TransferManager.TransferReason.Petroleum,
            TransferManager.TransferReason.Plastics,
            TransferManager.TransferReason.Glass,
            TransferManager.TransferReason.Metals,
            TransferManager.TransferReason.LuxuryProducts,
            TransferManager.TransferReason.Lumber,
            TransferManager.TransferReason.Food,
            TransferManager.TransferReason.Coal,
            TransferManager.TransferReason.Petrol,
            TransferManager.TransferReason.Goods
        };
        
        
        [HarmonyPatch(typeof(WarehouseWorldInfoPanel), "RefreshDropdownLists")]
        [HarmonyPrefix]
        public static bool Prefix(WarehouseWorldInfoPanel __instance)
        {
            var fieldInfo = AccessTools.Field(__instance.GetType(), "m_transferReasons");
            var reasons = (TransferManager.TransferReason[])fieldInfo.GetValue(__instance);
            if (reasons.Length != m_transferReasons.Length)
            {
                fieldInfo.SetValue(__instance, m_transferReasons);
            }
            return true;
        }
    }
}