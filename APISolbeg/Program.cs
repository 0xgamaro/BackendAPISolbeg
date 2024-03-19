using DataLayer;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") 
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddDbContext<DAO>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDAO, DAO>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DAO>();
    dbContext.Database.Migrate();
}

app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/employees", async (IDAO dao) =>
    await dao.GetAllEmployeesAsync())
    .WithName("GetAllEmployees");

app.MapGet("/employee/{id}", async (IDAO dao, int id) =>
    await dao.GetEmployeeByIdAsync(id))
    .WithName("GetEmployeeById");

app.MapPost("/employee", async (IDAO dao, Employee employee) =>
{
    employee.Id = 0;
    await dao.AddEmployeeAsync(employee);
    return Results.Created($"/employee/{employee.Id}", employee);
})
.WithName("AddEmployee");

app.MapPut("/employee/{id}", async (IDAO dao, int id, Employee updatedEmployee) =>
{
    var employee = await dao.GetEmployeeByIdAsync(id);
    if (employee == null) return Results.NotFound();

    employee.FirstName = updatedEmployee.FirstName;
    employee.LastName = updatedEmployee.LastName;
    employee.Age = updatedEmployee.Age;
    employee.Sex = updatedEmployee.Sex;

    await dao.UpdateEmployeeAsync(employee);
    return Results.NoContent();
})
.WithName("UpdateEmployee");

app.MapDelete("/employee", async (IDAO dao, [FromBody] List<int> ids) =>
{
    foreach (var id in ids)
    {
        await dao.DeleteEmployeeAsync(id);
    }
    return Results.NoContent();
})
.WithName("DeleteMultipleEmployees");

app.Run();

