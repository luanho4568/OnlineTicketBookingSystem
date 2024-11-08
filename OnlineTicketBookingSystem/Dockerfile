# Sử dụng .NET SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Thiết lập thư mục làm việc trong container
WORKDIR /app

# Sao chép các tệp .csproj vào container (đảm bảo đường dẫn chính xác)
COPY OnlineTicketBookingSystem/OnlineTicketBookingSystem.csproj ./OnlineTicketBookingSystem.csproj

# Khôi phục các gói NuGet
RUN dotnet restore

# Sao chép toàn bộ mã nguồn vào container
COPY OnlineTicketBookingSystem/ .

# Xây dựng ứng dụng
RUN dotnet publish -c Release -o /app/publish

# Thiết lập image chạy
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Sao chép ứng dụng đã xây dựng từ bước trước
COPY --from=build /app/publish .

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "OnlineTicketBookingSystem.dll"]
