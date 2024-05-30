"use client";

import React, { useEffect, useState, useMemo } from "react";
import { useRouter } from 'next/navigation';
import { Table, TableHeader, TableColumn, TableBody, TableRow, TableCell, getKeyValue } from "@nextui-org/table";
import { Pagination } from "@nextui-org/pagination";
import {EditIcon} from "../app/Editicon";

interface Vehicle {
  id: number;
  brand: string;
  model: string;
  year: number;
  trim_level: string;
}

const columns = [
  { key: "year", label: "Année" },
  { key: "model", label: "Modèle" },
  { key: "brand", label: "Marque" },
  { key: "trim_level", label: "Finition" }
];

export default function App() {
  const router = useRouter()
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [page, setPage] = useState(1);
  const rowsPerPage: number = 10;
  const pages: number = Math.ceil(vehicles.length / rowsPerPage);

  useEffect(() => {
    if (typeof window !== 'undefined') {
      const token = localStorage.getItem('token');
      const headers = new Headers({
        'Content-Type': 'application/json',
        'Authorization': token ? `Bearer ${token}` : ''
      });

      if (!token) {
        router.push('/login');
      }

      fetch('http://192.168.1.101:5000/vehicle', { headers })
        .then((response) => {
          if (!response.ok) {
            throw new Error(`Failed to fetch: ${response.statusText}`);
          }
          return response.json();
        })
        .then((data) => {
          setVehicles(data);
        })
        .catch((error) => {
          console.error("Error fetching vehicles:", error);
        });
    }
  }, []);

  const items = useMemo(() => {
    const start = (page - 1) * rowsPerPage;
    const end = start + rowsPerPage;
    return vehicles.slice(start, end);
  }, [page, vehicles]);

  return (
    <Table 
      aria-label="Example table with client side pagination"
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
        {(column) => <TableColumn key={column.key}>{column.label}</TableColumn>}
      </TableHeader>
      <TableBody items={items}>
        {(item) => (
          <TableRow key={item.id}>
            {(columnKey) => <TableCell>{getKeyValue(item, columnKey)}</TableCell>}
          </TableRow>
        )}
      </TableBody>
    </Table>
  );
}
