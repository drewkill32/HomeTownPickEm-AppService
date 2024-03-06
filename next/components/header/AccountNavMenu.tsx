"use client";

import { cn } from "@/utils/tailwind";
import { NavItem } from "./navItems";
import {
  NavigationMenu,
  NavigationMenuContent,
  NavigationMenuItem,
  NavigationMenuLink,
  NavigationMenuList,
  NavigationMenuTrigger,
  NavigationMenuViewport,
} from "@/components/ui/navigation-menu";
import Link from "next/link";
import React from "react";
import Account from "../icons/Account";

export interface NavMenuProps {
  className?: string;
  menuItems: NavItem[];
}

export default function AccountNavMenu({ menuItems, className }: NavMenuProps) {
  console.log("menuItems", menuItems);
  return (
    <NavigationMenu className={className}>
      <NavigationMenuList>
        <NavigationMenuItem>
          <NavigationMenuTrigger>
            <Account className="text-2xl" />
          </NavigationMenuTrigger>
          <NavigationMenuContent>
            <ul className="flex w-[400px] flex-col gap-3 p-4 pr-5">
              {menuItems.map((menuItem) => (
                <ListItem key={menuItem.label} href={menuItem.href}>
                  {menuItem.label}
                </ListItem>
              ))}
            </ul>
          </NavigationMenuContent>
        </NavigationMenuItem>
      </NavigationMenuList>
      <NavigationMenuViewport />
    </NavigationMenu>
  );
}

const ListItem = React.forwardRef<
  React.ElementRef<typeof Link>,
  React.ComponentPropsWithoutRef<typeof Link>
>(({ className, children, ...props }, ref) => {
  return (
    <li>
      <NavigationMenuLink asChild>
        <Link
          ref={ref}
          className={cn(
            "block select-none space-y-1 rounded-md p-3 leading-none no-underline outline-none transition-colors hover:bg-accent hover:text-accent-foreground focus:bg-accent focus:text-accent-foreground",
            className,
          )}
          {...props}
        >
          <div className="text-sm font-medium leading-none">{children}</div>
        </Link>
      </NavigationMenuLink>
    </li>
  );
});
ListItem.displayName = "ListItem";
