// ============================================================
// FILE: CinemaManagement.BLL/Helpers/SessionManager.cs
// Mục đích: Lưu thông tin user đang đăng nhập (Singleton)
// Dùng ở bất kỳ Form nào cần kiểm tra quyền hoặc lấy tên NV
// ============================================================

namespace CinemaManagement.BLL.Helpers
{
    /// <summary>
    /// Static class lưu thông tin phiên đăng nhập hiện tại.
    /// Truy cập từ bất kỳ đâu: SessionManager.HoTenNhanVien
    /// </summary>
    public static class SessionManager
    {
        // Thông tin user đang đăng nhập
        public static int    TaiKhoanId      { get; private set; }
        public static int    NhanVienId      { get; private set; }
        public static string TenDangNhap     { get; private set; }
        public static string HoTenNhanVien   { get; private set; }
        public static string TenVaiTro       { get; private set; }
        public static int    VaiTroId        { get; private set; }
        public static bool   DaDangNhap      => TaiKhoanId > 0;

        // Hằng số VaiTroId để kiểm tra quyền
        public const int VAI_TRO_ADMIN      = 1;
        public const int VAI_TRO_QUAN_LY    = 2;
        public const int VAI_TRO_NHAN_VIEN  = 3;

        /// <summary>
        /// Ghi nhận thông tin sau khi đăng nhập thành công.
        /// Gọi từ frmLogin sau khi AuthService.DangNhap trả về ThanhCong = true.
        /// </summary>
        public static void DangNhap(
            int taiKhoanId, int nhanVienId,
            string tenDangNhap, string hoTenNhanVien,
            string tenVaiTro, int vaiTroId)
        {
            TaiKhoanId      = taiKhoanId;
            NhanVienId      = nhanVienId;
            TenDangNhap     = tenDangNhap;
            HoTenNhanVien   = hoTenNhanVien;
            TenVaiTro       = tenVaiTro;
            VaiTroId        = vaiTroId;
        }

        /// <summary>
        /// Xóa toàn bộ thông tin session khi đăng xuất.
        /// </summary>
        public static void DangXuat()
        {
            TaiKhoanId      = 0;
            NhanVienId      = 0;
            TenDangNhap     = null;
            HoTenNhanVien   = null;
            TenVaiTro       = null;
            VaiTroId        = 0;
        }

        /// <summary>
        /// Kiểm tra user hiện tại có quyền Admin không.
        /// Dùng để ẩn/hiện các menu nhạy cảm trên MainForm.
        /// </summary>
        public static bool IsAdmin()       => VaiTroId == VAI_TRO_ADMIN;
        public static bool IsQuanLy()      => VaiTroId == VAI_TRO_ADMIN
                                           || VaiTroId == VAI_TRO_QUAN_LY;
        public static bool IsNhanVien()    => DaDangNhap; // Mọi role đều là nhân viên
    }
}
