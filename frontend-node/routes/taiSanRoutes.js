const express = require("express");
const createApiClient = require("../services/apiClient");

const router = express.Router();

router.get("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get("/TaiSans");
    res.render("taisan/index", { title: "Quan ly Tai san", items: data });
  } catch (error) {
    next(error);
  }
});

router.post("/", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.post("/TaiSans", {
      maTaiSan: Number(req.body.maTaiSan),
      tenTaiSan: req.body.tenTaiSan,
      tinhTrang: req.body.tinhTrang,
      ngayMua: req.body.ngayMua
    });
    res.redirect("/taisan");
  } catch (error) {
    next(error);
  }
});

router.get("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    const { data } = await api.get(`/TaiSans/${req.params.id}`);
    res.render("taisan/edit", { title: "Cap nhat Tai san", item: data });
  } catch (error) {
    next(error);
  }
});

router.post("/:id/edit", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.put(`/TaiSans/${req.params.id}`, {
      maTaiSan: Number(req.params.id),
      tenTaiSan: req.body.tenTaiSan,
      tinhTrang: req.body.tinhTrang,
      ngayMua: req.body.ngayMua
    });
    res.redirect("/taisan");
  } catch (error) {
    next(error);
  }
});

router.post("/:id/delete", async (req, res, next) => {
  try {
    const api = createApiClient(req.session?.token);
    await api.delete(`/TaiSans/${req.params.id}`);
    res.redirect("/taisan");
  } catch (error) {
    next(error);
  }
});

module.exports = router;
