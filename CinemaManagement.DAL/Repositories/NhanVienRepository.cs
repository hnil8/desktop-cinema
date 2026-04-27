using System;
using System.Collections.Generic;
using System.Linq;

// ============================================================
// FILE: CinemaManagement.DAL/Repositories/NhanVienRepository.cs
// Tầng DAL: Thao tác với bảng NhanViens và CaLamViecs
// Context: CinemaDBEntities (tên sinh ra bởi EF6 Database First)
// Tên bảng dùng đúng theo tên DbSet: NhanViens, CaLamViecs
// ============================================================

namespace CinemaManagement.DAL.Repositories
{
    public class NhanVienRepository
    {
        private readonly CinemaDBEntities _context;

        public NhanVienRepository()
        {
            _context = new CinemaDBEntities();
        }

        // ══════════════════════════════════════════════════════
        // PHẦN 1: CRUD NHÂN VIÊN
        // ══════════════════════════════════════════════════════

        // ----------------------------------------------------------
        // GET LIST: Lấy danh sách nhân viên chưa xóa mềm
        // Include TaiKhoan để lấy thêm VaiTro liên quan
        // ----------------------------------------------------------
        // 1. Sửa hàm GetList()
        public List<NhanVien> GetList()
        {
            return _context.NhanViens
                           .Where(nv => nv.IsDeleted == false)
                           .OrderBy(nv => nv.HoTen)
                           .ToList();
        }

        // ----------------------------------------------------------
        // GET BY ID: Lấy 1 nhân viên theo khóa chính
        // ----------------------------------------------------------
        public NhanVien GetById(int nhanVienId)
        {
            return _context.NhanViens
                           .FirstOrDefault(nv => nv.NhanVienId == nhanVienId
                                              && nv.IsDeleted == false);
        }

        // ----------------------------------------------------------
        // SEARCH: Tìm kiếm theo tên hoặc SĐT
        // ----------------------------------------------------------
        // 2. Sửa hàm Search()
        public List<NhanVien> Search(string keyword)
        {
            string kw = keyword.Trim().ToLower();
            return _context.NhanViens
                           .Where(nv => nv.IsDeleted == false
                                     && (nv.HoTen.ToLower().Contains(kw)
                                      || nv.SoDienThoai.Contains(kw)))
                           .OrderBy(nv => nv.HoTen)
                           .ToList();
        }

        // ----------------------------------------------------------
        // INSERT: Thêm nhân viên mới
        // ----------------------------------------------------------
        public int Insert(NhanVien nhanVien)
        {
            nhanVien.IsDeleted = false;
            nhanVien.NgayVaoLam = nhanVien.NgayVaoLam == null
                                    ? DateTime.Today
                                    : nhanVien.NgayVaoLam;

            _context.NhanViens.Add(nhanVien);
            _context.SaveChanges();
            return nhanVien.NhanVienId;
        }

        // ----------------------------------------------------------
        // UPDATE: Cập nhật thông tin nhân viên
        // Chỉ sửa các cột nghiệp vụ, KHÔNG sửa NgayTao / IsDeleted
        // ----------------------------------------------------------
        public bool Update(NhanVien nvCapNhat)
        {
            NhanVien existing = _context.NhanViens.Find(nvCapNhat.NhanVienId);
            if (existing == null || existing.IsDeleted == true)
                return false;

            existing.HoTen = nvCapNhat.HoTen;
            existing.SoDienThoai = nvCapNhat.SoDienThoai;
            existing.Email = nvCapNhat.Email;
            existing.GioiTinh = nvCapNhat.GioiTinh;
            existing.NgaySinh = nvCapNhat.NgaySinh;
            existing.DiaChi = nvCapNhat.DiaChi;
            existing.NgayVaoLam = nvCapNhat.NgayVaoLam;

            _context.SaveChanges();
            return true;
        }

