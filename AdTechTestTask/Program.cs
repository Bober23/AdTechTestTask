using AdTechTestTask;

var cl = new EmployeesManager("test.json");
while (true)
{
    Console.WriteLine("Input args:");
    string[] inputArgs = Console.ReadLine().Split(';');
    await cl.MakeChangesByArgs(inputArgs);
}

