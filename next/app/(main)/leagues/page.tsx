import { Metadata } from "next";
import { metadataTitle } from "@/utils";
import { redirect } from "next/navigation";

export const metadata: Metadata = metadataTitle("Dashboard");
type Props = {};

export default function Page({}: Props) {
  return redirect("/leagues/st");
}
