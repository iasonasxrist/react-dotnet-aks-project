import MovieSearch from "../components/MovieSearch";
import MovieList from "../components/MovieList";
import { Bot, Clapperboard } from "lucide-react";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "../components/ui/Card";
import { useState } from "react";

export function Home() {
  const [searchQuery, setSearchQuery] = useState("");

  const handleSearch = (query: string) => setSearchQuery(query);

  return (
    <div className="container mx-auto p-4 md:p-8 space-y-12">
      <section
        id="hero"
        className="text-center py-8 rounded-lg bg-gradient-to-br from-primary/20 via-background to-background"
      >
        <h1 className="text-4xl md:text-5xl font-extrabold mb-3 tracking-tight">
          Welcome to <span className="text-primary">CineScope</span>
        </h1>
        <p className="text-lg md:text-xl text-foreground/80 max-w-2xl mx-auto">
          Your ultimate destination to discover, explore, and get personalized
          recommendations for movies.
        </p>
      </section>

      <section
        id="search-filters"
        className="grid grid-cols-1 lg:grid-cols-3 gap-6 items-start"
      >
        <div className="lg:col-span-2">
          <MovieSearch onSearch={handleSearch} initialQuery={searchQuery} />
        </div>
        {/* <MovieFilters genres={uniqueGenres} onFilterChange={handleFilterChange} initialFilters={activeFilters} /> */}
      </section>

      {/* <Separator />
       */}
      <section id="movie-feed">
        <div className="flex items-center gap-3 mb-6">
          <Clapperboard size={32} className="text-primary" />
          <h2 className="text-3xl font-semibold">Movie Showcase</h2>
        </div>
        <MovieList movies={[]} isLoading={false} />
      </section>

      <section id="ai-tools">
        <div className="flex items-center gap-3 mb-6">
          <Bot size={32} className="text-primary" />
          <h2 className="text-3xl font-semibold">AI Powered Insights</h2>
        </div>
        <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
          <Card className="shadow-lg hover:shadow-primary/20 transition-shadow duration-300">
            <CardHeader>
              <CardTitle className="text-xl flex items-center gap-2">
                <Bot size={24} className="text-accent" />
                AI Movie Recommendations
              </CardTitle>
            </CardHeader>
            <CardContent>{/* <Recommendations /> */}</CardContent>
          </Card>
          <Card className="shadow-lg hover:shadow-primary/20 transition-shadow duration-300">
            <CardHeader>
              <CardTitle className="text-xl flex items-center gap-2">
                <Bot size={24} className="text-accent" />
                Emotional Synopsis Generator
              </CardTitle>
            </CardHeader>
            <CardContent>{/* <EmotionalTool /> */}</CardContent>
          </Card>
        </div>
      </section>
    </div>
  );
}

export default Home;
