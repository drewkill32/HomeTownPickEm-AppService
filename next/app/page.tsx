import { createClient } from "@/utils/supabase/server";
import Header from "@/components/header/Header";

export default async function Index() {
  const canInitSupabaseClient = () => {
    // This function is just for the interactive tutorial.
    // Feel free to remove it once you have Supabase connected.
    try {
      createClient();
      return true;
    } catch (e) {
      return false;
    }
  };

  const isSupabaseConnected = canInitSupabaseClient();

  return (
    <>
      <Header />
      <section className="bg-[#002244] py-20 text-center text-white">
        <h2 className="text-5xl font-bold">Pick Your Winning Teams</h2>
        <p className="mt-4 text-xl">
          Join the ultimate college football game-picking app.
        </p>
        <a
          className="mt-6 inline-flex rounded-md bg-[#d50a0a] px-6 py-3 text-sm font-medium text-white hover:bg-[#b80808]"
          href="/register"
        >
          Sign Up Now
        </a>
      </section>
      <section id="features" className="px-4 py-20 sm:px-6 lg:px-8">
        <h2 className="mb-12 text-center text-4xl font-bold">Features</h2>
        <div className="grid grid-cols-1 gap-8 md:grid-cols-2 lg:grid-cols-3">
          <div className="flex flex-col items-center text-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
              className="mb-4 h-16 w-16"
            >
              <path d="M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1z"></path>
              <line x1="4" x2="4" y1="22" y2="15"></line>
            </svg>
            <h3 className="mb-2 text-2xl font-bold">Pick Games</h3>
            <p>Choose your winning teams for each week's games.</p>
          </div>
          <div className="flex flex-col items-center text-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
              className="mb-4 h-16 w-16"
            >
              <path d="M6 9H4.5a2.5 2.5 0 0 1 0-5H6"></path>
              <path d="M18 9h1.5a2.5 2.5 0 0 0 0-5H18"></path>
              <path d="M4 22h16"></path>
              <path d="M10 14.66V17c0 .55-.47.98-.97 1.21C7.85 18.75 7 20.24 7 22"></path>
              <path d="M14 14.66V17c0 .55.47.98.97 1.21C16.15 18.75 17 20.24 17 22"></path>
              <path d="M18 2H6v7a6 6 0 0 0 12 0V2Z"></path>
            </svg>
            <h3 className="mb-2 text-2xl font-bold">Compete</h3>
            <p>Compete against friends and other users for the top spot.</p>
          </div>
          <div className="flex flex-col items-center text-center">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="24"
              height="24"
              viewBox="0 0 24 24"
              fill="none"
              stroke="currentColor"
              stroke-width="2"
              stroke-linecap="round"
              stroke-linejoin="round"
              className="mb-4 h-16 w-16"
            >
              <polygon points="12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2"></polygon>
            </svg>
            <h3 className="mb-2 text-2xl font-bold">Earn Points</h3>
            <p>Earn points for each correct pick and climb the leaderboard.</p>
          </div>
        </div>
      </section>
      <section
        id="testimonials"
        className="bg-[#f2f2f2] px-4 py-20 sm:px-6 lg:px-8"
      >
        <h2 className="mb-12 text-center text-4xl font-bold">
          What Users Are Saying
        </h2>
        <div className="grid grid-cols-1 gap-8 md:grid-cols-2">
          <div className="rounded-lg bg-white p-6 shadow">
            <p>
              "GamePicker has made watching college football even more exciting.
              I love competing against my friends each week!"
            </p>
            <p className="mt-4 font-bold">- User 1</p>
          </div>
          <div className="rounded-lg bg-white p-6 shadow">
            <p>
              "The app is easy to use and I love seeing how I rank against other
              users."
            </p>
            <p className="mt-4 font-bold">- User 2</p>
          </div>
        </div>
      </section>
      <section id="signup" className="px-4 py-20 sm:px-6 lg:px-8">
        <div className="mx-auto max-w-md text-center">
          <h2 className="mb-8 text-4xl font-bold">Ready to start picking?</h2>
          <a
            className="inline-flex rounded-md bg-[#d50a0a] px-6 py-3 text-sm font-medium text-white hover:bg-[#b80808]"
            href="/register"
          >
            Sign Up Now
          </a>
        </div>
      </section>
    </>
  );
}
