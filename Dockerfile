# Sử dụng image .NET 8
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Dùng image .NET SDK để build ứng dụng
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép tất cả tệp vào container
COPY ["OnlineTicketBookingSystem/OnlineTicketBookingSystem.csproj", "OnlineTicketBookingSystem/"]
COPY ["OnlineTicketBookingSystem.Models/OnlineTicketBookingSystem.Models.csproj", "OnlineTicketBookingSystem.Models/"]
COPY ["OnlineTicketBookingSystem.Utility/OnlineTicketBookingSystem.Utility.csproj", "OnlineTicketBookingSystem.Utility/"]
COPY ["OnlineTicketBookingSystem.DAL/OnlineTicketBookingSystem.DAL.csproj", "OnlineTicketBookingSystem.DAL/"]

# Khôi phục các dependencies
RUN dotnet restore "OnlineTicketBookingSystem/OnlineTicketBookingSystem.csproj"

# Sao chép toàn bộ mã nguồn vào container
COPY . .

# Build ứng dụng
WORKDIR "/src/OnlineTicketBookingSystem"
RUN dotnet publish -c Release -o /app/publish

# Thiết lập image chạy ứng dụng
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "OnlineTicketBookingSystem.dll"]
