import { Metadata } from "next";

export * from "./tailwind";

export const metadataTitle = (title: string) => {
  return {
    title: `St. Pete Pick'em | ${title}`,
  } satisfies Metadata;
};
