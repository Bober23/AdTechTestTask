using AdTechTestTask.DTOs;

namespace AdTechTestTask.Tests;

public class EmployeeManagerTests
{
    [Fact]
    public async Task TestAddNormalEmployee()
    {
        EmployeesManager manager = new EmployeesManager("test.json");
        string args = "-add FirstName:Test LastName:User Salary:100.50";
        int newUserId = await manager.AddNewEmployee(args);
        Employee newEmployee = await manager.GetEmployeeById(newUserId);
        Assert.NotNull(newEmployee);
        Assert.Equal("Test", newEmployee.FirstName);
        Assert.Equal("User", newEmployee.LastName);
        Assert.True(newEmployee.SalaryPerHour == (decimal)100.50);
    }
    [Fact]
    public async Task TestAddIncorrectEmployee()
    {
        EmployeesManager manager = new EmployeesManager("test.json");
        string args = "-add FstNe:Test Le:User Sry:100.50";
        int newUserId = await manager.AddNewEmployee(args);
        Employee newEmployee = await manager.GetEmployeeById(newUserId);
        Assert.Null(newEmployee);
    }
}