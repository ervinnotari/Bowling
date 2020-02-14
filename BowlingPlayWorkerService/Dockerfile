#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BowlingPlayWorkerService/BowlingPlayWorkerService.csproj", "BowlingPlayWorkerService/"]
COPY ["Bowling/Bowling.csproj", "Bowling/"]
RUN dotnet restore "BowlingPlayWorkerService/BowlingPlayWorkerService.csproj"
COPY . .
WORKDIR "/src/BowlingPlayWorkerService"
RUN dotnet build "BowlingPlayWorkerService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BowlingPlayWorkerService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BowlingPlayWorkerService.dll"]
