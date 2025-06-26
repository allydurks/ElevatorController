namespace EllevationElevator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create elevator system
            ElevatorSimulator elevatorSystem = new ElevatorSimulator();

            // Scenario 1: Basic Test Case from Problem Description
            // At time 0, a user on floor 3 presses the "Down" request button. Their destination is floor 2.
            elevatorSystem.AddElevatorCall(3, 2, Direction.Down, 0);

            // At time 1, a user on floor 10 presses the "Down" request button. Their destination is floor 1.
            elevatorSystem.AddElevatorCall(10, 1, Direction.Down, 1);

            // Run the simulation
            elevatorSystem.Simulate();
        }
    }
}
