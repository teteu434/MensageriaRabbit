FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["src/Producer/Producer.csproj", "./"]
RUN dotnet restore "Producer.csproj"

COPY ["src/Producer/.", "."]
RUN dotnet publish "Producer.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Producer.dll"]