"use client";

import { Button, buttonVariants } from "@/components/ui/button";
import { cn } from "@/utils/tailwind";
import { NavItem } from "./navItems";
import { Sheet, SheetContent, SheetTitle, SheetTrigger } from "../ui/sheet";
import Link from "next/link";
import React from "react";
import { LogOut } from "lucide-react";
import { User } from "@supabase/supabase-js";

interface MobileNavMenuButtonProps {
  navigationItems: NavItem[];
  className?: string;
}

export const MobileNavMenuButton = ({
  navigationItems,
  className,
}: MobileNavMenuButtonProps) => {
  return (
    <Sheet>
      <SheetTrigger asChild>
        <Button
          size="icon"
          variant="outline"
          aria-label="Open Menu"
          className={cn(
            " inline-flex h-10 w-10 items-center justify-center whitespace-nowrap rounded-md border border-input bg-background text-sm font-medium ring-offset-background transition-colors hover:bg-accent hover:text-accent-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50",
            className,
          )}
        >
          <svg
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            className="h-6 w-6"
          >
            <line x1="4" x2="20" y1="12" y2="12"></line>
            <line x1="4" x2="20" y1="6" y2="6"></line>
            <line x1="4" x2="20" y1="18" y2="18"></line>
          </svg>
        </Button>
      </SheetTrigger>
      <SheetContent className="h-full w-full sm:max-w-full">
        <MobileNavMenu navigationItems={navigationItems} />
      </SheetContent>
    </Sheet>
  );
};

type MobileNavMenuProps = {
  navigationItems: NavItem[];
};

const MobileNavMenu = ({ navigationItems }: MobileNavMenuProps) => {
  return (
    <nav className="flex h-full flex-col pt-9">
      {navigationItems.map((item) => {
        var NavFunction = navComponents[item.label];
        if (NavFunction) {
          return <NavFunction key={item.label} item={item} />;
        }

        return (
          <NavLink key={item.label} href={item.href} className="flex gap-2">
            {item.icon}
            <span className={item.icon ? "" : "pl-8"}>{item.label}</span>
          </NavLink>
        );
      })}
    </nav>
  );
};

const navComponents = {
  "Sign Out": ({ item }: { item: NavItem }) => {
    return (
      <>
        <div className="flex-grow"></div>
        <NavLink href={item.href} className={cn(buttonVariants(), "mx-3")}>
          <LogOut className="mr-2" />
          {item.label}
        </NavLink>
      </>
    );
  },
} as { [key: string]: ({ item }: { item: NavItem }) => React.ReactNode };

const NavLink = React.forwardRef<
  React.ElementRef<typeof Link>,
  React.ComponentPropsWithoutRef<typeof Link>
>(({ className, href, children, ...props }, ref) => {
  return (
    <Link
      className={cn(
        "item  py-4 pl-2 hover:bg-slate-500 hover:text-white hover:no-underline",
        className,
      )}
      href={href}
      {...props}
    >
      {children}
    </Link>
  );
});
NavLink.displayName = "NavLink";
