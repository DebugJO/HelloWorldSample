using System.Collections.Generic;
using FirstWPF.Models;

namespace FirstWPF.Services
{
    public interface IPersonServices
    {
        List<PersonModel> AddItmes();
        List<PersonModel> ClearItmes();
    }
}