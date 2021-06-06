using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NguyenThiPhuongThao_51.Models;

namespace NguyenThiPhuongThao_51.Controllers
{
    public class NhanViensController : Controller
    {
        private LTQLDbContext db = new LTQLDbContext();

        // GET: NhanViens
        public ActionResult Index()
        {
            return View(db.NhanViens.ToList());
        }

        // GET: NhanViens/Details/5
        public ActionResult Index()
        {
            return View(db.NhanViens.ToList());
        }

        // GET: SINHVIENs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // GET: SINHVIENs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SINHVIENs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Masv,Hoten,Diachi,Malop")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.NhanViens.Add(nhanVien);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nhanVien);
        }

        // GET: SINHVIENs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: SINHVIENs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNV,Hoten,MaPhongBan")] NhanVien nhanVien)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nhanVien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nhanVien);
        }

        // GET: SINHVIENs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhanVien nhanVien = db.NhanViens.Find(id);
            if (nhanVien == null)
            {
                return HttpNotFound();
            }
            return View(nhanVien);
        }

        // POST: SINHVIENs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NhanVien nhanVien = db.NhanViens.Find(id);
            db.NhanViens.Remove(nhanVien);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DownloadFile()
        {
            //duong dan chua file muon download
            string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/ExcelFile/"; // tu muc chua fiel excel
            //chuyen file sang dang byte
            byte[] fileBytes = System.IO.File.ReadAllBytes(path + "fielexcel.xlsx"); //doan nay de file excel
            //ten file khi download ve
            string fileName = "NhanViencapnhat.xlsx";
            //tra ve file
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            //dat ten cho file
            string _FileName = "NHANVIEN.xls";
            //duong dan luu file
            string _path = Path.Combine(Server.MapPath("~/Uploads/ExcelFile"), _FileName);
            //luu file len server
            file.SaveAs(_path);

            //doc du lieu tu file excel

            DataTable dt = ReadDataFromExcelFile(_path);
            /* CopyDataByBulk(dt);*/
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                NhanVien sv = new NhanVien();
                sv.MaNV = dt.Rows[i][0].ToString();
                sv.HoTen = dt.Rows[i][1].ToString();
                db.NhanViens.Add(sv);
                db.SaveChanges();
            }

            /* CopyDataByBulk(excel.ReadDataFromExcelFile(_path));*/
            return RedirectToAction("Index");
        }
        private void CopyDataByBulk(object v)
        {
            throw new NotImplementedException();
        }

        //upload file
        public DataTable ReadDataFromExcelFile(string filepath)
        {
            string connectionString = "";
            string fileExtention = filepath.Substring(filepath.Length - 4).ToLower();
            if (fileExtention.IndexOf("xlsx") == 0)
            {
                connectionString = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + filepath + ";Extended Properties=\"Excel 12.0 Xml;HDR=NO\"";
            }
            else if (fileExtention.IndexOf(".xls") == 0)
            {
                connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filepath + ";Extended Properties=Excel 8.0";
            }

            // Tạo đối tượng kết nối
            OleDbConnection oledbConn = new OleDbConnection(connectionString);
            DataTable data = null;
            try
            {
                // Mở kết nối
                oledbConn.Open();

                // Tạo đối tượng OleDBCommand và query data từ sheet có tên "Sheet1"
                OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Sheet1$]", oledbConn);

                // Tạo đối tượng OleDbDataAdapter để thực thi việc query lấy dữ liệu từ tập tin excel
                OleDbDataAdapter oleda = new OleDbDataAdapter();

                oleda.SelectCommand = cmd;

                // Tạo đối tượng DataSet để hứng dữ liệu từ tập tin excel
                DataSet ds = new DataSet();

                // Đổ đữ liệu từ tập excel vào DataSet
                oleda.Fill(ds);

                data = ds.Tables[0];
            }
            catch
            {
            }
            finally
            {
                // Đóng chuỗi kết nối
                oledbConn.Close();
            }
            return data;
        }
        /* public ActionResult UploadFile (HttpPostedFileBase file)
         {
             try
             {
                 if (file.ContentLength > 0)
                 {

                     string _FileName = DateTime.Now.Year.ToString() + DateTime.Now.Date.ToString();

                     string _path = Path.Combine(Server.MapPath("~/Uploads/ExcelFile/"), _FileName);

                     file.SaveAs(_path);

                     CopyDataByBulk(ReadDataFromExcelFile(_path));
                     ViewBag.ThongBao = "cap nhat thanh cong";
                 }
             }
             catch (Exception ex)
             {
                 ViewBag.ThongBao = "cap nhat thanh cong";
             }
             return RedirectToAction("Index");
         }*/

        //copy large data from datatable to sqlserver
        private void CopyDataByBulk(DataTable dt)
        {
            //lay ket noi voi database luu trong file webconfig
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LTQLDbContext"].ConnectionString);
            SqlBulkCopy bulkcopy = new SqlBulkCopy(con);
            bulkcopy.DestinationTableName = "SINHVIENs";
            bulkcopy.ColumnMappings.Add(0, "MaNV");
            bulkcopy.ColumnMappings.Add(1, "Hoten");
            bulkcopy.ColumnMappings.Add(2, "MaPB");
            con.Open();
            bulkcopy.WriteToServer(dt);
            con.Close();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
