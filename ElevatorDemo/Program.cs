var elevator = new Elevator(0, 5, 1000);

elevator.FloorReached += (sender, args) =>
{
    Console.WriteLine($"The elevator reached {args.Floor} floor.");
};
elevator.DestinationFloorReached += (sender, args) =>
{
    Console.WriteLine($"The elevator reached destination, floor: {args.Floor}");
};

Console.WriteLine(elevator.CurrentFloor);

await elevator.Move(5);
await elevator.Move(3);
Console.WriteLine(elevator.CurrentFloor);

await elevator.Move(0);
Console.WriteLine(elevator.CurrentFloor);

public class ElevatorFloorEventArgs : EventArgs
{
    public int Floor { get; set; }
}

public class Elevator
{
    public int CurrentFloor { get; private set; }
    public int MaxFloor { get; private set; }
    public int MinFloor { get; private set; }
    public int MillisecondsBetweenFloors { get; private set; }
    private bool ElevatorMooving;

    public event EventHandler<ElevatorFloorEventArgs> FloorReached;
    public event EventHandler<ElevatorFloorEventArgs> DestinationFloorReached;

    public Elevator(int minFloor,  int maxFloor, int millisecondsBetweenFloors)
    {
        MinFloor = minFloor;
        MaxFloor = maxFloor;
        MillisecondsBetweenFloors = millisecondsBetweenFloors;
    }

    public async Task Move (int destinationFloor)
    {
        if (ElevatorMooving == true)
        {
            throw new Exception("Elevator is moving, wait till is stops");
        }

        ElevatorMooving = true;
        if (destinationFloor < MinFloor || destinationFloor > MaxFloor)
        {
            throw new ArgumentException("Wrong Floor");
        }

        await Task.Run(async () =>
        {
            while (CurrentFloor != destinationFloor)
            {   
                await Task.Delay(MillisecondsBetweenFloors);

                CurrentFloor += CurrentFloor > destinationFloor ? -1 : 1;

                FloorReached?.Invoke(this, new ElevatorFloorEventArgs { Floor = CurrentFloor });
            }
        });

        DestinationFloorReached?.Invoke(this, new ElevatorFloorEventArgs { Floor = CurrentFloor });
        ElevatorMooving = false;
    }
}