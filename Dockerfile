FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BandHub.Bff/BandHub.Bff.csproj", "BandHub.Bff/"]
RUN dotnet restore "BandHub.Bff/BandHub.Bff.csproj"
COPY . .
WORKDIR "/src/BandHub.Bff"
RUN dotnet build "BandHub.Bff.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BandHub.Bff.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BandHub.Bff.dll"]
