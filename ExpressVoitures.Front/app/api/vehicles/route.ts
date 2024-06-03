"use server";

import { NextRequest, NextResponse } from "next/server";
import { getToken } from "next-auth/jwt";

export async function GET(request: NextRequest) {
  const secret = process.env.NEXTAUTH_SECRET;
  const token = await getToken({ req: request, secret });

  if (!token) {
    return NextResponse.json({ error: "Unauthorized" }, { status: 401 });
  }

  try {
    const res = await fetch(
      "http://localhost:5000/vehicle?pageNumber=1&pageSize=1000",
      {
        headers: {
          Authorization: `Bearer ${token.accessToken}`,
        },
        next: { revalidate: 5 },
      }
    );

    if (!res.ok) {
      throw new Error("Failed to fetch vehicles");
    }

    const vehicles = await res.json();
    return NextResponse.json(vehicles);
  } catch (error: any) {
    return NextResponse.json({ error: error.message }, { status: 500 });
  }
}
