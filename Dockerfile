FROM mcr.microsoft.com/dotnet/aspnet:3.1-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1-bionic AS publish
WORKDIR /src
COPY . .
WORKDIR /src/src/BackgroundApp
RUN dotnet publish BackgroundApp.csproj -c Release -o /out/publish

FROM base AS final
COPY --from=publish /out/publish .
ENV TZ=America/Sao_Paulo
ENTRYPOINT ["dotnet", "BackgroundApp.dll"]