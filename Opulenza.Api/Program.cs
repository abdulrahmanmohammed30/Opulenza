using Microsoft.AspNetCore.Mvc;
using Opulenza.Api;
using Opulenza.Api.Endpoints.Internal;
using Opulenza.Api.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ApplicationName = "Opulenza.Api",
    EnvironmentName = Environments.Development,
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = "./wwwroot"
});

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Configuration.AddJsonFile("seeder.json", optional: false, reloadOnChange: true);

builder.Services.AddPresentation(builder.Configuration);

builder.Services.AddEndpoints<Program>(builder.Configuration);
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
    // setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", name: "Opulenza API V1");
    // setupAction.SwaggerEndpoint("/swagger/v2/swagger.json", "Opulenza API V2");
    // setupAction.RoutePrefix = string.Empty;
    // var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    // foreach (var description in provider.ApiVersionDescriptions)
    // {
    //     setupAction.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
    // }

    setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "Opulenza API V1");
    setupAction.SwaggerEndpoint("/swagger/v2/swagger.json", "Opulenza API V2");
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

ApiVersioningHelpers.SetupApiVersionSet(app);
app.UseEndpoints<Program>();

app.Run();