using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShelter.Model.Core.Interfaces
{
    public interface IChangeable
    {
        // методы добавления и удаления животного из приюта
        Pet Add(Pet pet);
        Pet Remove(Pet pet);
    }
}
