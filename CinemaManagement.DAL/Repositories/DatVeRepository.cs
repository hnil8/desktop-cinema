using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

// ============================================================
// FILE: CinemaManagement.DAL/Repositories/DatVeRepository.cs
// Tầng DAL: Truy vấn & Transaction bán vé
// ============================================================

namespace CinemaManagement.DAL.Repositories
{
    // ----------------------------------------------------------
    // DTO: Thông tin phim hiển thị ở POS (kèm lịch chiếu hôm nay)
    // ----------------------------------------------------------
    public class PhimPosDto
    {
        public int PhimId { get; set; }
        public string TenPhim { get; set; }
        public int ThoiLuongPhut { get; set; }
        public string GioiHanDoTuoi { get; set; }

        // THÊM ĐOẠN NÀY VÀO ĐỂ WINFORMS HIỂN THỊ ĐÚNG TÊN PHIM
        public override string ToString()
        {
            return TenPhim;
        }
    }

    // ----------------------------------------------------------
    // DTO: Suất chiếu của 1 phim trong ngày
    // ----------------------------------------------------------
    public class SuatChieuPosDto
    {
        public int      LichChieuId  { get; set; }
        public string   TenPhong     { get; set; }
        public DateTime GioBatDau    { get; set; }
        public DateTime GioKetThuc   { get; set; }
        public decimal  GiaVeCoBan   { get; set; }
        public string   TrangThai    { get; set; }
        public string   GioHienThi   { get; set; } // "19:30" để hiển thị trên button
    }

    // ----------------------------------------------------------
    // DTO: Trạng thái từng ghế trong 1 suất chiếu
    // ----------------------------------------------------------
    public class GheSoDoDto
    {
        public int     LichChieuGheId { get; set; }
        public int     GheId          { get; set; }
        public string  TenGhe         { get; set; } // "A1", "B5" (computed column)
        public string  DayGhe         { get; set; }
        public int     CotGhe         { get; set; }
        public string  TrangThaiGhe   { get; set; } // "Trong", "DangGiu", "DaBan"
        public string  TenLoaiGhe     { get; set; } // "Thuong", "VIP"
        public decimal HeSoGia        { get; set; }
    }

    // ----------------------------------------------------------
    // DTO: Yêu cầu đặt vé (truyền từ BLL xuống DAL)
    // ----------------------------------------------------------
    public class DatVeRequest
    {
        public int          LichChieuId           { get; set; }
        public List<int>    DanhSachLichChieuGheId { get; set; }
        public decimal      GiaVeCoBan             { get; set; }
        public int          CaId                   { get; set; }
        public int?         KhachHangId            { get; set; }
        public string       PhuongThucTT           { get; set; } // "TienMat","ChuyenKhoan","The"
        public decimal      TienKhachDua           { get; set; }
        public decimal      ThanhTien              { get; set; }
        public int          DiemTichDuoc           { get; set; }
    }

    // ----------------------------------------------------------
    // DTO: Kết quả sau khi đặt vé thành công
    // ----------------------------------------------------------
    public class DatVeResult
    {
        public int      HoaDonId    { get; set; }
        public decimal  ThanhTien   { get; set; }
        public decimal  TienThoiLai { get; set; }
    }

    public class DatVeRepository
    {
        private readonly CinemaDBEntities _context;

        public DatVeRepository()
        {
            _context = new CinemaDBEntities();
        }

        // ----------------------------------------------------------
        // GET PHIM ĐANG CHIẾU HÔM NAY
        // Lấy phim có ít nhất 1 lịch chiếu trong ngày hôm nay
        // ----------------------------------------------------------
        public List<PhimPosDto> GetPhimDangChieuHomNay()
        {
            DateTime homNay     = DateTime.Today;
            DateTime ngayMai    = homNay.AddDays(1);

            var query =
                from p in _context.Phims
                where p.IsDeleted == false
                   && p.TrangThai != "NgungChieu"
                   && _context.LichChieux.Any(
                       lc => lc.PhimId    == p.PhimId
                          && lc.IsDeleted == false
                          && lc.TrangThai != "HuyChieu"
                          && lc.GioBatDau >= homNay
                          && lc.GioBatDau < ngayMai)
                orderby p.TenPhim
                select new PhimPosDto
                {
                    PhimId        = p.PhimId,
                    TenPhim       = p.TenPhim,
                    ThoiLuongPhut = p.ThoiLuongPhut,
                    GioiHanDoTuoi = p.GioiHanDoTuoi
                };

            return query.ToList();
        }

