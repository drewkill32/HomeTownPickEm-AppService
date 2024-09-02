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
  icons: [
    {
      rel: "apple-touch-icon",
      sizes: "180x180",
      url: "/apple-touch-icon.png",
    },
    {
      rel: "icon",
      sizes: "48x48",
      url: "/favicon.ico",
    },
    {
      rel: "icon",
      sizes: "any",
      url: "/favicon.svg",
      type: "image/svg+xml",
    },
    {
      rel: "manifest",
      url: "/site.webmanifest",
    },
    {
      rel: "mask-icon",
      sizes: "48x48",
      url: "/safari-pinned-tab.svg",
      color: "#5bbad5",
    },
  ],
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
