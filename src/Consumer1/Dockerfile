FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["src/Consumer1/Consumer1.csproj", "./"]
RUN dotnet restore "Consumer1.csproj"

COPY ["src/Consumer1/.", "."]
RUN dotnet publish "Consumer1.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Consumer1.dll"]