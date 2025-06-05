## CI/CD (GitHub Actions)

- The workflow `.github/workflows/build.yml` runs automatically after every `push` to the `main` branch.
- It builds the Web API application in an Ubuntu environment.
- It verifies that the project compiles successfully.

## Connecting to the Deployed Application

- The application is deployed on Azure App Service and is accessible at:  
  `https://ecommerce-orders-api-h5ecd5fsgzbscea8.northeurope-01.azurewebsites.net`

- Example endpoints:
  - `GET /api/orders` – Retrieve all orders
  - `POST /api/orders` – Create a new order
  - `DELETE /api/orders/{id}` – Delete an order
  - `DELETE /api/orders/{orderId}/{productId}` – Remove a product from an order

## Azure Services Used

- **Azure App Service** – Hosts the ASP.NET Core web API
- **Azure SQL Database** – Stores order and product data
- **Azure Resource Group** – Organizes and manages Azure resources used by the application
- **Application Insights** – For application monitoring and diagnostics
