import { ChangeEvent, FormEvent, useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { Movie } from "../types/movieType";

type Props = {
  onSave: (movie: Movie) => void;
  movies: Movie[];
};

const MovieForm = ({ onSave, movies = [] }: Props) => {
  const { id } = useParams();
  const navigate = useNavigate();
  const editing = Boolean(id);

  const [form, setForm] = useState<Movie>({
    id: "",
    title: "",
    slug: "",
    yearOfRelease: "",
    genres: String[],
  });

  useEffect(() => {
    if (editing) {
      const movie = movies.find((m: Movie) => m.id === id);
      if (movie) {
        setForm({
          id: movie.id,
          title: movie.title,
          slug: movie.slug,
          yearOfRelease: movie.yearOfRelease.toString(),
          genres: movie.genres.join(", "),
        });
      }
    }
  }, [id, movies, editing]);

  const handleChangeInput = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    const payload = {
      ...form,
      yearOfRelease: Number(form.yearOfRelease),
      genres: form.genres.map((g) => g.split(", ")),
    };
    onSave(payload as unknown as Movie);
    navigate("/");
  };
  return (
    <form onSubmit={handleSubmit} className="max-w-md mx-auto space-y-4">
      <h2 className="text-xl font-bold">{editing ? "Edit" : "Add"} Movie</h2>
      <input
        className="w-full  border p-2"
        name="title"
        placeholder="Title"
        value={form.title}
        onChange={handleChangeInput}
        required
      />
      <input
        className="w-full border p-2"
        name="slug"
        placeholder="Slug (e.g. the-matrix)"
        value={form.slug}
        onChange={handleChangeInput}
        required
      />
      <input
        className="w-full border p-2"
        name="yearOfRelease"
        type="number"
        placeholder="Year of Release"
        value={form.yearOfRelease}
        onChange={handleChangeInput}
        required
      />
      <input
        className="w-full border p-2"
        name="genres"
        placeholder="Genres (comma separated)"
        value={form.genres}
        onChange={handleChangeInput}
        required
      />
      <button
        className="bg-blue-600 text-white px-4 py-2 rounded"
        type="submit"
      >
        Save
      </button>
    </form>
  );
};

export default MovieForm;
