import { useEffect } from "react";

export const useEscape = (excapeFunc: () => void) => {
  const handleEscape = (e: KeyboardEvent) => {
    if (e.key === "Esc" || e.key === "Escape") {
      excapeFunc();
    }
  };
  useEffect(() => {
    document.addEventListener("keydown", handleEscape);

    return () => {
      document.removeEventListener("keydown", handleEscape);
    };
  }, []);
};
