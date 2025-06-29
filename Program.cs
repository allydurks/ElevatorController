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
            elevatorSystem.AddElevatorCall(new ElevatorCall(3, 2, Direction.Down, 0));

            // At time 1, a user on floor 10 presses the "Down" request button. Their destination is floor 1.
            elevatorSystem.AddElevatorCall(new ElevatorCall(10, 1, Direction.Down, 1));

            // Generate and add 10 random requests
            var randomRequests = RandomRequestGenerator.GenerateRandomElevatorRequests(10);

            foreach (var request in randomRequests)
            {
                /*Console.WriteLine($"At time {request.TimeOfCall}, a user on floor {request.OriginFloor} " +
                                $"presses the \"{request.CallDirection}\" request button. " +
                                $"Their destination is floor {request.DestinationFloor}.");*/

                elevatorSystem.AddElevatorCall(request);
            }

            // Run the simulation
            elevatorSystem.Simulate();
        }
    }
}
