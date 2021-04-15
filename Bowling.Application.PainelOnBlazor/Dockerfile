#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Bowling.Application.PainelOnBlazor/Bowling.Application.PainelOnBlazor.csproj", "Bowling.Application.PainelOnBlazor/"]
COPY ["Bowling.Domain.Game/Bowling.Domain.Game.csproj", "Bowling.Domain.Game/"]
COPY ["Bowling.Infra.CrossCutting.IoC/Bowling.Infra.CrossCutting.IoC.csproj", "Bowling.Infra.CrossCutting.IoC/"]
COPY ["Bowling.Infra.Utilities/Bowling.Infra.Utilities.csproj", "Bowling.Infra.Utilities/"]
COPY ["Bowling.Service.Game/Bowling.Service.Game.csproj", "Bowling.Service.Game/"]
COPY ["Bowling.Service.Bus.MQTT/Bowling.Service.Bus.MQTT.csproj", "Bowling.Service.Bus.MQTT/"]
RUN dotnet restore "Bowling.Application.PainelOnBlazor/Bowling.Application.PainelOnBlazor.csproj"
COPY . .
WORKDIR "/src/Bowling.Application.PainelOnBlazor"
RUN dotnet build "Bowling.Application.PainelOnBlazor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bowling.Application.PainelOnBlazor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bowling.Application.PainelOnBlazor.dll"]
