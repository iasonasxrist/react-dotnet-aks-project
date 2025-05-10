import { Movie } from "../types/movieType";

function Home({
  movies,
  deleteMovie,
}: {
  movies: Movie[];
  deleteMovie: (movie: Movie) => void;
}) {
  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4">Movie List</h1>
      <a href="/add" className="text-blue-600 underline">
        Add New Movie
      </a>
      <ul className="space-y-4 mt-6">
        {movies.map((movie) => (
          <li key={movie.id} className="border p-4 rounded shadow">
            <h2 className="text-lg font-semibold">
              {movie.title} ({movie.yearOfRelease})
            </h2>
            <p>
              Slug: <span className="text-sm text-gray-600">{movie.slug}</span>
            </p>
            <p>Genres: {movie.genres.join(", ")}</p>
            <div className="space-x-2 mt-2">
              <a href={`/edit/${movie.id}`} className="text-sm text-blue-500">
                Edit
              </a>
              <button
                onClick={(movie) => deleteMovie(movie?.id)}
                className="text-sm text-red-500"
              >
                Delete
              </button>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default Home;
