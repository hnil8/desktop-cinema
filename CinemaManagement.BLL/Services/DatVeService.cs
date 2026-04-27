using System;
using System.Collections.Generic;
using CinemaManagement.BLL.Helpers;
using CinemaManagement.DAL.Repositories;

// ============================================================
// FILE: CinemaManagement.BLL/Services/DatVeService.cs
// Tầng BLL: Nghiệp vụ bán vé tại quầy POS
//   - Tính tiền tổng theo loại ghế (hệ số giá)
//   - Tính điểm tích lũy
//   - Validate trước khi thực hiện transaction
// ============================================================

namespace CinemaManagement.BLL.Services
{
    // ----------------------------------------------------------
    // DTO: Tóm tắt đơn hàng trên panel phải của POS
    // ----------------------------------------------------------
    public class TomTatDonHangDto
    {
        public string   TenPhim       { get; set; }
        public string   TenPhong      { get; set; }
        public string   GioChieu      { get; set; }
        public List<string> DanhSachTenGhe { get; set; }
        public decimal  TongTien      { get; set; }
        public string   TongTienHienThi { get; set; }
        public int      DiemTichDuoc  { get; set; }
    }

    public class DatVeService
    {
        private readonly DatVeRepository _datVeRepo;

        // Tỉ lệ tích điểm: cứ 10,000 VNĐ = 1 điểm
        private const int TIEN_PER_DIEM = 10000;

        public DatVeService()
        {
            _datVeRepo = new DatVeRepository();
        }

