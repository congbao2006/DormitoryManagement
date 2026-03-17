const test = require("node:test");
const assert = require("node:assert/strict");
const request = require("supertest");
const { createApp } = require("../server");

async function taoAgent(user) {
  const app = createApp({ enableTestRoutes: true });
  const agent = request.agent(app);

  if (user) {
    await agent
      .post("/__test__/session")
      .send({
        token: "test-token",
        user
      })
      .expect(204);
  }

  return agent;
}

test("Nguoi dung chua dang nhap bi chuyen huong ve /login", async () => {
  const agent = await taoAgent();

  const response = await agent.get("/phong");

  assert.equal(response.status, 302);
  assert.equal(response.headers.location, "/login");
});

test("SinhVien khong duoc truy cap module nhanvien", async () => {
  const agent = await taoAgent({ id: "1", role: "SinhVien", hoTen: "Nguyen Van An", email: "an@kytu.com" });

  const response = await agent.get("/nhanvien");

  assert.equal(response.status, 403);
  assert.match(response.text, /Bạn không có quyền truy cập chức năng này\./);
});

test("SinhVien khong duoc sua yeu cau cua module frontend", async () => {
  const agent = await taoAgent({ id: "1", role: "SinhVien", hoTen: "Nguyen Van An", email: "an@kytu.com" });

  const response = await agent.get("/yeucau/5/edit");

  assert.equal(response.status, 403);
  assert.match(response.text, /Sinh vien khong duoc sua hoac xoa yeu cau sua chua\./);
});

test("SinhVien khong duoc ghi thanh toan o frontend", async () => {
  const agent = await taoAgent({ id: "1", role: "SinhVien", hoTen: "Nguyen Van An", email: "an@kytu.com" });

  const response = await agent.post("/thanhtoan/5/delete");

  assert.equal(response.status, 403);
  assert.match(response.text, /Sinh vien khong duoc sua hoac xoa thanh toan\./);
});

test("SinhVien chi duoc sua ho so cua chinh minh tren frontend", async () => {
  const agent = await taoAgent({ id: "1", role: "SinhVien", hoTen: "Nguyen Van An", email: "an@kytu.com" });

  const response = await agent.get("/sinhvien/2/edit");

  assert.equal(response.status, 403);
  assert.match(response.text, /Ban chi duoc cap nhat ho so cua chinh minh\./);
});

test("Nguoi da dang nhap vao /login se duoc chuyen huong ve dashboard", async () => {
  const agent = await taoAgent({ id: "0", role: "Admin", hoTen: "Quan tri vien", email: "admin@kytu.com" });

  const response = await agent.get("/login");

  assert.equal(response.status, 302);
  assert.equal(response.headers.location, "/");
});
