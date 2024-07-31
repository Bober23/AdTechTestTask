using AdTechTestTask.DTOs;
using System.Globalization;

namespace AdTechTestTask;
public class EmployeesManager
{
    private readonly string _textFileLocation;

    public EmployeesManager(string textFileLocation)
    {
        _textFileLocation = textFileLocation;
    }

    public async Task MakeChangesByArgs(string[] args)
    {
        foreach (var arg in args)
        {
            string argString = arg.TrimStart();
            string argOperation = argString.Split()[0];
            switch (argOperation)
            {
                case "-add":
                    int newUserId = await AddNewEmployee(argString);
                    if (newUserId > 0)
                    {
                        Console.WriteLine($"Added new user with Id = {newUserId}");
                    }
                    break;
                case "-update":
                    await UpdateEmployee(argString);
                    break;
                case "-delete":
                    int employeeId = Convert.ToInt32(GetParamValue(argString, "Id"));
                    Employee? deletedEmployee = await DeleteEmployee(employeeId);
                    if (deletedEmployee == null)
                    {
                        Console.WriteLine($"Employee with Id = {employeeId} not found");
                    }
                    break;
                case "-get":
                    employeeId = Convert.ToInt32(GetParamValue(argString, "Id"));
                    Employee? employee = await GetEmployeeById(employeeId);
                    if (employee != null)
                    {
                        Console.WriteLine($"Id = {employee.Id}, FirstName = {employee.FirstName}, LastName = {employee.LastName}, SalaryPerHour = {employee.SalaryPerHour}");
                    }
                    else
                    {
                        Console.WriteLine($"Employee with Id = {employeeId} not found");
                    }
                    break;
                case "-getall":
                    List<Employee> employees = await GetAllEmployees();
                    foreach (var element in employees)
                    {
                        Console.WriteLine($"Id = {element.Id}, FirstName = {element.FirstName}, LastName = {element.LastName}, SalaryPerHour = {element.SalaryPerHour}");
                    }
                    break;
                default:
                    Console.WriteLine("none");
                    break;
            }
        }
    }

    public async Task<int> AddNewEmployee(string arg)
    {
        List<Employee> employees = await JsonProcessing.GetEmployeesFromJson(_textFileLocation);
        if (employees == null)
        {
            employees = new List<Employee>();
        }
        int maxId = 0;
        if (employees.Count > 0)
        {
            maxId = employees.Max(x => x.Id);
        }
        Employee employee = new Employee()
        {
            Id = maxId + 1,
            FirstName = GetParamValue(arg, "FirstName"),
            LastName = GetParamValue(arg, "LastName"),
            SalaryPerHour = Convert.ToDecimal(GetParamValue(arg, "Salary"), CultureInfo.InvariantCulture),  //в задании написан запрос с Salary, но поле называется SalaryPerHour, что вносит путаницу
        };
        if (employee.FirstName != null && employee.LastName!=null && employee.SalaryPerHour!=null) 
        {
            employees.Add(employee);
            await JsonProcessing.UpdateJsonAsync(employees, _textFileLocation);
            return maxId + 1;
        }
        return 0;
        
    }

    public async Task UpdateEmployee(string arg)
    {
        List<Employee> employees = await JsonProcessing.GetEmployeesFromJson(_textFileLocation);
        if (employees == null)
        {
            return;
        }
        int id = Convert.ToInt32(GetParamValue(arg, "Id"));
        string firstName = GetParamValue(arg, "FirstName");
        string lastName = GetParamValue(arg, "LastName");
        decimal? salary = Convert.ToDecimal(GetParamValue(arg, "SalaryPerHour"), CultureInfo.InvariantCulture);
        Employee? targetEmployee = employees.FirstOrDefault(x => x.Id == id);
        if (targetEmployee == null)
        {
            return;
        }
        if (firstName != null)
        {
            targetEmployee.FirstName = firstName;
        }
        if (lastName != null)
        {
            targetEmployee.LastName = lastName;
        }
        if (salary != null)
        {
            targetEmployee.SalaryPerHour = salary.Value;
        }
        await JsonProcessing.UpdateJsonAsync(employees, _textFileLocation);
    }

    public async Task<Employee?> DeleteEmployee(int id)
    {
        List<Employee> employees = await JsonProcessing.GetEmployeesFromJson(_textFileLocation);
        Employee targetEmployee = employees.FirstOrDefault(x => x.Id == id);
        if (targetEmployee == null)
        { 
            return null; 
        }
        employees.Remove(targetEmployee);
        await JsonProcessing.UpdateJsonAsync(employees, _textFileLocation);
        return targetEmployee;
    }

    public async Task<Employee?> GetEmployeeById(int id)
    {
        List<Employee> employees = await JsonProcessing.GetEmployeesFromJson(_textFileLocation);
        return employees.FirstOrDefault(x => x.Id == id);
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        List<Employee> employees = await JsonProcessing.GetEmployeesFromJson(_textFileLocation);
        return employees;
    }

    private string? GetParamValue(string args, string paramName)
    {
        string? param = args.Split().FirstOrDefault(str => str.Contains($"{paramName}:"));
        if (param == null)
        {
            return null;
        }
        return param.Split(':').Last();
    }
}
