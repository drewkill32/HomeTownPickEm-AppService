import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { getNumberSuffix } from "@/utils";
import Image from "next/image";
import Link from "next/link";
import roundLogo from "@/assets/round-logo.png";
import WeekProgress from "@/features/league-dashboard/components/WeekProgress";
import LeaderboardCard, {
  LeaderboardData,
} from "@/features/league-dashboard/components/LeaderboardCard";

type Props = {
  params: { slug: string };
};

export default function Page({ params: { slug } }: Props) {
  const dashboardData = {
    leaugeName: "St Pete Bois",
    leaugeImage: "/placeholder.svg",
    leaugeRank: 3,
    weeksRemainng: 2,
    currentWeek: 2,
    firstGameStart: new Date("2022-08-28T00:00:00Z"),
    weeks: [
      {
        week: 1,
        date: "Aug 28",
        picked: 5,
        totalPicks: 20,
      },
      {
        week: 2,
        date: "Sep 4",
        picked: 10,
        totalPicks: 20,
      },
      {
        week: 3,
        date: "Sep 11",
        picked: 15,
        totalPicks: 20,
      },
      {
        week: 4,
        date: "Sep 18",
        picked: 20,
        totalPicks: 20,
      },
    ],
  };

  const leaderboardData = [
    {
      rank: 1,
      id: "1",
      img: "https://api.dicebear.com/7.x/bottts/svg?seed=John-Doe",
      name: "John Doe",
      points: 100,
      behind: 0,
    },
    {
      rank: 2,
      id: "3",
      name: "Jane Doe",
      points: 98,
      behind: 2,
    },
    {
      rank: 3,
      id: "4",
      img: "https://api.dicebear.com/7.x/bottts/svg?seed=John-Smith",
      name: "John Smith",
      points: 94,
      behind: 6,
    },
    {
      rank: 4,
      id: "5",
      img: "https://api.dicebear.com/7.x/bottts/svg?seed=Jane-Smith",
      name: "Jane Smith",
      points: 93,
      behind: 7,
    },
    {
      rank: 5,
      id: "6",
      img: "https://api.dicebear.com/7.x/bottts/svg?seed=Luke-Skywalker",
      name: "Luke Skywalker",
      points: 90,
      behind: 10,
    },
    {
      rank: 6,
      id: "7",
      img: "https://api.dicebear.com/7.x/bottts/svg?seed=Leia-Skywalker",
      name: "Leia Skywalker",
      points: 88,
      behind: 12,
    },
    {
      rank: 7,
      id: "8",
      img: "https://api.dicebear.com/7.x/bottts/svg?seed=johnny-longname",
      name: "Johnny Longname Has a really long name like really long so long you don't event know",
      points: 80,
      behind: 20,
    },
    {
      rank: 7,
      id: "9",
      img: "https://api.dicebear.com/7.x/bottts/svg?seed=johnny-short",
      name: "Johnny Short",
      points: 79,
      behind: 21,
    },
  ] as LeaderboardData[];

  return (
    <main className="grid gap-4 md:gap-8 lg:gap-12 xl:gap-16">
      <Card className="p-0">
        <CardContent className="p-0">
          <div className="grid w-full grid-cols-1 items-stretch md:grid-cols-4">
            <div className="flex flex-col items-center justify-center p-4 text-center md:items-start md:justify-start md:gap-2 md:p-6 lg:gap-4">
              <h1 className="pb-2 text-3xl font-bold tracking-tighter">
                {dashboardData.leaugeName}
              </h1>
              <div className="w-full rounded-md border py-3 text-sm text-gray-500 dark:text-gray-400">
                <p className="text-lg font-semibold text-primary">
                  Week {dashboardData.currentWeek}
                </p>
                <p>
                  You are {getNumberSuffix(dashboardData.leaugeRank)} in this
                  league
                </p>

                <p>
                  {dashboardData.weeksRemainng} week
                  {dashboardData.weeksRemainng > 1 && "s"} remaining
                </p>
                <p>
                  {`First game at ${dashboardData.firstGameStart.toLocaleDateString(
                    "en-us",
                    {
                      weekday: "short",
                      month: "short",
                    },
                  )} ${getNumberSuffix(dashboardData.firstGameStart.getDate())}
                  ${dashboardData.firstGameStart.toLocaleTimeString("en-us", {
                    hour: "numeric",
                    minute: "2-digit",
                  })}`}
                </p>
              </div>
              <div className="mx-auto flex w-full items-center justify-center p-4 md:p-6">
                <Image
                  alt="League image"
                  className="rounded-full object-cover"
                  src={roundLogo}
                  style={{
                    aspectRatio: "200/200",
                    objectFit: "cover",
                  }}
                  width={200}
                />
              </div>
            </div>

            <div className="grid w-full grid-cols-1 items-stretch gap-1 p-4 text-center md:grid-cols-2 md:gap-2 md:p-6 lg:gap-4">
              {dashboardData.weeks.map((week) => (
                <WeekProgress key={week.week} {...week} />
              ))}
            </div>
          </div>
        </CardContent>
      </Card>
      <LeaderboardCard data={leaderboardData} />
    </main>
  );
}
