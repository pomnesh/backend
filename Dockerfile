# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only .csproj files to leverage Docker layer caching for restore
COPY Pomnesh/Pomnesh.API/Pomnesh.API.csproj Pomnesh/Pomnesh.API/
COPY Pomnesh/Pomnesh.Application/Pomnesh.Application.csproj Pomnesh/Pomnesh.Application/
COPY Pomnesh/Pomnesh.Infrastructure/Pomnesh.Infrastructure.csproj Pomnesh/Pomnesh.Infrastructure/
COPY Pomnesh/Pomnesh.Domain/Pomnesh.Domain.csproj Pomnesh/Pomnesh.Domain/
COPY Pomnesh.sln .

# Restore dependencies
RUN dotnet restore "Pomnesh/Pomnesh.API/Pomnesh.API.csproj"

# Copy the rest of the files
COPY . .

# Publish the project
RUN dotnet publish "Pomnesh/Pomnesh.API/Pomnesh.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Configure to listen on port 8080
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Pomnesh.API.dll"]
