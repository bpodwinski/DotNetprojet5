"use client";

import React, { useEffect, useState, useMemo } from "react";

import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableCell,
  TableColumn,
  Pagination,
  getKeyValue,
  Spinner,
  Tooltip,
  Modal,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
  useDisclosure,
} from "@nextui-org/react";
import { title } from "@/components/primitives";
import { EditIcon } from "../../public/editicon";
import { DeleteIcon } from "../../public/deleteicon";
import { EyeIcon } from "../../public/eyeicon";

interface Vehicle {
  id: number;
  brand: string;
  model: string;
  year: number;
  trim_level: string;
}

export default function DashboardPage() {
  const [loading, setLoading] = useState(true);
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);

  const [page, setPage] = useState(1);
  const rowsPerPage: number = 10;
  const pages: number = Math.ceil(vehicles.length / rowsPerPage);

  const [modalVisible, setModalVisible] = useState(false);
  const [selectedVehicle, setSelectedVehicle] = useState<Vehicle | null>(null);
  const [selectedVehicleId, setSelectedVehicleId] = useState<number | null>(
    null
  );
  const [vehicleDetails, setVehicleDetails] = useState<Vehicle | null>(null);

  const columns = [
    { key: "year", label: "Année" },
    { key: "model", label: "Modèle" },
    { key: "brand", label: "Marque" },
    { key: "trim_level", label: "Finition" },
    { key: "action", label: "Action" },
  ];

  const items = useMemo(() => {
    const start = (page - 1) * rowsPerPage;
    const end = start + rowsPerPage;
    return vehicles.slice(start, end);
  }, [page, vehicles]);

  // Fetch all vehicles
  useEffect(() => {
    async function fetchVehicles() {
      const response = await fetch("/api/vehicles");
      const vehicles = await response.json();
      setVehicles(vehicles);
      setLoading(false);
    }
    fetchVehicles();
  }, []);

  // Fetch vehicle details when modal is opened
  const fetchVehicleDetails = async (id: number) => {
    const response = await fetch("/api/vehicles/id", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ id }),
    });
    const vehicle = await response.json();
    setVehicleDetails(vehicle);
  };

  useEffect(() => {
    // Fetch vehicle details when modal is opened
    if (modalVisible && selectedVehicleId !== null) {
      fetchVehicleDetails(selectedVehicleId);
    }
  }, [modalVisible, selectedVehicleId]);

  const handleDetailsClick = (vehicleId: number) => {
    setSelectedVehicleId(vehicleId);
    setModalVisible(true);
  };

  if (loading || !vehicles) {
    return (
      <div className="fixed inset-0 flex justify-center items-center">
        <Spinner />
      </div>
    );
  }

  return (
    <div>
      <div className="text-center">
        <h1 className={`${title()}`}>Dashboard</h1>
      </div>

      <div className="pt-4">
        <Table
          aria-label="Vehicles list"
          bottomContent={
            <div className="flex w-full justify-center">
              <Pagination
                isCompact
                showControls
                showShadow
                color="secondary"
                page={page}
                total={pages}
                onChange={(page: number) => setPage(page)}
              />
            </div>
          }
        >
          <TableHeader columns={columns}>
            {(column) => (
              <TableColumn key={column.key}>{column.label}</TableColumn>
            )}
          </TableHeader>
          <TableBody items={items}>
            {(item) => (
              <TableRow key={item.id}>
                {columns.map((column) => (
                  <TableCell key={column.key}>
                    {column.key === "action" ? (
                      <div className="relative flex items-center gap-2">
                        <Tooltip content="Details">
                          <span className="text-lg text-default-400 cursor-pointer active:opacity-50">
                            <EyeIcon
                              onClick={() => handleDetailsClick(item.id)}
                            />
                          </span>
                        </Tooltip>
                        <Tooltip content="Edit vehicle">
                          <span className="text-lg text-default-400 cursor-pointer active:opacity-50">
                            <EditIcon />
                          </span>
                        </Tooltip>
                        <Tooltip color="danger" content="Delete vehicle">
                          <span className="text-lg text-danger cursor-pointer active:opacity-50">
                            <DeleteIcon />
                          </span>
                        </Tooltip>
                      </div>
                    ) : (
                      item[column.key as keyof Vehicle]
                    )}
                  </TableCell>
                ))}
              </TableRow>
            )}
          </TableBody>
        </Table>
      </div>

      <Modal
        backdrop="blur"
        isOpen={modalVisible}
        onClose={() => setModalVisible(false)}
      >
        <ModalContent>
          {(onClose) => (
            <>
              <ModalHeader className="flex flex-col gap-1">
                <h3>Vehicle Details</h3>
              </ModalHeader>
              <ModalBody>
                {vehicleDetails ? (
                  <div>
                    <p>
                      <strong>Brand:</strong> {vehicleDetails.brand}
                    </p>
                    <p>
                      <strong>Model:</strong> {vehicleDetails.model}
                    </p>
                    <p>
                      <strong>Year:</strong> {vehicleDetails.year}
                    </p>
                    <p>
                      <strong>Trim Level:</strong> {vehicleDetails.trim_level}
                    </p>
                  </div>
                ) : (
                  <Spinner />
                )}
              </ModalBody>
              <ModalFooter>
                <Button variant="light" onPress={onClose}>
                  Close
                </Button>
              </ModalFooter>
            </>
          )}
        </ModalContent>
      </Modal>
    </div>
  );
}