        // ──────────────────────────────────────────────────────
        // LẤY DANH SÁCH PHIM CÓ LỊCH CHIẾU HÔM NAY
        // ──────────────────────────────────────────────────────
        public List<PhimPosDto> GetPhimHomNay()
        {
            try
            {
                return _datVeRepo.GetPhimDangChieuHomNay();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DatVeService.GetPhimHomNay] " + ex.Message);
                return new List<PhimPosDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // LẤY CÁC SUẤT CHIẾU CỦA PHIM TRONG HÔM NAY
        // ──────────────────────────────────────────────────────
        public List<SuatChieuPosDto> GetSuatChieuByPhim(int phimId)
        {
            try
            {
                List<SuatChieuPosDto> list = _datVeRepo.GetLichChieuByPhim(phimId);

                // Tính GioHienThi tại BLL
                foreach (SuatChieuPosDto sc in list)
                {
                    sc.GioHienThi = sc.GioBatDau.ToString("HH:mm");
                }

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DatVeService.GetSuatChieuByPhim] " + ex.Message);
                return new List<SuatChieuPosDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // LẤY SƠ ĐỒ GHẾ
        // ──────────────────────────────────────────────────────
        public List<GheSoDoDto> GetSoDoGhe(int lichChieuId)
        {
            try
            {
                return _datVeRepo.GetSoDoGhe(lichChieuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DatVeService.GetSoDoGhe] " + ex.Message);
                return new List<GheSoDoDto>();
            }
        }

        // ──────────────────────────────────────────────────────
        // TRA CỨU KHÁCH HÀNG THEO SĐT
        // ──────────────────────────────────────────────────────
        public DAL.KhachHang TimKhachHangBySoDT(string soDienThoai)
        {
            if (string.IsNullOrWhiteSpace(soDienThoai))
                return null;
            try
            {
                return _datVeRepo.GetKhachHangBySoDT(soDienThoai.Trim());
            }
            catch (Exception ex)
            {
                Console.WriteLine("[DatVeService.TimKhachHangBySoDT] " + ex.Message);
                return null;
            }
        }

        // ──────────────────────────────────────────────────────
        // TÍNH TIỀN TỔNG + TÓM TẮT ĐƠN HÀNG
        // Gọi mỗi khi user chọn/bỏ ghế để cập nhật realtime
        // ──────────────────────────────────────────────────────
        public TomTatDonHangDto TinhTienDonHang(
            string tenPhim, string tenPhong, string gioChieu,
            List<GheSoDoDto> danhSachGheDangChon,
            decimal giaVeCoBan)
        {
            decimal tongTien = 0;
            List<string> danhSachTenGhe = new List<string>();

            foreach (GheSoDoDto ghe in danhSachGheDangChon)
            {
                // Giá mỗi ghế = Giá cơ bản × Hệ số loại ghế
                decimal giaMoiGhe = giaVeCoBan * ghe.HeSoGia;
                tongTien         += giaMoiGhe;
                danhSachTenGhe.Add(
                    string.Format("{0} ({1})", ghe.TenGhe, ghe.TenLoaiGhe));
            }

            // Tính điểm: làm tròn xuống
            int diemTichDuoc = (int)(tongTien / TIEN_PER_DIEM);

            return new TomTatDonHangDto
            {
                TenPhim             = tenPhim,
                TenPhong            = tenPhong,
                GioChieu            = gioChieu,
                DanhSachTenGhe      = danhSachTenGhe,
                TongTien            = tongTien,
                TongTienHienThi     = tongTien.ToString("N0") + " ₫",
                DiemTichDuoc        = diemTichDuoc
            };
        }

        // ──────────────────────────────────────────────────────
        // THỰC HIỆN BÁN VÉ (gọi Transaction trong DAL)
        // ──────────────────────────────────────────────────────
        public ServiceResult BanVe(
            int lichChieuId,
            List<GheSoDoDto> danhSachGheDangChon,
            decimal giaVeCoBan,
            int? khachHangId,
            string phuongThucTT,
            decimal tienKhachDua)
        {
            // ─── Validate ─────────────────────────────────────
            if (danhSachGheDangChon == null || danhSachGheDangChon.Count == 0)
                return ServiceResult.Fail("Chưa chọn ghế nào. Vui lòng chọn ít nhất 1 ghế.");

            if (danhSachGheDangChon.Count > 10)
                return ServiceResult.Fail("Chỉ được mua tối đa 10 vé trong 1 lần giao dịch.");

            string[] ptttHopLe = { "TienMat", "ChuyenKhoan", "The" };
            bool ptttOk = false;
            foreach (string pt in ptttHopLe)
            {
                if (pt == phuongThucTT) { ptttOk = true; break; }
            }
            if (!ptttOk)
                return ServiceResult.Fail("Phương thức thanh toán không hợp lệ.");

            // ─── Tính tổng tiền ───────────────────────────────
            decimal tongTien = 0;
            foreach (GheSoDoDto ghe in danhSachGheDangChon)
            {
                tongTien += giaVeCoBan * ghe.HeSoGia;
            }

            if (phuongThucTT == "TienMat" && tienKhachDua < tongTien)
                return ServiceResult.Fail(
                    string.Format(
                        "Tiền khách đưa ({0} ₫) không đủ. Cần ít nhất {1} ₫.",
                        tienKhachDua.ToString("N0"),
                        tongTien.ToString("N0")));

            // ─── Kiểm tra ca làm việc ─────────────────────────
            int nhanVienId = SessionManager.NhanVienId;
            DAL.CaLamViec caHienTai = _datVeRepo.GetCaDangMo(nhanVienId);
            if (caHienTai == null)
                return ServiceResult.Fail(
                    "Chưa mở ca làm việc!\n" +
                    "Vui lòng mở ca trước khi bắt đầu bán vé.");

            // ─── Chuẩn bị request ─────────────────────────────
            List<int> danhSachLCGId = new List<int>();
            foreach (GheSoDoDto ghe in danhSachGheDangChon)
            {
                danhSachLCGId.Add(ghe.LichChieuGheId);
            }

            int diemTichDuoc = (int)(tongTien / TIEN_PER_DIEM);

            DatVeRequest request = new DatVeRequest
            {
                LichChieuId           = lichChieuId,
                DanhSachLichChieuGheId = danhSachLCGId,
                GiaVeCoBan            = giaVeCoBan,
                CaId                  = caHienTai.CaId,
                KhachHangId           = khachHangId,
                PhuongThucTT          = phuongThucTT,
                TienKhachDua          = tienKhachDua,
                ThanhTien             = tongTien,
                DiemTichDuoc          = diemTichDuoc
            };

            try
            {
                DatVeResult ketQua = _datVeRepo.DatVe(request);

                string thongBao = string.Format(
                    "Bán vé thành công!\nHóa đơn #{0} | Tổng: {1} ₫{2}",
                    ketQua.HoaDonId,
                    ketQua.ThanhTien.ToString("N0"),
                    ketQua.TienThoiLai > 0
                        ? " | Thối lại: " + ketQua.TienThoiLai.ToString("N0") + " ₫"
                        : "");

                return ServiceResult.Ok(thongBao, ketQua);
            }
            catch (Exception ex)
            {
                // Exception từ transaction chứa thông báo nghiệp vụ
                string msg = ex.Message.Contains("đã được người khác mua")
                    ? ex.Message
                    : "Lỗi hệ thống khi xử lý giao dịch. Vui lòng thử lại.";
                Console.WriteLine("[DatVeService.BanVe] " + ex.Message);
                return ServiceResult.Fail(msg);
            }
        }

        public void Dispose()
        {
            _datVeRepo.Dispose();
        }
    }
}
