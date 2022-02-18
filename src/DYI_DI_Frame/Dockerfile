FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DYI_DI_Frame/DYI_DI_Frame.csproj", "DYI_DI_Frame/"]
RUN dotnet restore "DYI_DI_Frame/DYI_DI_Frame.csproj"
COPY . .
WORKDIR "/src/DYI_DI_Frame"
RUN dotnet build "DYI_DI_Frame.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DYI_DI_Frame.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DYI_DI_Frame.dll"]
