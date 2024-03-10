import React from "react";

type Props = {
  week: number;
  date: string;
  picked: number;
  totalPicks: number;
};

export default function WeekProgress({
  week,
  date,
  picked,
  totalPicks,
}: Props) {
  return (
    <div className="flex flex-col items-center justify-center gap-1 md:items-start md:justify-start md:gap-0">
      <h3 className="text-lg font-bold">Week {week}</h3>
      <p className="text-sm text-gray-500 dark:text-gray-400">{date}</p>
      <div className="relative h-2 w-full overflow-hidden rounded-full bg-gray-200">
        <div
          className="h-full bg-blue-500"
          style={{
            width: `${(picked / totalPicks) * 100}%`,
          }}
        />
      </div>
    </div>
  );
}
