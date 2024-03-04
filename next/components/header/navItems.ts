export const mobileNavItems: NavItem[] = [
  { label: "Dashboard", href: "/dashboard" },
  { label: "Account", href: "/account" },
  { label: "Signout", href: "/signout" },
];

export const navItems: NavItem[] = [
  { label: "Start a League", href: "#" },
  { label: "Pricing", href: "#" },
  { label: "Support", href: "#" },
];

export type NavItem = {
  label: string;
  href: string;
};
