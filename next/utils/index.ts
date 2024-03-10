import { Metadata } from "next";

export * from "./tailwind";

export const metadataTitle = (title: string) => {
  return {
    title: `St. Pete Pick'em | ${title}`,
  } satisfies Metadata;
};

//get the suffex of a number 1st 2nd 3rd 4th 12th
export const getNumberSuffix = (num: number) => {
  const j = num % 10;
  const k = num % 100;
  if (j === 1 && k !== 11) {
    return num + "st";
  }
  if (j === 2 && k !== 12) {
    return num + "nd";
  }
  if (j === 3 && k !== 13) {
    return num + "rd";
  }
  return num + "th";
};
