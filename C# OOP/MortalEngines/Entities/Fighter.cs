using MortalEngines.Entities.Contracts;
using System.Text;

namespace MortalEngines.Entities
{
    public class Fighter : BaseMachine, IFighter
    {

        public Fighter(string name, double attackPoints, double defensePoints) 
            : base(name, attackPoints, defensePoints, healthPoints: 200)
        {
            this.AggressiveMode = false;
            this.ToggleAggressiveMode();
        }

        public bool AggressiveMode { get; private set; }


        public void ToggleAggressiveMode()
        {
            if(this.AggressiveMode == false)
            {
                this.AttackPoints += 50;
                this.DefensePoints -= 25;
                this.AggressiveMode = true;
            }

            else
            {
                this.AttackPoints -= 50;
                this.DefensePoints += 25;
                this.AggressiveMode = false;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.Append(" *Aggressive: ");
            sb.Append(this.AggressiveMode == true ? "ON" : "OFF");

            return sb.ToString().TrimEnd();
        }
    }
}
