FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["exchaRazor02/exchaRazor02.csproj", "exchaRazor02/"]
RUN dotnet restore "exchaRazor02/exchaRazor02.csproj"
COPY . .
WORKDIR "/src/exchaRazor02"
RUN dotnet build "exchaRazor02.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "exchaRazor02.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "exchaRazor02.dll"]