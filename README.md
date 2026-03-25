# 🎬 Quản Lý Phim & Lịch Chiếu

Ứng dụng desktop Windows quản lý phim, phòng chiếu, suất chiếu và bán vé — xây dựng bằng **C# WinForms (.NET 6)** theo kiến trúc 3 lớp.

## 📸 Giao diện

Ứng dụng sử dụng giao diện **Dark Theme** hiện đại với:
- Sidebar điều hướng
- Dashboard tổng quan
- DataGridView theo phong cách tối

## 🏗️ Kiến trúc

```
QuanLyPhimVaLichChieu/
├── Models/          # Các lớp thực thể (Phim, PhongChieu, SuatChieu, Ve, ...)
├── DataAccess/      # Tầng truy cập dữ liệu (DAL)
├── BusinessLogic/   # Tầng xử lý nghiệp vụ (BLL)
├── Forms/           # Giao diện WinForms + UITheme
├── Database/        # Script tạo CSDL MySQL
├── Program.cs       # Entry point
└── App.config       # Cấu hình connection string
```

## ⚙️ Yêu cầu

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) trở lên
- **MySQL 8.0+** (hoặc MariaDB)

## 🚀 Hướng dẫn chạy

### 1. Tạo cơ sở dữ liệu

```bash
mysql -u root -p < Database/CreateDatabase.sql
```

> ⚠️ Nếu tài khoản `root` có mật khẩu, nhập mật khẩu khi được hỏi.

### 2. Cấu hình connection string

Mở `App.config` và chỉnh thông tin kết nối cho khớp:

```xml
<add name="QuanLyPhimDB" 
     connectionString="Server=localhost;Port=3306;Database=QuanLyPhimDB;Uid=root;Pwd=;" 
     providerName="MySqlConnector" />
```

> Thay `Uid` và `Pwd` nếu bạn dùng tài khoản/mật khẩu khác.

### 3. Chạy ứng dụng

```bash
dotnet restore
dotnet run
```

Hoặc mở `QuanLyPhimVaLichChieu.sln` bằng **Visual Studio** và nhấn **F5**.

## 📋 Chức năng

| Chức năng | Mô tả |
|-----------|-------|
| **Quản lý phim** | Thêm, sửa, xóa, tìm kiếm phim theo tên và thể loại |
| **Phòng chiếu** | Quản lý phòng chiếu (2D, 3D, IMAX) và trạng thái |
| **Suất chiếu** | Tạo và quản lý lịch chiếu, lọc theo phim/ngày |
| **Bán vé** | Bán vé, chọn ghế, hủy vé |
| **Thống kê** | Báo cáo vé bán theo phim, tổng doanh thu |

## 🛠️ Công nghệ

- **C# / .NET 6** — WinForms
- **MySQL** — Cơ sở dữ liệu
- **MySqlConnector** — Kết nối database
- **Kiến trúc 3 lớp** — Models / DataAccess / BusinessLogic
