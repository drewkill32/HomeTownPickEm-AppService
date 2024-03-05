import { cn } from "@/utils/tailwind";
import { buttonVariants } from "../ui/button";
import { User } from "@supabase/supabase-js";

export const getProtectedRoutes = (user: User | null) => {
  if (!user) return [];
  return protectedRoutes;
};

const protectedRoutes: ProtectedNavItem[] = [
  { label: "Dashboard", href: "/dashboard", divider: true },
  { label: "Account", href: "/account" },
  {
    label: "Signout",
    href: "/signout",

    className: `${cn(buttonVariants(), " mx-4 mt-3")}`,
  },
];

export const navItems: NavItem[] = [
  { label: "Start a League", href: "#" },
  { label: "Pricing", href: "#" },
  { label: "Support", href: "#" },
];

export interface ProtectedNavItem extends NavItem {}

export type NavItem = {
  label: string;
  href: string;
  className?: string;
  divider?: boolean;
};
