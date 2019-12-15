﻿namespace AnimalCentre.Models.Procedures
{
    using Models.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public abstract class Procedure : IProcedure
    {
        protected ICollection<IAnimal> procedureHistory;

        public Procedure()
        {
            this.procedureHistory = new List<IAnimal>();
        }

        public virtual void DoService(IAnimal animal, int procedureTime)
        {
            if (animal.ProcedureTime < procedureTime)
            {
                throw new ArgumentException("Animal doesn't have enough procedure time");
            }
            animal.ProcedureTime -= procedureTime;
            this.procedureHistory.Add(animal);

        }


        public string History()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.GetType().Name);
            foreach (var animal in this.procedureHistory)
            {
                sb.AppendLine(animal.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
