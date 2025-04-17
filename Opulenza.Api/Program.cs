using Opulenza.Api;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("seeder.json", optional: false, reloadOnChange: true);

builder.Services.AddPresentation(builder.Configuration);

// hijacking the default logging
builder.Host.UseSerilog();

var app = builder.Build();

// // #region SeedData
// using (var scope = app.Services.CreateScope())
// {
//     var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//     var seeder = builder.Configuration.GetSection("Seeder").Get<Seeder>();
//     context.Set<Product>().AddRange(seeder.Products);
//     context.Set<Category>().AddRange(seeder.Categories);
//     context.Set<ApplicationUser>().AddRange(seeder.Users);
//     context.SaveChanges();
//
//     // context.Database.EnsureCreated();
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
//app.UseMiddleware<ApiKeyAuthMiddleware>();
app.UseAuthorization();
//app.UseResponseCaching();
// app.Use(async (context, next) =>
// {
//     context.Response.GetTypedHeaders().CacheControl =
//         new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
//         {
//             Public = true,
//             MaxAge = TimeSpan.FromSeconds(60)
//         };
//     await next();
// });
app.MapControllers();
app.Run();