/*
** This encapsulates the Elevator, the Passengers, the clock,
** and requests made as well as the process logic and the output function. 
*/
namespace EllevationElevator
{
    public enum Direction { Idle, Up, Down }

    // Passenger that boards elevator and selects floor to go to.
    public class Passenger
    {
        public int OriginFloor { get; set; }
        public int DestinationFloor { get; set; }
        public Direction InitialDirection { get; set; }
    }

    // Call made when person hits up or down button outside elevator.
    public class ElevatorCall
    {
        public int OriginFloor { get; set; }
        public int DestinationFloor { get; set; }
        public Direction CallDirection { get; set; }
        public int TimeOfCall { get; set; }

        public ElevatorCall(int originFloor, int destinationFloor, Direction direction, int timeOfCall)
        {
            OriginFloor = originFloor;
            DestinationFloor = destinationFloor;
            CallDirection = direction;
            TimeOfCall = timeOfCall;
        }
    }

    public class ElevatorSimulator
    {
        private ElevatorState elevator;
        private List<ElevatorCall> pendingCalls;
        private List<Passenger> waitingPassengers;
        private int currentTime;
        private const int InitialFloor = 1;
        private const int TotalFloors = 10;

        private class ElevatorState
        {
            public int CurrentFloor { get; set; }
            public Direction CurrentDirection { get; set; }
            public HashSet<int> DestinationFloors { get; set; }
            public List<Passenger> Passengers { get; set; }

            public ElevatorState()
            {
                CurrentFloor = InitialFloor;
                CurrentDirection = Direction.Idle;
                DestinationFloors = new HashSet<int>();
                Passengers = new List<Passenger>();
            }
        }

        public ElevatorSimulator()
        {
            elevator = new ElevatorState();
            pendingCalls = new List<ElevatorCall>();
            waitingPassengers = new List<Passenger>();
            currentTime = 0;
        }

        // Add an elevator call
        public void AddElevatorCall(ElevatorCall call)
        {
            pendingCalls.Add(call);
        }

        // Simulate elevator movement
        public void Simulate()
        {
            while (pendingCalls.Any() || waitingPassengers.Any() || elevator.DestinationFloors.Any())
            {
                // Process calls at current time
                ProcessElevatorCalls();

                // Move elevator
                MoveElevator();

                // Handle passenger getting on/off the elevator
                HandlePassengers();

                // Output current state
                OutputElevatorState();

                currentTime++;
            }
        }

        // Process elevator calls
        private void ProcessElevatorCalls()
        {
            // Get calls at current time
            var currentTimeCalls = pendingCalls
                .Where(c => c.TimeOfCall == currentTime)
                .ToList();

            foreach (var call in currentTimeCalls)
            {
                // Create waiting passenger for calls made
                var passenger = new Passenger
                {
                    OriginFloor = call.OriginFloor,
                    InitialDirection = call.CallDirection,
                    DestinationFloor = call.DestinationFloor
                };

                waitingPassengers.Add(passenger);
                // Need to go to origin to pick up passenger
                elevator.DestinationFloors.Add(passenger.OriginFloor);
                pendingCalls.Remove(call);
            }
        }

        // Move elevator
        private void MoveElevator()
        {
            // If no destination floors, set to idle
            if (!elevator.DestinationFloors.Any())
            {
                elevator.CurrentDirection = Direction.Idle;
                return;
            }
            
            if (elevator.CurrentDirection == Direction.Idle)
            {
                // Choose direction based on first destination
                elevator.CurrentDirection = elevator.DestinationFloors.First() > elevator.CurrentFloor 
                    ? Direction.Up 
                    : Direction.Down;
            }

            // Move elevator based on current direction, cannot go outside of bounds of building floors
            if (elevator.CurrentDirection == Direction.Up && elevator.CurrentFloor < TotalFloors)
            {
                elevator.CurrentFloor++;
            }
            else if (elevator.CurrentDirection == Direction.Down && elevator.CurrentFloor > InitialFloor)
            {
                elevator.CurrentFloor--;
            }
        }

        private void HandlePassengers()
        {
            // Find passengers who want to get off on this current floor
            var departingPassengers = elevator.Passengers
                .Where(p => p.DestinationFloor == elevator.CurrentFloor)
                .ToList();

            foreach (var passenger in departingPassengers)
            {
                // Need to have passenger leave elevator and remove the destination tracking for them.
                elevator.Passengers.Remove(passenger);
                elevator.DestinationFloors.Remove(elevator.CurrentFloor);
                // Console.WriteLine($"Passenger got off at time: {currentTime} " + $"and on floor: {elevator.CurrentFloor}, ");
                if (!elevator.Passengers.Any())
                {
                    // if nobody left on elevator, send to Idle state
                    elevator.CurrentDirection = Direction.Idle;
                }
            }

            // Find waiting passengers at current floor
            var boardingPassengers = waitingPassengers
                .Where(p => p.OriginFloor == elevator.CurrentFloor)
                .ToList();

            foreach (var passenger in boardingPassengers)
            {
                if (elevator.Passengers.Any())
                {
                    if (elevator.CurrentDirection == passenger.InitialDirection)
                    {
                        // If there are other passengesr on the elevator and it is going in the right direction
                        // for the new passenger, they will board the elevator
                        elevator.Passengers.Add(passenger);
                        waitingPassengers.Remove(passenger);
                        // Add destination floor
                        elevator.DestinationFloors.Add(passenger.DestinationFloor);
                        elevator.DestinationFloors.Remove(passenger.OriginFloor);
                    }
                    else
                    {
                        // otherwise they will have to wait until the elevator request is at the right point in the queue
                    }
                }
                else
                {
                    // if the elevator is empty, board the passenger and change the elevator direction
                    elevator.Passengers.Add(passenger);
                    waitingPassengers.Remove(passenger);
                    // Add destination floor
                    elevator.DestinationFloors.Add(passenger.DestinationFloor);
                    elevator.DestinationFloors.Remove(passenger.OriginFloor);
                    elevator.CurrentDirection = passenger.InitialDirection;
                    // Console.WriteLine($"Passenger got on at time: {currentTime} " + $"and on floor: {elevator.CurrentFloor}, ");
                }

            }
        }

        private void OutputElevatorState()
        {
            /*Console.WriteLine(
                $"Time: {currentTime}, " +
                $"Floor: {elevator.CurrentFloor}, " +
                $"Direction: {elevator.CurrentDirection}, " +
                $"Passengers: {elevator.Passengers.Count}, " +
                $"Destinations: {string.Join(",", elevator.DestinationFloors)}"
            );*/

            Console.WriteLine($"{{ time: {currentTime}, direction {elevator.CurrentDirection}, floor: {elevator.CurrentFloor}}}");
        }
    }
}