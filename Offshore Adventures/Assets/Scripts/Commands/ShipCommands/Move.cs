
namespace ShipCommands
{
    public class Move : ICommand
    {
        private readonly Ship ship;
        private readonly Ship.EOT eot;

        public Move(Ship ship, Ship.EOT eot)
        {
            this.ship = ship;
            this.eot = eot;
        }

        public void Execute()
        {
            ship.SetSpeed(eot);
        }
    }
}