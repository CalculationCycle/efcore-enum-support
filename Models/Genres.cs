using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEFCoreEnum.Models
{
    [Flags]
    public enum Genres
    {
        None = 0,
        Horror = 1,
        Drama = 2,
        Thriller = 4,
        Documentary = 8,
        Comedy = 16,
        HiSchool = 32,
        Animated = 64,
        Musical = 128
    }
}