        // ----------------------------------------------------------
        // GET LỊCH CHIẾU CỦA 1 PHIM TRONG HÔM NAY
        // ----------------------------------------------------------
        public List<SuatChieuPosDto> GetLichChieuByPhim(int phimId)
        {
            DateTime homNay  = DateTime.Today;
            DateTime ngayMai = homNay.AddDays(1);

            var query =
                from lc in _context.LichChieux
                join pc in _context.PhongChieux on lc.PhongId equals pc.PhongId
                where lc.PhimId    == phimId
                   && lc.IsDeleted == false
                   && lc.TrangThai != "HuyChieu"
                   && lc.GioBatDau >= homNay
                   && lc.GioBatDau <  ngayMai
                orderby lc.GioBatDau
                select new SuatChieuPosDto
                {
                    LichChieuId = lc.LichChieuId,
                    TenPhong    = pc.TenPhong,
                    GioBatDau   = lc.GioBatDau,
                    GioKetThuc  = lc.GioKetThuc,
                    GiaVeCoBan  = lc.GiaVeCoBan,
                    TrangThai   = lc.TrangThai,
                    GioHienThi  = "" // Tính ở BLL
                };

            return query.ToList();
        }

        // ----------------------------------------------------------
        // GET SƠ ĐỒ GHẾ: Join LichChieu_Ghe + GheNgoi + LoaiGhe
        // Trả về DTO sắp xếp theo DayGhe rồi CotGhe để vẽ đúng thứ tự
        // ----------------------------------------------------------
        public List<GheSoDoDto> GetSoDoGhe(int lichChieuId)
        {
            var query =
                from lcg in _context.LichChieu_Ghe
                join g  in _context.GheNgois on lcg.GheId       equals g.GheId
                join lg in _context.LoaiGhes on g.LoaiGheId     equals lg.LoaiGheId
                where lcg.LichChieuId == lichChieuId
                   && g.IsDeleted     == false
                orderby g.DayGhe, g.CotGhe
                select new GheSoDoDto
                {
                    LichChieuGheId = lcg.LichChieuGheId,
                    GheId          = g.GheId,
                    TenGhe         = g.TenGhe,      // Computed column "A1", "B5"
                    DayGhe         = g.DayGhe,
                    CotGhe         = g.CotGhe,
                    TrangThaiGhe   = lcg.TrangThaiGhe,
                    TenLoaiGhe     = lg.TenLoai,
                    HeSoGia        = lg.HeSoGia
                };

            return query.ToList();
        }

        // ----------------------------------------------------------
        // GET KHÁCH HÀNG THEO SĐT (tra cứu nhanh tại quầy)
        // ----------------------------------------------------------
        public KhachHang GetKhachHangBySoDT(string soDienThoai)
        {
            return _context.KhachHangs
                           .Include("HangThanhVien")
                           .FirstOrDefault(kh => kh.SoDienThoai  == soDienThoai
                                              && kh.IsDeleted     == false);
        }

        // ----------------------------------------------------------
        // GET CA LÀM VIỆC ĐANG MỞ của nhân viên hiện tại
        // ----------------------------------------------------------
        public CaLamViec GetCaDangMo(int nhanVienId)
        {
            return _context.CaLamViecs
                           .FirstOrDefault(ca => ca.NhanVienId == nhanVienId
                                              && ca.TrangThai  == "DangMo");
        }

