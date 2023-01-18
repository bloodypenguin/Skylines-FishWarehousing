using HarmonyLib;
using System;

namespace FishWarehousing.HarmonyPatches
{
    [HarmonyPatch(typeof(WarehouseWorldInfoPanel))]
    public static class WarehouseWorldInfoPanelPatch
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
        public static bool RefreshDropdownLists(WarehouseWorldInfoPanel __instance)
        {
            var fieldInfo = AccessTools.Field(__instance.GetType(), "m_transferReasons");
            var reasons = (TransferManager.TransferReason[])fieldInfo.GetValue(__instance);
            if (reasons.Length != m_transferReasons.Length)
            {
                fieldInfo.SetValue(__instance, m_transferReasons);
            }
            return true;
        }

        [HarmonyPatch(typeof(WarehouseWorldInfoPanel), "GenerateResourceDescription")]
        [HarmonyPrefix]
        public static bool GenerateResourceDescription(TransferManager.TransferReason resource, ref string __result, bool isForWarehousePanel = false)
        {
            if(isForWarehousePanel && resource == TransferManager.TransferReason.Fish)
			{
                string text = "Fish is produced by Fish Farms and Fish Harbors.";
                text += Environment.NewLine;
		        text += Environment.NewLine;
                text = text + "- " + ColossalFramework.Globalization.Locale.Get("RESOURCE_CANNOTBEIMPORTED");
                text += Environment.NewLine;
                __result = text + "- " + ColossalFramework.Globalization.Locale.Get("RESOURCE_CANBEEXPORTED_COST");
                return false;
			}
            return true;
        }
    }
}