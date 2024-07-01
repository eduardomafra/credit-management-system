# Use the official .NET image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore any dependencies
COPY *.sln .
COPY src/CustomerService.API/CustomerService.API.csproj src/CustomerService.API/
COPY src/CustomerService.Application/CustomerService.Application.csproj src/CustomerService.Application/
COPY src/CustomerService.Domain/CustomerService.Domain.csproj src/CustomerService.Domain/
COPY src/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj src/CustomerService.Infrastructure/
COPY src/CustomerService.IoC/CustomerService.IoC.csproj src/CustomerService.IoC/
COPY tests/CustomerService.Tests/CustomerService.Tests.csproj tests/CustomerService.Tests/

RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/src/CustomerService.API
RUN dotnet publish -c Release -o out

# Generate runtime image using SDK for migrations
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS runtime
WORKDIR /app

# Add wait-for-it.sh
ADD https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

# Copy the necessary project files
COPY --from=build /app /app

# Copy published application
COPY --from=build /app/src/CustomerService.API/out ./

# Set environment variable
ENV PATH="$PATH:/root/.dotnet/tools"
ENV ASPNETCORE_ENVIRONMENT Docker

# Install dotnet-ef tool in runtime for applying migrations
RUN dotnet tool install --global dotnet-ef

# Use wait-for-it to ensure RabbitMQ and SQL Server are available before starting the app
CMD /wait-for-it.sh rabbitmq:5672 -- /wait-for-it.sh sqlserver:1433 -- \
    dotnet ef database update --startup-project /app/src/CustomerService.API/CustomerService.API.csproj --project /app/src/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj && \
    dotnet CustomerService.API.dll
