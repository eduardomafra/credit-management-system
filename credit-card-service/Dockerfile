# Use the official .NET image as a build environment
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the solution and restore any dependencies
COPY *.sln .
COPY src/CreditCardService.Api/CreditCardService.Api.csproj src/CreditCardService.Api/
COPY src/CreditCardService.Application/CreditCardService.Application.csproj src/CreditCardService.Application/
COPY src/CreditCardService.Domain/CreditCardService.Domain.csproj src/CreditCardService.Domain/
COPY src/CreditCardService.Infrastructure/CreditCardService.Infrastructure.csproj src/CreditCardService.Infrastructure/
COPY src/CreditCardService.IoC/CreditCardService.IoC.csproj src/CreditCardService.IoC/
COPY tests/CreditCardService.Tests/CreditCardService.Tests.csproj tests/CreditCardService.Tests/

RUN dotnet restore

# Copy everything else and build
COPY . .
WORKDIR /app/src/CreditCardService.Api
RUN dotnet publish -c Release -o out

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install the .NET SDK for EF migrations
RUN apt-get update && apt-get install -y wget \
    && wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh \
    && chmod +x dotnet-install.sh \
    && ./dotnet-install.sh --channel 8.0 --install-dir /usr/share/dotnet \
    && [ ! -e /usr/bin/dotnet ] && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet || true

# Install dotnet-ef tool in runtime
RUN dotnet tool install --global dotnet-ef

# Install libgssapi-krb5-2 for SQL Server authentication
RUN apt-get install -y libgssapi-krb5-2

# Add wait-for-it.sh
ADD https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

# Copy published application
COPY --from=build /app/src/CreditCardService.Api/out ./

# Copy the necessary project files
COPY --from=build /app /app

# Set environment variable
ENV PATH="$PATH:/root/.dotnet/tools"
ENV ASPNETCORE_ENVIRONMENT Docker

# Use wait-for-it to ensure RabbitMQ is available before starting the app
CMD /wait-for-it.sh customer-service:8080 -- \
    dotnet ef database update --startup-project /app/src/CreditCardService.Api/CreditCardService.Api.csproj --project /app/src/CreditCardService.Infrastructure/CreditCardService.Infrastructure.csproj && \
    dotnet CreditCardService.Api.dll
