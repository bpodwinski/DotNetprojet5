import NextAuth, { NextAuthOptions } from "next-auth";
import CredentialsProvider from "next-auth/providers/credentials";
import { JWT } from "next-auth/jwt";
import { Session } from "next-auth";

export const authOptions: NextAuthOptions = {
  providers: [
    CredentialsProvider({
      name: "Credentials",
      credentials: {
        email: { label: "Email", type: "email" },
        password: { label: "Password", type: "password" },
      },
      authorize: async (credentials) => {
        if (!credentials) return null;
        try {
          const response = await fetch("http://localhost:5000/user/login", {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify({
              email: credentials.email,
              password: credentials.password,
            }),
          });

          if (!response.ok) {
            console.error(await response.json());
            throw new Error("Failed to authenticate");
          }

          const user = await response.json();
          //console.log('API response:', user);

          if (user && user.token) {
            return {
              id: String(user.id),
              accessToken: String(user.token),
              name: `${user.firstname} ${user.lastname}`,
              email: user.email,
            };
          }
          return null;
        } catch (error) {
          console.error("Error authenticating", error);
          return null;
        }
      },
    }),
  ],
  secret: process.env.NEXTAUTH_SECRET,
  callbacks: {
    async jwt({ token, user }: { token: JWT; user?: any }) {
      if (user && typeof user.accessToken === "string") {
        token.id = user.id;
        token.accessToken = user.accessToken;
        token.name = user.name;
        token.email = user.email;
      }

      console.log("JWT Callback:", token);
      return token;
    },

    async session({ session, token }: { session: Session; token: JWT }) {
      if (typeof token.accessToken === "string") {
        session.accessToken = token.accessToken;
      }
      session.user.id = token.id as string;
      session.user.name = token.name as string;
      session.user.email = token.email as string;

      console.log("Session Callback:", session);
      return session;
    },
  },
};

const handler = NextAuth(authOptions);

export { handler as GET, handler as POST };
