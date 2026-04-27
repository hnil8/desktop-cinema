using System;
using System.Collections.Generic;
using System.Linq;

// ============================================================
// FILE: CinemaManagement.DAL/Repositories/PhimRepository.cs
// Tầng DAL - Chỉ thao tác trực tiếp với DB qua EF Context
// KHÔNG chứa logic nghiệp vụ, KHÔNG validate dữ liệu
// ============================================================

namespace CinemaManagement.DAL.Repositories
{
    public class PhimRepository
    {
        private readonly CinemaDBEntities _context;

        public PhimRepository()
        {
            // ĐỔI TỪ: _context = new CinemaDbContext();
            _context = new CinemaDBEntities();
        }

        // ----------------------------------------------------------
        // GET LIST: Lấy danh sách phim chưa bị xóa mềm
        // Kết quả được sắp xếp mới nhất lên đầu
        // ----------------------------------------------------------
        public List<Phim> GetList()
        {
            return _context.Phims
                           .Where(p => p.IsDeleted == false)
                           .OrderByDescending(p => p.NgayTao)
                           .ToList();
        }

        // ----------------------------------------------------------
        // GET LIST WITH THE LOAI: Lấy kèm thể loại (dùng cho display)
        // Include Navigation Property Phim_TheLoai → TheLoaiPhim
        // ----------------------------------------------------------
        public List<Phim> GetListWithTheLoai()
        {
            return _context.Phims
                           .Where(p => p.IsDeleted == false)
                           .OrderByDescending(p => p.NgayTao)
                           .ToList();
        }

        // ----------------------------------------------------------
        // GET BY ID: Tìm phim theo khóa chính
        // ----------------------------------------------------------
        public Phim GetById(int phimId)
        {
            return _context.Phims // (Hoặc _context.Phim tùy máy bạn đang báo)
                           .FirstOrDefault(p => p.PhimId == phimId
                                             && p.IsDeleted == false);
        }

        // ----------------------------------------------------------
        // SEARCH: Tìm kiếm theo tên phim (không phân biệt hoa/thường)
        // ----------------------------------------------------------
        public List<Phim> SearchByTen(string keyword)
        {
            string kw = keyword.Trim().ToLower();
            return _context.Phims
                           .Where(p => p.IsDeleted == false
                                    && p.TenPhim.ToLower().Contains(kw))
                           .OrderBy(p => p.TenPhim)
                           .ToList();
        }

        // ----------------------------------------------------------
        // INSERT: Thêm phim mới
        // Trả về PhimId vừa được tạo (IDENTITY)
        // ----------------------------------------------------------
        public int Insert(Phim phim)
        {
            phim.NgayTao = DateTime.Now;
            phim.IsDeleted = false;

            _context.Phims.Add(phim);
            _context.SaveChanges();

            return phim.PhimId; // EF tự điền lại sau SaveChanges
        }

        // ----------------------------------------------------------
        // UPDATE: Cập nhật thông tin phim
        // Dùng kỹ thuật Attach + Modified để chỉ UPDATE đúng record
        // ----------------------------------------------------------
        public bool Update(Phim phimCapNhat)
        {
            Phim existing = _context.Phims.Find(phimCapNhat.PhimId);
            if (existing == null || existing.IsDeleted == true)
                return false;

            // Chỉ cập nhật các trường nghiệp vụ, GIỮ NGUYÊN NgayTao và IsDeleted
            existing.TenPhim = phimCapNhat.TenPhim;
            existing.TenGoc = phimCapNhat.TenGoc;
            existing.DaoDien = phimCapNhat.DaoDien;
            existing.DienVienChinh = phimCapNhat.DienVienChinh;
            existing.ThoiLuongPhut = phimCapNhat.ThoiLuongPhut;
            existing.NuocSanXuat = phimCapNhat.NuocSanXuat;
            existing.NamPhatHanh = phimCapNhat.NamPhatHanh;
            existing.GioiHanDoTuoi = phimCapNhat.GioiHanDoTuoi;
            existing.NgonNgu = phimCapNhat.NgonNgu;
            existing.MoTa = phimCapNhat.MoTa;
            existing.PosterUrl = phimCapNhat.PosterUrl;
            existing.TrangThai = phimCapNhat.TrangThai;
            existing.NgayKhoiChieu = phimCapNhat.NgayKhoiChieu;

            _context.SaveChanges();
            return true;
        }

        // ----------------------------------------------------------
        // SOFT DELETE: Xóa mềm - chỉ set IsDeleted = true
        // KHÔNG xóa vật lý để bảo toàn FK với LichChieu, VePhim
        // ----------------------------------------------------------
        public bool SoftDelete(int phimId)
        {
            Phim phim = _context.Phims.Find(phimId);
            if (phim == null || phim.IsDeleted == true)
                return false;

            phim.IsDeleted = true;
            phim.TrangThai = "NgungChieu"; // Đổi trạng thái kèm theo

            _context.SaveChanges();
            return true;
        }

        // ----------------------------------------------------------
        // CHECK DUPLICATE: Kiểm tra trùng tên phim
        // excludeId: bỏ qua record đang edit (khi Update)
        // ----------------------------------------------------------
        public bool IsTenPhimTonTai(string tenPhim, int excludeId = 0)
        {
            return _context.Phims
                           .Any(p => p.IsDeleted == false
                                  && p.TenPhim.ToLower() == tenPhim.Trim().ToLower()
                                  && p.PhimId != excludeId);
        }

        // ----------------------------------------------------------
        // CHECK CONSTRAINT: Kiểm tra phim có lịch chiếu chưa
        // Dùng trước khi xóa để cảnh báo user
        // ----------------------------------------------------------
        public bool CoLichChieu(int phimId)
        {
            return _context.LichChieux
                           .Any(lc => lc.PhimId == phimId
                                   && lc.IsDeleted == false);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}