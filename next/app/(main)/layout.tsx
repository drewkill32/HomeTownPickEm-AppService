import SupabaseContextProvider from "@/components/SupabaseContext";
import Header from "@/components/header/Header";
import { getUser } from "@/server/user";

export default async function MainLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  const { user } = await getUser();
  return (
    <SupabaseContextProvider user={user}>
      <Header />
      <main className="container">{children}</main>
      <footer className="bg-[#002244] px-4 py-6 text-white sm:px-6 lg:px-8">
        <div className="text-center">
          <p>
            © <span id="copyright-year" /> St. Pete Pick'em. All rights
            reserved.
          </p>
        </div>
      </footer>
    </SupabaseContextProvider>
  );
}
