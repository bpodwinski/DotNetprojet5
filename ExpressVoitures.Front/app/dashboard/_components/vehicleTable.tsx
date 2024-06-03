import React, { useMemo } from "react";
import {
  Table,
  TableHeader,
  TableBody,
  TableRow,
  TableCell,
  TableColumn,
  Pagination,
  Tooltip,
} from "@nextui-org/react";
import { EditIcon } from "../../../public/editicon";
import { DeleteIcon } from "../../../public/deleteicon";
import { EyeIcon } from "../../../public/eyeicon";
import { Vehicle } from ".././type";

interface VehicleTableProps {
  vehicles: Vehicle[];
  page: number;
  rowsPerPage: number;
  setPage: (page: number) => void;
  handleDetailsClick: (vehicleId: number) => void;
}

const VehicleTable: React.FC<VehicleTableProps> = ({
  vehicles,
  page,
  rowsPerPage,
  setPage,
  handleDetailsClick,
}) => {
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
  }, [page, vehicles, rowsPerPage]);

  const pages = Math.ceil(vehicles.length / rowsPerPage);

  return (
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
            onChange={(newPage: number) => setPage(newPage)}
          />
        </div>
      }
    >
      <TableHeader columns={columns}>
        {(column) => <TableColumn key={column.key}>{column.label}</TableColumn>}
      </TableHeader>
      <TableBody items={items}>
        {(item) => (
          <TableRow key={item.id}>
            {columns.map((column) => (
              <TableCell key={column.key}>
                {column.key === "action" ? (
                  <div className="relative flex justify-end items-center gap-2">
                    <Tooltip content="Details">
                      <span className="text-lg text-default-400 cursor-pointer active:opacity-50">
                        <EyeIcon onClick={() => handleDetailsClick(item.id)} />
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
                  item[column.key as keyof Vehicle]?.toString()
                )}
              </TableCell>
            ))}
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
};

export default VehicleTable;
