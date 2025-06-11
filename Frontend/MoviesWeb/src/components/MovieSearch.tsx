import { FormEvent, useEffect, useState } from "react";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "../components/ui/Card";
import { Button } from "./ui/Button";
import { Input } from "./ui/Input";
import { Search } from "lucide-react";

interface MovieSearchProps {
  onSearch: (query: string) => void;
  initialQuery?: string;
}
const MovieSearch: React.FC<MovieSearchProps> = ({
  onSearch,
  initialQuery = "",
}) => {
  const [query, setQuery] = useState(initialQuery);

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    onSearch(query);
  };

  // Optional: Debounce search if you want to search as user types
  useEffect(() => {
    const timerId = setTimeout(() => {
      onSearch(query);
    }, 500); // 500ms delay
    return () => clearTimeout(timerId);
  }, [query, onSearch]);

  return (
    <Card className="shadow-md col-span-1 md:col-span-2">
      <CardHeader className="pb-4">
        <CardTitle className="text-lg flex items-center gap-2">
          <Search size={20} className="text-accent" />
          Search Movies
        </CardTitle>
      </CardHeader>
      <CardContent>
        <form onSubmit={handleSubmit} className="flex items-center gap-2">
          <Input
            type="text"
            placeholder="Search by title, actor, director..."
            value={query}
            onChange={(e) => setQuery(e.target.value)}
            className="flex-grow"
            aria-label="Search movies"
          />
          <Button type="submit" aria-label="Submit search">
            <Search size={18} />
          </Button>
        </form>
      </CardContent>
    </Card>
  );
};

export default MovieSearch;
