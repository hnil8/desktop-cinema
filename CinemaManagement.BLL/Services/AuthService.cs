using System;
using BCrypt.Net;   
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.BLL/Services/AuthService.cs
// Mục đích: Xử lý toàn bộ nghiệp vụ xác thực (Authentication)
// Tầng BLL chứa logic, KHÔNG biết gì về UI hay SQL trực tiếp
// ============================================================

namespace CinemaManagement.BLL.Services
{
    // DTO: Đóng gói thông tin user sau khi đăng nhập thành công.
    // Dùng DTO thay vì truyền thẳng Entity để tách biệt tầng.
    public class LoginResultDto
    {
        public bool ThanhCong       { get; set; }
        public string ThongBaoLoi   { get; set; }
        public int    TaiKhoanId    { get; set; }
        public string TenDangNhap   { get; set; }
        public string HoTenNhanVien { get; set; }
        public string TenVaiTro     { get; set; }  // 'Admin', 'Quan ly', 'Nhan vien'
        public int    VaiTroId      { get; set; }
        public int    NhanVienId    { get; set; }
    }

    /// <summary>
    /// Service xử lý đăng nhập, đổi mật khẩu, và quản lý session người dùng.
    /// </summary>
    public class AuthService
    {
        private readonly TaiKhoanRepository _taiKhoanRepo;

        public AuthService()
        {
            _taiKhoanRepo = new TaiKhoanRepository();
        }

        // ----------------------------------------------------------
        // HẰNG SỐ CẤU HÌNH
        // ----------------------------------------------------------
        /// <summary>
        /// BCrypt work factor: 11 = cân bằng bảo mật vs tốc độ.
        /// Tăng lên 12-13 nếu cần bảo mật cao hơn (chậm hơn).
        /// </summary>
        private const int BCRYPT_WORK_FACTOR = 11;

        // ----------------------------------------------------------
        // PUBLIC METHODS
        // ----------------------------------------------------------

        /// <summary>
        /// Kiểm tra thông tin đăng nhập.
        /// Quy trình: Tìm TK theo tên → Verify BCrypt hash → Trả về kết quả
        /// </summary>
        /// <param name="tenDangNhap">Username từ TextBox</param>
        /// <param name="matKhauNhapVao">Plain-text password từ TextBox</param>
        public LoginResultDto DangNhap(string tenDangNhap, string matKhauNhapVao)
        {
            // --- Validate input cơ bản ---
            if (string.IsNullOrWhiteSpace(tenDangNhap) || string.IsNullOrWhiteSpace(matKhauNhapVao))
            {
                return Fail("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
            }

            try
            {
                // --- Bước 1: Truy vấn DB qua DAL ---
                var taiKhoan = _taiKhoanRepo.GetByTenDangNhap(tenDangNhap.Trim());

                // --- Bước 2: Kiểm tra tài khoản có tồn tại không ---
                // QUAN TRỌNG: Không phân biệt "sai username" vs "sai password"
                // để tránh attacker dò tìm username hợp lệ (Security best practice)
                if (taiKhoan == null)
                {
                    return Fail("Tên đăng nhập hoặc mật khẩu không chính xác.");
                }

                // --- Bước 3: Verify mật khẩu bằng BCrypt ---
                // BCrypt.Verify so sánh plain-text với hash đã lưu trong DB
                // Hàm này tự xử lý salt bên trong, không cần truyền salt riêng
                bool matKhauDung = BCrypt.Net.BCrypt.Verify(matKhauNhapVao, taiKhoan.MatKhauHash);

                if (!matKhauDung)
                {
                    return Fail("Tên đăng nhập hoặc mật khẩu không chính xác.");
                }

                // --- Bước 4: Cập nhật thời gian đăng nhập ---
                _taiKhoanRepo.CapNhatLanDangNhapCuoi(taiKhoan.TaiKhoanId);

                // --- Bước 5: Trả về kết quả thành công ---
                return new LoginResultDto
                {
                    ThanhCong       = true,
                    ThongBaoLoi     = string.Empty,
                    TaiKhoanId      = taiKhoan.TaiKhoanId,
                    TenDangNhap     = taiKhoan.TenDangNhap,
                    HoTenNhanVien   = taiKhoan.NhanVien?.HoTen ?? "Không rõ",
                    TenVaiTro       = taiKhoan.VaiTro?.TenVaiTro ?? "Không rõ",
                    VaiTroId        = taiKhoan.VaiTroId,
                    NhanVienId      = taiKhoan.NhanVienId
                };
            }
            catch (Exception ex)
            {
                // Log lỗi ra file/console (sau này tích hợp NLog/Serilog)
                Console.WriteLine($"[AuthService.DangNhap] Lỗi: {ex.Message}");
                return Fail("Lỗi hệ thống. Vui lòng thử lại sau.");
            }
        }

        /// <summary>
        /// Tạo BCrypt hash từ mật khẩu plain-text.
        /// Dùng khi: Admin tạo tài khoản mới, hoặc user đổi mật khẩu.
        /// </summary>
        public string HashMatKhau(string matKhauMoi)
        {
            if (string.IsNullOrWhiteSpace(matKhauMoi))
                throw new ArgumentException("Mật khẩu không được để trống.");

            // BCrypt tự sinh salt ngẫu nhiên và nhúng vào trong chuỗi hash
            // Mỗi lần gọi HashPassword cho cùng input → kết quả hash khác nhau (do salt khác)
            return BCrypt.Net.BCrypt.HashPassword(matKhauMoi, BCRYPT_WORK_FACTOR);
        }

        /// <summary>
        /// Đổi mật khẩu: Xác thực mật khẩu cũ trước, sau đó lưu hash mới.
        /// </summary>
        public (bool ThanhCong, string ThongBao) DoiMatKhau(
            int taiKhoanId, string matKhauCu, string matKhauMoi, string xacNhanMoi)
        {
            // --- Validate ---
            if (string.IsNullOrWhiteSpace(matKhauCu) ||
                string.IsNullOrWhiteSpace(matKhauMoi) ||
                string.IsNullOrWhiteSpace(xacNhanMoi))
                return (false, "Vui lòng nhập đầy đủ thông tin.");

            if (matKhauMoi != xacNhanMoi)
                return (false, "Mật khẩu mới và xác nhận không khớp.");

            if (matKhauMoi.Length < 6)
                return (false, "Mật khẩu mới phải có ít nhất 6 ký tự.");

            try
            {
                var taiKhoan = _taiKhoanRepo.GetByTenDangNhap(
                    /* Cần truy vấn theo ID - tạm thời dùng workaround */
                    // TODO: Thêm method GetById vào Repository
                    ""
                );

                // Kiểm tra mật khẩu cũ
                if (!BCrypt.Net.BCrypt.Verify(matKhauCu, taiKhoan?.MatKhauHash ?? ""))
                    return (false, "Mật khẩu hiện tại không chính xác.");

                // Hash mật khẩu mới và lưu
                string hashMoi = HashMatKhau(matKhauMoi);
                _taiKhoanRepo.CapNhatMatKhau(taiKhoanId, hashMoi);
                return (true, "Đổi mật khẩu thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AuthService.DoiMatKhau] Lỗi: {ex.Message}");
                return (false, "Lỗi hệ thống khi đổi mật khẩu.");
            }
        }

        // ----------------------------------------------------------
        // PRIVATE HELPERS
        // ----------------------------------------------------------
        private LoginResultDto Fail(string thongBao) =>
            new LoginResultDto { ThanhCong = false, ThongBaoLoi = thongBao };
    }
}
