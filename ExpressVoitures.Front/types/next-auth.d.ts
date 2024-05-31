import NextAuth, { DefaultSession, DefaultJWT, DefaultUser } from "next-auth";

declare module "next-auth" {
  interface Session {
    accessToken?: string;
    user: {
      id?: string;
    } & DefaultSession["user"];
  }

  interface JWT {
    accessToken?: string;
    id?: string;
  }

  interface User {
    id: string;
  }
}
