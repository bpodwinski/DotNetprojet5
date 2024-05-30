import { NextResponse } from 'next/server';
import { fetchVehicles } from "@/utils/fetchVehicles";

export async function GET(request: Request) {
  try {
    const data = await fetchVehicles('user/login');
    console.log('Data fetched:', data);
    
    return NextResponse.json(data);
  } catch (error: any) {
    console.error('Error fetching vehicles:', error.message);
    return NextResponse.json({ error: error.message }, { status: 500 });
  }
}