        // ----------------------------------------------------------
        // SOFT DELETE: Xóa mềm nhân viên
        // Ghi lại thời điểm xóa vào cột NgayXoa
        // ----------------------------------------------------------
        public bool SoftDelete(int nhanVienId)
        {
            NhanVien nv = _context.NhanViens.Find(nhanVienId);
            if (nv == null || nv.IsDeleted == true)
                return false;

            nv.IsDeleted = true;
            nv.NgayXoa = DateTime.Now;

            _context.SaveChanges();
            return true;
        }

        // ----------------------------------------------------------
        // CHECK DUPLICATE SĐT: Kiểm tra trùng số điện thoại
        // excludeId: bỏ qua record đang sửa
        // ----------------------------------------------------------
        public bool IsSoDienThoaiTonTai(string sdt, int excludeId = 0)
        {
            return _context.NhanViens
                           .Any(nv => nv.IsDeleted == false
                                   && nv.SoDienThoai == sdt.Trim()
                                   && nv.NhanVienId != excludeId);
        }

        // ----------------------------------------------------------
        // CHECK CONSTRAINT: Kiểm tra NV có tài khoản chưa
        // Dùng trước khi xóa để cảnh báo
        // ----------------------------------------------------------
        public bool CoTaiKhoan(int nhanVienId)
        {
            return _context.TaiKhoans
                           .Any(tk => tk.NhanVienId == nhanVienId
                                   && tk.IsDeleted == false);
        }

        // ══════════════════════════════════════════════════════
        // PHẦN 2: QUẢN LÝ CA LÀM VIỆC
        // ══════════════════════════════════════════════════════

        // ----------------------------------------------------------
        // GET CA ĐANG MỞ của nhân viên (nếu có)
        // ----------------------------------------------------------
        public CaLamViec GetCaDangMo(int nhanVienId)
        {
            return _context.CaLamViecs
                           .FirstOrDefault(ca => ca.NhanVienId == nhanVienId
                                              && ca.TrangThai == "DangMo");
        }

        // ----------------------------------------------------------
        // GET LỊCH SỬ CA của nhân viên (10 ca gần nhất)
        // ----------------------------------------------------------
        public List<CaLamViec> GetLichSuCa(int nhanVienId)
        {
            return _context.CaLamViecs
                           .Where(ca => ca.NhanVienId == nhanVienId)
                           .OrderByDescending(ca => ca.ThoiGianMoCa)
                           .Take(10)
                           .ToList();
        }

        // ----------------------------------------------------------
        // MỞ CA: Insert bản ghi CaLamViec mới với TrangThai = 'DangMo'
        // ----------------------------------------------------------
        public int MoCa(CaLamViec ca)
        {
            ca.TrangThai = "DangMo";
            ca.ThoiGianMoCa = DateTime.Now;
            ca.ThoiGianChotCa = null;
            ca.TongThuTienMat = 0;
            ca.TongThuChuyenKhoan = 0;
            ca.TongThuThe = 0;

            _context.CaLamViecs.Add(ca);
            _context.SaveChanges();
            return ca.CaId;
        }

        // ----------------------------------------------------------
        // ĐÓNG CA: Cập nhật ThoiGianChotCa, tổng thu và trạng thái
        // ----------------------------------------------------------
        public bool DongCa(
            int caId,
            decimal tongThuTienMat,
            decimal tongThuChuyenKhoan,
            decimal tongThuThe,
            string ghiChu)
        {
            CaLamViec ca = _context.CaLamViecs.Find(caId);
            if (ca == null || ca.TrangThai != "DangMo")
                return false;

            ca.TrangThai = "DaChotCa";
            ca.ThoiGianChotCa = DateTime.Now;
            ca.TongThuTienMat = tongThuTienMat;
            ca.TongThuChuyenKhoan = tongThuChuyenKhoan;
            ca.TongThuThe = tongThuThe;
            ca.GhiChuChotCa = ghiChu;

            _context.SaveChanges();
            return true;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}