namespace EllevationElevator
{
    public class RandomRequestGenerator
    {
        public static List<ElevatorCall> GenerateRandomElevatorRequests(int numRequests)
        {
            var random = new Random();
            var requests = new List<ElevatorCall>();

            for (int i = 0; i < numRequests; i++)
            {
                // Randomly generate origin floor (1-10)
                int originFloor = random.Next(1, 11);

                // Ensure destination floor is different from origin floor
                int destinationFloor;
                do
                {
                    destinationFloor = random.Next(1, 11);
                } while (destinationFloor == originFloor);

                // Determine direction based on source and destination
                Direction direction = originFloor < destinationFloor ? Direction.Up : Direction.Down;

                // Random time of request (0-10)
                int timeOfCall = random.Next(0, 11);
                var request = new ElevatorCall(originFloor, destinationFloor, direction, timeOfCall);

                requests.Add(request);
            }

            // Sort requests by time to simulate realistic scenario
            return requests.OrderBy(r => r.TimeOfCall).ToList();
        }
    
    }
}