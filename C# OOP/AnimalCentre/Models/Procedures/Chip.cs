namespace AnimalCentre.Models.Procedures
{
    using System;
    using Models.Contracts;

    public class Chip : Procedure
    {
       
        public override void DoService(IAnimal animal, int procedureTime)
        {
            if (animal.IsChipped)
            {
                throw new ArgumentException($"{animal.Name} is already chipped");
            }

            base.DoService(animal, procedureTime);

            animal.Happiness -= 5;
            animal.IsChipped = true;
        }
    }
}
