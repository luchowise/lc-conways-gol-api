# Base Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ["Lc.MicroService.ConwayGoL/Lc.MicroService.ConwayGoL.csproj", "Lc.MicroService.ConwayGoL/"]
COPY ["Lc.MicroService.ConwayGoL.Persistence/Lc.MicroService.ConwayGoL.Persistence.csproj", "Lc.MicroService.ConwayGoL.Persistence/"]
COPY ["Lc.MicroService.ConwayGoL.Services/Lc.MicroService.ConwayGoL.Services.csproj", "Lc.MicroService.ConwayGoL.Services/"]
COPY ["Lc.MicroService.ConwayGoL.Models/Lc.MicroService.ConwayGoL.Models.csproj", "Lc.MicroService.ConwayGoL.Models/"]
COPY ["Lc.MicroService.ConwayGoL.Factories/Lc.MicroService.ConwayGoL.Factories.csproj", "Lc.MicroService.ConwayGoL.Factories/"]

RUN dotnet restore "Lc.MicroService.ConwayGoL/Lc.MicroService.ConwayGoL.csproj"
COPY . .
WORKDIR "/src/Lc.MicroService.ConwayGoL"
RUN dotnet build "Lc.MicroService.ConwayGoL.csproj" -c Release -o /app/build

# Publish Stage
FROM build AS publish
RUN dotnet publish "Lc.MicroService.ConwayGoL.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime Stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "Lc.MicroService.ConwayGoL.dll"]
