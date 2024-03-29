import { User } from "@supabase/supabase-js";
import { CircleUser, LogOut } from "lucide-react";

export const getProtectedRoutes = (user: User | null) => {
  if (!user) return [];
  return protectedRoutes;
};

const protectedRoutes: ProtectedNavItem[] = [
  { label: "Leagues", href: "/leagues" },
];

export const getAccountNavItems = (user: User | null): NavItem[] => {
  if (!user) return [];
  return [
    {
      label: "Account",
      href: "/account",
      icon: <CircleUser />,
    },
    { label: "Sign Out", href: "/signout", icon: <LogOut /> },
  ];
};

export const navItems: NavItem[] = [
  { label: "Start a League", href: "/create-league" },
  { label: "Pricing", href: "#" },
  { label: "Support", href: "#" },
];

export type NavItem = BaseNavItem;

export type ProtectedNavItem = NavItem;

type BaseNavItem = {
  label: string;
  href: string;
  icon?: React.ReactNode;
  protected?: boolean;
};
