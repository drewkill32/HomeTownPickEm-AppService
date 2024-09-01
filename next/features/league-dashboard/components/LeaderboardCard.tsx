import { Avatar, AvatarImage, AvatarFallback } from "@/components/ui/avatar";
import { Card, CardHeader, CardTitle, CardContent } from "@/components/ui/card";
import React from "react";
import { cn } from "@/utils";

export type LeaderboardData = {
  id: string;
  rank: number;
  img?: string;
  name: string;
  points: number;
  behind: number;
};

type Props = {
  data: LeaderboardData[];
  className?: string;
};

export default function LeaderboardCard({ data, className }: Props) {
  return (
    <Card className={cn("bg-inherit", className)}>
      <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle className="text-center text-lg font-semibold">
          Leaderboard
        </CardTitle>
      </CardHeader>
      <CardContent className="p-0">
        <div className="grid w-full items-center gap-2 p-2">
          {data.map((item) => (
            <LeaderboardItem key={item.id} {...item} />
          ))}
        </div>
      </CardContent>
    </Card>
  );
}

const LeaderboardItem = ({
  id,
  rank,
  img,
  name,
  points,
  behind,
}: LeaderboardData) => {
  return (
    <div className="bg-lime flex items-center justify-stretch gap-2">
      <span className="font-semibold">{rank}.</span>
      <Avatar>
        <AvatarImage src={img} />
        <AvatarFallback>{getItitials(name)}</AvatarFallback>
      </Avatar>
      <p className="w-40 flex-grow truncate">{name}</p>
      <p className="pl-1 font-semibold">{points}</p>
    </div>
  );
};

const getItitials = (name: string) => {
  const initials = name.split(" ").map((word) => word.charAt(0).toUpperCase());
  if (initials.length > 1) {
    return `${initials[0]}${initials[initials.length - 1]}`;
  }
  return initials[0];
};
