FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Kirel.Identity.Server.API/Identity.API.csproj", "Identity.API/"]
RUN dotnet restore "Kirel.Identity.Server.API/Identity.API.csproj"
COPY . .
WORKDIR "/src/Identity.API"
RUN dotnet build "Kirel.Identity.Server.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kirel.Identity.Server.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kirel.Identity.Server.API.dll"]
