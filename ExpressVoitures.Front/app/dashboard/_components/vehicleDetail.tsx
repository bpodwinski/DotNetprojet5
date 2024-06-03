import React from "react";
import {
  Modal,
  ModalContent,
  ModalHeader,
  ModalBody,
  ModalFooter,
  Button,
  Spinner,
} from "@nextui-org/react";
import { Vehicle } from ".././type";

interface VehicleDetailsModalProps {
  vehicleDetails: Vehicle | null;
  isOpen: boolean;
  onClose: () => void;
}

const VehicleDetailsModal: React.FC<VehicleDetailsModalProps> = ({
  vehicleDetails,
  isOpen,
  onClose,
}) => {
  return (
    <Modal backdrop="blur" isOpen={isOpen} onClose={onClose}>
      <ModalContent>
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
      </ModalContent>
    </Modal>
  );
};

export default VehicleDetailsModal;
