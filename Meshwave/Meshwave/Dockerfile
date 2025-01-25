FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Meshwave/Meshwave.csproj", "Meshwave/"]
RUN dotnet restore "Meshwave/Meshwave.csproj"
COPY . .
WORKDIR "/src/Meshwave"
RUN dotnet build "Meshwave.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Meshwave.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Meshwave.dll"]
