namespace AnimalCentre.Core
{
    using System;
    using Models.Factories;
    using Models;
    using Models.Contracts;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Models.Procedures;

    public class AnimalCentre
    {
        private Hotel hotel;
        private AnimalFactory animalFactory;
        private ProcedureFactory procedureFactory;
        private List<IAnimal> animals;
        private List<IProcedure> procedures;

        public AnimalCentre()
        {
            this.hotel = new Hotel();
            this.animalFactory = new AnimalFactory();
            this.procedureFactory = new ProcedureFactory();
            this.animals = new List<IAnimal>();
            this.procedures = new List<IProcedure>
            { new Chip(), new DentalCare(), new Fitness(), new NailTrim(), new Play(), new Vaccinate() };
        }

        public string RegisterAnimal(string type, string name, int energy, int happiness, int procedureTime)
        {
            IAnimal animal = this.animalFactory.Create(type, name, energy, happiness, procedureTime);

            this.hotel.Accommodate(animal);
            this.animals.Add(animal);
            return $"Animal {name} registered successfully";
        }

        public string Chip(string name, int procedureTime)
        {
            CheckforAnimal(name, this.hotel);
            IAnimal animal = hotel.Animals.FirstOrDefault(x => x.Key == name).Value;

            DoProcedure(name, procedureTime, this.procedureFactory, this.animalFactory, nameof(Chip), this.hotel, this.procedures);
            
            return $"{name} had chip procedure";
        }

        public string Vaccinate(string name, int procedureTime)
        {
            CheckforAnimal(name, this.hotel);
            DoProcedure(name, procedureTime, this.procedureFactory, this.animalFactory, nameof(Vaccinate), this.hotel, this.procedures);
            return $"{name} had vaccination procedure";
        }

        public string Fitness(string name, int procedureTime)
        {
            CheckforAnimal(name, this.hotel);
            DoProcedure(name, procedureTime, this.procedureFactory, this.animalFactory, nameof(Fitness), this.hotel, this.procedures);
            return $"{name} had fitness procedure";
        }

        public string Play(string name, int procedureTime)
        {
            CheckforAnimal(name, this.hotel);
            DoProcedure(name, procedureTime, this.procedureFactory, this.animalFactory, nameof(Play), this.hotel, this.procedures);
            return $"{name} was playing for {procedureTime} hours";
        }

        public string DentalCare(string name, int procedureTime)
        {
            CheckforAnimal(name, this.hotel);
            DoProcedure(name, procedureTime, this.procedureFactory, this.animalFactory, nameof(DentalCare), this.hotel, this.procedures);
            return $"{name} had dental care procedure";
        }

        public string NailTrim(string name, int procedureTime)
        {
            CheckforAnimal(name, this.hotel);
            DoProcedure(name, procedureTime, this.procedureFactory, this.animalFactory, nameof(NailTrim), this.hotel, this.procedures);
            return $"{name} had nail trim procedure";
        }

        public string Adopt(string animalName, string owner)
        {
            CheckforAnimal(animalName, this.hotel);
            IAnimal animal = this.hotel.Animals.FirstOrDefault(x => x.Key == animalName).Value;
            this.hotel.Adopt(animal.Name, owner);

            return animal.IsChipped ? $"{owner} adopted animal with chip" : $"{owner} adopted animal without chip";
        }

        private static void CheckforAnimal(string animalName, Hotel hotel)
        {
            if (!hotel.Animals.ContainsKey(animalName))
            {
                throw new ArgumentException($"Animal {animalName} does not exist");
            }
        }

        public string History(string type)
        {
            IProcedure procedure = this.procedures.FirstOrDefault(x => x.GetType().Name == type);
            return procedure.History();
        }

        public string GetSummary()
        {
            var groupedAnimals = this.animals.Where(x => x.IsAdopt)
                .GroupBy(x => x.Owner)
                .Select(g => new
                {
                    OwnerName = g.Key,
                    Animals = g.Select(a => a.Name)
                })
                .OrderBy(g => g.OwnerName)
                .ToArray();
            
            StringBuilder sb = new StringBuilder();

            foreach (var group in groupedAnimals)
            {
                sb.AppendLine($"--Owner: {group.OwnerName}");
                sb.AppendLine($"    - Adopted animals: {string.Join(" ", group.Animals.OrderBy(a => a))}");
            }

            return sb.ToString().TrimEnd();
        }
    
        

        private static void DoProcedure(string name, int procedureTime, ProcedureFactory procedureFactory, AnimalFactory animalFactory, string type, Hotel hotel, List<IProcedure> procedures)
        {

            IAnimal animal = hotel.Animals.FirstOrDefault(x => x.Key == name).Value;
            IProcedure procedure = procedures.FirstOrDefault(x => x.GetType().Name == type);
            
            procedure.DoService(animal, procedureTime);
        }
    }
}
