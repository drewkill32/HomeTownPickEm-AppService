import Header from "@/components/header/Header";

export default function MainLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <>
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
    </>
  );
}
