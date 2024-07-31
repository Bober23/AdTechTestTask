using AdTechTestTask.DTOs;
using System.Text.Json;

namespace AdTechTestTask;
public static class JsonProcessing
{
    public static async Task UpdateJsonAsync(List<Employee> employees, string textFileName)
    {
        using (FileStream stream = File.Open(textFileName, FileMode.Create, FileAccess.Write, FileShare.Read))
        {
            await JsonSerializer.SerializeAsync(stream, employees);
            stream.Close();
            stream.Dispose();
        }
    }

    public static async Task<List<Employee>> GetEmployeesFromJson(string textFileName)
    {
        List<Employee>? employees;
        using (FileStream stream = File.Open(textFileName, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
        {
            if (stream.Length > 0)
            {
                employees = await JsonSerializer.DeserializeAsync<List<Employee>>(stream);
            }
            else
            {
                employees = new List<Employee>();
            }
            stream.Close();
            stream.Dispose();
        }
        return employees;
    }
}
