using LibApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibApp.Repositories.Interfaces
{
    public interface IGenreRepository
    {
        IEnumerable<Genre> GetAll();
    }
}
