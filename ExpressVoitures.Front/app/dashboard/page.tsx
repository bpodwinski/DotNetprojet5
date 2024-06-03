"use client";

import React, { useEffect, useState } from "react";
import dynamic from "next/dynamic";

import { Spacer } from "@nextui-org/react";
import { title } from "@/components/primitives";
import Loading from "./loading";

// Import data
import { useVehicles, useVehicleDetails } from "./_data/set_vehicle";

// Import component
const VehicleTable = dynamic(() => import("./_components/vehicleTable"), {
  ssr: false,
  loading: () => <Loading />,
});
const VehicleDetailModal = dynamic(
  () => import("./_components/vehicleDetail"),
  {
    ssr: false,
    loading: () => <Loading />,
  }
);

export default function DashboardPage() {
  const { vehicles, refetch: refetchVehicles } = useVehicles();
  const { vehicleDetails, fetchVehicleDetails } = useVehicleDetails();

  const rowsPerPage: number = 50;
  const [page, setPage] = useState(1);
  const [modalVisible, setModalVisible] = useState(false);
  const [selectedVehicleId, setSelectedVehicleId] = useState<number | null>(
    null
  );

  const handleDetailsClick = (vehicleId: number) => {
    setSelectedVehicleId(vehicleId);
    setModalVisible(true);
  };

  useEffect(() => {
    refetchVehicles();
  }, [refetchVehicles]);

  useEffect(() => {
    if (modalVisible && selectedVehicleId !== null) {
      fetchVehicleDetails(selectedVehicleId);
    }
  }, [modalVisible, selectedVehicleId]);

  return (
    <div>
      <div className="text-center">
        <h1 className={`${title()}`}>Dashboard</h1>
      </div>

      <Spacer y={8} />

      <div>
        {vehicles.length > 0 && (
          <VehicleTable
            vehicles={vehicles}
            page={page}
            rowsPerPage={rowsPerPage}
            setPage={setPage}
            handleDetailsClick={handleDetailsClick}
          />
        )}
      </div>

      <VehicleDetailModal
        vehicleDetails={vehicleDetails}
        isOpen={modalVisible}
        onClose={() => setModalVisible(false)}
      />
    </div>
  );
}
