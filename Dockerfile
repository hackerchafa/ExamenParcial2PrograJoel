FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ExamenApi.csproj", "./"]
RUN dotnet restore "ExamenApi.csproj"
COPY . .
RUN dotnet build "ExamenApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExamenApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExamenApi.dll"] 