const express = require("express");
const path = require("path");
const session = require("express-session");
require("dotenv").config();

const authRoutes = require("./routes/authRoutes");
const dashboardRoutes = require("./routes/dashboardRoutes");
const sinhVienRoutes = require("./routes/sinhVienRoutes");
const phongRoutes = require("./routes/phongRoutes");
const hopDongRoutes = require("./routes/hopDongRoutes");
const hoaDonRoutes = require("./routes/hoaDonRoutes");
const nhanVienRoutes = require("./routes/nhanVienRoutes");
const toaNhaRoutes = require("./routes/toaNhaRoutes");
const thongBaoRoutes = require("./routes/thongBaoRoutes");
const yeuCauRoutes = require("./routes/yeuCauRoutes");
const taiSanRoutes = require("./routes/taiSanRoutes");
const thanhToanRoutes = require("./routes/thanhToanRoutes");

function requireAuth(req, res, next) {
  if (!req.session?.token) {
    return res.redirect("/login");
  }

  return next();
}

function requireRole(...allowedRoles) {
  return (req, res, next) => {
    const role = req.session?.user?.role;

    if (!role || !allowedRoles.includes(role)) {
      return res.status(403).render("dashboard/error", {
        title: "Không đủ quyền",
        message: "Bạn không có quyền truy cập chức năng này."
      });
    }

    return next();
  };
}

function createApp(options = {}) {
  const app = express();
  const enableTestRoutes = options.enableTestRoutes === true;

  app.set("view engine", "ejs");
  app.set("views", path.join(__dirname, "views"));

  app.use(express.urlencoded({ extended: true }));
  app.use(express.json());
  app.use(express.static(path.join(__dirname, "public")));

  app.use(session({
    secret: process.env.SESSION_SECRET || "quanky_session_secret",
    resave: false,
    saveUninitialized: false,
    cookie: {
      httpOnly: true,
      maxAge: 1000 * 60 * 60 * 8
    }
  }));

  app.use((req, res, next) => {
    res.locals.currentUser = req.session?.user || null;
    res.locals.currentPath = req.path || "/";
    next();
  });

  if (enableTestRoutes) {
    app.post("/__test__/session", (req, res) => {
      req.session.token = req.body.token || null;
      req.session.user = req.body.user || null;
      res.status(204).end();
    });
  }

  app.use("/", authRoutes);

  app.use("/", requireAuth, dashboardRoutes);
  app.use("/sinhvien", requireAuth, requireRole("Admin", "NhanVien", "SinhVien"), sinhVienRoutes);
  app.use("/phong", requireAuth, requireRole("Admin", "NhanVien"), phongRoutes);
  app.use("/hopdong", requireAuth, requireRole("Admin", "NhanVien", "SinhVien"), hopDongRoutes);
  app.use("/hoadon", requireAuth, requireRole("Admin", "NhanVien", "SinhVien"), hoaDonRoutes);
  app.use("/nhanvien", requireAuth, requireRole("Admin"), nhanVienRoutes);
  app.use("/toanha", requireAuth, requireRole("Admin", "NhanVien"), toaNhaRoutes);
  app.use("/thongbao", requireAuth, requireRole("Admin", "NhanVien"), thongBaoRoutes);
  app.use("/yeucau", requireAuth, requireRole("Admin", "NhanVien", "SinhVien"), yeuCauRoutes);
  app.use("/taisan", requireAuth, requireRole("Admin", "NhanVien"), taiSanRoutes);
  app.use("/thanhtoan", requireAuth, requireRole("Admin", "NhanVien", "SinhVien"), thanhToanRoutes);

  app.use((err, req, res, next) => {
    res.status(500).render("dashboard/error", {
      title: "Lỗi hệ thống",
      message: err.message || "Đã xảy ra lỗi ngoài ý muốn"
    });
  });

  return app;
}

if (require.main === module) {
  const app = createApp();
  const port = process.env.PORT || 3000;
  app.listen(port, () => {
    console.log(`Frontend is running at http://localhost:${port}`);
  });
}

module.exports = { createApp };
