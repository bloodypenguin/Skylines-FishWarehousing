using ColossalFramework;

namespace FishWarehousing.HarmonyPatches.WarehouseAIPatches
{
    internal static class StartTransferPatch
    {
        internal static void Apply()
        {
            PatchUtil.Patch(
                new PatchUtil.MethodDefinition(typeof(WarehouseAI), nameof(WarehouseAI.StartTransfer)),
                new PatchUtil.MethodDefinition(typeof(StartTransferPatch), (nameof(Prefix))));
        }

        internal static void Undo()
        {
            PatchUtil.Unpatch(new PatchUtil.MethodDefinition(typeof(WarehouseAI), nameof(WarehouseAI.StartTransfer)));
        }

        private static bool Prefix(
            WarehouseAI __instance,
            ushort buildingID,
            ref Building data,
            TransferManager.TransferReason material,
            TransferManager.TransferOffer offer)
        {
            if (material == TransferManager.TransferReason.Fish)
            {
                VehicleInfo transferVehicleService = GetTransferVehicleService(buildingID, ref data);
                if (transferVehicleService == null)
                    return false;
                Array16<Vehicle> vehicles = Singleton<VehicleManager>.instance.m_vehicles;
                ushort vehicle;
                if (!Singleton<VehicleManager>.instance.CreateVehicle(out vehicle, ref Singleton<SimulationManager>.instance.m_randomizer, transferVehicleService, data.m_position, material, false, true))
                    return false;
                transferVehicleService.m_vehicleAI.SetSource(vehicle, ref vehicles.m_buffer[(int) vehicle], buildingID);
                transferVehicleService.m_vehicleAI.StartTransfer(vehicle, ref vehicles.m_buffer[(int) vehicle], material, offer);
                ushort building = offer.Building;
                if (building != (ushort) 0 && (Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int) building].m_flags & Building.Flags.IncomingOutgoing) != Building.Flags.None)
                {
                    int size;
                    transferVehicleService.m_vehicleAI.GetSize(vehicle, ref vehicles.m_buffer[(int) vehicle], out size, out int _);
                    CommonBuildingAI.ExportResource(buildingID, ref data, material, size);
                }
                data.m_outgoingProblemTimer = (byte) 0;
                return false;
            }

            return true;
        }

        //patch me if you want to select vehicle per building / district
        private static VehicleInfo GetTransferVehicleService(ushort buildingID, ref Building data)
        {
            return Singleton<VehicleManager>.instance.GetRandomVehicleInfo(
                ref Singleton<SimulationManager>.instance.m_randomizer, ItemClass.Service.Fishing,
                ItemClass.SubService.None, ItemClass.Level.Level1, VehicleInfo.VehicleType.Car);
        }
    }
}