        // ----------------------------------------------------------
        // ĐẶT VÉ - TRANSACTION
        // Đây là hàm quan trọng nhất trong toàn bộ hệ thống POS
        //
        // Thứ tự thực hiện trong transaction:
        //   1. INSERT HoaDon              → lấy HoaDonId
        //   2. Với mỗi ghế được chọn:
        //      a. INSERT VePhim           → lấy VeId
        //      b. UPDATE LichChieu_Ghe    → gán VePhimId + TrangThaiGhe='DaBan'
        //   3. UPDATE KhachHang           → cộng điểm (nếu có)
        //   4. UPDATE CaLamViec           → cộng doanh thu
        //   5. COMMIT
        // ----------------------------------------------------------
        public DatVeResult DatVe(DatVeRequest request)
        {
            // BeginTransaction dùng using (...) { } chuẩn C# 7.3
            using (System.Data.Entity.DbContextTransaction transaction =
                       _context.Database.BeginTransaction())
            {
                try
                {
                    // ─── BƯỚC 1: Tạo Hóa Đơn ─────────────────
                    decimal tongTienVe   = request.ThanhTien;
                    decimal tienThoiLai  = request.PhuongThucTT == "TienMat"
                        ? request.TienKhachDua - request.ThanhTien
                        : 0;

                    HoaDon hoaDon = new HoaDon
                    {
                        CaId                 = request.CaId,
                        KhachHangId          = request.KhachHangId,
                        KhuyenMaiId          = null,
                        TongTienVe           = tongTienVe,
                        TongTienFnB          = 0,
                        TongTienGoc          = tongTienVe,
                        TienGiamKM           = 0,
                        TienGiamDiem         = 0,
                        TienGiamThanhVien    = 0,
                        ThanhTien            = request.ThanhTien,
                        PhuongThucTT         = request.PhuongThucTT,
                        TienKhachDua         = request.TienKhachDua > 0
                                                ? request.TienKhachDua
                                                : (decimal?)null,
                        TienThoiLai          = tienThoiLai > 0
                                                ? tienThoiLai
                                                : (decimal?)null,
                        DiemTichDuoc         = request.DiemTichDuoc,
                        DiemSuDung           = 0,
                        TrangThai            = "HoanThanh",
                        ThoiGianTao          = DateTime.Now
                    };

                    _context.HoaDons.Add(hoaDon);
                    _context.SaveChanges(); // Flush để lấy HoaDonId

                    // ─── BƯỚC 2: Tạo Vé + Cập nhật trạng thái ghế ──
                    foreach (int lichChieuGheId in request.DanhSachLichChieuGheId)
                    {
                        // Lấy thông tin ghế để tính giá
                        LichChieu_Ghe lcg = _context.LichChieu_Ghe
                                                     .Find(lichChieuGheId);
                        if (lcg == null || lcg.TrangThaiGhe == "DaBan")
                            throw new Exception(
                                "Ghế " + lichChieuGheId + " đã được người khác mua. " +
                                "Vui lòng chọn lại ghế khác.");

                        GheNgoi ghe = _context.GheNgois.Find(lcg.GheId);
                        LoaiGhe lg  = _context.LoaiGhes.Find(ghe.LoaiGheId);

                        // Giá vé = Giá cơ bản của lịch chiếu × Hệ số loại ghế
                        decimal giaBan = request.GiaVeCoBan * lg.HeSoGia;

                        // INSERT VePhim
                        VePhim ve = new VePhim
                        {
                            HoaDonId          = hoaDon.HoaDonId,
                            LichChieuGheId    = lichChieuGheId,
                            QuyTacId          = null,
                            GiaGoc            = giaBan,
                            GiaBan            = giaBan,
                            DoiTuongKhach     = "NguoiLon",
                            TrangThai         = "DaBan"
                        };
                        _context.VePhims.Add(ve);
                        _context.SaveChanges(); // Flush để lấy VeId

                        // UPDATE LichChieu_Ghe → DaBan + gán VePhimId
                        lcg.TrangThaiGhe = "DaBan";
                        lcg.VePhimId     = ve.VeId;
                        lcg.ThoiGianGiu  = null; // Giải phóng lock
                        _context.SaveChanges();
                    }

                    // ─── BƯỚC 3: Cộng điểm cho khách hàng ────────
                    if (request.KhachHangId.HasValue && request.DiemTichDuoc > 0)
                    {
                        KhachHang kh = _context.KhachHangs.Find(request.KhachHangId.Value);
                        if (kh != null)
                        {
                            kh.DiemTichLuy += request.DiemTichDuoc;
                            _context.SaveChanges();
                        }
                    }

                    // ─── BƯỚC 4: Cộng doanh thu vào ca làm việc ──
                    CaLamViec ca = _context.CaLamViecs.Find(request.CaId);
                    if (ca != null)
                    {
                        if (request.PhuongThucTT == "TienMat")
                            ca.TongThuTienMat += request.ThanhTien;
                        else if (request.PhuongThucTT == "ChuyenKhoan")
                            ca.TongThuChuyenKhoan += request.ThanhTien;
                        else if (request.PhuongThucTT == "The")
                            ca.TongThuThe += request.ThanhTien;

                        _context.SaveChanges();
                    }

                    // ─── BƯỚC 5: Commit ───────────────────────────
                    transaction.Commit();

                    return new DatVeResult
                    {
                        HoaDonId    = hoaDon.HoaDonId,
                        ThanhTien   = request.ThanhTien,
                        TienThoiLai = tienThoiLai
                    };
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw; // Ném lại để BLL bắt và xử lý
                }
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
