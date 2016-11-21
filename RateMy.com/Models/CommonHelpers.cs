using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MySql.Data.MySqlClient;

namespace RateMy.com.Models
{
    public class CommonHelpers
    {
        public ImageForDb CreateImageForDb(string name, string email, HttpPostedFileBase uploadFile)
        {
            var uploadFileAsStream = uploadFile.InputStream;
            name = FirstLetterUppercase(name); // First letter in name to Uppercase
            var imageForDb = new ImageForDb(name, email, 0, 0, 0, false, uploadFile.FileName, uploadFile.ContentType,
                uploadFileAsStream);
            return imageForDb;
        }

        public string FirstLetterUppercase(string s)
        {
            if (string.IsNullOrEmpty(s))
                return string.Empty;
            var a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }

        public List<ImageForWeb> GetImagesForWebFromDb(bool admin)
        {
            var listOfImageForWeb = new List<ImageForWeb>();

            var connectionString = ConfigurationManager.ConnectionStrings["Mysql"].ConnectionString;
            using (var con = new MySqlConnection(connectionString))
            {
                const string nameQuery = "SELECT * FROM Images";

                using (var cmd = new MySqlCommand(nameQuery))
                {
                    cmd.Connection = con;
                    con.Open(); // Open connection

                    // Setting timeout on mysqlServer
                    var cmdtest = new MySqlCommand("set net_write_timeout=99999; set net_read_timeout=99999", con);
                    cmdtest.ExecuteNonQuery();

                    var reader = cmd.ExecuteReader();
                    while (reader.Read()) // Start reader
                    {
                        var id = Convert.ToInt32(reader["id"].ToString());
                        var name = reader["Name"].ToString();
                        var email = reader["Email"].ToString();
                        var upVotes = Convert.ToInt32(reader["UpVotes"].ToString());
                        var downVotes = Convert.ToInt32(reader["DownVotes"].ToString());
                        var reports = Convert.ToInt32(reader["Reports"].ToString());
                        var shown = Convert.ToBoolean(reader["Shown"].ToString());
                        var imageName = reader["ImageName"].ToString();
                        var imageType = reader["ImageType"].ToString();
                        var imageFileBytes = (byte[]) reader["ImageFile"];

                        if (imageFileBytes == null) continue;

                        var fullImage = Image.FromStream(new MemoryStream(imageFileBytes));

                        Image thumbImage = ResizeImage(fullImage, 360, 247);
                        //Image resizedTo1200 = ResizeImage(Image.FromStream(new MemoryStream(imageFileBytes)), 1980, 1200); // Resize thumbnail hack
                        //Image thumbImage = ResizeImage(resizedTo1200, 360, 247);


                        // Get the right image corresponding to admin or alm user
                        if (admin && (shown == false))
                        {
                            listOfImageForWeb.Add(new ImageForWeb(id, name, email, upVotes, downVotes, reports, imageName,
                                imageType, fullImage, GetFullImageFromFolder(id, name), GetThumbImageFromFolder(id, name)));
                        }
                        else if ((admin == false) && shown)
                        {
                            listOfImageForWeb.Add(new ImageForWeb(id, name, email, upVotes, downVotes, reports, imageName,
                                imageType, fullImage, GetFullImageFromFolder(id, name), GetThumbImageFromFolder(id, name)));
                        }
                            

                        // Save save image to folder if not already there
                        SaveNewImagesFromDbToContentFolder(id.ToString(), name, true, fullImage); // Full image
                        SaveNewImagesFromDbToContentFolder(id.ToString(), name, false, thumbImage); // Thumb image
                    }

                    reader.Close(); // Stop reader
                    con.Close(); // Close connection
                }
            }
            return listOfImageForWeb;
        }

        public void SaveNewImagesFromDbToContentFolder(string id, string name, bool full, Image image)
        {
            string savePath;

            if (full)
            {
                // If full image
                savePath = HostingEnvironment.MapPath("~/Content/images/Fulls/") + id + name + ".Jpeg";
            }
            else
            {
                // If thumb image
                savePath = HostingEnvironment.MapPath("~/Content/images/Thumbs/") + id + name + ".Jpeg";
            }

            if (!File.Exists(savePath))
                image.Save(savePath, ImageFormat.Jpeg);
        }

        public IEnumerable<string> GetAllFullImagesFromFolder()
        {
            var images = Directory.EnumerateFiles(HostingEnvironment.MapPath("~/Content/images/Fulls/"))
                .Select(fn => "~/Content/images/Fulls/" + Path.GetFileName(fn));

            // Trim for HTML
            images = images.Select(x => x.TrimStart('~', '/'));
            return images;
        }

        public IEnumerable<string> GetAllThumbImagesFromFolder()
        {
            var images = Directory.EnumerateFiles(HostingEnvironment.MapPath("~/Content/images/Thumbs/"))
                .Select(fn => "~/Content/images/Thumbs/" + Path.GetFileName(fn));

            // Trim for HTML
            images = images.Select(x => x.TrimStart('~', '/'));
            return images;
        }

        public string GetFullImageFromFolder(int id, string name)
        {
            var images = GetAllFullImagesFromFolder();
            var image = images.FirstOrDefault(x => x.Contains(id.ToString() + name));


            return image;
        }

        public string GetThumbImageFromFolder(int id, string name)
        {
            var images = GetAllThumbImagesFromFolder();
            var image = images.FirstOrDefault(x => x.Contains(id.ToString() + name));

            return image;
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}