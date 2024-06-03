"use client";

import { useSession } from "next-auth/react";
import { useRouter } from "next/navigation";

export default function DashboardLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { data: session, status } = useSession();
  const router = useRouter();

  if (status === "loading") {
    return null;
  }

  if (!session) {
    router.push("/login");
    return null;
  }

  return (
    <section className="flex flex-row items-center justify-center">
      <div className="display-block w-full">{children}</div>
    </section>
  );
}
