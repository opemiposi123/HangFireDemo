using Hangfire;
using HangFireDemo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Hangfire with SQL Server storage
builder.Services.AddHangfire((sp, config) =>
{
	var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection");
	config.UseSqlServerStorage(connectionString);
});
builder.Services.AddHangfireServer();

// Register FileProcessingService
builder.Services.AddSingleton<FileProcessingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Configure Hangfire Dashboard
app.UseHangfireDashboard();

app.MapControllers();
app.Run();
