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
        public void AddElevatorCall(int originFloor, int destinationFloor, Direction direction, int timeOfCall)
        {
            pendingCalls.Add(new ElevatorCall()
            {
                OriginFloor = originFloor,
                DestinationFloor = destinationFloor,
                CallDirection = direction,
                TimeOfCall = timeOfCall
            });
        }

        // Simulate elevator movement
        public void Simulate()
        {
            while ((pendingCalls.Any() || waitingPassengers.Any() || elevator.DestinationFloors.Any()) && currentTime < 11)
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
                // Create waiting passenger
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

            // Determine movement direction
            if (elevator.CurrentDirection == Direction.Idle)
            {
                // If idle, choose direction based on first destination
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
            else if (elevator.CurrentFloor == InitialFloor || elevator.CurrentFloor == TotalFloors)
            {
                // In the case that we have reached the bottom floor or the top, idle the elevator
                elevator.CurrentDirection = Direction.Idle;
            }

            // Adjust direction if no more destinations in current direction
            if (!elevator.DestinationFloors.Any())
            {
                elevator.CurrentDirection = Direction.Idle;
            }
            else
            {
                // Determine if we need to change direction
                bool hasUpcomingDestination = elevator.CurrentDirection == Direction.Up 
                    ? elevator.DestinationFloors.Any(f => f > elevator.CurrentFloor)
                    : elevator.DestinationFloors.Any(f => f < elevator.CurrentFloor);

                if (!hasUpcomingDestination)
                {
                    // Reverse direction
                    elevator.CurrentDirection = elevator.CurrentDirection == Direction.Up 
                        ? Direction.Down 
                        : Direction.Up;
                }
            }
        }

        private void HandlePassengers()
        {
            // Find waiting passengers at current floor
            var boardingPassengers = waitingPassengers
                .Where(p => p.OriginFloor == elevator.CurrentFloor)
                .ToList();

            foreach (var passenger in boardingPassengers)
            {
                // Check if elevator is going in passenger's desired direction
                bool canBoard = elevator.CurrentDirection == passenger.InitialDirection ||
                                elevator.CurrentDirection == Direction.Idle;

                // Elevator will only stop to pick up passenger if its traveling in the correct direction.
                bool goingRightWay = (elevator.CurrentDirection == Direction.Up && passenger.DestinationFloor > elevator.CurrentFloor) ||
                                    (elevator.CurrentDirection == Direction.Down && passenger.DestinationFloor < elevator.CurrentFloor);

                if (canBoard && goingRightWay)
                {
                    // Passenger boards the elevator
                    elevator.Passengers.Add(passenger);
                    waitingPassengers.Remove(passenger);

                    // Add destination floor
                    elevator.DestinationFloors.Add(passenger.DestinationFloor);

                    // Update elevator direction if idle
                    if (elevator.CurrentDirection == Direction.Idle)
                    {
                        elevator.CurrentDirection = passenger.DestinationFloor > elevator.CurrentFloor
                            ? Direction.Up
                            : Direction.Down;
                    }
                }
            }

            // Find passengers who want to get off on this current floor
            var departingPassengers = elevator.Passengers
                .Where(p => p.DestinationFloor == elevator.CurrentFloor)
                .ToList();

            foreach (var passenger in departingPassengers)
            {
                // Need to have passenger leave elevator and remove the destination tracking for them.
                elevator.Passengers.Remove(passenger);
                elevator.DestinationFloors.Remove(elevator.CurrentFloor);
            }
        }

        private void OutputElevatorState()
        {
            Console.WriteLine(
                $"Time: {currentTime}, " +
                $"Floor: {elevator.CurrentFloor}, " +
                $"Direction: {elevator.CurrentDirection}, " +
                $"Passengers: {elevator.Passengers.Count}, " +
                $"Destinations: {string.Join(",", elevator.DestinationFloors)}"
            );
        }
    }
}