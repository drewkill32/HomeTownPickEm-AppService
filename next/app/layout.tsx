import { GeistSans } from "geist/font/sans";
import "./globals.css";
import { Metadata } from "next";
import { Toaster } from "@/components/ui/toaster";

const defaultUrl = process.env.VERCEL_URL
  ? `https://${process.env.VERCEL_URL}`
  : "http://localhost:3000";

export const metadata: Metadata = {
  metadataBase: new URL(defaultUrl),
  title: "St. Pete Pick'em",
  description: "The ultimate college football game-picking app.",
};

export default function RootLayout({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <html lang="en" className={GeistSans.className}>
      <body className="bg-background text-foreground">
        <>
          {children}
          <Toaster />
        </>
      </body>
    </html>
  );
}
