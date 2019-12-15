using MortalEngines.Entities.Contracts;
using System.Text;

namespace MortalEngines.Entities
{
    public class Tank : BaseMachine, ITank
    {

        public Tank(string name, double attackPoints, double defensePoints)
            : base(name, attackPoints, defensePoints, healthPoints: 100)
        {
            this.DefenseMode = false;
            this.ToggleDefenseMode();
        }

        public bool DefenseMode { get; private set; }


        public void ToggleDefenseMode()
        {
            if (this.DefenseMode == false)
            {
                this.AttackPoints -= 40;
                this.DefensePoints += 30;
                this.DefenseMode = true;
            }

            else
            {
                this.AttackPoints += 40;
                this.DefensePoints -= 30;
                this.DefenseMode = false;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(base.ToString());
            sb.Append(" *Defense: ");
            sb.Append(this.DefenseMode == true ? "ON" : "OFF");

            return sb.ToString().TrimEnd();
        }
    }
}
