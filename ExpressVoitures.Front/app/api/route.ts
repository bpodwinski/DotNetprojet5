import type { NextApiRequest, NextApiResponse } from 'next';
import { Vehicles } from '../../types';

export default async function handler(
  req: NextApiRequest,
  res: NextApiResponse<Vehicles[]>
) {
  try {
    const response = await fetch('http://localhost:5000/vehicle');
    const vehicles: Vehicles[] = await response.json();
    res.status(200).json(vehicles);
  } catch (error) {
    res.status(500).json([]);
  }
}
