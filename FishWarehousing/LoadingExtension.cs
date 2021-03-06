using CitiesHarmony.API;
using ICities;

namespace FishWarehousing
{
    public class LoadingExtension : LoadingExtensionBase
    {
        public override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (!HarmonyHelper.IsHarmonyInstalled)
            {
                return;
            }
            HarmonyPatches.WarehouseAIPatches.GetTransferVehicleServicePatch.Apply();
            HarmonyPatches.WarehouseWorldInfoPanelPatches.RefreshDropdownListsPatch.Apply();
        }

        public override void OnReleased()
        {
            base.OnReleased();
            if (!HarmonyHelper.IsHarmonyInstalled)
            {
                return;
            }
            HarmonyPatches.WarehouseAIPatches.GetTransferVehicleServicePatch.Undo();
            HarmonyPatches.WarehouseWorldInfoPanelPatches.RefreshDropdownListsPatch.Undo();
        }
    }
}