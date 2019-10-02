
namespace ShipCommands
{
    public class Steer : ICommand
    {
        private readonly Ship ship;
        private readonly Direction direction;

        public Steer(Ship ship, Direction direction)
        {
            this.ship = ship;
            this.direction = direction;
        }

        public void Execute()
        {
            if (direction == Direction.Port)
                ship.SteerTickCounterClockWise();
            else if (direction == Direction.Starboard)
                ship.SteerTickClockWise();
            else if (direction == Direction.Ahead)
                ship.ResetSteer();
        }

        public enum Direction { Ahead, Port, Starboard }
    }
}