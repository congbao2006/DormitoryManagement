const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

function khongChoSinhVienGhi(req, res, next) {
  if (req.session?.user?.role === "SinhVien") {
    return res.status(403).render("dashboard/error", {
      title: "Khong du quyen",
      message: "Sinh vien khong duoc thay doi hop dong."
    });
  }

  return next();
}

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const [hopDongRes, sinhVienRes, phongRes] = await Promise.all([
      api.get("/HopDongs"),
      api.get("/SinhViens"),
      api.get("/Phongs")
    ]);

    res.render("hopdong/index", {
      title: "Quan ly Hop dong",
      items: hopDongRes.data,
      sinhViens: sinhVienRes.data,
      phongs: phongRes.data
    });
  } catch (error) {
    next(error);
  }
});

router.post("/", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const maSinhVien = Number(req.body.maSinhVien);
    const maPhong = Number(req.body.maPhong);
    if (!Number.isInteger(maSinhVien) || maSinhVien <= 0 || !Number.isInteger(maPhong) || maPhong <= 0) {
      return res.status(400).render("dashboard/error", {
        title: "Dữ liệu không hợp lệ",
        message: "Vui lòng chọn sinh viên và phòng hợp lệ cho hợp đồng."
      });
    }

    const api = createApiClient(req.session?.token);
    await api.post("/HopDongs", {
      maHopDong: Number(req.body.maHopDong),
      ngayBatDau: req.body.ngayBatDau,
      ngayKetThuc: req.body.ngayKetThuc,
      tienDatCoc: Number(req.body.tienDatCoc),
      trangThai: req.body.trangThai,
      maSinhVien,
      maPhong
    });
    res.redirect("/hopdong");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const [hopDongRes, sinhVienRes, phongRes] = await Promise.all([
      api.get(`/HopDongs/${req.params.id}`),
      api.get("/SinhViens"),
      api.get("/Phongs")
    ]);

    res.render("hopdong/edit", {
      title: "Cap nhat Hop dong",
      item: hopDongRes.data,
      sinhViens: sinhVienRes.data,
      phongs: phongRes.data
    });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const maSinhVien = Number(req.body.maSinhVien);
    const maPhong = Number(req.body.maPhong);
    if (!Number.isInteger(maSinhVien) || maSinhVien <= 0 || !Number.isInteger(maPhong) || maPhong <= 0) {
      return res.status(400).render("dashboard/error", {
        title: "Dữ liệu không hợp lệ",
        message: "Vui lòng chọn sinh viên và phòng hợp lệ cho hợp đồng."
      });
    }

    const api = createApiClient(req.session?.token);
    await api.put(`/HopDongs/${req.params.id}`, {
      maHopDong: Number(req.params.id),
      ngayBatDau: req.body.ngayBatDau,
      ngayKetThuc: req.body.ngayKetThuc,
      tienDatCoc: Number(req.body.tienDatCoc),
      trangThai: req.body.trangThai,
      maSinhVien,
      maPhong
    });
    res.redirect("/hopdong");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", khongChoSinhVienGhi, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/HopDongs/${req.params.id}`);
    res.redirect("/hopdong");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
