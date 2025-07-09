import React from "react";
import { Movie } from "../types/movieType";
import { Alert, AlertDescription, AlertTitle } from "./ui/Alert";
import { Film } from "lucide-react";
import { MovieCard } from "./MovieCard";

interface MovieListProps {
  movies: Movie[];
  isLoading?: boolean;
}

const MovieList: React.FC<MovieListProps> = ({ movies, isLoading }) => {
  if (isLoading) {
    return (
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
        {[...Array(8)].map((_, index) => (
          <CardSkeleton key={index} />
        ))}
      </div>
    );
  }

  if (movies.length == 0) {
    return (
      <Alert className="mt-8">
        <Film className="h-5 w-5" />
        <AlertTitle>No Movies Found</AlertTitle>
        <AlertDescription>
          Try adjusting your search or filter criteria, or check back later for
          new releases.
        </AlertDescription>
      </Alert>
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-6">
      {movies.map((movie) => (
        <MovieCard key={movie.id} movie={movie} />
      ))}
    </div>
  );
};

const CardSkeleton = () => (
  <div className="flex flex-col space-y-3 p-4 border rounded-lg shadow-md bg-card">
    <div className="relative w-full h-64 md:h-72 bg-muted rounded-md animate-pulse"></div>
    <div className="space-y-2">
      <div className="h-6 bg-muted rounded w-3/4 animate-pulse"></div>
      <div className="h-4 bg-muted rounded w-1/2 animate-pulse"></div>
    </div>
    <div className="h-10 bg-muted rounded w-full animate-pulse"></div>
    <div className="h-4 bg-muted rounded w-1/3 animate-pulse"></div>
    <div className="h-8 bg-muted rounded w-full animate-pulse"></div>
  </div>
);
export default MovieList;
