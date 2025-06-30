namespace EllevationElevator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create elevator simulator
            ElevatorSimulator elevatorSimulator = new ElevatorSimulator();

            // At time 0, a user on floor 3 presses the "Down" request button. Their destination is floor 2.
            //elevatorSimulator.AddElevatorCall(new ElevatorCall(3, 2, Direction.Down, 0));

            // At time 1, a user on floor 10 presses the "Down" request button. Their destination is floor 1.
            //elevatorSimulator.AddElevatorCall(new ElevatorCall(10, 1, Direction.Down, 1));

            //elevatorSimulator.AddElevatorCall(new ElevatorCall(8, 10, Direction.Up, 0));
            //elevatorSimulator.AddElevatorCall(new ElevatorCall(3, 5, Direction.Up, 1));
            /*○ T0 up request on 8 
            ○ T1 up request on 3 
            ○ When elevator stops at 8, button pressed for Floor 10 
            ○ When elevator stops at 3, button pressed for Floor 5*/

            // Generate and add 10 random requests
            /*var randomRequests = RandomRequestGenerator.GenerateRandomElevatorRequests(10);

            foreach (var request in randomRequests)
            {
                Console.WriteLine($"At time {request.TimeOfCall}, a user on floor {request.OriginFloor} " +
                                $"presses the \"{request.CallDirection}\" request button. " +
                                $"Their destination is floor {request.DestinationFloor}.");

                elevatorSimulator.AddElevatorCall(request);
            }*/

            elevatorSimulator.AddElevatorCall(new ElevatorCall(9, 10, Direction.Up, 0));
            elevatorSimulator.AddElevatorCall(new ElevatorCall(4, 1, Direction.Down, 1));
            elevatorSimulator.AddElevatorCall(new ElevatorCall(8, 10, Direction.Up, 1));
            elevatorSimulator.AddElevatorCall(new ElevatorCall(8, 6, Direction.Down, 2));

            // Run the simulation
            elevatorSimulator.Simulate();
        }
    }
}
