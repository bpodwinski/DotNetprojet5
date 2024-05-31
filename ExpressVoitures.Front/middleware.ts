import { NextResponse } from "next/server";
import { getToken } from "next-auth/jwt";
import type { NextRequest } from "next/server";

export async function middleware(req: NextRequest) {
  const token = await getToken({ req, secret: process.env.NEXTAUTH_SECRET });
  const { pathname } = req.nextUrl;

  // Allow the request if it's for next-auth session & provider fetching
  // or if the token exists
  if (pathname.includes("/api/auth") || token) {
    return NextResponse.next();
  }

  // List of public paths that do not require authentication
  const publicPaths = ["/", "/register", "/login", "/about"];

  // Check if the path is one of the public paths
  if (publicPaths.includes(pathname)) {
    return NextResponse.next();
  }

  // Redirect to login if they don't have token AND are not requesting a public path
  if (!token) {
    return NextResponse.redirect(new URL("/", req.url));
  }

  // Default response
  return NextResponse.next();
}

export const config = {
  matcher: ["/((?!_next/static|_next/image|favicon.ico).*)", "/api/(.*)"],
};
