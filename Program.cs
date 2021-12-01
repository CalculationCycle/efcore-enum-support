using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleAppEFCoreEnum.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleAppEFCoreEnum
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                 .AddJsonFile($"appsettings.json", true, true);

            var config = builder.Build();
            var connectionString = config["ConnectionStrings:SQL"];

            using (var _context = new MainContext(connectionString))
            {
                _context.Database.Migrate();

                AddMovies(_context);

                PrintAllMovieDetails(_context);

                GetAndFilteredMovies(_context, Genres.Comedy | Genres.Drama);

                GetOrFilteredMovies(_context, Genres.Comedy | Genres.Drama);
            }
        }

        private static void AddMovies(MainContext context)
        {
            //context.Movies.Add(new Movie("Jaws", 1975, Genres.Horror | Genres.Documentary | Genres.Comedy));
            //context.Movies.Add(new Movie("Jaws 2", 1978, Genres.Horror | Genres.Documentary));
            //context.Movies.Add(new Movie("Highlander", 1986, Genres.Animated | Genres.Musical));
            //context.Movies.Add(new Movie("Highlander", 1986, (Genres)3));

            int addedMovieCount = 0;
            for (int i = 0; i < 100; i++)
            {
                var title = $"Fast&Furious {i}";
                var movie = context.Movies.FirstOrDefault(m => m.Title == title);
                if (movie is null)
                { 
                    context.Movies.Add(new Movie(title, 1990+i, (Genres)i));
                    addedMovieCount++;
                }
            }

            context.SaveChanges();

            Console.WriteLine($"Added {addedMovieCount} movies");
            Console.WriteLine("");
        }

        private static void PrintAllMovieDetails(MainContext context)
        {
            var movieList = context.Movies
                .OrderBy(m => m.Year)
                .ToList();

            Console.WriteLine($"-- ALL MOVIES ({movieList.Count} movies) --");
            foreach (var movie in movieList)
            {
                OutputMovieInfo(movie, (Genres)0, FilterType.None);
            }
            Console.WriteLine("");
        }

        private static void GetAndFilteredMovies(MainContext context, Genres filterGenres)
        {
            var movieList = context.Movies
                .Where(m => m.Genres.HasFlag(filterGenres))
                .OrderBy(m => m.Year)
                .ToList();

            Console.WriteLine($"-- MOVIES AND-FILTERED BY GENRE [{filterGenres}] ({movieList.Count} movies) --");

            foreach (var movie in movieList)
            {
                OutputMovieInfo(movie, filterGenres, FilterType.And);
            }
        }

        private static void GetOrFilteredMovies(MainContext context, Genres filterGenres)
        {
            var query = context.Movies.AsQueryable();

            query = query.Where(m => ((int)m.Genres & (int)filterGenres) != 0);

            var movieList = query
                .OrderBy(m => m.Year)
                .ToList();

            Console.WriteLine($"-- MOVIES OR-FILTERED BY GENRE [{filterGenres}] ({movieList.Count} movies) --");

            foreach (var movie in movieList)
            {
                OutputMovieInfo(movie, filterGenres, FilterType.Or);
            }
        }

        private static void OutputMovieInfo(Movie movie, Genres filterGenres, FilterType filterType)
        {

            Console.Write($"Id: {movie.Id,-4} Title: {movie.Title,-20} Year: {movie.Year}  GenresInt: {(int)movie.Genres,-5} Genres: ");
            if ((int)filterGenres == 0)
            { 
                Console.WriteLine($"{movie.Genres}");
            }
            else
            {
                string movieGenresString = movie.Genres.ToString();
                List<ColoredString> colorStringList = new();
                var filterFlagList = Enum.GetValues(filterGenres.GetType()).Cast<Enum>().Where(filterGenres.HasFlag).ToList();
                foreach (var f in filterFlagList)
                {
                    var filterName = f.ToString();
                    var indexOfFilterName = movieGenresString.IndexOf(filterName);
                    if (indexOfFilterName != -1)
                    {
                        colorStringList.Add(new ColoredString()
                        {
                            StartPos = indexOfFilterName,
                            Text = filterName
                        });
                    }
                }

                colorStringList.Sort((a, b) => a.StartPos - b.StartPos);

                //Set colors
                if (filterFlagList.Count == colorStringList.Count)
                    colorStringList.ForEach(cs => cs.TextColor = ConsoleColor.Green);
                else
                    colorStringList.ForEach(cs => cs.TextColor = ConsoleColor.Yellow);

                //Output
                int currPos = 0;
                foreach (var colorString in colorStringList)
                {
                    if (currPos < colorString.StartPos)
                    { 
                        Console.Write(movieGenresString.Substring(currPos, colorString.StartPos-currPos));
                    }
                    Console.ForegroundColor = colorString.TextColor;
                    Console.Write(colorString.Text);
                    Console.ResetColor();

                    currPos = colorString.StartPos + colorString.Text.Length;
                }

                if (currPos < movieGenresString.Length)
                {
                    Console.Write(movieGenresString.Substring(currPos, movieGenresString.Length - currPos));
                }

                Console.WriteLine("");
            }
        }
    }
}
