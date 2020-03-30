#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["BaseFrameworkDemo/WebApiCoreFx/WebApiCoreFx.csproj", "BaseFrameworkDemo/WebApiCoreFx/"]
COPY ["BaseFrameworkDemo/DBLayer/DBLayer.csproj", "BaseFrameworkDemo/DBLayer/"]
COPY ["BaseFrameworkDemo/IDBLayer/IDBLayer.csproj", "BaseFrameworkDemo/IDBLayer/"]
COPY ["BaseFrameworkDemo/DBModel/DBModel.csproj", "BaseFrameworkDemo/DBModel/"]
COPY ["BaseFrameworkDemo/LogicLayer/LogicLayer.csproj", "BaseFrameworkDemo/LogicLayer/"]
COPY ["BaseFrameworkDemo/ILogicLayer/ILogicLayer.csproj", "BaseFrameworkDemo/ILogicLayer/"]
COPY ["BaseFrameworkDemo/AopDLL/AopDLL.csproj", "BaseFrameworkDemo/AopDLL/"]
COPY ["BaseFrameworkDemo/CustomizeMiddleware/CustomizeMiddleware.csproj", "BaseFrameworkDemo/CustomizeMiddleware/"]
RUN dotnet restore "BaseFrameworkDemo/WebApiCoreFx/WebApiCoreFx.csproj"
COPY . .
WORKDIR "/src/BaseFrameworkDemo/WebApiCoreFx"
RUN dotnet build "WebApiCoreFx.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiCoreFx.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiCoreFx.dll"]