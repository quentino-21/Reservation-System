FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ReservationSystem.API/ReservationSystem.API.csproj", "ReservationSystem.API/"]
COPY ["ReservationSystem.Application/ReservationSystem.Application.csproj", "ReservationSystem.Application/"]
COPY ["ReservationSystem.Domain/ReservationSystem.Domain.csproj", "ReservationSystem.Domain/"]
COPY ["ReservationSystem.Infrastructure/ReservationSystem.Infrastructure.csproj", "ReservationSystem.Infrastructure/"]
RUN dotnet restore "ReservationSystem.API/ReservationSystem.API.csproj"
COPY . .
WORKDIR "/src/ReservationSystem.API"
RUN dotnet build "./ReservationSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ReservationSystem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ReservationSystem.API.dll"]
