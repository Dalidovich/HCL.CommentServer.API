#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HCL.CommentServer.API/HCL.CommentServer.API.csproj", "HCL.CommentServer.API/"]
COPY ["HCL.CommentServer.API.BLL/HCL.CommentServer.API.BLL.csproj", "HCL.CommentServer.API.BLL/"]
COPY ["HCL.CommentServer.API.DAL/HCL.CommentServer.API.DAL.csproj", "HCL.CommentServer.API.DAL/"]
COPY ["HCL.CommentServer.API.Domain/HCL.CommentServer.API.Domain.csproj", "HCL.CommentServer.API.Domain/"]
RUN dotnet restore "HCL.CommentServer.API/HCL.CommentServer.API.csproj"
COPY . .
WORKDIR "/src/HCL.CommentServer.API"
RUN dotnet build "HCL.CommentServer.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HCL.CommentServer.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HCL.CommentServer.API.dll"]