using System;
using System.Collections.Generic;
using System.Linq;

// ============================================================
// FILE: CinemaManagement.DAL/Repositories/LichChieuRepository.cs
// Tầng DAL: Thao tác DB, KHÔNG chứa logic nghiệp vụ
// Định nghĩa thêm class LichChieuHienThi (ViewModel JOIN) tại đây
// để BLL có thể dùng mà không cần tạo class riêng
// ============================================================

namespace CinemaManagement.DAL.Repositories
{
    // ----------------------------------------------------------
    // VIEW MODEL: Kết quả JOIN LichChieu + Phim + PhongChieu
    // Đặt trong DAL để tái sử dụng ở BLL (BLL reference DAL)
    // ----------------------------------------------------------
    public class LichChieuHienThi
    {
        public int      LichChieuId  { get; set; }
        public int      PhimId       { get; set; }
        public int      PhongId      { get; set; }
        public string   TenPhim      { get; set; }
        public string   TenPhong     { get; set; }
        public DateTime GioBatDau    { get; set; }
        public DateTime GioKetThuc   { get; set; }
        public decimal  GiaVeCoBan   { get; set; }
        public string   TrangThai    { get; set; }
    }

    public class LichChieuRepository
    {
        private readonly CinemaDBEntities _context;

        public LichChieuRepository()
        {
            _context = new CinemaDBEntities();
        }

        // ----------------------------------------------------------
        // GET LIST: Lấy danh sách lịch chiếu kèm Tên Phim, Tên Phòng
        // Dùng LINQ JOIN tường minh, không phụ thuộc tên Navigation Property
        // ----------------------------------------------------------
        public List<LichChieuHienThi> GetList()
        {
            var query =
                from lc in _context.LichChieux
                join p  in _context.Phims       on lc.PhimId  equals p.PhimId
                join pc in _context.PhongChieux on lc.PhongId equals pc.PhongId
                where lc.IsDeleted == false
                orderby lc.GioBatDau descending
                select new LichChieuHienThi
                {
                    LichChieuId = lc.LichChieuId,
                    PhimId      = lc.PhimId,
                    PhongId     = lc.PhongId,
                    TenPhim     = p.TenPhim,
                    TenPhong    = pc.TenPhong,
                    GioBatDau   = lc.GioBatDau,
                    GioKetThuc  = lc.GioKetThuc,
                    GiaVeCoBan  = lc.GiaVeCoBan,
                    TrangThai   = lc.TrangThai
                };

            return query.ToList();
        }

        // ----------------------------------------------------------
        // SEARCH: Tìm kiếm theo Tên Phim (không phân biệt hoa/thường)
        // ----------------------------------------------------------
        public List<LichChieuHienThi> SearchByTenPhim(string keyword)
        {
            string kw = keyword.Trim().ToLower();

            var query =
                from lc in _context.LichChieux
                join p  in _context.Phims       on lc.PhimId  equals p.PhimId
                join pc in _context.PhongChieux on lc.PhongId equals pc.PhongId
                where lc.IsDeleted == false
                   && p.TenPhim.ToLower().Contains(kw)
                orderby lc.GioBatDau descending
                select new LichChieuHienThi
                {
                    LichChieuId = lc.LichChieuId,
                    PhimId      = lc.PhimId,
                    PhongId     = lc.PhongId,
                    TenPhim     = p.TenPhim,
                    TenPhong    = pc.TenPhong,
                    GioBatDau   = lc.GioBatDau,
                    GioKetThuc  = lc.GioKetThuc,
                    GiaVeCoBan  = lc.GiaVeCoBan,
                    TrangThai   = lc.TrangThai
                };

            return query.ToList();
        }

        // ----------------------------------------------------------
        // GET BY ID: Lấy entity đầy đủ (dùng khi Edit)
        // ----------------------------------------------------------
        public LichChieu GetById(int lichChieuId)
        {
            return _context.LichChieux
                           .FirstOrDefault(lc => lc.LichChieuId == lichChieuId
                                              && lc.IsDeleted == false);
        }

        // ----------------------------------------------------------
        // KIỂM TRA TRÙNG LỊCH
        // Logic: Hai suất chiếu bị trùng nếu cùng phòng VÀ khoảng thời gian chồng nhau
        // Công thức chồng nhau: A.start < B.end  AND  A.end > B.start
        // excludeId: bỏ qua record đang sửa để không tự trùng với chính nó
        // ----------------------------------------------------------
        public bool KiemTraTrungLich(
            int phongId, DateTime gioBatDau, DateTime gioKetThuc, int excludeId = 0)
        {
            return _context.LichChieux
                           .Any(lc => lc.PhongId    == phongId
                                   && lc.IsDeleted  == false
                                   && lc.TrangThai  != "HuyChieu"
                                   && lc.LichChieuId != excludeId
                                   // Điều kiện chồng khung giờ
                                   && lc.GioBatDau  < gioKetThuc
                                   && lc.GioKetThuc > gioBatDau);
        }

