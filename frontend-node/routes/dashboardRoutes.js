const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const role = req.session?.user?.role;

    const [sinhVienRes, phongRes, hopDongRes, hoaDonRes, nhanVienRes, toaNhaRes, yeuCauRes, taiSanRes] = await Promise.all([
      api.get("/SinhViens"),
      role === "SinhVien" ? Promise.resolve({ data: [] }) : api.get("/Phongs"),
      api.get("/HopDongs"),
      api.get("/HoaDons"),
      role === "Admin" ? api.get("/NhanViens") : Promise.resolve({ data: [] }),
      role === "SinhVien" ? Promise.resolve({ data: [] }) : api.get("/ToaNhas"),
      api.get("/YeuCauSuaChuas"),
      role === "SinhVien" ? Promise.resolve({ data: [] }) : api.get("/TaiSans")
    ]);

    res.render("dashboard/index", {
      title: "Quan ly Ky tuc xa",
      stats: {
        sinhVien: sinhVienRes.data.length,
        phongConCho: phongRes.data.filter(item => item.trangThai === "ConCho").length,
        hopDong: hopDongRes.data.length,
        hoaDon: hoaDonRes.data.length,
        nhanVien: nhanVienRes.data.length,
        toaNha: toaNhaRes.data.length,
        yeuCau: yeuCauRes.data.length,
        taiSan: taiSanRes.data.length
      }
    });
  } catch (error) {
    next(error);
  }
});

module.exports = router;
