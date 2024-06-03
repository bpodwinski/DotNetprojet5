"use client";

import { useState } from "react";
import { useQuery } from "react-query";
import { Vehicle } from "../type";
export const revalidate = 0; //Very important

export const dynamic = "force-dynamic";
export const fetchCache = "force-no-store";
/**
 * Custom hook to fetch and manage the list of vehicles.
 * @returns An object containing the loading state, list of vehicles, and a function to update the vehicles.
 */
export const useVehicles = () => {
  const fetchVehicles = async (): Promise<Vehicle[]> => {
    const response = await fetch("/api/vehicles");
    if (!response.ok) {
      throw new Error("Network response was not ok");
    }
    return response.json();
  };

  const {
    data: vehicles = [],
    error,
    refetch,
  } = useQuery<Vehicle[], Error>("vehicles", fetchVehicles, {
    staleTime: 5000,
    cacheTime: 10000,
    refetchInterval: 10000,
    refetchOnWindowFocus: true,
  });

  return { vehicles, error, refetch };
};

/**
 * Custom hook to fetch and manage the details of a specific vehicle.
 * @returns An object containing the vehicle details and a function to fetch the details of a vehicle by ID.
 */
export const useVehicleDetails = () => {
  const [vehicleDetails, setVehicleDetails] = useState<Vehicle | null>(null);

  /**
   * Async function to fetch the details of a specific vehicle from the API.
   * Updates the state of vehicle details once the data is retrieved.
   * @param id - The ID of the vehicle to fetch details for.
   */
  const fetchVehicleDetails = async (id: number) => {
    const response = await fetch("/api/vehicles/id", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ id }),
    });
    const vehicle: Vehicle = await response.json();
    setVehicleDetails(vehicle);
  };

  return { vehicleDetails, fetchVehicleDetails };
};
