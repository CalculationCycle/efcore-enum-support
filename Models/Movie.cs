using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEFCoreEnum.Models
{
    class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public Genres Genres { get; set; }

        public Movie()
        {
        }

        public Movie(string title, int year, Genres genres)
        {
            Title = title;
            Year = year;
            Genres = genres;
        }
    }
}
