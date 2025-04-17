**Opulenza API**

A robust e-commerce API for an online electronics store specializing in laptops and computers, built with ASP.NET Core and Clean Architecture. This system seamlessly integrates modern functionalities to provide a secure and dynamic shopping experience.

**Key Features:**

- **Robust User Management:**  
  - Comprehensive account endpoints for registering, logging in (including GitHub OAuth), refreshing tokens, email confirmations, password changes, account deletion, and more.  
  - JWT-based authentication coupled with Identity for ironclad security.

- **Dynamic Catalog & Product Handling:**  
  - Full CRUD operations on products and categories, including managing images and file uploads.  
  - Intuitive endpoints that cover everything from product details (`/api/products/{id}`) to category image management.

- **Seamless Shopping Flow:**  
  - Integrated shopping cart functionality with endpoints to add, view, and manage items.  
  - Effortless checkout process for orders, enhanced by Stripe payment integration.

- **Real-Time & Interactive Communication:**  
  - Utilization of SignalR for real-time updates and notifications, ensuring a fluid user experience.

- **Cutting-Edge Middleware & Services:**  
  - Custom middlewares, Fluent Validations, and CORS policies to safeguard and streamline communications.  
  - Mediator pattern with a generic repository for decoupled business logic and scalable architecture.

- **Developer-Centric Tools & Monitoring:**  
  - Swagger-powered API documentation for a self-explanatory, interactive exploration of endpoints.  
  - Advanced logging with Serilog (MSSqlServer and Seq sinks), Redis caching, and comprehensive error handling using ErrorOr.

- **Comprehensive Package Ecosystem:**  
  - A suite of modern .NET packages such as Asp.Versioning.Mvc, AspNet.Security.OAuth.GitHub, MailKit, MediatR, and Entity Framework Core – ensuring productivity and maintainability.

![image](https://github.com/user-attachments/assets/1cecab01-bbfe-49e4-92d9-f82f332e916e)



### **Steps to Download and Run the Project**

1. **Clone the Repository**  
   Open your terminal and run:

   ```bash
   git clone https://github.com/abdulrahmanmohammed30/Opulenza.git
   ```

2. **Navigate to the Project Directory**  
   ```bash
   cd Opulenza
   ```

3. **Ensure You Have the Correct .NET SDK Installed**  
   This project is built with ASP.NET Core and Clean Architecture. Make sure you have the required .NET SDK installed (check the project’s README for the recommended version). You can download it from the [official .NET website](https://dotnet.microsoft.com/download).

4. **Restore Project Dependencies**  
   Execute the following command to restore all NuGet packages:

   ```bash
   dotnet restore
   ```

5. **Configure App Settings**  
   Open the `appsettings.json` file and review the configuration sections. Key areas to check include:
   
   - **Connection Strings:**  
     The `"DefaultConnection"` is set to use SQL Server running on localhost:
     
     ```json
     "DefaultConnection": "Server=localhost;Database=Opulenza; Integrated Security = true; Trust Server Certificate = true"
     ```
     
     *If you’re using a different SQL Server instance or credentials, update this string accordingly.*

   - **JWT Settings:**  
     Ensure `"Issuer"`, `"Audience"`, and `"SecretKey"` match your security requirements.
   
   - **Logging & Serilog:**  
     Review settings under `"Serilog"` to control logging levels and sinks (Console, MSSqlServer, and Seq).
   
   - **External Services:**  
     Update sections such as `"SmtpSettings"`, `"GitHub"`, and `"Stripe"` with your credentials if you plan on using these integrations.

6. **Set Up the Database with EF Core**  

   - **Ensure SQL Server is Running:**  
     Verify that your SQL Server instance is operational. If you don't have SQL Server, consider installing SQL Server Express or updating the connection string for another database system.
   
   - **Install EF Core Tool (if not already installed):**
   
     ```bash
     dotnet tool install --global dotnet-ef
     ```
   
   - **Apply Migrations to Set Up the Database Schema:**  
     Run the following command in your project directory:
   
     ```bash
     dotnet ef database update
     ```
   
     This command applies any pending migrations and creates/updates your database schema according to the code-first models.

7. **Build and Run the Application**

   - **Using the Command Line:**
   
     ```bash
     dotnet run
     ```
   
   - **Or via Visual Studio:**  
     Open the solution file in Visual Studio, build the project, and run it using the integrated IIS Express or Kestrel server.

8. **Access the API Documentation**  
   Once the application is running, open your web browser and navigate to the Swagger UI (default URL, unless configured otherwise):

   ```
   http://localhost:5000/swagger
   ```

   This interactive documentation will allow you to explore and test the API endpoints.

9. **Additional Configurations (Optional)**

   - **Stripe Integration:**  
     Insert your Stripe API keys in the `"Stripe"` section of the `appsettings.json` file if you plan to use the payment processing features.
   
   - **GitHub Authentication:**  
     Update the `"ClientId"` and `"ClientSecret"` in the `"GitHub"` configuration for OAuth authentication.
   
   - **Redis Caching:**  
     If you decide to integrate caching, configure the `"Redis"` connection string.
   
   - **SMTP Service:**  
     Replace the placeholder SMTP credentials in `"SmtpSettings"` with your actual email server details for sending out notifications or emails.
