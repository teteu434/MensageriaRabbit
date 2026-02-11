FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["src/Consumer3/Consumer3.csproj", "./"]
RUN dotnet restore "Consumer3.csproj"

COPY ["src/Consumer3/.", "."]
RUN dotnet publish "Consumer3.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/runtime:8.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Consumer3.dll"]