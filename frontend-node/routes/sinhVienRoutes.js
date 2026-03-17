const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

function laSinhVien(req) {
  return req.session?.user?.role === "SinhVien";
}

function khongChoSinhVienTaoXoa(req, res, next) {
  if (laSinhVien(req)) {
    return res.status(403).render("dashboard/error", {
      title: "Khong du quyen",
      message: "Sinh vien khong duoc tao hoac xoa sinh vien."
    });
  }

  return next();
}

function chiSuaThongTinChinhMinh(req, res, next) {
  if (!laSinhVien(req)) {
    return next();
  }

  const ownId = Number(req.session?.user?.id);
  const requestId = Number(req.params.id);
  if (!ownId || ownId !== requestId) {
    return res.status(403).render("dashboard/error", {
      title: "Khong du quyen",
      message: "Ban chi duoc cap nhat ho so cua chinh minh."
    });
  }

  return next();
}

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const [sinhVienRes, hopDongRes] = await Promise.all([
      api.get("/SinhViens"),
      api.get("/HopDongs")
    ]);

    const activeContracts = new Map();
    const contracts = Array.isArray(hopDongRes.data) ? hopDongRes.data : [];
    for (const contract of contracts) {
      if (!contract || typeof contract.maSinhVien !== "number") {
        continue;
      }

      const current = activeContracts.get(contract.maSinhVien);
      if (!current || new Date(contract.ngayBatDau) > new Date(current.ngayBatDau)) {
        activeContracts.set(contract.maSinhVien, contract);
      }
    }

    const students = (Array.isArray(sinhVienRes.data) ? sinhVienRes.data : []).map((item) => {
      const contract = activeContracts.get(item.maSinhVien);
      return {
        ...item,
        maPhongHienTai: contract?.maPhong ?? null
      };
    });

    res.render("sinhvien/index", { title: "Quan ly Sinh vien", items: students });
  } catch (error) {
    next(error);
  }
});

router.post("/", khongChoSinhVienTaoXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post("/SinhViens", {
      maSinhVien: Number(req.body.maSinhVien),
      hoTen: req.body.hoTen,
      ngaySinh: req.body.ngaySinh,
      gioiTinh: req.body.gioiTinh,
      soDienThoai: req.body.soDienThoai,
      email: req.body.email,
      cccd: req.body.cccd,
      trangThai: req.body.trangThai
    });
    res.redirect("/sinhvien");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", chiSuaThongTinChinhMinh, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/SinhViens/${req.params.id}`);
    res.render("sinhvien/edit", { title: "Cap nhat Sinh vien", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", chiSuaThongTinChinhMinh, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/SinhViens/${req.params.id}`, {
      maSinhVien: Number(req.params.id),
      hoTen: req.body.hoTen,
      ngaySinh: req.body.ngaySinh,
      gioiTinh: req.body.gioiTinh,
      soDienThoai: req.body.soDienThoai,
      email: req.body.email,
      cccd: req.body.cccd,
      trangThai: req.body.trangThai
    });
    res.redirect("/sinhvien");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", khongChoSinhVienTaoXoa, async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/SinhViens/${req.params.id}`);
    res.redirect("/sinhvien");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
