import React, { ComponentProps } from "react";
import Logo from "../icons/Logo";
import { buttonVariants } from "../ui/button";
import { MobileNavMenuButton } from "./MobileNavMenuButton";
import AccountNavMenu from "./AccountNavMenu";
import { getUser } from "@/server/user";
import { cn } from "@/utils/tailwind";
import { getAccountNavItems, getProtectedRoutes, navItems } from "./navItems";
import Link from "next/link";

interface HeaderProps extends ComponentProps<"header"> {}

export default async function Header({ className, ...rest }: HeaderProps) {
  const { isAuthenticated, user } = await getUser();
  console.log("isAuthenticated", isAuthenticated);
  const protectedRoutes = getProtectedRoutes(user);
  const acountNavItems = getAccountNavItems(user);

  const allNavItems = [...navItems, ...protectedRoutes];
  return (
    <header
      className={cn("bg-white px-4 sm:px-6 lg:px-8", className)}
      {...rest}
    >
      <div className="flex h-16 items-center justify-evenly">
        <div className="flex grow items-center gap-5 pl-2">
          <Logo className="text-4xl text-emerald-700" width={55} height={65} />
          <h1 className="font-bold text-[#002244] sm:text-2xl">
            St. Pete Pick'em
          </h1>
        </div>
        <MobileNavMenuButton
          navigationItems={[...allNavItems, ...acountNavItems]}
          className="order-2 ml-2 mr-2 md:hidden"
        />

        <nav className="hidden justify-self-end md:block md:shrink md:pr-3">
          <ul className="hidden items-center space-x-4 md:flex">
            {allNavItems.map((item) => (
              <li key={item.label}>
                <a className="transition hover:text-[#d50a0a]" href={item.href}>
                  {item.label}
                </a>
              </li>
            ))}
          </ul>
        </nav>
        {isAuthenticated ? (
          <AccountNavMenu
            menuItems={acountNavItems}
            className="hidden md:block"
          ></AccountNavMenu>
        ) : (
          <Link
            className={cn(buttonVariants(), "justify-self-start")}
            href="/login"
          >
            Log In
          </Link>
        )}
      </div>
    </header>
  );
}
