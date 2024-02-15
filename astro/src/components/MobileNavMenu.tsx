import { Button } from "./ui/button";
import { cn } from "@/lib/utils";
import useLocalStorage from "@/hooks/useLocalStorage";

export type NavtigationItem = {
  label: string;
  href: string;
};

interface MobileNavMenuButtonProps
  extends React.ButtonHTMLAttributes<HTMLButtonElement> {
  navigationItems: NavtigationItem[];
}

export const MobileNavMenuButton = ({
  navigationItems,
  className,
  ...props
}: MobileNavMenuButtonProps) => {
  const [isOpen, setIsOpen] = useLocalStorage("drawerOpen", false);

  return (
    <>
      <Button
        size="icon"
        variant="outline"
        aria-label="Open Menu"
        onClick={() => setIsOpen(!isOpen)}
        {...props}
        className={cn(
          "inline-flex h-10 w-10 items-center justify-center whitespace-nowrap rounded-md border border-input bg-background text-sm font-medium ring-offset-background transition-colors hover:bg-accent hover:text-accent-foreground focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring focus-visible:ring-offset-2 disabled:pointer-events-none disabled:opacity-50",
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
  navigationItems: NavtigationItem[];
  open: boolean;
};

const MobileNavMenu = ({ navigationItems, open }: MobileNavMenuProps) => {
  return (
    <nav
      className={cn(
        "fixed inset-y-0 left-0 w-44 transform border bg-white pt-16 transition-transform duration-200 ease-in-out",
        open ? "translate-x-0" : "-translate-x-full",
      )}
    >
      <ul className="flex flex-col">
        {navigationItems.map((item) => (
          <li
            key={item.label}
            className="py-4 pl-2 hover:bg-slate-500 hover:text-white"
          >
            <a href={item.href}>{item.label}</a>
          </li>
        ))}
      </ul>
    </nav>
  );
};
