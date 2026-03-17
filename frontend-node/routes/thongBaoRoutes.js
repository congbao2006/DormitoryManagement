const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/ThongBaos");
    res.render("thongbao/index", { title: "Quan ly Thong bao", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post("/ThongBaos", {
      maThongBao: Number(req.body.maThongBao),
      tieuDe: req.body.tieuDe,
      noiDung: req.body.noiDung,
      ngayTao: req.body.ngayTao
    });
    res.redirect("/thongbao");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/ThongBaos/${req.params.id}`);
    res.render("thongbao/edit", { title: "Cap nhat Thong bao", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/ThongBaos/${req.params.id}`, {
      maThongBao: Number(req.params.id),
      tieuDe: req.body.tieuDe,
      noiDung: req.body.noiDung,
      ngayTao: req.body.ngayTao
    });
    res.redirect("/thongbao");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/ThongBaos/${req.params.id}`);
    res.redirect("/thongbao");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
