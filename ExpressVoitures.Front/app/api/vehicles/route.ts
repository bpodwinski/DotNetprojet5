import { NextRequest, NextResponse } from 'next/server';

export async function GET(request: NextRequest) {
  console.log('Fetching vehicles from backend...');
  try {
    const res = await fetch('http://192.168.1.101:5000/vehicle');
    if (!res.ok) {
      console.error(`Failed to fetch: ${res.statusText}`);
      throw new Error(`Failed to fetch: ${res.statusText}`);
    }
    const data = await res.json();
    console.log('Fetched vehicles:', data);
    return NextResponse.json(data);
  } catch (error: any) {
    console.error('Error fetching vehicles:', error.message);
    return NextResponse.json({ error: error.message }, { status: 500 });
  }
}
