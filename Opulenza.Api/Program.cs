using Opulenza.Api;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddPresentation(builder.Configuration);

// hijacking the default logging
builder.Host.UseSerilog();

var app = builder.Build();

// #region SeedData
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     context.Database.EnsureCreated();
// }

//Configure the HTTP request pipeline.

// #endregion

app.UseSerilogRequestLogging();

app.UseSwagger();

app.UseSwaggerUI(setupAction =>
{
    setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", name: "Opulenza API");
    setupAction.RoutePrefix = string.Empty;
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //app.UseExceptionMiddleware();
    app.MapOpenApi();
}
else
{
    app.UseExceptionHandler("/error");
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();