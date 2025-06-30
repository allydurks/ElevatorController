namespace EllevationElevator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create elevator simulator
            ElevatorSimulator elevatorSimulator = new ElevatorSimulator();

            // At time 0, a user on floor 3 presses the "Down" request button. Their destination is floor 2.
            elevatorSimulator.AddElevatorCall(new ElevatorCall(3, 2, Direction.Down, 0));

            // At time 1, a user on floor 10 presses the "Down" request button. Their destination is floor 1.
            elevatorSimulator.AddElevatorCall(new ElevatorCall(10, 1, Direction.Down, 1));

            // Generate and add 10 random requests
            /*var randomRequests = RandomRequestGenerator.GenerateRandomElevatorRequests(10);

            foreach (var request in randomRequests)
            {
                Console.WriteLine($"At time {request.TimeOfCall}, a user on floor {request.OriginFloor} " +
                                $"presses the \"{request.CallDirection}\" request button. " +
                                $"Their destination is floor {request.DestinationFloor}.");

                elevatorSimulator.AddElevatorCall(request);
            }*/

            // Run the simulation
            elevatorSimulator.Simulate();
        }
    }
}