        // ----------------------------------------------------------
        // INSERT: Thêm suất chiếu mới VÀ tự động rải ghế
        // ----------------------------------------------------------
        public int Insert(LichChieu lichChieu)
        {
            lichChieu.IsDeleted = false;
            lichChieu.NgayTao = DateTime.Now;

            _context.LichChieux.Add(lichChieu);
            _context.SaveChanges(); // Lưu để lấy LichChieuId

            // --- BƯỚC MỚI: TỰ ĐỘNG TẠO GHẾ CHO SUẤT CHIẾU NÀY ---
            // 1. Lấy danh sách ghế vật lý của phòng chiếu này
            var danhSachGhe = _context.GheNgois
                                      .Where(g => g.PhongId == lichChieu.PhongId && g.IsDeleted == false)
                                      .ToList();

            // 2. Rải ghế vào bảng LichChieu_Ghe
            foreach (var ghe in danhSachGhe)
            {
                var lcg = new LichChieu_Ghe
                {
                    LichChieuId = lichChieu.LichChieuId,
                    GheId = ghe.GheId,
                    TrangThaiGhe = "Trong" // Ghế mới tạo mặc định là Trống
                };
                _context.LichChieu_Ghe.Add(lcg);
            }
            _context.SaveChanges();

            return lichChieu.LichChieuId;
        }

        // ----------------------------------------------------------
        // UPDATE: Cập nhật suất chiếu
        // ----------------------------------------------------------
        public bool Update(LichChieu lichChieuCapNhat)
        {
            LichChieu existing = _context.LichChieux.Find(lichChieuCapNhat.LichChieuId);
            if (existing == null || existing.IsDeleted == true)
                return false;

            existing.PhimId      = lichChieuCapNhat.PhimId;
            existing.PhongId     = lichChieuCapNhat.PhongId;
            existing.GioBatDau   = lichChieuCapNhat.GioBatDau;
            existing.GioKetThuc  = lichChieuCapNhat.GioKetThuc;
            existing.GiaVeCoBan  = lichChieuCapNhat.GiaVeCoBan;
            existing.TrangThai   = lichChieuCapNhat.TrangThai;

            _context.SaveChanges();
            return true;
        }

        // ----------------------------------------------------------
        // SOFT DELETE: Đổi IsDeleted = true và TrangThai = HuyChieu
        // KHÔNG xóa vật lý vì có FK từ LichChieu_Ghe
        // ----------------------------------------------------------
        public bool SoftDelete(int lichChieuId)
        {
            LichChieu lc = _context.LichChieux.Find(lichChieuId);
            if (lc == null || lc.IsDeleted == true)
                return false;

            lc.IsDeleted = true;
            lc.TrangThai = "HuyChieu";

            _context.SaveChanges();
            return true;
        }

        // ----------------------------------------------------------
        // GET DANH SACH PHIM (cho ComboBox)
        // Chỉ lấy phim "SapChieu" hoặc "DangChieu"
        // ----------------------------------------------------------
        public List<Phim> GetDanhSachPhimDangChieu()
        {
            return _context.Phims
                           .Where(p => p.IsDeleted == false
                                    && (p.TrangThai == "SapChieu"
                                     || p.TrangThai == "DangChieu"))
                           .OrderBy(p => p.TenPhim)
                           .ToList();
        }

        // ----------------------------------------------------------
        // GET DANH SACH PHONG CHIEU (cho ComboBox)
        // Chỉ lấy phòng "HoatDong"
        // ----------------------------------------------------------
        public List<PhongChieu> GetDanhSachPhongChieu()
        {
            return _context.PhongChieux
                           .Where(pc => pc.IsDeleted == false
                                     && pc.TrangThai == "HoatDong")
                           .OrderBy(pc => pc.TenPhong)
                           .ToList();
        }

        // ----------------------------------------------------------
        // KIỂM TRA: Suất chiếu có vé đã bán chưa
        // Dùng trước khi cho phép xóa/sửa
        // ----------------------------------------------------------
        public bool CoBanVe(int lichChieuId)
        {
            return _context.LichChieu_Ghe
                           .Any(lcg => lcg.LichChieuId   == lichChieuId
                                    && lcg.TrangThaiGhe  == "DaBan");
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
