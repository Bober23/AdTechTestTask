using AdTechTestTask;

var manager = new EmployeesManager("test.json");
while (true)
{
    Console.WriteLine("Input args:");
    string[] inputArgs = Console.ReadLine().Split(';');
    await manager.MakeChangesByArgs(inputArgs);
}

