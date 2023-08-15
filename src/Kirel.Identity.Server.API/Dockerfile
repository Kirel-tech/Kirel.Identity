FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "/src/Kirel.Identity.Server.API/Kirel.Identity.Server.API.csproj"
WORKDIR "/src/Kirel.Identity.Server.API"
RUN dotnet build "Kirel.Identity.Server.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kirel.Identity.Server.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kirel.Identity.Server.API.dll"]
