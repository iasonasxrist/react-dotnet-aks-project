import { Movie } from "../types/movieType";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "./ui/Card";
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
  AlertDialogTrigger,
} from "../components/ui/Alert-Dialog";
import { Badge, CalendarDays, Star } from "lucide-react";

interface MovieCardProps {
  movie: Movie;
}

export const MovieCard: React.FC<MovieCardProps> = ({ movie }) => {
  const releaseDateObj = new Date(movie.releaseDate);
  const isUpcomingMovie = movie.isUpcoming || releaseDateObj > new Date();
  return (
    <Card className="flex flex-col overflow-hidden shadow-lg hover:shadow-primary/20 transition-shadow duration-300 h-full">
      <div className="relative w-full h-64 md:h-72">
        <img
          src={movie.posterUrl}
          alt={`Poster for ${movie.title}`}
          data-ai-hint={`${movie.genre.toLowerCase()} movie poster`}
          className="transition-transform duration-300 group-hover:scale-105"
        />
      </div>
      <CardHeader className="pb-2">
        <CardTitle className="text-xl leading-tight">{movie.title}</CardTitle>
        <CardDescription className="flex items-center gap-2 text-sm pt-1">
          <Badge variant="secondary">{movie.genre}</Badge>
          {movie.rating > 0 && (
            <span className="flex items-center gap-1">
              <Star size={16} className="text-yellow-400" />
              {movie.rating.toFixed(1)}/10
            </span>
          )}
        </CardDescription>
      </CardHeader>
      <CardContent className="flex-grow pb-3">
        <p className="text-sm text-muted-foreground line-clamp-3">
          {movie.synopsis}
        </p>
      </CardContent>
      <CardFooter className="flex flex-col items-start gap-3 pt-0">
        <div className="flex items-center gap-2 text-sm text-muted-foreground">
          <CalendarDays size={16} className="text-accent" />
          <span>
            {new Date(movie.releaseDate).toLocaleDateString("en-US", {
              year: "numeric",
              month: "long",
              day: "numeric",
            })}
          </span>
        </div>
        {/* {isUpcomingMovie && <Countdown targetDate={movie.releaseDate} />} */}

        <AlertDialog>
          <AlertDialogTrigger asChild>
            {/* <Button variant="outline" size="sm" className="w-full mt-2" onClick={handleGetEmotionalSynopsis} disabled={isLoadingSynopsis}>
              {isLoadingSynopsis ? <Skeleton className="h-4 w-3/4" /> : <><Wand2 size={16} /> Get Emotional Synopsis</>}
            </Button> */}
          </AlertDialogTrigger>
          <AlertDialogContent>
            <AlertDialogHeader>
              <AlertDialogTitle>
                Emotional Synopsis for {movie.title}
              </AlertDialogTitle>
              <AlertDialogDescription>
                {/* {isLoadingSynopsis && <div className="space-y-2 mt-2"><Skeleton className="h-4 w-full" /><Skeleton className="h-4 w-5/6" /></div>}
                {emotionalSynopsis && emotionalSynopsis.emotionalSynopsis}
                {!isLoadingSynopsis && !emotionalSynopsis && "Click 'Generate' or there was an error."} */}
              </AlertDialogDescription>
            </AlertDialogHeader>
            <AlertDialogFooter>
              <AlertDialogCancel>Close</AlertDialogCancel>
              {/* {!emotionalSynopsis && !isLoadingSynopsis && (
                 <AlertDialogAction onClick={handleGetEmotionalSynopsis}>Generate</AlertDialogAction>
              )} */}
            </AlertDialogFooter>
          </AlertDialogContent>
        </AlertDialog>
      </CardFooter>
    </Card>
  );
};
