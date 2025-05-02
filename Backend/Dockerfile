FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app
COPY . .

WORKDIR /app/MoneyTracker.MS/MoneyTracker.API

RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/MoneyTracker.MS/MoneyTracker.API/out ./
RUN find . -name "*.pdb" -type f -delete

ENTRYPOINT ["dotnet", "MoneyTracker.API.dll"]

# docker build -t moneyTracker.api -f Dockerfile.Api .
# docker run -d -p 8080:80 --name moneyTracker.api moneyTracker.api
