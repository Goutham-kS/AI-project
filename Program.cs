using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseStaticFiles(); // Needed for serving custom JS/CSS files

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Swagger UI at root "/"
        c.InjectStylesheet("/swagger-ui/custom.css");
        c.InjectJavascript("/swagger-ui/custom.js");
    });
}

app.UseHttpsRedirection();


app.MapGet("/getPerson", () =>
{
    string connectionString = "Server=tcp:asuredatabase.database.windows.net,1433;" +
                              "Initial Catalog=Ai_asure_database;" +
                              "Persist Security Info=False;" +
                              "User ID=Gouthamroot;" +
                              "Password=Goutham@10;" +
                              "MultipleActiveResultSets=False;" +
                              "Encrypt=True;" +
                              "TrustServerCertificate=False;" +
                              "Connection Timeout=30;";

    using var connection = new SqlConnection(connectionString);
    connection.Open();

    string query = "SELECT * FROM TableNamePerson";
    using var command = new SqlCommand(query, connection);
    using var reader = command.ExecuteReader();

    var results = new List<Dictionary<string, object>>();

    while (reader.Read())
    {
        var row = new Dictionary<string, object>();
        for (int i = 0; i < reader.FieldCount; i++)
        {
            row[reader.GetName(i)] = reader[i];
        }
        results.Add(row);
    }

    return results;
});



// ✅ Insert into TableNamePerson
app.MapPost("/addperson", async (string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber) =>
{
    string connectionString = "Server=tcp:asuredatabase.database.windows.net,1433;" +
                              "Initial Catalog=Ai_asure_database;" +
                              "Persist Security Info=False;" +
                              "User ID=Gouthamroot;" +
                              "Password=Goutham@10;" +
                              "MultipleActiveResultSets=False;" +
                              "Encrypt=True;" +
                              "TrustServerCertificate=False;" +
                              "Connection Timeout=30;";

    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    string query = @"INSERT INTO TableNamePerson (FirstName, LastName, DateOfBirth, Email, PhoneNumber) 
                     VALUES (@FirstName, @LastName, @DateOfBirth, @Email, @PhoneNumber)";

    using var command = new SqlCommand(query, connection);

    command.Parameters.AddWithValue("@FirstName", firstName);
    command.Parameters.AddWithValue("@LastName", lastName);
    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
    command.Parameters.AddWithValue("@Email", email);
    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

    int rowsAffected = await command.ExecuteNonQueryAsync();

    return Results.Ok($"Inserted {rowsAffected} row(s) into TableNamePerson.");
});


app.MapPost("/updatePerson", async (int personId, string firstName, string lastName, DateTime dateOfBirth, string email, string phoneNumber) =>
{
    string connectionString = "Server=tcp:asuredatabase.database.windows.net,1433;" +
                               "Initial Catalog=Ai_asure_database;" +
                               "Persist Security Info=False;" +
                               "User ID=Gouthamroot;" +
                               "Password=Goutham@10;" +
                               "MultipleActiveResultSets=False;" +
                               "Encrypt=True;" +
                               "TrustServerCertificate=False;" +
                               "Connection Timeout=30;";

    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    string query = @"UPDATE TableNamePerson 
                     SET FirstName=@FirstName, LastName=@LastName, DateOfBirth=@DateOfBirth, 
                         Email=@Email, PhoneNumber=@PhoneNumber
                     WHERE PersonID=@PersonID";

    using var command = new SqlCommand(query, connection);
    command.Parameters.AddWithValue("@PersonID", personId);
    command.Parameters.AddWithValue("@FirstName", firstName);
    command.Parameters.AddWithValue("@LastName", lastName);
    command.Parameters.AddWithValue("@DateOfBirth", dateOfBirth);
    command.Parameters.AddWithValue("@Email", email);
    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);

    int rowsAffected = await command.ExecuteNonQueryAsync();

    return rowsAffected > 0 ? Results.Ok("Person updated successfully")
                            : Results.NotFound("Person not found");
});

app.MapDelete("/deletePerson", async (int personId) =>
{
    string connectionString = "Server=tcp:asuredatabase.database.windows.net,1433;" +
                               "Initial Catalog=Ai_asure_database;" +
                               "Persist Security Info=False;" +
                               "User ID=Gouthamroot;" +
                               "Password=Goutham@10;" +
                               "MultipleActiveResultSets=False;" +
                               "Encrypt=True;" +
                               "TrustServerCertificate=False;" +
                               "Connection Timeout=30;";


    using var connection = new SqlConnection(connectionString);
    await connection.OpenAsync();

    string query = "DELETE FROM TableNamePerson WHERE PersonID=@PersonID";

    using var command = new SqlCommand(query, connection);
    command.Parameters.AddWithValue("@PersonID", personId);

    int rowsAffected = await command.ExecuteNonQueryAsync();

    return rowsAffected > 0 ? Results.Ok(" Person deleted successfully") 
                            : Results.NotFound("Person not found");
});

                            



app.Run();
