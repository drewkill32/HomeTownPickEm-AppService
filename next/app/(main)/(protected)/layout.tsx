import { getUser } from "@/server/user";

export default async function ProtectedLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { user } = await getUser();
  if (!user) {
    return null;
  }
  return children;
}
