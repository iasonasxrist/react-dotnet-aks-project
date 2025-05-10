import { useEffect, useState } from "react";
import "./App.css";
import { Route, Routes } from "react-router-dom";
import Home from "./pages/Home";
import { Movie } from "./types/movieType";
import MovieForm from "./pages/MovieForm";

function App() {
  const [movies, setMovies] = useState<Movie[]>([]);

  // useEffect(() => {
  //   fetchMovies().then((res) => setMovies(res.data));
  //     .catch((err) => console.error("Failed to fetch movies", err));
  // },[]);
  
  const addMovie = (movie: Movie) => setMovies([...movies, movie]);

  const updateMovie = (updated: Movie) => {
    setMovies(movies.map((m: Movie) => (m.id === updated.id ? updated : m)));
  };

  const deleteMovie = (movie: Movie) =>
    setMovies(movies.filter((m) => m.id !== movie.id));

  return (
    <Routes>
      <Route
        path="/"
        element={<Home movies={movies} deleteMovie={deleteMovie} />}
      />
      <Route
        path="/add"
        element={<MovieForm onSave={addMovie} movies={movies} />}
      />

      <Route
        path="/edit/:id"
        element={<MovieForm movies={movies} onSave={updateMovie} />}
      />
    </Routes>
  );
}

export default App;
