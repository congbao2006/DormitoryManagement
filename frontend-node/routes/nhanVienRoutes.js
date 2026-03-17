const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/NhanViens");
    res.render("nhanvien/index", { title: "Quan ly Nhan vien", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post("/NhanViens", {
      maNhanVien: Number(req.body.maNhanVien),
      hoTen: req.body.hoTen,
      ngaySinh: req.body.ngaySinh,
      gioiTinh: req.body.gioiTinh,
      soDienThoai: req.body.soDienThoai,
      email: req.body.email,
      cccd: req.body.cccd,
      trangThai: req.body.trangThai
    });
    res.redirect("/nhanvien");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/NhanViens/${req.params.id}`);
    res.render("nhanvien/edit", { title: "Cap nhat Nhan vien", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/NhanViens/${req.params.id}`, {
      maNhanVien: Number(req.params.id),
      hoTen: req.body.hoTen,
      ngaySinh: req.body.ngaySinh,
      gioiTinh: req.body.gioiTinh,
      soDienThoai: req.body.soDienThoai,
      email: req.body.email,
      cccd: req.body.cccd,
      trangThai: req.body.trangThai
    });
    res.redirect("/nhanvien");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/NhanViens/${req.params.id}`);
    res.redirect("/nhanvien");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
