using System.Collections.Generic;
using FirstWPF.Models;

namespace FirstWPF.Services
{
    public class PersonServices : IPersonServices
    {
        public List<PersonModel> AddItmes()
        {
            return new List<PersonModel>
            {
                new() { Id = "111", Name = "홍길동", Address = "가나닭" },
                new() { Id = "222", Name = "홍길서", Address = "마바삵" },
                new() { Id = "333", Name = "홍길남", Address = "아차탉" }
            };
        }

        public List<PersonModel> ClearItmes()
        {
            return new List<PersonModel>
            {
                new() { Id = "", Name = "", Address = "" },
                new() { Id = "", Name = "", Address = "" },
                new() { Id = "", Name = "", Address = "" }
            };
        }
    }
}