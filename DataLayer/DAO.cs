using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class DAO : DbContext, IDAO
    {
        public DAO(DbContextOptions<DAO> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public async Task<List<Employee>> GetAllEmployeesAsync() =>
            await Employees.ToListAsync();

        public async Task<Employee> GetEmployeeByIdAsync(int id) =>
            await Employees.FindAsync(id);

        public async Task AddEmployeeAsync(Employee employee)
        {
            await Employees.AddAsync(employee);
            await SaveChangesAsync();
        }
        public async Task UpdateEmployeeAsync(Employee employee)
        {
            Employees.Update(employee);
            await SaveChangesAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await Employees.FindAsync(id);
            if (employee != null)
            {
                Employees.Remove(employee);
                await SaveChangesAsync();
            }
        }
    }
}