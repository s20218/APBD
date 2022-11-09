using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tutorial4_webApp.Models;

namespace Tutorial4_webApp.Services
{ 
        public interface IDatabaseService
        {
            int AddAnimal(Animal newAnimal);
            int DeleteAnimal(int idAnimal);

            int ModifyAnimal(int idAnimal, Animal updAnimal);
            IEnumerable<Animal> GetAnimals(string orderBy);
        }
}
