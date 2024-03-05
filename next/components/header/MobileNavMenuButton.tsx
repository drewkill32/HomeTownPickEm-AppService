"use client";

import { Button } from "@/components/ui/button";
import { cn } from "@/utils/tailwind";
import { useEscape } from "@/hooks/useEscape";
import { NavItem } from "./navItems";
import { useEffect, useState } from "react";

interface MobileNavMenuButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  navigationItems: NavItem[];
}

export const MobileNavMenuButton = ({
  navigationItems,
  className,
  ...props
}: MobileNavMenuButtonProps) => {
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    if (isOpen) {
      document.body.style.overflow = "hidden";
    } else {
      document.body.style.overflow = "auto";
    }

    // Clean up function
    return () => {
      document.body.style.overflow = "auto";
    };
  }, [isOpen]);

  useEscape(() => setIsOpen(false));

  return (
    <>
      <Button
        size="icon"
        variant="outline"
        aria-label="Open Menu"
        onClick={() => setIsOpen(!isOpen)}
        {...props}
        className={cn(
          "z-50 inline-flex h-10 w-10 items-center  justify-center whitespace-nowrap rounded-md border border-input bg-background text-sm font-medium ring-offset-background transition-colors hover:bg-accent hover:text-accent-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50",
          className,
        )}
      >
        {isOpen ? (
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            className="h-6 w-6"
          >
            <line x1="18" y1="6" x2="6" y2="18"></line>
            <line x1="6" y1="6" x2="18" y2="18"></line>
          </svg>
        ) : (
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            stroke="currentColor"
            className="h-6 w-6"
          >
            <line x1="4" x2="20" y1="12" y2="12"></line>
            <line x1="4" x2="20" y1="6" y2="6"></line>
            <line x1="4" x2="20" y1="18" y2="18"></line>
          </svg>
        )}
      </Button>
      <MobileNavMenu navigationItems={navigationItems} open={isOpen} />
    </>
  );
};

type MobileNavMenuProps = {
  navigationItems: NavItem[];
  open: boolean;
};

const MobileNavMenu = ({ navigationItems, open }: MobileNavMenuProps) => {
  return (
    <nav
      className={cn(
        "fixed inset-0  transform border bg-white pt-16 transition-transform duration-300 ease-in-out",
        open ? "translate-x-0" : "translate-x-full",
      )}
    >
      <div className="flex-start flex h-full flex-col">
        {navigationItems.map((item) => (
          <>
            {item.divider && (
              <div className="mx-auto my-3 w-[90%] border-b-2 border-gray-400" />
            )}
            <a
              key={item.label}
              className={cn(
                "item  py-4 pl-2 hover:bg-slate-500 hover:text-white hover:no-underline",
                item.className || "",
              )}
              href={item.href}
            >
              {item.label}
            </a>
          </>
        ))}
      </div>
    </nav>
  );
};
