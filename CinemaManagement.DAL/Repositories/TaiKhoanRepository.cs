using System;
using System.Data.Entity;
using System.Linq;

// ============================================================
// FILE: CinemaManagement.DAL/Repositories/TaiKhoanRepository.cs
// Mục đích: Thao tác với bảng TaiKhoan trong DB
// Tầng DAL chỉ biết "lấy data" - KHÔNG chứa logic nghiệp vụ
// ============================================================

namespace CinemaManagement.DAL.Repositories
{
    /// <summary>
    /// Repository xử lý các truy vấn liên quan đến TaiKhoan và NhanVien.
    /// Được BLL gọi để lấy thông tin phục vụ xác thực.
    /// </summary>
    public class TaiKhoanRepository
    {
        // Sử dụng DbContext của EF (tên class tùy theo khi bạn scaffold)
        private readonly CinemaDBEntities _context;

        public TaiKhoanRepository()
        {
            _context = new CinemaDBEntities();
        }

        /// <summary>
        /// Tìm tài khoản theo TenDangNhap.
        /// Trả về null nếu không tìm thấy hoặc tài khoản đã bị xóa/khóa.
        /// DAL trả về toàn bộ object TaiKhoan (kèm VaiTro, NhanVien) để BLL xử lý tiếp.
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập người dùng nhập vào</param>
        /// <returns>Object TaiKhoan với Navigation Properties đã được load</returns>
        public TaiKhoan GetByTenDangNhap(string tenDangNhap)
        {
            // Include() để load VaiTro và NhanVien cùng 1 lần (tránh N+1 query)
            return _context.TaiKhoans
                           .Include(tk => tk.VaiTro)
                           .Include(tk => tk.NhanVien)
                           .FirstOrDefault(tk =>
                               tk.TenDangNhap == tenDangNhap &&
                               tk.IsDeleted == false &&
                               tk.IsActive == true);
        }

        /// <summary>
        /// Cập nhật thời gian đăng nhập gần nhất của tài khoản.
        /// Gọi sau khi xác thực thành công.
        /// </summary>
        public void CapNhatLanDangNhapCuoi(int taiKhoanId)
        {
            var taiKhoan = _context.TaiKhoans.Find(taiKhoanId);
            if (taiKhoan != null)
            {
                taiKhoan.LanDangNhapCuoi = DateTime.Now;
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Kiểm tra TenDangNhap đã tồn tại chưa (dùng khi tạo tài khoản mới).
        /// </summary>
        public bool IsTenDangNhapTonTai(string tenDangNhap)
        {
            return _context.TaiKhoans
                           .Any(tk => tk.TenDangNhap == tenDangNhap &&
                                      tk.IsDeleted == false);
        }

        /// <summary>
        /// Cập nhật mật khẩu mới (đã hash) cho tài khoản.
        /// </summary>
        public bool CapNhatMatKhau(int taiKhoanId, string matKhauHashMoi)
        {
            var taiKhoan = _context.TaiKhoans.Find(taiKhoanId);
            if (taiKhoan == null) return false;

            taiKhoan.MatKhauHash = matKhauHashMoi;
            _context.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
