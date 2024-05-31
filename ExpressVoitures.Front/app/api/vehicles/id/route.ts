"use server";

import { NextRequest, NextResponse } from "next/server";
import { getToken } from "next-auth/jwt";

export async function POST(request: NextRequest) {
  const secret = process.env.NEXTAUTH_SECRET;
  const token = await getToken({ req: request, secret });

  if (!token) {
    return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
  }

  try {
    const { id } = await request.json();
    const res = await fetch(`http://localhost:5000/vehicle/${id}`, {
      headers: {
        Authorization: `Bearer ${token.accessToken}`,
      },
    });

    if (!res.ok) {
      throw new Error("Failed to fetch vehicles");
    }

    const vehicle = await res.json();
    return NextResponse.json(vehicle);
  } catch (error: any) {
    return NextResponse.json({ error: error.message }, { status: 500 });
  }
}
