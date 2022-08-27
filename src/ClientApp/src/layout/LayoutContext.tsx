import React, { createContext, useContext } from 'react';

interface ContextProps {
  paddingBottom: string | number;
  setPaddingBottom: (value: string | number) => void;
}
const LayoutContext = createContext<ContextProps>({
  paddingBottom: 0,
  setPaddingBottom: () => {},
});

interface LayoutContextProviderProps extends ContextProps {
  children: React.ReactNode;
}

export const useLayout = () => {
  return useContext(LayoutContext);
};

export function LayoutContextProvider({
  children,
  paddingBottom,
  setPaddingBottom,
}: LayoutContextProviderProps) {
  return (
    <LayoutContext.Provider value={{ paddingBottom, setPaddingBottom }}>
      {children}
    </LayoutContext.Provider>
  );
}
