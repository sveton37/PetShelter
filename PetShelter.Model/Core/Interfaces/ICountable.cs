using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.Model.Core.Interfaces
{
     public interface ICountable
    {
        int Count();
        int Count(Type pet);
        int Percentage(Type pet);
    }
}
